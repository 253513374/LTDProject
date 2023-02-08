using FluentValidation;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Enum;

namespace Wtdl.Admin.Pages.FluentValidator
{
    public class RedPacketValidator : AbstractValidator<ScanRedPacket>
    {
        public RedPacketValidator()
        {
            RuleFor(x => x.CashValue).GreaterThan(0)
                .WithMessage("金额必须填写")
                .When(w => w.RedPacketType == RedPacketType.AVERAGE);

            RuleFor(x => x.CashValue).LessThan(50)
                .WithMessage("固定金额必须小于50")
                .When(w => w.RedPacketType == RedPacketType.AVERAGE);

            RuleFor(x => x.MinCashValue).GreaterThan(0)
                .WithMessage("最小金额必须大于0")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MinCashValue).LessThan(l => l.MaxCashValue)
                .WithMessage("最小金额必须小于最大金额")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MaxCashValue).GreaterThan(0)
                .WithMessage("最大金额必须大于0")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);
            RuleFor(x => x.MaxCashValue).LessThan(50)
                .WithMessage("最大金额必须小于50")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);
            RuleFor(x => x.MaxCashValue).GreaterThan(g => g.MinCashValue)
                .WithMessage("最大金额必须大于最小金额")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ScanRedPacket>.CreateWithOptions((ScanRedPacket)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}