using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Model.ResponseModel
{
    public enum ApiResponseCodes
    {
        // 2xx Success
        Success = 200, // 请求已成功处理

        Created = 201, // 请求已成功处理，并创建了新的资源
        Accepted = 202, // 请求已接受，但尚未处理完成
        NoContent = 204, // 请求已成功处理，但没有实体内容返回

        // 4xx Client errors
        BadRequest = 400, // 客户端请求错误，服务器无法理解或无法处理

        Unauthorized = 401, // 请求需要用户身份验证
        PaymentRequired = 402, // 保留，以后使用
        Forbidden = 403, // 服务器理解请求，但拒绝执行
        NotFound = 404, // 请求的资源在服务器上未找到
        MethodNotAllowed = 405, // 请求方法不允许
        NotAcceptable = 406, // 服务器无法生成用户请求所需的响应
        Conflict = 409, // 请求与服务器上的资源存在冲突
        Gone = 410, // 请求的资源已从服务器上永久删除
        UnprocessableEntity = 422, // 请求格式正确，但包含无效字段

        // 5xx Server errors
        InternalServerError = 500, // 服务器内部错误

        NotImplemented = 501, // 服务器不支持实现请求所需的功能
        BadGateway = 502, // 作为代理或网关使用的服务器从上游服务器接收到的响应无效或错误
        ServiceUnavailable = 503, // 由于临时过载或维护，服务器无法处理请求
        GatewayTimeout = 504 // 作为代理或网关使用的服务器未及时从上游服务器接收请求
    }
}