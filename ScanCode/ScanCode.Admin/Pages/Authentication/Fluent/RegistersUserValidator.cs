using FluentValidation;
using ScanCode.Web.Admin.Pages.Authentication.ViewModel;

namespace ScanCode.Web.Admin.Pages.Authentication.Fluent
{
    /// <summary>
    /// 验证注册类
    /// </summary>
    public class RegistersUserValidator : AbstractValidator<RegistersUser>
    {
        public RegistersUserValidator()
        {
            RuleFor(r => r.Email).NotEmpty()
                .NotEmpty().WithMessage("Email地址不能为空")
                .EmailAddress().WithMessage("请输入有效的Email地址");

            RuleFor(r => r.PhoneNumber).NotEmpty().WithMessage("手机号不能为空")
                .Matches(@"^1[3456789]\d{9}$").WithMessage("手机号格式不正确");

            RuleFor(r => r.UserName).NotEmpty().WithMessage("用户名不能为空");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密码不能为空");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("确认密码不能为空")
                .Equal(x => x.ConfirmPassword).WithMessage("两次密码输入不一致");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<RegistersUser>.CreateWithOptions((RegistersUser)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}