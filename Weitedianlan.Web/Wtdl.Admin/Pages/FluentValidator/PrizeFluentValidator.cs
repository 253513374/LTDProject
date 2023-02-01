using FluentValidation;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Enum;

namespace Wtdl.Admin.Pages.FluentValidator
{
    public class PrizeFluentValidator : AbstractValidator<Prize>
    {
        public PrizeFluentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("奖品名称不能为空");
            RuleFor(x => x.Name).Length(1, 20).WithMessage("奖品名称长度不能超过20个字符");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("奖品数量不能为空");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("奖品数量必须大于0");
            RuleFor(x => x.Amount).LessThan(100000).WithMessage("奖品数量必须小于100000");
            RuleFor(x => x.Probability).NotEmpty().WithMessage("奖品中奖概率不能为空");
            RuleFor(x => x.Probability).GreaterThan(0).WithMessage("奖品中奖概率必须大于0");
            RuleFor(x => x.Probability).LessThan(100).WithMessage("奖品中奖概率必须小于100");

            RuleFor(x => x.ImageUrl).NotNull().WithMessage("图片不能为空");

            RuleFor(x => x.CashValue).GreaterThan(0)
                .WithMessage("金额必须填写")
                .When(w => w.Type == PrizeType.Cash);
            RuleFor(x => x.CashValue).LessThan(50)
                .WithMessage("固定金额必须小于50")
                .When(w => w.Type == PrizeType.Cash);

            RuleFor(x => x.MinCashValue).GreaterThan(0)
                .WithMessage("最小金额必须大于0")
                .When(w => w.Type == PrizeType.Cash);
            RuleFor(x => x.MinCashValue).LessThan(l => l.MaxCashValue)
                .WithMessage("最小金额必须小于最大金额")
                .When(w => w.Type == PrizeType.Cash);

            RuleFor(x => x.MaxCashValue).GreaterThan(0)
                .WithMessage("最大金额必须大于0")
                .When(w => w.Type == PrizeType.Cash);
            RuleFor(x => x.MaxCashValue).LessThan(50)
                .WithMessage("最大金额必须小于50")
                .When(w => w.Type == PrizeType.Cash);
            RuleFor(x => x.MaxCashValue).GreaterThan(g => g.MinCashValue)
                .WithMessage("最大金额必须大于最小金额")
                .When(w => w.Type == PrizeType.Cash);
        }

        //private async Task<bool> IsUniqueAsync(string email)
        //{
        //    // 模拟长时间运行的 http 调用
        //    await Task.Delay(2000);
        //    return email.ToLower() != "test@test.com";
        //}

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Prize>.CreateWithOptions((Prize)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}