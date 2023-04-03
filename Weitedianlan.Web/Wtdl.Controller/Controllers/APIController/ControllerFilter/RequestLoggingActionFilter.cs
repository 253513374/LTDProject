using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Wtdl.Controller.Controllers.APIController
{
    public class RequestLoggingActionFilter : IActionFilter
    {
        private readonly ILogger _logger;
        private Stopwatch _timer;

        public RequestLoggingActionFilter(ILogger<RequestLoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = new Stopwatch();
            _timer.Start();

            var request = context.HttpContext.Request;
            var method = request.Method;
            var path = request.GetDisplayUrl();
            var clientIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var userName = context.HttpContext.User.Identity.Name;

            using (_logger.BeginScope($"Request: {method} {path} from IP {clientIp} User {userName}", method, path,
                       clientIp, userName))
            {
                _logger.LogInformation($"开始请求: {method} {path} from IP {clientIp} User {userName}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            var elapsedTime = _timer.Elapsed.TotalMilliseconds;

            var request = context.HttpContext.Request;
            var method = request.Method;
            var path = request.GetDisplayUrl();
            var userName = context.HttpContext.User.Identity.Name;

            using (_logger.BeginScope($"Request: {method} {path} User {userName}", method, path, userName))
            {
                if (context.Exception != null)
                {
                    _logger.LogError(context.Exception,
                        $"请求时出现异常: {method} {path} User {userName}");
                }
                else
                {
                    _logger.LogInformation($"完成请求: {method} {path}   耗时： {elapsedTime} 毫秒");
                }
            }
        }
    }
}