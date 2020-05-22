using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace PPK_Logistics.Verify
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StackDateConverter : IValueConverter
    {
        private string id = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            id = value as string;

            App.ProductListView.Filter += ProductListView_Filter;

            IEnumerator _item = App.ProductListView.View.GetEnumerator();
            _item.MoveNext();
            if (_item != null)
            {
                return ((Product)_item.Current).PRODUCTNUMBER;
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

        private void ProductListView_Filter(object sender, FilterEventArgs e)
        {
            Product _Product = e.Item as Product;
            if (_Product != null)
            {
                e.Accepted = (_Product.ID.ToString().ToLower().Trim() == id);
            }
            //throw new NotImplementedException();
        }
    }
}