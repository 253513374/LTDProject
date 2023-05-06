using System.Security.Claims;

namespace ScanCode.Web.Admin.Authenticated.IdentityModel;

/// <summary>
/// 登录结果
/// </summary>
public class SignInWResult
{
    public string UserIdentifier { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// 错误描述
    /// </summary>
    public string ErrorDescription { get; set; }

    public List<Claim> Claims { get; set; }

    public SignInWResult()
    {
        Claims = new List<Claim>();
    }

    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <returns></returns>
    public static SignInWResult Success(List<Claim> claims = null, string userid = "")
    {
        return new SignInWResult()
        {
            Claims = claims,
            Succeeded = true,
            UserIdentifier = userid
        };
    }

    /// <summary>
    /// 创建失败结果
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <param name="errorDescription">错误描述</param>
    /// <returns></returns>
    public static SignInWResult Failure(string error, string errorDescription = null)
    {
        return new SignInWResult
        {
            Succeeded = false,
            Error = error,
            ErrorDescription = errorDescription
        };
    }
}