using PPK_Logistics.Config;
using PPK_Logistics.DataSource;
using PPK_Logistics.Verify;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;


namespace PPK_Logistics
{
    /// <summary>
    /// WorkingView.xaml 的交互逻辑
    /// </summary>
    public partial class WorkingView : Window
    {

        //private T_BDX_ORDER_SERVICE _orderservice;

        #region 属性字段
        public CollectionViewSource listproductnumber;

        public int YSFCount = 0;

        private List<string> _list = new List<string>(1000);

        /// <summary>
        /// 解散数据记录数据源
        /// </summary>
        private ObservableCollection<PackDataDelete> _PackDataDelete;

        /// <summary>
        /// 串口通讯资源
        /// </summary>
        private SerialPort _SerialPort = new SerialPort();

        ///// <summary>
        ///// 垛数据源
        ///// </summary>
        //private ObservableCollection<Checkin_ltjyStack> App.ListCheckin_ltjyStack;

        /// <summary>
        /// 包装数据 记录数据源
        /// </summary>
        private ObservableCollection<Checkin_ltjy> PackData_Simpl;

        private List<PackData> pdata = null;

        /// <summary>
        /// 包装数据 记录数据源
        /// </summary>
        private ObservableCollection<Checkout> Checkout_Simpl;

        /// <summary>
        /// 产品数据源
        /// </summary>
        private ObservableCollection<OutboundOrder> Product_Observable;

        /// <summary>
        /// 调拨单数据源
        /// </summary>
        private List<OutboundOrder> DBCKOrder_Observable;

        /// <summary>
        /// 销售出库单数据源
        /// </summary>
        private List<OutboundOrder> XSCKOrder_Observable;

        private string SearchNum = "";

        private Task<Checkin_ltjyStack> SendTask;

        /// <summary>
        /// 二维码序号
        /// </summary>
        private string txbBarcode = "";

        /// <summary>
        /// 商品码
        /// </summary>
        private string txbCKD = "";

        private CollectionViewSource OutOrderYetStatus;

        private const int WM_SYSCOMMAND = 0x112;

        private HwndSource _HwndSource;

        
        #endregion
        public WorkingView()
        {
            InitializeComponent();


          //  _orderservice.Get_T_BDX_ORDER_List();

            App.ProductListView = new CollectionViewSource();
            OutOrderYetStatus = new CollectionViewSource();
            listproductnumber = (CollectionViewSource)this.Resources["datashowviewppnumber"];
            pdata = new List<PackData>();
            this.SourceInitialized += delegate (object sender, EventArgs e)
            {
                this._HwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            };
        }

        private delegate void ThreadDelegate();

        private delegate void ThreadDelegate2(int index);

        public void DeletePackdata(string text)
        {
            ThreadDelegate changeTetBoxDel = delegate()
            {
                string _code = Tools.ReplaceScanWord(text);
                if (PackData_Simpl != null)
                {
                    for (int k = 0; k < PackData_Simpl.Count; k++)
                    {
                        if (PackData_Simpl[k].barcode == _code)
                        {
                            PackData_Simpl.RemoveAt(k);
                        }
                    }
                }
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, changeTetBoxDel); //启动委托
        }



        /// <summary>
        /// 自动检索当前检索包装产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetProductName(object sender, FilterEventArgs e)
        {
            Product _Product = e.Item as Product;
            if (_Product == null)
            {
                return;
            }
            else
            {
                if (SearchNum == "") return;
                e.Accepted = (_Product.PRODUCTNUMBER.ToLower().Trim() == SearchNum);
            }
        }


        /// <summary>
        /// 关键字过滤器  索引 出库单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetOutboundOrder(object sender, FilterEventArgs e)
        {
            OutboundOrder _OutboundOrder = e.Item as OutboundOrder;
            if (_OutboundOrder == null)
            {
                return;
            }
            else
            {
                if (SearchNum == "") return;
                e.Accepted = (_OutboundOrder.KCSWZ_SWKCBH1.ToLower().Trim().Contains(SearchNum));
            }
        }

        public void ProductListView_Filter1(object sender, FilterEventArgs e)
        {
            Button _but = sender as Button;

            _but.IsEnabled = false;
            TaskFactory taskFactory = new TaskFactory();
            Task<string>[] tasks = new Task<string>[]
                {
                   // taskFactory.StartNew(() => GetProduct()),
                    taskFactory.StartNew(() => {
                     XSCKOrder_Observable = App.Data_Sync.HproseDownDataOrder();
                         return "OK";}),
                };
            //CancellationToken.None指示TasksEnded不能被取消
            taskFactory.ContinueWhenAll<string>(tasks, FillData, CancellationToken.None);
        }

        public string SendDataDelete(string code)
        {
            return App.Data_Sync.HproseDataDelete(code);
        }

        /// <summary>
        /// 数据同步任务 同步箱数据
        /// </summary>
        /// <param name="simple"></param>
        /// <returns></returns>
        public string SendDataServer(PackData_Simple simple)
        {
            return App.Data_Sync.HproseSendData(simple);
        }

        /// <summary>
        /// 数据同步任务 同步箱垛关系数据
        /// </summary>
        /// <param name="packData_Simple_New"></param>
        /// <returns></returns>
        public Checkin_ltjyStack SendDataServerD(Checkin_ltjyStack packData_Simple_New)
        {
            return App.Data_Sync.HproseSendDataStow(packData_Simple_New);
        }

        /// <summary>
        /// 线程结束后的结果处理,监测数据同步状态
        /// </summary>
        /// <param name="task"></param>
        public void TaskEndedByCatch(Task<string> task)
        {
            if (task.IsCompleted)
            {
                if (PackData_Simpl == null)
                    return;
                string _result = task.Result.ToString();
                if (_result.ToUpper().Contains("ERROR"))
                {
                    for (int k = 0; k < PackData_Simpl.Count; k++)
                    {
                        if (_result.ToUpper().Contains(PackData_Simpl[k].barcode))
                        {
                            PackData_Simpl[k].status = _result;
                            ThreadDelegate TetBoxDel2 = delegate()
                            {
                                textpackError.Text = Tools.PackData_Simpl_Error.Count.ToString();
                            };
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, TetBoxDel2); //启动委托
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < PackData_Simpl.Count; k++)
                    {
                        if (PackData_Simpl[k].barcode == _result)
                        {
                            PackData_Simpl[k].status = "上传成功";
                            Tools.SendCount++;
                            ThreadDelegate TetBoxDel = delegate()
                            {
                                textSendCount.Text = Tools.SendCount.ToString();
                                PackCountTxt_Copy.Content = Tools.SendCount.ToString();
                            };
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, TetBoxDel); //启动委托
                        }
                    }
                }
            }
        }

        public void TaskEndedByCatchD(Task<Checkin_ltjyStack> task)
        {
            if (task.IsCompleted)
            {
                Checkin_ltjyStack TaskResult = task.Result;
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
                            ThreadDelegate TetBoxDel2 = delegate()
                            {
                                PackCountTxt_Copy.Content = Tools.GetSendStatus(true).ToString();
                            };
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, TetBoxDel2);
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
                                ThreadDelegate TetBoxDel2 = delegate()
                                {
                                    PackCountStackE.Content = Tools.GetSendStatus(false).ToString();
                                };
                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, TetBoxDel2);

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
                Tools.Serialize(App.ListCheckin_ltjyStack);
            }
        }

        /// <summary>
        /// 更新解散包装数据状态
        /// </summary>
        /// <param name="task"></param>
        public void TaskEndedDelete(Task<string> task)
        {
            if (task.IsCompleted)
            {
            }
        }


        #region     拖动窗口拉伸，拖动


        private Dictionary<ResizeDirection, Cursor> cursors = new Dictionary<ResizeDirection, Cursor>
        {
             {ResizeDirection.Top, Cursors.SizeNS},

             {ResizeDirection.Bottom, Cursors.SizeNS},

             {ResizeDirection.Left, Cursors.SizeWE},

             {ResizeDirection.Right, Cursors.SizeWE},

             {ResizeDirection.TopLeft, Cursors.SizeNWSE},

             {ResizeDirection.BottomRight, Cursors.SizeNWSE},

             {ResizeDirection.TopRight, Cursors.SizeNESW},

             {ResizeDirection.BottomLeft, Cursors.SizeNESW}

        };
        private int indexRowColmn=0;

        private enum ResizeDirection
        {

            Left = 1,

            Right = 2,

            Top = 3,

            TopLeft = 4,

            TopRight = 5,

            Bottom = 6,

            BottomLeft = 7,

            BottomRight = 8,

        }

        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;
                if (element != null && !element.Name.Contains("RectangleSize"))
                    this.Cursor = Cursors.Arrow;

            }
        }
        private void Rectangle_Resize(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            ResizeDirection direction = (ResizeDirection)Enum.Parse(typeof(ResizeDirection), element.Name.Replace("RectangleSize", ""));

            this.Cursor = cursors[direction];
            //System.Drawing.Point pix;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Tools.SendMessage(_HwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction),  IntPtr.Zero);
            }
            e.Handled = true;
        }

        private void Rectangle_Resize(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }
        #endregion


        /// <summary>
        /// 绑定垛号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBind_Click(object sender, RoutedEventArgs e)
        {
            if (CribTxt.Text.Trim().Length > 0 && PackData_Simpl.Count > 0)
            {
                string _code = this.CribTxt.Text.Trim();

                if (App.ListCheckin_ltjyStack.VerifyDataD(_code))
                {
                    BeepUP.Beep(2000, 500);
                    WringText.Content = "重复垛号";
                    return;
                }

                Checkin_ltjyStack _Checkin_ltjyD = new Checkin_ltjyStack();
                _Checkin_ltjyD.LISTPD_SIMPLE = new List<Checkin_ltjy>(PackData_Simpl.Count);
                _Checkin_ltjyD.CRIB = this.CribTxt.Text.Trim();

                _Checkin_ltjyD.LISTPD_SIMPLE.AddRange(PackData_Simpl);

                App.ListCheckin_ltjyStack.Insert(0, _Checkin_ltjyD);

                PackData_Simpl.Clear();
                Tools.SerializeShared(PackData_Simpl);

                CribTxt.Text = "";
                PackCountTxtD.Content = App.ListCheckin_ltjyStack.Count.ToString();

                SendTask = new Task<Checkin_ltjyStack>(() => SendDataServerD(_Checkin_ltjyD));
                SendTask.Start();
                SendTask.ContinueWith(TaskEndedByCatchD);
            }
            else
            {
                MessageBox.Show(CribTxt.Text.Trim().Length > 0 ? "请输入完整 箱标条码与商品码" : "请输入完整的垛号");
                return;
            }
        }

        /// <summary>
        /// 绑定包装产品信息--绑定扫描箱号与产品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BrCodeTxt.Text.Trim().Length > 0 && NumCodeTxt.Text.Trim().Length > 0)
            {
                if (App.ProductListView.View.CanFilter && App.ProductListView.View.IsEmpty)
                {
                    BeepUP.Beep(1800, 1000);
                    MessageBox.Show("请检查当前扫描的商品是否正确");

                    return;
                }
                UpdateUIView(BrCodeTxt.Text.Trim(), NumCodeTxt.Text.Trim());
            }
            else
            {
                MessageBox.Show("请输入完整 箱标条码与商品码");
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ComWindow _ComWindow = new ComWindow();
            if (_ComWindow.ShowDialog().Value)
            {
               // App.DateCom.ComPort.DataReceived += ComPort_DataReceived;
                NumCodeTxt.IsEnabled = true;
                BrCodeTxt.IsEnabled = true;
                CribTxt.IsEnabled = true;
                ButBind.IsEnabled = true;
            }
        }

        /// <summary>
        /// 按产品包装记录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string CodeString = "";
            if (sender is string) CodeText.Text = CodeString = sender.ToString();

            if(sender is TextBox) CodeString = Tools.ReplaceScanWord(CodeText.Text);
            if(sender is Button)  CodeString = Tools.ReplaceScanWord(CodeText.Text);

            const int codeLength = 12;
            if ( CodeString.Length == codeLength&& CodeText.Text.Length== codeLength)
            {
                if (Tools.SFCount >0)
                {
                    if (YSFCount >= Tools.SFCount)
                    {
                        CodeText.Text = "";
                        MessageBox.Show("该发货单已经发货完成");
                        return;
                    }
                }
                else if(YSFCount >= double.Parse(CheckoutInfo.PDSL) )
                {
                    CodeText.Text = "";
                    MessageBox.Show("该发货单已经发货完成");
                    return;
                }
                Checkout checkout = new Checkout();
                checkout.BRACODE = CodeString.Trim();//扫描的序号
                checkout.CKD = CheckoutInfo.CKD_CKBH;//产品出单号
                checkout.AGEENID = CheckoutInfo.AGEENID_KHID;//收货客户ID
                checkout.Timejzrq = CheckoutInfo.PRODUCTID_JZRQ;
                checkout.AgentName = CheckoutInfo.StockOut_KHMC;
                checkout.USERID = App.UserReadonly.UserId;
                checkout.OutType = CheckoutInfo.KCSWZ_SWLX1.Substring(0,4);
                checkout.Outstock = CheckoutInfo.KCSWZ_BRCK1;
                checkout.Outstockid=CheckoutInfo.KCSWZ_BRCKID;
                checkout.STATUS = "待";//tools.OutType.Trim();
                checkout.Remarks = CheckoutInfo.Remarks;

                Checkout _ReOut= new PDAServer().MarketOut2(checkout);
                Checkout_Simpl.Insert(0,_ReOut);

                if(_ReOut.STATUS.Contains("重复"))
                {
                    //BeepUP.player1.PlaySync();
                }
               // textSendCount.Text = Checkout_Simpl
                CodeText.Text = "";
                YSFCount= Checkout_Simpl.GetSendStatusCheckout("成功");
                Codelable.Text =YSFCount.ToString();
                SOutnumText.Text = (Convert.ToUInt32(CheckoutInfo.SFSL) + YSFCount).ToString();
                textpackError.Text = Checkout_Simpl.GetSendStatusCheckout("重复").ToString();
            }
            else
            {
                //BeepUP.player.PlaySync();
                //MessageBox.Show("该枚标签长度不是12位，请检查扫描的标签是否正确");
                Textinfo.Text = "请检查输入的标签是否正确";
            }
        }

        /// <summary>
        /// 解散包装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string code = "";
            if (textbox != null)
            {
                code = textbox.Text.Trim();
            }
            else
            {
                code = Tools.ReplaceScanWord(deletetext.Text);
                if (code.Length !=12)
                {
                    deletetext.Focus();
                    BeepUP.Beep(1500, 500);
                    //BeepUP.player.PlaySync();
                    MessageBox.Show("该枚标签长度不是12位，请检查扫描的标签是否正确");
                    return;
                }
            }
            PackDataDelete packdata= new PDAServer().MarketReturn2(new PackDataDelete(code, App.UserReadonly.UserId));
            _PackDataDelete.Insert(0, packdata);
            deletetext.Text = "";

            Retextcount.Text =  _PackDataDelete.GetSendStatusPackDataDelete("成功").ToString();

            if(packdata.STATUS.Contains("成功"))
            {
                Checkout_Simpl.RemoveCheckout(packdata.NUMBER);

               // Checkout_Simpl.GetSendStatusCheckout("成功");
                YSFCount = Checkout_Simpl.GetSendStatusCheckout("成功");
                Codelable.Text = YSFCount.ToString();
            }

            //SendTaskstring = new Task<string>(() => SendDataDelete(code));
            //SendTaskstring.Start();
            //SendTaskstring.ContinueWith(TaskEndedDelete);
        }

        private void butviewFill_Click(object sender, RoutedEventArgs e)
        {
            Button _but = sender as Button;

            _but.IsEnabled = false;
            TaskFactory taskFactory = new TaskFactory();
            Task<string>[] tasks = new Task<string>[]
                {
                   // taskFactory.StartNew(() => GetProduct()),
                    taskFactory.StartNew(() => {
                     XSCKOrder_Observable = App.Data_Sync.HproseDownDataOrder();
                         return "OK";}),
                };
            //CancellationToken.None指示TasksEnded不能被取消
            taskFactory.ContinueWhenAll<string>(tasks, FillData, CancellationToken.None);
        }

        private void CodeText_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox == null|| !textbox.IsEnabled)
                return;
            if (e.Key == Key.Enter)
            {
                if (!textbox.Text.ToUpper().Contains("HTTP"))
                {
                    textbox.Text = "";
                    //BeepUP.player.PlaySync();
                    return;
                }
                string Code = Tools.ReplaceScanWord(textbox.Text);
                if (Code.Length ==12)
                {
                    textbox.Text = Code;
                    Button_Click_3(sender, null);
                }
                else
                {
                    //BeepUP.player.PlaySync();
                    //MessageBox.Show("该枚标签长度不够12位，请检查扫描的标签是否正确");
                    Textinfo.Text = "请检查输入的标签是否正确！";
                    textbox.Text = "";
                    return;
                }
                e.Handled = true;
            }
            else
            {
            }
        }

        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _SerialPort = sender as SerialPort;

            byte[] _buf = new byte[_SerialPort.BytesToRead];

            _SerialPort.Read(_buf, 0, _buf.Length);
            if (_buf.Length != 0)
            {
                string _Code = Encoding.UTF8.GetString(_buf);

                _Code = _Code.TrimEnd('\n');
                _Code = _Code.TrimEnd('\r');

                if (_Code.ToUpper().Contains("HTTP"))
                {
                    txbBarcode = Tools.ReplaceScanWord(_Code.ToString());

                    //string _temp = txbCKD;

                    ThreadDelegate _ThreadDelegate = delegate()
                    {
                        BrCodeTxt.Text = txbBarcode;
                        if (NumCodeTxt.Text.Trim() != "")
                        {
                            if (App.ProductListView.View.CanFilter && App.ProductListView.View.IsEmpty)
                            {
                                BeepUP.Beep(1800, 1000);
                                MessageBox.Show("请检查当前扫描的商品是否正确");
                                NumCodeTxt.Text = "";
                                return;
                            }
                        }
                        if (NumCodeTxt.Text.Length > 0 && txbBarcode.Length > 0)
                        {
                            UpdateUIView(txbBarcode, NumCodeTxt.Text);
                        }
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, _ThreadDelegate);
                }
                else if (_Code.Length == 13 && _Code.Contains("69322758"))
                {
                    txbCKD = "";
                    txbCKD = _Code.ToString();
                    ThreadDelegate _ThreadDelegate = delegate()
                    {
                        NumCodeTxt.Text = txbCKD;
                        if (App.ProductListView.View.CanFilter && App.ProductListView.View.IsEmpty)
                        {
                            BeepUP.Beep(1800, 1000);
                            MessageBox.Show("请检查当前扫描的商品是否正确");
                            NumCodeTxt.Text = "";
                            return;
                        }
                        if (txbBarcode.Length > 0 && txbCKD.Length > 0)
                        {
                            UpdateUIView(txbBarcode, txbCKD);
                        }
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, _ThreadDelegate);
                }
                else
                {
                    ThreadDelegate _ThreadDelegate = delegate()
                    {
                        CribTxt.Text = _Code;
                        UpdateUIView(txbBarcode, txbCKD);
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, _ThreadDelegate);
                }
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 绑定产品信息
        /// </summary>
        /// <param name="task"></param>
        private void FillData(Task<string>[] task)
        {
            //DBCKOrder_Observable
           
            if (task[0].IsCompleted && task[0].Result == "OK")
            {

                if(DBCKOrder_Observable!=null) XSCKOrder_Observable.AddRange(DBCKOrder_Observable);
                Product_Observable = new ObservableCollection<OutboundOrder>(XSCKOrder_Observable);//调拨出库与销售出库合并

                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(()=> {

                    butviewFill.IsEnabled = true;
                    App.ProductListView.Source = Product_Observable;//.GetOutOrderStatus_Not();

                    this.ListboxP.ItemsSource = null;
                    this.ListboxP.ItemsSource = App.ProductListView.View;

                    SetFliterSCount();
                    EndQueryBut_Click(null, null);

                    PackData_Simpl = new ObservableCollection<Checkin_ltjy>();
                    PackData_Simpl = Tools.DeSerializeShared(PackData_Simpl);

                    Checkout_Simpl = new ObservableCollection<Checkout>();
                    Listviewsimple.ItemsSource = Checkout_Simpl;

                    Listviewsimple_Temp.ItemsSource = PackData_Simpl;
                    PackCountTxt.Content = PackData_Simpl.Count.ToString();

                    App.ListCheckin_ltjyStack = Tools.DeSerialize(App.ListCheckin_ltjyStack);
                    Listviewsimple_Copy.ItemsSource = App.ListCheckin_ltjyStack;
                    PackCountTxtD.Content = App.ListCheckin_ltjyStack.Count.ToString();

                    _PackDataDelete = new ObservableCollection<PackDataDelete>();
                    ListViewDetele.ItemsSource = _PackDataDelete;

                }));
            }
        }

        /// <summary>
        /// 选择产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            if (listbox.SelectedItem != null)
            {
                OutboundOrder OutboundOrderOK = listbox.SelectedItem as OutboundOrder;
                if (OutboundOrderOK != null)
                {
                    Tools.SFCount = 0;
                    SetOutOrderWord(OutboundOrderOK);
                }
            }
        }

        /// <summary>
        /// 视图条件过滤。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listproductnumber_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        /// <summary>
        /// 显示垛标对应的箱标信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listviewsimple_CopyDoubleClick(object sender, RoutedEventArgs e)
        {
            if (this.Listviewsimple_Copy.Items.Count > 0)
            {
                Checkin_ltjyStack _PackData_Simple_New = this.Listviewsimple_Copy.SelectedItem as Checkin_ltjyStack;

                if (_PackData_Simple_New != null)
                {
                    ViewInfo _view = new ViewInfo(_PackData_Simple_New, this.Listviewsimple_Copy.SelectedIndex);
                    _view.Topmost = true;
                    _view.ShowDialog();
                    PackCountTxtD.Content = App.ListCheckin_ltjyStack.Count.ToString();
                }
            }
        }

        /// <summary>
        /// 清楚当前选中包装箱数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listviewsimple_Temp_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Listviewsimple_Temp.Items.Count > 0 && Listviewsimple_Temp.SelectedItem != null)
            {
                if (MessageBox.Show("是否确定删除这条数据", "警告", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Checkin_ltjy _Checkin_ltjy = this.Listviewsimple_Temp.SelectedItem as Checkin_ltjy;
                    if (_Checkin_ltjy != null)
                    {
                        PackData_Simpl.Remove(_Checkin_ltjy);
                        PackCountTxt.Content = PackData_Simpl.Count.ToString();
                        Tools.SerializeShared(PackData_Simpl);
                    }
                }
            }
        }

        private void NumCodeTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox _TextBox = sender as TextBox;
            if (_TextBox != null && _TextBox.Text != "")
            {
                SearchNum = _TextBox.Text.Trim();
                App.ProductListView.Filter += GetProductName;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox == null)
                return;
            if (e.Key == Key.Enter)
            {
                if (!textbox.Text.ToUpper().Contains("HTTP"))
                {
                    textbox.Text = "";
                    //BeepUP.player.PlaySync();
                    return;
                }
                string Code = Tools.ReplaceScanWord(textbox.Text);
                if (Code.Length > 11)
                {
                    textbox.Text = Code;
                    Button_Click_4(sender, null);
                }
                else
                {
                    //BeepUP.player.PlaySync();
                    MessageBox.Show("该枚标签长度不够12位，请检查扫描的标签是否正确");
                    textbox.Text = "";
                    return;
                }
                e.Handled = true;
            }
            else
            {
                // MessageBox.Show(textbox.Text);
            }
        }

        private void TextBoxP_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox _TextBox = sender as TextBox;
            if (_TextBox != null)
            {
                SearchNum = _TextBox.Text.Trim();
                App.ProductListView.Filter += GetOutboundOrder;//GetProductName;
            }
        }

        /// <summary>
        /// _code 箱码，numcode产品编号
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="numcode"></param>
        private void UpdateUIView(string _code, string numcode)
        {
            System.Collections.IEnumerator item = App.ProductListView.View.GetEnumerator();

            item.MoveNext();

            if (PackData_Simpl.Count == int.Parse(XCounttextbox.Text.Trim()))
            {
                btnBind_Click(null, null);
                return;
            }

            if (PackData_Simpl.VerifyData(_code))
            {
                BeepUP.Beep(2000, 500);
                WringText.Content = "重复箱号";
                //MessageBox.Show("请勿重复包装");
                return;
            }
            WringText.Content = "";
            Checkin_ltjy _Checkin_ltjy = new Checkin_ltjy();
            _Checkin_ltjy.barcode = _code;
            // _Checkin_ltjy.status = ((Product)item.Current).PRODUCTNAME.Trim();
            _Checkin_ltjy.productid = ((Product)item.Current).ID.ToString().Trim();
            _Checkin_ltjy.userid = App.UserReadonly.UserNunber;

            PackData_Simpl.Insert(0, _Checkin_ltjy);

            Tools.SerializeShared(PackData_Simpl);

            PackCountTxt.Content = PackData_Simpl.Count.ToString();

            BrCodeTxt.Text = "";
            txbBarcode = "";
            txbCKD = "";
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            Tools.SerializeShared(App.UserReadonly);
            Tools.SerializeShared(PackData_Simpl);
            if (SendTask == null)
                return;
            if (SendTask.Status == TaskStatus.Running)
            {
                if (MessageBox.Show("数据还在同步是否强制关闭程序", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Tools.Serialize(App.ListCheckin_ltjyStack);
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 窗体 启动加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

          
            this.textuser.Text = App.UserReadonly.UserId.ToString();
            this.textfirm.Text = App.UserReadonly.UserToAge.ToString();

            TaskFactory taskFactory = new TaskFactory();
            Task<string>[] tasks = new Task<string>[]
                {
                    //taskFactory.StartNew(() => {
                    //     DBCKOrder_Observable = App.Data_Sync.HproseDownDataDBCK();
                    //     return"OK";
                    //}),
                    taskFactory.StartNew(() => {
                         XSCKOrder_Observable = App.Data_Sync.HproseDownDataOrder();
                         return "OK";
                    }),
                };
            taskFactory.ContinueWhenAll<string>(tasks, FillData, CancellationToken.None);
        }

        private void textpici_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            const int leng = 6;
            TextBox _TextBlock = sender as TextBox;
            if (_TextBlock == null) return;
            if (_TextBlock.Text.Length > leng)
            {
                MessageBox.Show("请输入正确的数值范围：1~100000");
                _TextBlock.Text = "";
                return;
            }
            if (!Tools.RegexValidate(_TextBlock.Text.Trim()))
            {
                MessageBox.Show("请输入正确的数字");
                _TextBlock.Text = "";
                return;
            }
        }

        /// <summary>
        /// 确定工作单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(ListboxP.SelectedIndex!=-1)
            {
                OutboundOrder order=  Product_Observable.GetOutOrderFind(CheckoutInfo.CKD_CKBH);
                SetOutOrderWord(order);
            }
            Button _button = sender as Button;
            if(_button!=null)
            {
                int sjcount = (int)double.Parse(CheckoutInfo.YFSL);
                int sfsl = Convert.ToInt32(CheckoutInfo.SFSL);
                if (CheckoutInfo.KCSWZ_FJLDW == "")
                {
                    sjcount = new PDAServer().GetERP_KcswzCount(CheckoutInfo.CKD_CKBH);
                    textnum.Text = CheckoutInfo.YFSL = sjcount.ToString();
                }
               
                if (textpici.Text != "")
                {
                    if (sfsl <= sjcount) sjcount = sjcount - sfsl;
                    int count = Convert.ToInt32(textpici.Text);
                    if (count > sjcount)
                    {
                        MessageBox.Show("填写的发货数量不能大于等于发货单上的出库数量");
                        textpici.Text = "";
                        Tools.SFCount = 0;
                        return;
                    }
                    else
                    {
                        Tools.SFCount = count;// Convert.ToInt32(textpici.Text);}
                        CheckoutInfo.PDSL = sjcount.ToString();
                    }
                }
                else
                {
                    Tools.SFCount = 0;
                    CheckoutInfo.PDSL = (sjcount - sfsl).ToString();
                }

                ListboxP.SelectedItem = null;
                ButtonConfirm.IsEnabled = false;
                ReadButton.IsEnabled = true;
                CodeText.IsEnabled = true;
                ListboxP.IsEnabled = false;
                TextBoxP.IsEnabled = false;
                butviewFill.IsEnabled = false;
                ButtonConfirmRE.IsEnabled = true;
                CodeText.Focus();
            }
        }

        private void ButtonConfirmRE_Click(object sender, RoutedEventArgs e)
        {
            Button _button = sender as Button;
            if (_button != null)
            {
                if(MessageBox.Show("请确定当前发货单已经完成或者放弃当前选择发货单，重新现在发货单将清除现有的已扫描的本地数据","警告",MessageBoxButton.YesNo,MessageBoxImage.Warning,MessageBoxResult.No)==MessageBoxResult.Yes)
                {
                    Product_Observable.UpDataOutSCount(CheckoutInfo.CKD_CKBH);
                    Checkout_Simpl.Clear();

                    textpici.Text = "";
                    ButtonConfirmRE.IsEnabled = false;
                    ReadButton.IsEnabled = false;
                    CodeText.IsEnabled = false;
                    ListboxP.IsEnabled = true;
                    TextBoxP.IsEnabled = true;
                    butviewFill.IsEnabled = true;
                    ButtonConfirm.IsEnabled = true;
                   // StackPanel_1.IsEnabled = true;

                    YSFCount = Checkout_Simpl.GetSendStatusCheckout("成功");
                    Codelable.Text = YSFCount.ToString();
                    textpackError.Text = Checkout_Simpl.GetSendStatusCheckout("重复").ToString();
                }
            }
        }

        private void ListboxEndOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            if (listbox.SelectedItem != null)
            {
                OutboundOrder OutboundOrderOK = listbox.SelectedItem as OutboundOrder;
                if (OutboundOrderOK != null)
                {
                    EndTextkhNmae.Text = OutboundOrderOK.KH_MC1;
                    EndTextOrder.Text = OutboundOrderOK.KCSWZ_SWKCBH1;
                    EndTextSfsl.Text =OutboundOrderOK.KCSWZMX_SFSL1;
                    EndTextYfsl.Text = OutboundOrderOK.KCSWZMX_FZCKSL1.ToString();
                    EndTextJzrq.Text = OutboundOrderOK.KCSWZ_JZRQ1;
                }
            }
        }

        private void QueryContentBut_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox _TextBox = sender as TextBox;
            if (_TextBox != null)
            {
                SearchNum = _TextBox.Text.Trim();
                OutOrderYetStatus.Filter += GetOutboundOrder;//GetProductName;
            }
        }

        private void SetFliterSCount()
        {
            SearchNum = "";
            App.ProductListView.Filter += SetOutboundOrder;//GetProductName;
        }

        private void SetOutboundOrder(object sender, FilterEventArgs e)
        {
            OutboundOrder _OutboundOrder = e.Item as OutboundOrder;
            if (_OutboundOrder != null)
                e.Accepted = (Convert.ToInt32(_OutboundOrder.KCSWZMX_SFSL1) == 0);
        }

        private void EndQueryBut_Click(object sender, RoutedEventArgs e)
        {
            OutOrderYetStatus.Source = Product_Observable.GetOutOrderStatus_Yet();
            ListboxEndOut.ItemsSource = null;
            ListboxEndOut.ItemsSource = OutOrderYetStatus.View;
        }

        private void SetOutOrderWord(OutboundOrder OutboundOrderOK)
        {
            textPgx.Text      = CheckoutInfo.PRODUCTID_JZRQ = OutboundOrderOK.KCSWZ_JZRQ1;
            textPname.Text    = CheckoutInfo.StockOut_KHMC = OutboundOrderOK.KH_MC1;
            textPid.Text      = CheckoutInfo.AGEENID_KHID = OutboundOrderOK.KCSWZ_KHID1;
            textnum.Text      = CheckoutInfo.YFSL = OutboundOrderOK.KCSWZMX_FZCKSL1.ToString();
            OrderText.Text    = CheckoutInfo.CKD_CKBH = OutboundOrderOK.KCSWZ_SWKCBH1;
            SOutnumText.Text  = CheckoutInfo.SFSL = OutboundOrderOK.KCSWZMX_SFSL1;
            TextStoreIN.Text  = CheckoutInfo.KCSWZ_BRCK1 = OutboundOrderOK.KCSWZ_BRCK1;
            TextStoreOUT.Text = CheckoutInfo.KCSWZ_BCCK1 = OutboundOrderOK.KCSWZ_BCCK1;
            CheckoutInfo.KCSWZ_FJLDW = OutboundOrderOK.KCSWZ_FJLDW;
            CheckoutInfo.KCSWZ_BRCKID = OutboundOrderOK.KCSWZ_BRCKID1;
            CheckoutInfo.Remarks = OutboundOrderOK.Remarks;
            if (OutboundOrderOK.KCSWZ_SWLX1=="DBCK")
            {
                OrderTypeText.Text = CheckoutInfo.KCSWZ_SWLX1 = OutboundOrderOK.KCSWZ_SWLX1+"|调拨出库";
            }
            else
            {
                OrderTypeText.Text = CheckoutInfo.KCSWZ_SWLX1 = OutboundOrderOK.KCSWZ_SWLX1 + "|销售出库";
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MiniWindow(object sender, RoutedEventArgs e)
        {
            //if(this.WindowState = WindowState.;)
            this.WindowState = WindowState.Minimized;
        }

        private void MaxWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {

                //this.WindowStyle = System.Windows.WindowStyle.None;
               // this.ResizeMode = System.Windows.ResizeMode.NoResize;
                //this.Topmost = true;
                //this.Left = 0.0;
                //this.Top = 0.0;

                var rect= SystemParameters.WorkArea;

                this.MaxWidth = rect.Width;
                this.MaxHeight = rect.Height;
                this.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// 添加扫描枪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComWindow addcom = new ComWindow();
                addcom.Owner = this;
                addcom.ShowInTaskbar = false;

                //addcom.Show();
                if (addcom.ShowDialog().Value)
                {

                    App.DateCom.ListProt[App.DateCom.Name].DataReceived += ComPort_DataReceived1;
                    if (indexRowColmn <= 4)
                    {
                        CreateControl(indexRowColmn, App.DateCom.Name,App.DateCom.Notes);
                        indexRowColmn++;
                        if (indexRowColmn == 4) indexRowColmn = 0;
                    }
                }
                else
                {
                    //MessageBox.Show("0000");
                }
            }
            catch( Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            
        }

        private void ComPort_DataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort prot = sender as SerialPort;

            byte[] buf = new byte[prot.BytesToRead];

            prot.Read(buf, 0, buf.Length);

            string QRcode = Encoding.UTF8.GetString(buf);

            QRcode.TrimEnd('\n');
            QRcode.TrimEnd('\r');
            QRcode = Tools.ReplaceScanWord(QRcode);
            Button_Click_3(QRcode, null);
            //throw new NotImplementedException();            
        }

        private void CreateControl(int Index,string name,string notes)
        {
            Button but = new Button();
            but.Name = name;
            but.Content = notes+" | 删除";
            but.FontSize = 14;
            but.Click += But_Click;
            Grid.SetRow(but, Tools._point[Index].X);
            Grid.SetColumn(but, Tools._point[Index].Y);
            this.CanvasControl.Children.Add(but);
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            Button _but = sender as Button;
            if (MessageBox.Show(string.Format("请确认是否删除当前{0}扫描枪", _but.Content.ToString().Split('|')[0].ToString()), "提示",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.CanvasControl.Children.Remove(_but);

                App.DateCom.ListProt[_but.Name.ToString().Trim()].Close();
                App.DateCom.ListProt.Remove(_but.Name.ToString().Trim());
            }
        }
    }
}