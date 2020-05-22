using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Weitedianlan.WinUI
{
    [ValueConversion(typeof(string), typeof(string))]
    class DateTimeformatConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value as string;

            if (type.Trim() != "")  return type.Replace("/", "-"); ;
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
