using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using PPK_Logistics.Config;
using PPK_Logistics.DataSource;

namespace PPK_Logistics
{
    /// <summary>
    /// View.xaml 的交互逻辑
    /// </summary>
    public partial class ViewInfo : Window
    {
        private Checkin_ltjyStack _Checkin_ltjyD = new Checkin_ltjyStack();

        /// <summary>
        /// 当前堆垛的检索ID
        /// </summary>
        private int Idx = -1;

        private Task<Checkin_ltjyStack> SendTask;

        public ViewInfo(Checkin_ltjyStack p, int idx)
        {
            this._Checkin_ltjyD = p;
            Idx = idx;
            InitializeComponent();
        }

        private delegate void ThreadDelegate(int index);

        public void TaskEndedByCatchD(Task<Checkin_ltjyStack> task)
        {
            if (task.IsCompleted)
            {
                Checkin_ltjyStack TaskResult = task.Result;

                if (TaskResult.Status.Contains("网络超时")) return;

                if (TaskResult.Status.Contains("成功"))
                {
                    IEnumerator ienumerator = App.ListCheckin_ltjyStack.GetEnumerator();
                    int count = 0;
                    while (ienumerator.MoveNext())
                    {
                        if (((Checkin_ltjyStack)ienumerator.Current).CRIB == TaskResult.CRIB)
                        {
                            App.ListCheckin_ltjyStack[count].Status = TaskResult.Status;
                            App.ListCheckin_ltjyStack[count].LISTPD_SIMPLE.ForEach(Ltjy => Ltjy.status = "同步成功");
                            return;
                        }
                        count++;
                    }
                }
                else
                {
                    IEnumerator ienumerator = App.ListCheckin_ltjyStack.GetEnumerator();
                    int ecount = 0;
                    while (ienumerator.MoveNext())
                    {
                        Checkin_ltjyStack _Checkin_ltjyStack = (Checkin_ltjyStack)ienumerator.Current;

                        if (_Checkin_ltjyStack != null)
                        {
                            if (_Checkin_ltjyStack.CRIB == TaskResult.CRIB)
                            {
                                App.ListCheckin_ltjyStack[ecount].CRIB = TaskResult.CRIB;
                                App.ListCheckin_ltjyStack[ecount].Status = TaskResult.Status;
                                App.ListCheckin_ltjyStack[ecount].LISTPD_SIMPLE.Clear();
                                App.ListCheckin_ltjyStack[ecount].LISTPD_SIMPLE.AddRange(TaskResult.LISTPD_SIMPLE);
                                return;
                            }
                        }
                        ecount++;
                    }
                }
            }
        }

        private void Listviewsimple_View_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Checkin_ltjy _Checkin_ltjy = this.Listviewsimple_View.SelectedItem as Checkin_ltjy;

            if (_Checkin_ltjy != null)
            {
                ViewInfoSet _ViewInfoSet = new ViewInfoSet(_Checkin_ltjy, this.Listviewsimple_View.SelectedIndex, Idx);
                _ViewInfoSet.Topmost = true;
                _ViewInfoSet.ShowDialog();
            }
        }

        private Checkin_ltjyStack SendDataServerD(Checkin_ltjyStack _Checkin_ltjyStack)
        {
            return App.Data_Sync.HproseSendDataStow(App.ListCheckin_ltjyStack[Idx]);
        }

        private void StackDlete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确定删除当前整垛数据", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.ListCheckin_ltjyStack.RemoveAt(Idx);
                Tools.Serialize(App.ListCheckin_ltjyStack);
                this.Close();
            }
        }

        private void UpDataBut_Click(object sender, RoutedEventArgs e)
        {
            SendTask = new Task<Checkin_ltjyStack>(() => SendDataServerD(_Checkin_ltjyD));
            SendTask.Start();
            SendTask.ContinueWith(TaskEndedByCatchD);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.labCrib.Text = _Checkin_ltjyD.CRIB;
            this.statustext.Text = _Checkin_ltjyD.Status;
            this.Listviewsimple_View.ItemsSource = _Checkin_ltjyD.LISTPD_SIMPLE;
        }
    }
}