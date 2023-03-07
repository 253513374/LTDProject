using System.Windows.Controls;

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