using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weitedianlan.WinUI.Pages
{
    /// <summary>
    /// DialogControl.xaml 的交互逻辑
    /// </summary>
    public partial class DialogControl : UserControl
    {

        // Here we create the viewmodel with the current DialogCoordinator instance 
      //  ShellViewModel vm = new ShellViewModel(DialogCoordinator.Instance);

        public DialogControl()
        {
            InitializeComponent();
           // DataContext = vm;
        }

        //private async Task Button_ClickAsync(object sender, RoutedEventArgs e)
        //{
        //    await vm.FooMessageAsync();
        //}
    }
}
