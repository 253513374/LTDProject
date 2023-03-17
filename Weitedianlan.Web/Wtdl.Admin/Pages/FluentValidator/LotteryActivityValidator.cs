using FluentValidation;
using Wtdl.Model.Entity;

namespace Wtdl.Admin.Pages.FluentValidator
{
    public class LotteryActivityValidator : AbstractValidator<LotteryActivity>
    {
        public LotteryActivityValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("活动名称不能为空");
            RuleFor(m => m.StartTime).NotEmpty().WithMessage("开始时间不能为空");
            RuleFor(m => m.EndTime).NotEmpty().WithMessage("结束时间不能为空");
            RuleFor(m => m.IsActive).NotEmpty().WithMessage("奖品总数不能为空");
            //RuleFor(m => m.TotalChance).NotEmpty().WithMessage("总抽奖次数不能为空");
            //RuleFor(m => m.DailyChance).NotEmpty().WithMessage("每日抽奖次数不能为空");
            //RuleFor(m => m.PrizeAmount).GreaterThanOrEqualTo(1).WithMessage("奖品总数不能小于1");
            //RuleFor(m => m.TotalChance).GreaterThanOrEqualTo(1).WithMessage("总抽奖次数不能小于1");
            //RuleFor(m => m.DailyChance).GreaterThanOrEqualTo(1).WithMessage("每日抽奖次数不能小于1");
            RuleFor(m => m.StartTime).LessThan(m => m.EndTime).WithMessage("开始时间不能大于结束时间");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<LotteryActivity>.CreateWithOptions((LotteryActivity)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}