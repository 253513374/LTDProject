using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Weitedianlan.Model.Entity;
using Wtdl.Mvc.Models;
using Wtdl.Repository;

namespace Wtdl.Mvc.Services
{
    public class SearchByCodeService
    {
        private readonly WLabelStorageRepository _labelStorageRepository;
        private readonly AgentRepository _agentRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;
        private readonly IConfiguration _configuration;

        public SearchByCodeService(WLabelStorageRepository storageRepository,
            AgentRepository agentRepository,
            VerificationCodeRepository verificationCodeRepository,
            IConfiguration configuration)
        {
            _labelStorageRepository = storageRepository;
            _agentRepository = agentRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// 查询防伪标签数据
        /// </summary>
        /// <param name="hashtable"></param>
        /// <returns></returns>
        public async Task<SearchByCode> GetSearchByCodeAsync(string qrcode)
        {
            var url = _configuration.GetSection("QueryQRCode").Value;
            HttpWebRequest request = await CreateHttpWebRequest(url);
            var hashtable = await QueryParameter(qrcode);

            byte[] data = await EncodePars(hashtable);
            WriteRequestData(request, data);

            var servicedata = await ResponseFormat(request.GetResponse());

            return JsonSerializer.Deserialize<SearchByCode>(servicedata);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        private Task<Hashtable> QueryParameter(string querystring)
        {
            Hashtable pars = new Hashtable();
            //var code = await GetSpecialRuleCode(querystring);//queryLabelString.Substring(0, 4);
            // var CallCenterUser = await _UserManager.FindByNameAsync(UserName);
            pars["num"] = querystring;
            pars["code"] = querystring.Substring(0, 4); ;
            pars["query_type"] = "网络";
            pars["username"] = "wt";

            pars["ip"] = "13800000000";//电话？

            return Task.FromResult(pars);
        }

        private Task<HttpWebRequest> CreateHttpWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            return Task.FromResult(request);
        }

        private Task<byte[]> EncodePars(Hashtable hashtable)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in hashtable.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(hashtable[k].ToString()));
            }
            return Task.FromResult(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        private Task WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();

            return Task.CompletedTask;
        }

        private Task<string> ResponseFormat(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            return Task.FromResult(retXml);
        }

        public async Task<OutStorageResult> GetWLabelStorageAsync(string qrcode)
        {
            try
            {
                var wqrcode = await _labelStorageRepository.GetWLabelStorageAsync(qrcode);
                if (wqrcode is not null)
                {
                    var outqrcode = await _agentRepository.FindSingleAgentAsync(wqrcode.Dealers.Trim());

                    return new OutStorageResult()
                    {
                        Status = true,
                        AgentName = outqrcode.AName.Trim(),
                        OrderNumbels = wqrcode.OrderNumbels.Trim(),
                        OutTime = wqrcode.OutTime,
                        QRCode = wqrcode.QRCode
                    };
                }
                else
                {
                    return new OutStorageResult()
                    {
                        Status = false,
                        Msg = "查询不到标签，标签还未出库"
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}