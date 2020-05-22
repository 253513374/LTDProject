using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PPK_Logistics.Verify
{
    [ValueConversion(typeof(string), typeof(string))]
    class SWLXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string  type = value as string;

            if(type.Trim() =="") return "电缆";
            if (type.Trim() == "扎") return "电线";
            return "";
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
