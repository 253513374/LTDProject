using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Wtdl.Controller.Controllers.APIController;
using Wtdl.Model.ResponseModel;

public abstract class BaseController<T> : ControllerBase
{
    protected readonly ILogger<T> _logger;

    public BaseController(ILogger<T> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 返回成功状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    protected ApiResponse<T> Success<T>(string message, T data, object metadata = null)
    {
        return new ApiResponse<T>(StatusCodes.Status200OK, message, data, metadata);
    }

    /// <summary>
    /// 返回失败信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <param name="errorDetails"></param>
    /// <returns></returns>
    protected ApiResponse<T> Failure<T>(int statusCode, string message, List<string> errorDetails = null)
    {
        return new ApiResponse<T>(statusCode, message, default(T), null, errorDetails);
    }
}