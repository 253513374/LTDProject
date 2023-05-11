using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace ScanCode.WPF.View
{
    /// <summary>
    /// ScanCodeOutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScanCodeOutWindow : Window
    {
        public ScanCodeOutWindow()
        {
            InitializeComponent();
            this.DataContext = App.GetService<ScanCodeOutViewModel>();
        }

        private void ScanCodeOutWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 程序最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 程序最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutMaximizeButton_Click(object sender, RoutedEventArgs e)
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
        private void OutPinButton_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutPinButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ScanCodeTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            var viewModel = (ScanCodeOutViewModel)DataContext;

            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text.Length < 12)
                {
                    viewModel.ErrorInfo = $"请输入正确的二维码序号"; ;
                    return;
                }
                //var result = App.ReplaceScanCode(textBox.Text);

                ScanCodeButton.Command.Execute(textBox.Text);
                //_ = App.GetService<ScanCodeOutViewModel>().ExecuteScanCode(result);

                textBox.Text = "";
            }

            // throw new NotImplementedException();
        }
    }
}