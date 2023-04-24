using System.Collections.Generic;

using Wtdl.Model.ResponseModel;

namespace Wtdl.Model.ResponseModel
{
    public class ApiResponse<T>
    {
        /// <summary>
        ///
        /// </summary>
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Metadata { get; set; }
        public List<string> ErrorDetails { get; set; }

        public ApiResponse()
        {
            ErrorDetails = new List<string>();
        }

        public static ApiResponse<T> Success(T data, string message = "Success", object metadata = null)
        {
            return new ApiResponse<T>()
            {
                IsSuccess = true,
                StatusCode = (int)ApiResponseCodes.Success,
                Message = message,
                Data = data,
                Metadata = metadata,
            };
        }

        public static ApiResponse<T> Failure(T data = default, string message = "Failure", List<string> errorDetails = null)
        {
            return new ApiResponse<T>()
            {
                IsSuccess = false,
                StatusCode = (int)ApiResponseCodes.NotFound,
                Message = message,
                ErrorDetails = errorDetails,
                Metadata = default,
                Data = data,
            };
        }

        public static ApiResponse<List<TItem>> SuccessList<TItem>(List<TItem> dataList, string message = "SuccessList", object metadata = null)
        {
            return new ApiResponse<List<TItem>>
            {
                IsSuccess = true,
                StatusCode = (int)ApiResponseCodes.Success,
                Message = message,
                Data = dataList,
                Metadata = metadata,
            };
        }

        public static ApiResponse<List<TItem>> FailureList<TItem>(string message, List<string> errorDetails = null)
        {
            return new ApiResponse<List<TItem>>
            {
                IsSuccess = false,
                StatusCode = (int)ApiResponseCodes.NotFound,
                Message = message,
                ErrorDetails = errorDetails,
                Metadata = default,
                Data = default(List<TItem>),
            };
        }
    }
}