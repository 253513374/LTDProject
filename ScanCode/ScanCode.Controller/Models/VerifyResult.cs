namespace ScanCode.Mvc.Models;

/// <summary>
/// 红包发放资格校验结果
/// </summary>
public record VerifyResult
{
    /// <summary>
    /// 校验是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }
}