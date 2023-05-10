using Microsoft.AspNetCore.Mvc;
using ScanCode.Model.ResponseModel;
using ScanCode.Mvc.Services;
using ScanCode.RedisCache;
using ScanCode.Repository;
using Senparc.CO2NET.Extensions;

namespace ScanCode.Controller.Controllers.APIController
{
    [Route("[controller]")]
    [ApiController]
    public class TraceabilityController : BaseController<TraceabilityController>
    {
        private readonly SearchByCodeService _searchByCodeService;

        private readonly BdxOrderRepository _bdxOrderRepository;

        private ILogger<TraceabilityController> _logger;
        private readonly IRedisCache _redisCache;

        public TraceabilityController(SearchByCodeService codeService,
            IRedisCache redisCache,
            BdxOrderRepository bdxOrderRepository,
            ILogger<TraceabilityController> logger)
            : base(logger)
        {
            _bdxOrderRepository = bdxOrderRepository;
            _redisCache = redisCache;
            _searchByCodeService = codeService;
            _logger = logger;
        }

        /// <summary>
        /// 返回查询标签溯源信息
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>
        /// </returns>
        //[ResponseCache(Duration = 180, VaryByQueryKeys = new string[] { "qrcode" })]
        [AcceptVerbs("GET", "POST")]
        public async Task<ApiResponse<TraceabilityResult>> Get(string qrcode)
        {
            // your function code here
            if (string.IsNullOrEmpty(qrcode))
            {
                return Failure<TraceabilityResult>("查询标签序号不能为空");
                // return new TraceabilityResult() { Status = false, Msg = "查询标签序号不能为空" };
            }

            qrcode = qrcode.Trim();
            var result = await _searchByCodeService.GetWLabelStorageAsync(qrcode);
            _logger.LogInformation($"查询标签序号：{qrcode}，结果：{result.ToJson()}");

            if (!result.Status)
            {
                return Failure<TraceabilityResult>(result.Msg);
            }

            //var erpresult = await _bdxOrderRepository.GetSingleAsync(result.OrderNumbels);
            //_logger.LogInformation($"erp: {erpresult.ToJson()}");

            //if (erpresult is not null)
            //{
            //   // _redisCache.
            //}

            _ = await _redisCache.SetBitAsync(qrcode);

            return Success(result);
        }
    }
}