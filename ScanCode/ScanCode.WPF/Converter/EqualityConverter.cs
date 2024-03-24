using System;
using System.Globalization;
using System.Windows.Data;

namespace ScanCode.WPF.Converter
{
    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int actualOutCount && values[1] is int compareTo)
            {
                return actualOutCount == compareTo;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}