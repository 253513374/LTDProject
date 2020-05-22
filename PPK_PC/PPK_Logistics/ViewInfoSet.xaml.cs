using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PPK_Logistics.DataSource;

namespace PPK_Logistics
{
    /// <summary>
    /// ViewInfoSet.xaml 的交互逻辑
    /// </summary>
    public partial class ViewInfoSet : Window
    {
        private Checkin_ltjy Checkin_ltjySet = null;

        private int Idx = -1;
        private int Stackidx = -1;

        public ViewInfoSet(Checkin_ltjy checkin_ltjy, int idx, int stackidx)
        {
            InitializeComponent();
            Checkin_ltjySet = checkin_ltjy;
            Idx = idx;
            Stackidx = stackidx;
        }

        private void DeleteBut_Click(object sender, RoutedEventArgs e)
        {
            if (App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE.Count > 0)
            {
                App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE.RemoveAt(Idx);
                App.ListCheckin_ltjyStack[Stackidx].PACKDATACOUNT = App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE.Count;
                MessageBox.Show("当前选择箱数据删除成功");

                this.Close();
            }
        }

        private void PCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void PNumBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.ProductListView.Filter += ProductListViewNum_Filter;
        }

        private void ProductListViewID_Filter(object sender, FilterEventArgs e)
        {
            Product _Product = e.Item as Product;
            if (_Product == null)
            {
                return;
            }
            else
            {
                //if (SearchNum == "") return;
                e.Accepted = (_Product.ID.ToString().ToLower().Trim() == Checkin_ltjySet.productid);
                // return _Product;
            }
        }

        private void ProductListViewNum_Filter(object sender, FilterEventArgs e)
        {
            Product _Product = e.Item as Product;
            if (_Product == null)
            {
                return;
            }
            else
            {
                //if (SearchNum == "") return;
                e.Accepted = (_Product.PRODUCTNUMBER.ToString().ToLower().Trim() == PNumBox.Text.ToLower().Trim());
                // return _Product;
            }
        }

        private void UpdetaBut_Click(object sender, RoutedEventArgs e)
        {
            App.ProductListView.Filter += ProductListViewNum_Filter;
            IEnumerator item = App.ProductListView.View.GetEnumerator();
            item.MoveNext();

            if (App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE.Count > 0)
            {
                if (PCodeBox.Text.Trim().Length == 12 && !App.ProductListView.View.IsEmpty)
                {
                    if (App.ProductListView.View.CanFilter)
                    {
                        App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE[Idx].barcode = PCodeBox.Text.Trim();
                        App.ListCheckin_ltjyStack[Stackidx].LISTPD_SIMPLE[Idx].productid = ((Product)item.Current).ID.ToString().Trim();

                        MessageBox.Show("数据更新成功", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("请输入正确的商品码或者标签号");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.ProductListView.Filter += ProductListViewID_Filter;

            if (App.ProductListView.View.CanFilter)
            {
                IEnumerator item = App.ProductListView.View.GetEnumerator();
                item.MoveNext();
                PNumLabel.Content = ((Product)item.Current).PRODUCTNUMBER.Trim();// Checkin_ltjySet.productid;

                PNameLabel.Content = ((Product)item.Current).PRODUCTNAME.Trim();//Checkin_ltjySet.productid;

                PCodeLabel.Content = Checkin_ltjySet.barcode;
            }
        }
    }
}