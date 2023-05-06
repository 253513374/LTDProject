using Microsoft.AspNetCore.Mvc;
using ScanCode.Model.ResponseModel;

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
    protected ApiResponse<T> Success<T>(T data, string message = "Success", object metadata = null)
    {
        var sss = data.ToString();
        _logger.LogInformation($"{typeof(T)}:Success   Data:{data.ToString()}");
        return ApiResponse<T>.Success(data, message);
    }

    /// <summary>
    /// 返回失败信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <param name="errorDetails"></param>
    /// <returns></returns>
    protected ApiResponse<T> Failure<T>(string message, T data = default, List<string> errorDetails = null)
    {
        _logger.LogInformation($"{typeof(T)} Failure:{message}");

        return ApiResponse<T>.Failure(data, message, errorDetails);
    }

    protected ApiResponse<List<TItem>> SuccessList<TItem>(List<TItem> dataList)
    {
        return ApiResponse<List<TItem>>.SuccessList<TItem>(dataList);
    }

    protected ApiResponse<List<TItem>> FailureList<TItem>(string message, List<TItem> data = default, List<string> errorDetails = null)
    {
        return ApiResponse<List<TItem>>.Failure(data, message, errorDetails);
    }
}