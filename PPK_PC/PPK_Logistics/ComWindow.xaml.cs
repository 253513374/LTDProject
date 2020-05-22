using PPK_Logistics.Config;
using System;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PPK_Logistics
{
    /// <summary>
    /// ComWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ComWindow : Window
    {
        /// <summary>
        /// 串口通讯资源
        /// </summary>
        private SerialPort _SerialPort = new SerialPort();

        public ComWindow()
        {
            InitializeComponent();
        }

        private void ComboBoxCom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        /// <summary>
        /// 添加扫描枪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OPenSerialPort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxCom.SelectedItem == null)
                {
                    MessageBox.Show("请选择要开启的COM口");
                    return;
                }
                bool _Result = App.DateCom.OpenComParam(BaudRateBox.SelectedItem.ToString().Trim(),
                DataBitsBox.SelectedItem.ToString().Trim(),
                (Parity)Enum.Parse(typeof(Parity), ParityBox.SelectedItem.ToString()),
                ComboBoxCom.SelectedItem.ToString().Trim(),
                (StopBits)Enum.Parse(typeof(StopBits), StopBitsBox.SelectedItem.ToString()),
                (Handshake)Enum.Parse(typeof(Handshake), HandshakeBox.SelectedItem.ToString()),
                this.ComNotes.Text);
                if (!_Result)
                {
                    MessageBox.Show("扫描枪添加失败,请检测此接口是否有效或被其他占用！:" + App.DateCom.ErrorPort, "警告");
                    return;
                }
                MessageBox.Show("扫描枪添加成功");
                this.DialogResult = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("操作失败，请检查COM口状态：" + ex.Message, "警告");
                SetCommBox();
                return;
            }
        }

        private void RSerialPort_Click(object sender, RoutedEventArgs e)
        {
            //if (App.DateCom.IsOpen())
            //{
               // App.DateCom.ComPort.Close();
                SetCommBox();
            //}
        }

        private void SerialPortData(object sender, SerialDataReceivedEventArgs e)
        {
        }

        private void SetCommBox()
        {
            ParityBox.ItemsSource = Enum.GetValues(typeof(Parity));
            ParityBox.SelectedItem = Parity.None;

            StopBitsBox.ItemsSource = Enum.GetValues(typeof(StopBits));
            StopBitsBox.SelectedItem = StopBits.One;

            HandshakeBox.ItemsSource = Enum.GetValues(typeof(Handshake));
            HandshakeBox.SelectedItem = Handshake.None;

            BaudRateBox.SelectedItem = 115200;
            DataBitsBox.SelectedItem = 8;

            
            if (Tools._ComName != null)
            {
                ComboBoxCom.ItemsSource = null;
                ComboBoxCom.ItemsSource = Tools._ComName;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (!App.DateCom.IsOpen(ComboBoxCom.SelectedItem.ToString().Trim()))
            //{
            //    this.DialogResult = false;//
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommBox();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 打开测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComTestBut_Click(object sender, RoutedEventArgs e)
        {
            ComText.IsEnabled = true;

            try
            {
                if (ComboBoxCom.SelectedItem == null)
                {
                    ComText.Text = "请选择COM口";
                    return;
                }
                if (_SerialPort.IsOpen)
                {
                    ComText.Text = "扫描枪已经开启测试";
                    return;
                }
                _SerialPort.PortName = ComboBoxCom.SelectedItem.ToString().Trim();
                _SerialPort.BaudRate = int.Parse(BaudRateBox.SelectedItem.ToString().Trim());//int.Parse(baudRate);
                _SerialPort.DataBits = int.Parse(DataBitsBox.SelectedItem.ToString().Trim());
                _SerialPort.Parity = (Parity)Enum.Parse(typeof(Parity), ParityBox.SelectedItem.ToString());
                _SerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitsBox.SelectedItem.ToString());
                _SerialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), HandshakeBox.SelectedItem.ToString());
                _SerialPort.DataReceived += _SerialPort_DataReceived;
                _SerialPort.Open();

                ComText.Text = "请使用扫描枪扫描标签！";

            }
            catch(Exception ex)
            {
                ComText.Text = ex.Message;
            }
            
        }

        private void _SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort prot = sender as SerialPort;

            byte[] buf = new byte[prot.BytesToRead];

            prot.Read(buf, 0, buf.Length);

            string QRcode = Encoding.UTF8.GetString(buf);

            QRcode.TrimEnd('\n');
            QRcode.TrimEnd('\r');

            ComText.Text = QRcode;

            //throw new NotImplementedException();
        }

        private void ComTestButClose_Click(object sender, RoutedEventArgs e)
        {
            ComText.IsEnabled = false;
            _SerialPort.Close();
            ComText.Text = "";
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}