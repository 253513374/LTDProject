using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.HubServer.ViewModels;

namespace ScanCode.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    /// <summary>
    /// 登录成功通知事件，登录成功打开主窗口
    /// </summary>
    public event EventHandler EventLoggedIn;

    private readonly HubClientService _hubService;

    private bool _hasExecuted;

    public LoginViewModel(HubClientService hubservice)
    {
        _hubService = hubservice; //App.GetService<HubClientService>();
    }

    [ObservableProperty]
    private SecureString _securePassword;

    [ObservableProperty]
    private string? _username = "011";

    [ObservableProperty]
    private string _errorinfo = "";

    // [ObservableProperty]
    // private string? password;

    public async Task LoginPasswordBox(PasswordBox passwordBox)
    {
        _hasExecuted = true;
        //password = passwordBox.Password;

        // throw new NotImplementedException();
    }

    //[RelayCommand]
    //private void Close()
    //{
    //    Application.Current.Shutdown();
    //}

    // 引发LoggedIn事件的方法
    [RelayCommand(CanExecute = nameof(HasExecuted))]
    protected async Task LoggedIn()
    {
        if (string.IsNullOrWhiteSpace(_username) || _securePassword is null)
        {
            return;
        }
        string password = ConvertSecureStringToString(_securePassword);
        //  string psw = PasswordHelper.SecureStringToString(SecurePassword);
        var loginResult = await _hubService.UserLoging(new LoginData() { Username = _username, Password = password }, true);
        if (loginResult.Successed)
        {
            EventLoggedIn?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Errorinfo = $"登录失败：{loginResult.Message}";
        }
    }

    private bool HasExecuted()
    {
        return !_hasExecuted;
    }

    private string ConvertSecureStringToString(SecureString securePassword)
    {
        if (securePassword == null) throw new ArgumentNullException(nameof(securePassword));

        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
            return Marshal.PtrToStringUni(ptr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(ptr);
        }
    }
}