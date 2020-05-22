using System.Windows.Controls;

namespace PPK_Logistics
{
    public class NotNullValidationRule : ValidationRule
    {
        public NotNullValidationRule()
        {
            this.ValidatesOnTargetUpdated = true;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value as string) || string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "不能为空！");
            }
            return ValidationResult.ValidResult;
        }
    }
}