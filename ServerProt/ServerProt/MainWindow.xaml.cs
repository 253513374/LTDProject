using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hprose.Server;
using ServerProt.Service;
using ServerProt.ViewModel;


namespace ServerProt
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Queryservice  queryservice = new Queryservice();

        private HproseHttpListenerServer hpptserver = new HproseHttpListenerServer();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = queryservice;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hprose.Client.HproseHttpClient Client = new Hprose.Client.HproseHttpClient("http://124.225.216.98:2015/");
            string Re = Client.Invoke<string>("GetLogistinInfo", new object[] { "143804782711" });

            MessageBox.Show(Re);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            hpptserver = new HproseHttpListenerServer(txbServerIP.Text);
            hpptserver.Methods.AddInstanceMethods(new Queryservice());
           // hpptserver.Methods.AddInstanceMethods(new PDAServer());

            hpptserver.IsDebugEnabled = true;
            hpptserver.Start();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
