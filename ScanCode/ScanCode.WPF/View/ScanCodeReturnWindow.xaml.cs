using ScanCode.WPF.ViewModels;
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

namespace ScanCode.WPF.View
{
    /// <summary>
    /// ScanCodeReturnWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScanCodeReturnWindow : Window
    {
        public ScanCodeReturnWindow(ScanCodeReturnViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void ReturnMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ReturnPinButton_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        private void ReturnMaximizeButton_Click(object sender, RoutedEventArgs e)
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

        private void ReturnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ReturnPinButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        private void ScanCodeReturnTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            var viewModel = (ScanCodeReturnViewModel)DataContext;

            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text.Length < 12)
                {
                    viewModel.ErrorReturnInfo = $"请输入正确的二维码序号"; ;
                    return;
                }
                // var result = App.ReplaceScanCode(textBox.Text);

                ScanCodeReturnButton.Command.Execute(textBox.Text);
                //_ = App.GetService<ScanCodeOutViewModel>().ExecuteScanCode(result);

                textBox.Text = "";
            }
        }

        private void ScanCodeReturnWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            DragMove();
            // throw new NotImplementedException();
        }
    }
}