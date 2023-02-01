using MudBlazor;

namespace Wtdl.Admin.Utility
{
    public class StringToBoolConverter : BoolConverter<string>
    {
        public StringToBoolConverter()
        {
            SetFunc = OnSet;
            GetFunc = OnGet;
        }

        private string TrueString = "启用";
        private string FalseString = "禁用";
        private string NullString = "不确定";

        private string OnGet(bool? value)
        {
            try
            {
                return (value == true) ? TrueString : FalseString;
            }
            catch (Exception e)
            {
                UpdateGetError("Conversion error: " + e.Message);
                return NullString;
            }
        }

        private bool? OnSet(string arg)
        {
            if (arg == null)
                return null;
            try
            {
                if (arg == TrueString)
                    return true;
                if (arg == FalseString)
                    return false;
                else
                    return null;
            }
            catch (FormatException e)
            {
                UpdateSetError("Conversion error: " + e.Message);
                return null;
            }
        }
    }
}