using FluentValidation;
using Wtdl.Admin.Data;

namespace Wtdl.Admin.Pages.FluentValidator
{
    public class FileUploadFluentValidator : AbstractValidator<FileUploadFluent>
    {
        public FileUploadFluentValidator()
        {
            // RuleFor(x => x.PrizeName).NotEmpty().WithMessage("奖品名称不能为空");
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("文件不能为空");

            When(x => x.File != null,
                () =>
                {
                    RuleFor(x => x.File.Size).LessThanOrEqualTo(500000).WithMessage("上传的文件大小不能超过500kb");
                    RuleFor(x => x.File.ContentType).Must(x => x == "image/jpeg" || x == "image/png").WithMessage("请上传格式为jpg与png格式图片");
                });
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<FileUploadFluent>.CreateWithOptions((FileUploadFluent)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}