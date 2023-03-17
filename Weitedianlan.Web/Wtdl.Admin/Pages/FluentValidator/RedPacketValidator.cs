using FluentValidation;
using Wtdl.Model.Entity;

using Wtdl.Model.Enum;

namespace Wtdl.Admin.Pages.FluentValidator
{
    public class RedPacketValidator : AbstractValidator<ScanRedPacket>
    {
        public RedPacketValidator()
        {
            RuleFor(x => x.CashValue).GreaterThanOrEqualTo(1)
                .WithMessage("金额不能为0元")
                .When(w => w.RedPacketType == RedPacketType.AVERAGE);

            RuleFor(x => x.CashValue).LessThanOrEqualTo(20000)
                .WithMessage("固定金额必须小于200元")
                .When(w => w.RedPacketType == RedPacketType.AVERAGE);

            RuleFor(x => x.MinCashValue).GreaterThanOrEqualTo(1)
                .WithMessage("最小金额不能为0 元")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MinCashValue).LessThan(l => l.MaxCashValue)
                .WithMessage("最小金额不能大于或等于最大金额")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MaxCashValue).GreaterThanOrEqualTo(1)
                .WithMessage("最大金额不能为 0 元")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MaxCashValue).LessThanOrEqualTo(20000)
                .WithMessage("最大金额不能大于200元")
                .When(w => w.RedPacketType == RedPacketType.RANDOM);

            RuleFor(x => x.MaxCashValue).GreaterThan(g => g.MinCashValue)
                .WithMessage("最大金额不能小于最小金额")
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