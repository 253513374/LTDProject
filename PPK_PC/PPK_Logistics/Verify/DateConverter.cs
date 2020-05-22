using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace PPK_Logistics.Verify
{
    [ValueConversion(typeof(string), typeof(string))]
    public class DateConverter : IValueConverter
    {
        private string id = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            id = value as string;

            App.ProductListView.Filter += _Product_Filter;

            IEnumerator _item = App.ProductListView.View.GetEnumerator();
            _item.MoveNext();
            if (_item != null)
            {
                return ((Product)_item.Current).PRODUCTNAME;
            }
            else
            {
                return id;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            return value;
        }

        private void _Product_Filter(object sender, FilterEventArgs e)
        {
            Product _Product = e.Item as Product;
            if (_Product != null)
            {
                e.Accepted = (_Product.ID.ToString().ToLower().Trim() == id);
            }
        }
    }
}