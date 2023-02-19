using Senparc.CO2NET.Helpers;
using Senparc.Weixin;
using Wtdl.Mvc.Models;
using Wtdl.Repository;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.TenPay.V3;
using Weitedianlan.Model.Entity;
using System.Net.Sockets;
using System.Net;
using MediatR;
using Senparc.Weixin.Helpers;
using Weitedianlan.Model.Enum;
using Wtdl.Repository.Utility;
using Senparc.Weixin.TenPay.V2;
using StackExchange.Redis;
using Wtdl.Controller.Models.ResponseModel;
using Wtdl.Repository.MediatRHandler.Events;
using Microsoft.Extensions.Caching.Distributed;

namespace Wtdl.Mvc.Services
{
    public class ScanByRedPacketService
    {
        private readonly RedPacketRecordRepository _redPacketRecordRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;
        private readonly ScanRedPacketRepository _redPacketRepository;
        private readonly WLabelStorageRepository _wLabelStorageRepository;

        //private readonly IDistributedCache _distributedCache;
        private readonly IDatabase _database;

        private string appId = Config.SenparcWeixinSetting.WeixinAppId;

        private string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        private static TenPayV3Info _tenPayV3Info;

        private readonly IMediator _mediator;

        /// <summary>
        /// 本机IP，局域网IP
        /// </summary>
        private readonly string _localIPAddress;

        public ScanByRedPacketService(RedPacketRecordRepository redPacketRecordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ScanRedPacketRepository redPacketRepository,
            WLabelStorageRepository wLabelStorage,
            IDistributedCache distributedCache,
            IConnectionMultiplexer connectionMultiplexer,
             IMediator mediator)
        {
            _redPacketRecordRepository = redPacketRecordRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _redPacketRepository = redPacketRepository;
            _wLabelStorageRepository = wLabelStorage;
            //_distributedCache = distributedCache;
            _database = connectionMultiplexer.GetDatabase();
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
        /// 验证产品是否已经扫码出库，并且已经出库超过指定间隔时间
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyOut(string qrcode)
        {
            //抽奖限制：出库24小时才能抽奖
            var qrcodekey = qrcode.Substring(0, 4);
            var offset = qrcode.Substring(4, 7);

            //获取出库状态 偏移量的位置为 1,说明已经出库
            var bitValue = (await _database.StringGetBitAsync(qrcodekey, Convert.ToInt32(offset)));//.get(100);
            if (bitValue)
            {
                //获取出库数据是否超过24小时
                var value = await _database.StringGetAsync(qrcode);
                if (value.HasValue)
                {
                    // key 存在，说明还没有超过24小时,继续处理
                    return new VerifyResult()
                    {
                        IsSuccess = false,
                        Message = "标签已经扫码出库，但是还没有到抽奖时间",
                    };
                }
                else
                {
                    return new VerifyResult()
                    {
                        IsSuccess = true,
                        Message = "标签已经扫码出库，但是还没有到抽奖时间",
                    };
                }
            }
            else
            {
                // 偏移量的位置值为 0，说明数据还没有出库
                return new VerifyResult()
                {
                    IsSuccess = false,
                    Message = "标签还没有扫码出库，无法参与活动",
                };
            }
        }

        /// <summary>
        /// 验证码发放红包
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public async Task<RedPacketResult> GrantCaptchaRedPackets(string openid, string qrcode, string captcha)
        {
            var returnresult = new RedPacketResult();
            var result = await VerifyCaptchaRedPacket(openid, qrcode, captcha);
            if (result.IsSuccess)
            {
                var request = await CreateWxRequest(openid, qrcode, captcha);
                return await SendRedPackAsync(request);
            }

            return new RedPacketResult
            {
                IsSuccess = false,
                Message = result.Message
            };
        }

        private async Task<WXRedPackRequest> CreateWxRequest(string openid, string qrcode, string captcha = "")
        {
            var config = await _redPacketRepository.FindScanRedPacketAsync();

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
                mchBillNo = $"WTDL{SystemTime.Now.ToString("yyyyMMddHHmmssfff")}{TenPayV3Util.BuildRandomStr(3)}",
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
        /// 二维码第一次领取现金红包
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public async Task<RedPacketResult> GrantQRCodeRedPackets(string openid, string qrcode)
        {
            var result = await VerifyQRCodeRedPacket(openid, qrcode);
            if (result.IsSuccess)
            {
                var request = await CreateWxRequest(openid, qrcode);
                return await SendRedPackAsync(request);
            }
            return new RedPacketResult
            {
                IsSuccess = false,
                Message = result.Message
            };
        }

        /// <summary>
        /// 验证验证码发放现金红包的资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyCaptchaRedPacket(string openid, string qrcode, string captcha)
        {
            var value = await VerifyOut(qrcode);
            if (!value.IsSuccess)
            {
                return value;
            }

            var active = await _redPacketRepository.AnyRedPacketActiveAsync();
            if (!active)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "没有现金红包活动"
                };
            }

            //判断标签序号与验证码是否匹配
            var validation = await _verificationCodeRepository.AnyAsync(a => a.QRCode == qrcode && a.Captcha == captcha);
            if (!validation)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "标签序号或者验证码错误"
                };
            }

            //一枚标签验证码只能领取一次红包
            var reslut = await _redPacketRecordRepository.ExistAsync(a => a.Captcha == captcha);

            if (reslut)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "该验证码已经领取过红包"
                };
            }

            //验证是否需要关注才能领取红包
            var redpacket = await _redPacketRepository.FindScanRedPacketAsync();
            if (redpacket.IsSubscribe)
            {
                var userInfo = UserApi.Info(appId, openid, Language.zh_CN);
                if (userInfo.subscribe != 1)
                {
                    return new VerifyResult
                    {
                        IsSuccess = false,
                        Message = "需要先关注公众号才能领取现金红包",
                    };
                }
            }
            return new VerifyResult
            {
                IsSuccess = true,
                Message = "验证成功"
            };
        }

        /// <summary>
        /// 验证二维码第一次发放现金红包的资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyQRCodeRedPacket(string openid, string qrcode)
        {
            var value = await VerifyOut(qrcode);
            if (!value.IsSuccess)
            {
                return value;
            }

            var active = await _redPacketRepository.AnyRedPacketActiveAsync();
            if (!active)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "没有现金红包活动"
                };
            }

            //验证标签序号是否导入
            var validation = await _verificationCodeRepository.ExistAsync(a => a.QRCode.Contains(qrcode));
            if (!validation)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "标签序号没有导入数据"
                };
            }
            ////验证标签序号是否扫码出库
            //var validationout = await _wLabelStorageRepository.AnyRedPacket(qrcode);
            //if (!validationout)
            //{
            //    return new VerifyResult { IsSuccess = false, Message = "标签序号没有扫码出库" };
            //
            //}

            //一枚二维码标签序号只能领取一次红包
            var reslut = await _redPacketRecordRepository.ExistAsync(a => a.QrCode == qrcode);

            if (reslut)
            {
                return new VerifyResult
                {
                    IsSuccess = false,
                    Message = "该二维码已经领取过红包"
                };
            }

            //验证是否需要关注才能领取红包
            var redpacket = await _redPacketRepository.FindScanRedPacketAsync();
            if (redpacket.IsSubscribe)
            {
                var userInfo = UserApi.Info(appId, openid, Language.zh_CN);
                if (userInfo.subscribe != 1)
                {
                    return new VerifyResult
                    {
                        IsSuccess = false,
                        Message = "需要先关注公众号才能领取现金红包",
                    };
                }
            }
            return new VerifyResult
            {
                IsSuccess = true,
                Message = "验证成功"
            };
        }

        /// <summary>
        /// 返回该标签序号是否是第一次领取现金红包
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<RedStatusResult> AnyFirstRedPacket(string qrcode)
        {
            var list = await _redPacketRecordRepository.FindAsync(a => a.QrCode == qrcode);

            if (list is null || list.Count() == 0)
            {
                //第一次领取红包
                return new RedStatusResult() { IsSuccess = true, StuteCode = "QRCODE" };
            }

            if (list.Count() == 1)
            {
                //第二次领取红包
                return new RedStatusResult() { IsSuccess = true, StuteCode = "CAPTCHA" };
            }

            //if (list.Count() == 2)
            //{
            //    //不能再领取红包
            //    return new RedStatusResult() { IsSuccess = false, StuteCode = "NOT" };
            //}
            return new RedStatusResult() { IsSuccess = false, StuteCode = "NOT" };
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
                    };
                    //保存红包发放记录
                    var addresult = await _redPacketRecordRepository.AddAsync(packeresult);
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