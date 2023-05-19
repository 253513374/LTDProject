using Newtonsoft.Json;
using ScanCode.Model.Entity;
using ScanCode.Model.ResponseModel;
using ScanCode.Repository;

namespace ScanCode.Mvc.Services
{
    public class SearchByCodeService
    {
        private readonly WLabelStorageRepository _labelStorageRepository;
        private readonly AgentRepository _agentRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private ILogger<SearchByCodeService> _logger;

        public SearchByCodeService(WLabelStorageRepository storageRepository,
            AgentRepository agentRepository,
            IHttpClientFactory clientFactory,
            VerificationCodeRepository verificationCodeRepository,
            IConfiguration configuration,
            ILogger<SearchByCodeService> logger)
        {
            _clientFactory = clientFactory;
            _labelStorageRepository = storageRepository;
            _agentRepository = agentRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AntiFakeResult> QueryTag(string qrcode)
        {
            var url = BuildUrl(qrcode);
            _logger.LogInformation($"防伪查询URL：{url}");

            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"QueryTag防伪查询结果：{content}");
                    var result = JsonConvert.DeserializeObject<SearchByCode>(content);

                    return AntiFakeResult.Success(result);
                }
                else
                {
                    return AntiFakeResult.Fail($"请求返回失败代码：{response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
                return AntiFakeResult.Fail($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
                return AntiFakeResult.Fail($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
            }
        }

        private string BuildUrl(string qrcode)
        {
            var baseurl = _configuration.GetSection("QueryQRCode").Value;
            var num = qrcode;
            var code = qrcode.Substring(0, 4);
            var query_type = "手机网络";
            var username = "wt";
            var ip = "13800000000";

            return $"{baseurl}?lang=cn&num={num}&code={code}&query_type={query_type}&username={username}&ip={ip}";
        }

        /// <summary>
        /// 查询防伪信息
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        //public async Task<AntiFakeResult> QueryTag(string qrcode)
        //{
        //    var baseurl = _configuration.GetSection("QueryQRCode").Value;
        //    var num = qrcode;
        //    var code = qrcode.Substring(0, 4); ;
        //    var query_type = "手机网络";
        //    var username = "wt";
        //    var ip = "13800000000";//电话？
        //    string url = $"{baseurl}?lang=cn&num={num}&code={code}&query_type=手机网络&username={username}&ip={ip}";

        //    _logger.LogInformation($"防伪查询URL：{url}");
        //    var client = _clientFactory.CreateClient();

        //    try
        //    {
        //        var response = await client.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            // 对 content 进行处理，例如反序列化为一个对象
        //            // 将结果返回给客户端，或者执行其他操作
        //            _logger.LogInformation($"QueryTag防伪查询结果：{content}");
        //            var result = JsonConvert.DeserializeObject<SearchByCode>(content);

        //            return AntiFakeResult.Success(result);
        //        }
        //        else
        //        {
        //            // 如果请求失败，返回失败的状态码和错误信息
        //            return AntiFakeResult.Fail($"请求返回失败代码：{response.StatusCode}");
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        // 捕获由 HttpClient 发出的请求时引发的异常
        //        return AntiFakeResult.Fail($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        // 捕获其他可能出现的异常
        //        return AntiFakeResult.Fail($"Code:{StatusCodes.Status500InternalServerError}:Error: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// 查询溯源信息
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<TraceabilityResult> GetWLabelStorageAsync(string qrcode)
        {
            try
            {
                var wqrcode = await _labelStorageRepository.GetWLabelStorageAsync(qrcode).ConfigureAwait(false);

                if (wqrcode is not null)
                {
                    var outqrcode = await _agentRepository.FindSingleAgentAsync(wqrcode.Dealers.Trim()).ConfigureAwait(false);

                    return new TraceabilityResult
                    {
                        Status = true,
                        AgentName = outqrcode.AName,
                        OrderNumbels = wqrcode.OrderNumbels,
                        OutTime = wqrcode.OutTime,
                        QRCode = wqrcode.QRCode
                    };
                }
                else
                {
                    return new TraceabilityResult
                    {
                        Status = false,
                        Msg = "标签还未出库"
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"查询溯源信息出现异常：{e.Message}");
                return new TraceabilityResult();
            }
        }

        //public async Task<TraceabilityResult> GetWLabelStorageAsync(string qrcode)
        //{
        //    try
        //    {
        //        var wqrcode = await _labelStorageRepository.GetWLabelStorageAsync(qrcode);

        //        if (wqrcode is not null)
        //        {
        //            var outqrcode = await _agentRepository.FindSingleAgentAsync(wqrcode.Dealers.Trim());

        //            return new TraceabilityResult
        //            {
        //                Status = true,
        //                AgentName = outqrcode.AName.Trim(),
        //                OrderNumbels = wqrcode.OrderNumbels.Trim(),
        //                OutTime = wqrcode.OutTime,
        //                QRCode = wqrcode.QRCode
        //            };
        //        }
        //        else
        //        {
        //            return new TraceabilityResult
        //            {
        //                Status = false,
        //                Msg = "标签还未出库"
        //            };
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"查询溯源信息出现异常：{e.Message}");
        //        return new TraceabilityResult();
        //    }
        //}

        //
        /// <summary>
        /// 返回订单号
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<string> GetOrderNumbelsAsync(string qrcode)
        {
            try
            {
                var labelStorage = await _labelStorageRepository.GetWLabelStorageAsync(qrcode);
                if (labelStorage != null)
                    return labelStorage.OrderNumbels.Trim();

                _logger.LogWarning($"未找到与二维码 {qrcode} 相关的标签存储信息。");
                return "";
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"查询溯源信息出现异常：{e.Message}");
                return "";
            }
        }

        //public async Task<TraceabilityResult> GetWLabelStorageAsync(string qrcode)
        //{
        //    try
        //    {
        //        // OutTime,s.OrderNumbels,s.QRCode
        //        //var wqrcode = await _labelStorageRepository.GetWLabelStorageAsync(qrcode);
        //        var wqrcode = await _labelStorageRepository.GetWLabelStorageAsync(qrcode);
        //        if (wqrcode is not null)
        //        {
        //            // 使用这些属性执行其他操作
        //            var outqrcode = await _agentRepository.FindSingleAgentAsync(wqrcode.Dealers.Trim());
        //            return new TraceabilityResult()
        //            {
        //                Status = true,
        //                AgentName = outqrcode.AName.Trim(),
        //                OrderNumbels = wqrcode.OrderNumbels.Trim(),
        //                OutTime = wqrcode.OutTime,
        //                QRCode = wqrcode.QRCode
        //            };
        //        }
        //        else
        //        {
        //            return new TraceabilityResult()
        //            {
        //                Status = false,
        //                Msg = "标签还未出库"
        //            };
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"查询溯源信息出现异常：{e.Message}");
        //        return new TraceabilityResult();
        //    }
        //}
    }
}