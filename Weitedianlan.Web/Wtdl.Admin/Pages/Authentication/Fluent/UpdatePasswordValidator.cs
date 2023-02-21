using FluentValidation;
using Wtdl.Admin.Pages.Authentication.ViewModel;

namespace Wtdl.Admin.Pages.Authentication.Fluent
{
    /// <summary>
    /// 验证注册类
    /// </summary>
    public class UpdatePasswordValidator : AbstractValidator<UpdatePassword>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密码不能为空");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("确认密码不能为空")
                .Equal(x => x.Password).WithMessage("两次密码输入不一致");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("密码不能为空");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UpdatePassword>.CreateWithOptions((UpdatePassword)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}