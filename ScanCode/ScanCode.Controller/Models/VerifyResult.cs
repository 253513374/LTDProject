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

    public static readonly VerifyResult PrizeNotExist = new VerifyResult { IsSuccess = false, Message = "抽奖奖品不存在" };
    public static readonly VerifyResult ActivityNotActive = new VerifyResult { IsSuccess = false, Message = "当前活动未激活" };
    public static readonly VerifyResult ActivityNotStartedOrFinished = new VerifyResult { IsSuccess = false, Message = "当前活动未开始或已结束" };
    public static readonly VerifyResult AlreadyParticipated = new VerifyResult { IsSuccess = false, Message = "当前用户已经对标签序号抽过奖了" };
    public static readonly VerifyResult CanParticipate = new VerifyResult { IsSuccess = true, Message = "可以参与抽奖活动" };
}