using FluentValidation;
using Wtdl.Admin.Pages.Authentication.ViewModel;

namespace Wtdl.Admin.Pages.Authentication.Fluent
{
    public class FluentValidationValidator : AbstractValidator<LoginModel>
    {
        public FluentValidationValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("用户名必须填写.");
            RuleFor(m => m.Password).NotEmpty().WithMessage("密码不能为空.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<LoginModel>.CreateWithOptions((LoginModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}