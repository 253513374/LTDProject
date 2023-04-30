using System.Collections.Generic;

namespace Weitedianlan.model.Response
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            data = new List<dynamic>();
        }

        public int code { get; set; }
        public string result { get; set; }
        public dynamic data { get; set; }

        ///返回成功数据
        public static ResponseModel Success(dynamic data)
        {
            return new ResponseModel()
            {
                code = 200,
                result = "success",
                data = data
            };
        }

        public static ResponseModel Success(dynamic data, string result)
        {
            return new ResponseModel()
            {
                code = 200,
                result = result,
                data = data
            };
        }

        public static ResponseModel Fail(dynamic data, string result)
        {
            return new ResponseModel()
            {
                code = 500,
                result = result,
                data = data
            };
        }

        public static ResponseModel Fail(string errorMsg)
        {
            return new ResponseModel()
            {
                code = 500,
                result = errorMsg,
            };
        }
    }
}