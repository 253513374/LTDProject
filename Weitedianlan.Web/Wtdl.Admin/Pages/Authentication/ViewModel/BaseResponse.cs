using System.Security.Claims;

namespace Wtdl.Admin.Pages.Authentication.ViewModel
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BaseResponse<T> Success(T value)
        {
            return new BaseResponse<T>
            {
                IsSuccess = true,
                Message = "Success",

                Data = value
            };
        }

        ///返回失败信息
        public static BaseResponse<T> Fail(string message)
        {
            return new BaseResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }

        //internal static List<Claim> Success(IList<Claim> roles)
        //{
        //    return new BaseResponse<T>
        //    {
        //        IsSuccess = false,
        //        Message = "Success",
        //        Data = roles
        //    };
        //}
    }
}