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
using ScanCode.WPF.ViewModels;

namespace ScanCode.WPF
{
    /// <summary>
    /// HomeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            this.DataContext = App.GetService<HomeViewModel>();
        }

        private void HomeWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 程序最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 程序最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeMaximizeButton_Click(object sender, RoutedEventArgs e)
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
        private void HomePinButton_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomePinButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HomeWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
            //throw new NotImplementedException();
        }
    }
}