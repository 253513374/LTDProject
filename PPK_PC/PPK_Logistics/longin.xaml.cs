using PPK_Logistics.Config;
using PPK_Logistics.Verify;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PPK_Logistics
{
    /// <summary>
    /// longin.xaml 的交互逻辑
    /// </summary>
    public partial class longin : Window
    {
        public longin()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //WorkingView info = new WorkingView();
            //info.Show();
            //this.Close();
            // string str = DESEncrypt.Encrypt(this.Passwordtext.Password.ToString());

            // if (App.OracleDB.GetUserInfo(textuser.Text.ToString().Trim(), this.Passwordtext.Password.ToString()))
            string verify = App.Data_Sync.UserInfoVerify(textuser.Text.ToString().Trim(), this.Passwordtext.Password.ToString());

            if (verify == "1")
            {
                WorkingView info = new WorkingView();
                info.Show();
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(string.Format("登录失败:{0}", verify), "失败");
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // new PDAServer().GetdbckOutOrder();
            ((UserReadonly)this.Resources["userinfo"]).UserId = Tools.DeSerializeShared(new UserReadonly()).UserId;

            //_UserReadonly.UserId = Tools.DeSerializeShared(new UserReadonly()).UserId;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}