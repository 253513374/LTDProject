using Wtdl.Model.ResponseModel;

namespace Wtdl.Controller.Controllers.APIController
{
    public class ApiResponse<T>
    {
        /// <summary>
        ///
        /// </summary>
        public bool IsSuccess => StatusCode == StatusCodes.Status200OK || StatusCode == StatusCodes.Status201Created;

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Metadata { get; set; }
        public List<string> ErrorDetails { get; set; }

        public ApiResponse(int statusCode, string message, T data = default, object metadata = null, List<string> errorDetails = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Metadata = metadata;
            ErrorDetails = errorDetails;
        }

        public static ApiResponse<T> Ok(T data, object metadata = null)
        {
            return new ApiResponse<T>(StatusCodes.Status200OK, "Success", data, metadata);
        }

        public static ApiResponse<T> Created(string message, T data, object metadata = null)
        {
            return new ApiResponse<T>(StatusCodes.Status201Created, message, data, metadata);
        }

        public static ApiResponse<T> Success(int statusCode, string message, T data, object metadata = null)
        {
            return new ApiResponse<T>(statusCode, message, data, metadata);
        }

        public static ApiResponse<T> Failure(int statusCode, string message, List<string> errorDetails = null)
        {
            return new ApiResponse<T>(statusCode, message, default(T), null, errorDetails);
        }
    }
}