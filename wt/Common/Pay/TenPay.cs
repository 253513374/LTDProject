using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common.Pay
{
    public class TenPay
    {

        //微信公众号Appid
        private string wxAppid = System.Configuration.ConfigurationManager.AppSettings["wxAppid"];
        //微信公众号AppSecret
        private string wxAppSecret = System.Configuration.ConfigurationManager.AppSettings["wxAppSecret"];
        //微信商户平台账号
        private string MchId = System.Configuration.ConfigurationManager.AppSettings["MchId"];
        //微信商户平台秘钥
        private string AppKey = System.Configuration.ConfigurationManager.AppSettings["AppKey"];




        #region 企业向用户发红包
        /// <summary>
        /// 用于企业向微信用户个人发红包
        /// 目前支持向指定微信用户的openid个人发红包
        /// </summary>
        /// <param name="certPassword">apiclient_cert.p12证书密码即商户号</param>
        /// <param name="data">微信支付需要post的xml数据</param>
        /// <param name="certPath">apiclient_cert.p12的证书物理位置(例如：E:\projects\文档\微信商户平台证书\商户平台API证书</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string Sendredpack(string data, string certPassword, string certPath, int timeOut = 3000)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            string cert = certPath;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate2 cer = new X509Certificate2(cert, certPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            ms.Write(formDataBytes, 0, formDataBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlFormat);
            request.ClientCertificates.Add(cer);
            request.Method = "POST";
            request.Timeout = timeOut;

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            #region 输入二进制流
            if (ms != null)
            {
                ms.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }
                ms.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        #endregion




        /// <summary>
        /// 构造参数(现金红包)
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="Amount">红包金额</param>
        /// <returns></returns>
        public bool GetJsApiParameters(string openid, int Amount)
        {
            #region 发送红包
            bool fals = false;   //记录发送红包是否成功
            string xmlResult = null;  //现金红包接口返回的xml
            string certPath = null;  //证书在服务器的物理位置
            string data = null;  //调用现金红包接口需要的数据
            try
            {
                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();
                string nonceStr = TenpayUtil.getNoncestr();  //时间戳
                //设置package订单参数
                packageReqHandler.SetParameter("nonce_str", nonceStr);    //随机字符串，不长于32位
                packageReqHandler.SetParameter("mch_billno", MchId + DateTime.Now.ToString("yyyyMMdd") + TenpayUtil.GuidNO());//商户订单号（每个订单号必须唯一）组成：mch_id+yyyymmdd+10位一天内不能重复的数字。接口根据商户订单号支持重入，如出现超时可再调用。
                packageReqHandler.SetParameter("mch_id", MchId);  //微信支付分配的商户号
                packageReqHandler.SetParameter("wxappid", wxAppid);//微信分配的公众账号ID（企业号corpid即为此appId）。接口传入的所有appid应该为公众号的appid（在mp.weixin.qq.com申请的），不能为APP的appid（在open.weixin.qq.com申请的）。 
                packageReqHandler.SetParameter("send_name", "测试");//商户名称
                packageReqHandler.SetParameter("re_openid", openid);  //用户openid  接受红包的用户用户在wxappid下的openid
                packageReqHandler.SetParameter("total_amount", Convert.ToInt32((decimal)(Amount * 100M)).ToString());  //付款金额 单位分
                packageReqHandler.SetParameter("total_num", "1");  //红包发放总人数
                packageReqHandler.SetParameter("wishing", "测试红包");  //红包祝福语
                packageReqHandler.SetParameter("client_ip", "218.241.17.29");//Ip地址
                packageReqHandler.SetParameter("act_name", "测试红包");//活动名称
                packageReqHandler.SetParameter("remark", "测试红包");     //备注
                string sign = packageReqHandler.CreateMd5Sign("key", AppKey);
                packageReqHandler.SetParameter("sign", sign);                        //签名
                data = packageReqHandler.ParseXML();
                certPath = System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["certPath"];
                xmlResult = Sendredpack(data, MchId, certPath);
                var res = System.Xml.Linq.XDocument.Parse(xmlResult);
                string return_code = res.Element("xml").Element("return_code").Value;
                if ("SUCCESS".Equals(return_code))
                {
                    string result_code = res.Element("xml").Element("result_code").Value;
                    if ("SUCCESS".Equals(result_code))
                    {
                        fals = true;
                    }
                }
                WeiXin.LogHelper.CreateWebLog("现金红包接口返回的xml：" + xmlResult.ToString());
                WeiXin.LogHelper.CreateWebLog("证书在服务器的物理位置：" + certPath.ToString());
                WeiXin.LogHelper.CreateWebLog("调用现金红包接口需要的数据：" + data.ToString());

            }
            catch (Exception exception)
            {
                WeiXin.LogHelper.CreateWebLog("异常错误：" + exception.ToString());
            }

            return fals;

            #endregion
        }

    }
}
