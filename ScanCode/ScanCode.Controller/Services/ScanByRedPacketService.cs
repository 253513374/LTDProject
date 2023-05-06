using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ScanCode.Controller.Models.ResponseModel;
using ScanCode.Controller.Services;
using ScanCode.Model;
using ScanCode.Model.Entity;
using ScanCode.Model.Enum;
using ScanCode.Model.ResponseModel;
using ScanCode.Mvc.Models;
using ScanCode.RedisCache;
using ScanCode.Repository;
using ScanCode.Repository.MediatRHandler.Events;
using ScanCode.Repository.Utility;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.TenPay.V3;
using System.Net;
using System.Net.Sockets;

namespace ScanCode.Mvc.Services
{
    public class ScanByRedPacketService
    {
        private readonly RedPacketRecordRepository _redPacketRecordRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;
        private readonly ScanRedPacketRepository _redPacketRepository;
        private readonly WLabelStorageRepository _wLabelStorageRepository;
        private readonly BdxOrderRepository _bdxOrderRepository;
        //private readonly IDistributedCache _distributedCache;
        //  private readonly IDatabase _database;

        private string appId = Config.SenparcWeixinSetting.WeixinAppId;

        private string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        private static TenPayV3Info _tenPayV3Info;

        private readonly IMediator _mediator;
        private ILogger _logger;

        private readonly IRedisCache _redisCache;

        /// <summary>
        /// 本机IP，局域网IP
        /// </summary>
        private readonly string _localIPAddress;

        private HubService _hubService;

        public ScanByRedPacketService(RedPacketRecordRepository redPacketRecordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ScanRedPacketRepository redPacketRepository,
            WLabelStorageRepository wLabelStorage,
            IDistributedCache distributedCache,
                ILogger<ScanByRedPacketService> logger,
            IRedisCache redisCache,
            BdxOrderRepository bdxOrderRepository,
            //   IConnectionMultiplexer connectionMultiplexer,
            HubService service,
             IMediator mediator)
        {
            _bdxOrderRepository = bdxOrderRepository;
            _redisCache = redisCache;
            _logger = logger;
            _hubService = service;
            _redPacketRecordRepository = redPacketRecordRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _redPacketRepository = redPacketRepository;
            _wLabelStorageRepository = wLabelStorage;
            //_distributedCache = distributedCache;
            //  _database = connectionMultiplexer.GetDatabase();
            _mediator = mediator;
            //_localIPAddress = GetIp();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            _localIPAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    var key = TenPayHelper.GetRegisterKey(Config.SenparcWeixinSetting);

                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[key];
                }
                return _tenPayV3Info;
            }
        }

        /// <summary>
        /// 验证码领取现金红包
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public async Task<RedPacketResult> GrantCaptchaRedPackets(string openid, string qrcode, string captcha)
        {
            try
            {
                var returnresult = new RedPacketResult();
                var result = await VerifyCaptchaRedPacket(openid, qrcode, captcha);
                if (result.IsSuccess)
                {
                    var request = await CreateWxRequest(openid, qrcode, RedPacketConfigType.Captcha, captcha);
                    return await SendRedPackAsync(request);
                }
                return RedPacketResult.Fail(result.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"验证码发放红包出现异常:{e.Message}");
                return RedPacketResult.Fail($"验证码发放红包出现异常：{e.Message}");
            }
        }

        private async Task<WXRedPackRequest> CreateWxRequest(string openid, string qrcode, RedPacketConfigType configType, string captcha = "")
        {
            // var config = await _redPacketRepository.FindScanRedPacketAsync();
            var vCinfigs = await _redisCache.GetObjectAsync<List<RedPacketCinfig>>(CacheKeys.REDPACKET_OPTIONS);

            RedPacketCinfig? config = null;
            switch (configType)
            {
                case RedPacketConfigType.Captcha:

                    config = vCinfigs.FirstOrDefault(s => s.RedPacketConfigType == RedPacketConfigType.Captcha); //await _redPacketRepository.FindCaptchaRedPacketAsync();
                    break;

                case RedPacketConfigType.QRCode:

                    config = vCinfigs.FirstOrDefault(s => s.RedPacketConfigType == RedPacketConfigType.QRCode);
                    //config = await _redPacketRepository.FindScanRedPacketAsync();
                    break;

                default:

                    break;
            }

            //获取需要发放得红包金额
            int amount = 1;
            if (config.RedPacketType == RedPacketType.AVERAGE)
            {
                amount = config.CashValue;
            }
            else
            {
                amount = await GlobalUtility.GetRandomInt(config.MinCashValue, config.MaxCashValue);
            }

            return new WXRedPackRequest()
            {
                appId = TenPayV3Info.AppId,
                mchId = TenPayV3Info.MchId,
                tenPayKey = TenPayV3Info.Key,
                actionName = config.ActivityName,
                iP = _localIPAddress,
                mchBillNo = $"WT{SystemTime.Now.ToString("yyyyMMddHHmmssfff")}{TenPayV3Util.BuildRandomStr(3)}",
                openId = openid,
                senderName = config.SenderName,
                redPackAmount = amount,
                wishingWord = config.WishingWord,
                QRCode = qrcode,
                Captcha = captcha,
                TenPayCertPath = TenPayV3Info.CertPath,
            };
        }

        /// <summary>
        /// 二维码领取现金红包
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public async Task<RedPacketResult> GrantQRCodeRedPackets(string openid, string qrcode)
        {
            try
            {
                var result = await VerifyQRCodeRedPacket(openid, qrcode);
                if (result.IsSuccess)
                {
                    var request = await CreateWxRequest(openid, qrcode, RedPacketConfigType.QRCode);
                    return await SendRedPackAsync(request);
                }
                return RedPacketResult.Fail(result.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"二维码发放红包出现异常:{e.Message}");
                return RedPacketResult.Fail($"二维码发放红包出现异常：{e.Message}");
            }
        }

        /// <summary>
        /// 验证验证码发放现金红包的资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        private async Task<RedStatusResult> VerifyCaptchaRedPacket(string openid, string qrcode, string captcha)
        {
            var vcode = await VerifyCaptchaRedPacket(openid, qrcode);
            if (vcode.IsSuccess)
            {
                //判断验证码是否正确
                var validation = await _verificationCodeRepository.AnyAsync(a => a.QRCode == qrcode && a.Captcha == captcha);
                if (!validation)
                {
                    return RedStatusResult.FailInvalidCaptcha("验证码无效");
                }
                //一枚验证码只能领取一次红包
                var reslut = await _redPacketRecordRepository.ExistAsync(a => a.Captcha == captcha);
                if (reslut)
                {
                    return RedStatusResult.FailCaptchaUsed("验证码已经被使用过");
                }
            }
            return vcode;
        }

        /// <summary>
        /// 验证二维码发放现金红包的资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<RedStatusResult> VerifyQRCodeRedPacket(string openid, string qrcode)
        {
            var list = await _redPacketRecordRepository.FindAsync(qrcode);
            if (list == 1)
            {
                return RedStatusResult.FailMaximumLimit("红包已经领过啦!，请查看威特五金公众号红包信息。");
            }

            if (list == 2)
            {
                return RedStatusResult.FailMaximumLimit("当前标签序号红包已经领取完毕");
            }
            var userilmts = await _redPacketRecordRepository.FindUserLimt(openid);
            if (userilmts == 10)
            {
                return RedStatusResult.FailMaxUserLimit("今日已经领取10个红包上限，请明日再参与扫码领现金红包活动");
            }
            return RedStatusResult.Success("可以参加扫码得现金活动");
        }

        private async Task<RedStatusResult> VerifyCaptchaRedPacket(string openid, string qrcode)
        {
            var list = await _redPacketRecordRepository.FindAsync(qrcode);

            if (list == 2)
            {
                return RedStatusResult.FailMaximumLimit("当前标签序号红包已经领取完毕");
            }
            var userilmts = await _redPacketRecordRepository.FindUserLimt(openid);
            if (userilmts == 10)
            {
                return RedStatusResult.FailMaxUserLimit("今日已经领取10个红包上限，请明日再参与扫码领现金红包活动");
            }
            return RedStatusResult.Success("可以参加扫码得现金活动");
        }

        /// <summary>
        /// 返回该标签序号领取现金红包状态
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<RedStatusResult> GetRedStatusResultAsync(string openid, string qrcode, string ordernumbels)
        {
            try
            {
                var sss = await _redisCache.GetObjectAsync<List<RedPacketCinfig>>(CacheKeys.REDPACKET_OPTIONS);
                var optioncode = sss.FirstOrDefault(f => f.RedPacketConfigType == RedPacketConfigType.QRCode);
                var optioncaptch = sss.FirstOrDefault(f => f.RedPacketConfigType == RedPacketConfigType.Captcha);

                if (!optioncode.IsActivity && !optioncaptch.IsActivity)
                {
                    return RedStatusResult.FailNotActivated("扫码领红包活动还没有激活");
                }
                if (optioncode.IsSubscribe || optioncaptch.IsSubscribe)
                {
                    var userInfo = UserApi.Info(appId, openid, Language.zh_CN);
                    if (userInfo.subscribe != 1)
                    {
                        return RedStatusResult.FailNotFollowed("请先关注微信公众号在参加扫码领取现金红包活动");
                    }
                }
                //验证标签序号是否导入
                var validation = await _verificationCodeRepository.ExistAsync(a => a.QRCode.Contains(qrcode));
                if (!validation)
                {
                    return RedStatusResult.FailNotImportData("红包数据不存在");
                }

                //判断订单数据是否出库发车（激活）,
                var bdxOrder = await _bdxOrderRepository.GetSingleAsync(ordernumbels);
                if (bdxOrder is not null)
                {
                    if (string.IsNullOrWhiteSpace(bdxOrder.THRQ))
                    {
                        return RedStatusResult.FailNot("标签还没有确认提货放行单");
                    }
                    DateTime dateTime = DateTime.Parse(bdxOrder.THRQ);
                    dateTime.AddHours(2);
                    if (string.IsNullOrWhiteSpace(bdxOrder.THRQ))
                    {
                        return RedStatusResult.FailNotActivated("扫码领现金红包活动时间还未开始");
                    }
                    //当前时间大于设置的激活时间，说明活动已经激活。
                    if (DateTime.Now > dateTime)
                    {
                        //return RedStatusResult.Success("可以参加扫码得现金活动");
                    }
                    else
                    {
                        return RedStatusResult.FailNotActivated("扫码领现金红包活动时间还未开始");
                    }
                }
                else
                {
                    return RedStatusResult.FailNot("单号数据不存在");
                }

                //stopwatch.Restart();
                var list = await _redPacketRecordRepository.FindAsync(qrcode);

                if (list == 0)
                {
                    return RedStatusResult.SuccessQrCode("首次参加扫码领现金红包活动");
                }

                if (list == 1)
                {
                    return RedStatusResult.SuccessCaptcha("扫码输入验证码参与扫码领现金红包活动");
                }

                if (list == 2)
                {
                    return RedStatusResult.FailMaximumLimit("当前标签序号红包已经领取完毕");
                }

                return RedStatusResult.FailNot("验证失败");
            }
            catch (Exception e)
            {
                _logger.LogError($"标签序号领取现金红包状态验证出现异常：{e.Message}");
                return RedStatusResult.FailNotException($"标签序号领取现金红包状态验证出现异常：{e.Message}");
            }
        }

        private async Task<RedPacketResult> SendRedPackAsync(WXRedPackRequest request)
        {
            var returnresult = new RedPacketResult();

            string nonceStr; //随机字符串
            string paySign;  //签名
            var sendNormalRedPackResult = RedPackApi.SendNormalRedPack(
                request.appId, request.mchId, request.tenPayKey,
                request.TenPayCertPath, //证书物理地址
                request.openId, //接受收红包的用户的openId
                request.senderName, //红包发送者名称
                request.iP,
                //HttpContext.UserHostAddress()?.ToString(),      //IP
                request.redPackAmount, //付款金额，单位分
                request.wishingWord, //红包祝福语
                request.actionName, //活动名称
                request.remark, //备注信息
                out nonceStr,
                out paySign,
                null, //场景id（非必填）
                null, //活动信息（非必填）
                null //资金授权商户号，服务商替特约商户发放时使用（非必填）
            );

            if (sendNormalRedPackResult.ReturnCodeSuccess)
            {
                var msg = $"{sendNormalRedPackResult.err_code}:{sendNormalRedPackResult.err_code_des}";

                if (sendNormalRedPackResult.ResultCodeSuccess)
                {
                    //红包发放成功
                    returnresult.IsSuccess = true;
                    returnresult.Message = "红包发放成功";
                    returnresult.TotalAmount = sendNormalRedPackResult.total_amount;
                    var packeresult = new RedPacketRecord
                    {
                        MchbillNo = sendNormalRedPackResult.mch_billno,
                        TotalAmount = sendNormalRedPackResult.total_amount,
                        QrCode = request.QRCode,
                        Captcha = request.Captcha,
                        ReOpenId = request.openId,
                        SendListid = sendNormalRedPackResult.send_listid,
                        WxAppId = sendNormalRedPackResult.wxappid,
                        MchId = sendNormalRedPackResult.mch_id,
                        IssueTime = DateTime.Now,
                        NonceStr = nonceStr, //随机字符串
                        PaySign = paySign, //签名
                        ActivityName = request.actionName,
                        CreateTime = DateTime.Now,
                    };
                    //保存红包发放记录
                    var addresult = await _redPacketRecordRepository.AddAsync(packeresult);
                    //实时通知系统红包发放成功
                    await _hubService.SendSendRedpacketTotalAmountAsync(sendNormalRedPackResult.total_amount);
                }
                else
                {
                    //"账号余额不足，请到商户平台充值后再重试";
                    returnresult.IsSuccess = false;
                    returnresult.Message = msg;

                    if (sendNormalRedPackResult.err_code == "NOTENOUGH")
                    {
                        await _mediator.Publish(new SendEmailEvent()
                        {
                            Title = "扫码领微信红包活动重要通知",
                            Content = $"微信红包活动：{msg}，请及时充值。",
                            Email = ""
                        });
                    }

                    if (sendNormalRedPackResult.err_code == "SIGN_ERROR")
                    {
                        await _mediator.Publish(new SendEmailEvent()
                        {
                            Title = "扫码领微信红包活动重要通知",
                            Content = $"微信红包活动：{msg}.",
                            Email = ""
                        });
                    }
                }
                //else if (sendNormalRedPackResult.result_code == "SYSTEMERROR")
                //{
                //    // "系统繁忙，请稍后再试,使用原单号调用接口，查询发放结果";
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = msg;
                //}
                //else if (sendNormalRedPackResult.result_code == "SIGN_ERROR")
                //{
                //    // "参数签名结果不正确";
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = msg;
                //}
                //else if (sendNormalRedPackResult.result_code == "XML_ERROR")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "输入xml参数格式错误,请求参数未按指引进行填写";
                //}
                //else if (sendNormalRedPackResult.result_code == "FATAL_ERROR")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "openid和原始单参数不一致";
                //}
                //else if (sendNormalRedPackResult.result_code == "FREQ_LIMIT")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "超过频率限制，请稍后再试";
                //}
                //else if (sendNormalRedPackResult.result_code == "SENDAMOUNT_LIMIT")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "商户号今日发放金额超过限制";
                //}
                //else if (sendNormalRedPackResult.result_code == "MONEY_LIMIT")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "发送红包金额不在限制范围内";
                //}
                //else if (sendNormalRedPackResult.result_code == "CA_ERROR")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "商户API证书校验出错";
                //}
                //else if (sendNormalRedPackResult.result_code == "PARAM_ERROR")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message =
                //        $"{sendNormalRedPackResult.err_code}:{sendNormalRedPackResult.err_code_des}";
                //}
                //else if (sendNormalRedPackResult.result_code == "SENDNUM_LIMIT")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "今日领取红包个数超过限制";
                //}
                //else if (sendNormalRedPackResult.result_code == "SEND_FAILED")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "红包发放失败,请更换单号再重试";
                //}
                //else if (sendNormalRedPackResult.result_code == "API_METHOD_CLOSED")
                //{
                //    //请求接口失败
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "你的商户号API发放方式已关闭，请联系管理员在商户平台开启";
                //}
                //else if (sendNormalRedPackResult.result_code == "PROCESSING")
                //{
                //    //请求接口失败
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "请求已受理，请稍后使用原单号调用接口查询发放结果";
                //}
                //else if (sendNormalRedPackResult.result_code == "RCVDAMOUNT_LIMIT")
                //{
                //    //请求接口失败
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "该用户今日领取红包总金额超过您在微信支付商户平台配置的上限。";
                //}
                //else if (sendNormalRedPackResult.result_code == "PAYER_ACCOUNT_ABNORMAL")
                //{
                //    returnresult.IsSuccess = false;
                //    returnresult.Message = "商户号被处罚、冻结";
                //}
            }
            else
            {
                returnresult.IsSuccess = false;
                returnresult.Message = $"通信错误：{sendNormalRedPackResult.return_code}:{sendNormalRedPackResult.return_msg}";
            }

            return returnresult;
        }
    }
}