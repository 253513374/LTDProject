using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ScanCode.WPF.Converter
{
    public class ErrorInfoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorInfo = value as string;  // 或者其它你需要的数据类型
            if (errorInfo == null)
                return Brushes.Black;  // 未设置ErrorInfo时的默认颜色

            // 根据ErrorInfo的值来决定颜色
            if (errorInfo.Contains("出库成功") || errorInfo.Contains("退货成功"))
                return Brushes.Green;
            else
                return "#F9D142";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}