using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Weitedianlan.WinUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
               // Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
               // LoginDialogData result = this.ShowLoginAsync("身份验证", "请输入用户与密码", new LoginDialogSettings { ColorScheme = MetroDialogOptions.ColorScheme, RememberCheckBoxVisibility = Visibility.Visible });
               //// bool? dialogResult = window.ShowDialog();
               // if (Utils.(dialogResult))
               // {
               //     base.OnStartup(e);
               //     Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
               // }
               // else
               // {
               //     this.Shutdown();
               // }
                base.OnStartup(e);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
