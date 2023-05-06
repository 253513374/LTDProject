using System;
using System.Globalization;
using System.Windows.Data;

namespace ScanCode.WinUI
{
    [ValueConversion(typeof(string), typeof(string))]
    internal class DateTimeformatConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value as string;

            if (type.Trim() != "") return type.Replace("/", "-"); ;
            //if (type.Trim() == "扎") return "电线";
            return "";
            // throw new NotImplementedException();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value as string;

            if (type.Trim() != "") return type.Replace("/", "-"); ;
            //if (type.Trim() == "扎") return "电线";
            return "";
            //throw new NotImplementedException();
        }
    }
}