using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using ScanCode.WPF.ViewModels;

namespace ScanCode.WPF.View
{
    /// <summary>
    /// SplashScreenLogin.xaml 的交互逻辑
    /// </summary>
    public partial class SplashScreenLoginWindow : Window
    {
        public SplashScreenLoginWindow()
        {
            InitializeComponent();
            //var viewModel = DataContext as LoginViewModel;
            var viewModel = App.GetService<LoginViewModel>();
            viewModel.EventLoggedIn += ViewModel_LoggedIn;

            this.DataContext = viewModel;
            this.LoginButton.IsDefault = true;
        }

        private void ViewModel_LoggedIn(object? sender, EventArgs e)
        {
            //先打开主窗口，再关闭登录窗口
            var mainWindow = App.GetService<HomeWindow>();//<>();
            // 显示主窗口
            Dispatcher.Invoke(() =>
            {
                mainWindow.Show();
            });

            this.Close();
        }

        /// <summary>
        /// 拖动程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplashScreenLogin_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            //throw new NotImplementedException();//
        }

        /// <summary>
        /// 程序最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 程序最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// 程序置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PinButton_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PinButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            // Application.Current.Shutdown();
        }
    }
}