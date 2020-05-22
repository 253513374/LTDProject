using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PPK_Logistics.Verify
{
    [ValueConversion(typeof(string), typeof(string))]
    internal class OutOrderTypeConverter : IValueConverter
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    var str = value as string;
        //    if (str == "") return str;
        //    if (str.ToUpper() == "DBCK")
        //    {
        //        return str + "|调拨出库";
        //    }
        //    else {
        //        return str + "|销售出库";
        //    }
        //    //throw new NotImplementedException();
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return value;
        //    //throw new NotImplementedException();
        //}
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == "") return str;
            if (str.ToUpper() == "DBCK")
            {
                return str + "|调拨出库";
            }
            else {
                return str + "|销售出库";
            }
            //throw new NotImplementedException();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}