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

namespace ScanCode.WPF.View
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow(DialogViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void DialogWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void DialogPinButton_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
            // throw new NotImplementedException();
        }

        private void DialogPinButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        private void DialogCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DialogMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DialogMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}