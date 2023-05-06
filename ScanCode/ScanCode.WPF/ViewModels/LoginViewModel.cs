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
    public event EventHandler EventLoggedIn;

    private readonly WtdlSqlService hubService;

    private bool _hasExecuted;

    public LoginViewModel()
    {
        hubService = App.GetService<WtdlSqlService>();
    }

    [ObservableProperty]
    private SecureString _securePassword;

    [ObservableProperty]
    private string? username = "";

    // [ObservableProperty]
    // private string? password;

    public async Task LoginPasswordBox(PasswordBox passwordBox)
    {
        _hasExecuted = true;
        //password = passwordBox.Password;

        // throw new NotImplementedException();
    }

    [RelayCommand]
    private void Close()
    {
        Application.Current.Shutdown();
    }

    // 引发LoggedIn事件的方法
    [RelayCommand(CanExecute = nameof(HasExecuted))]
    protected async Task LoggedIn()
    {
        if (string.IsNullOrWhiteSpace(username) || SecurePassword is null)
        {
            return;
        }
        string password = ConvertSecureStringToString(SecurePassword);
        //  string psw = PasswordHelper.SecureStringToString(SecurePassword);
        // var loginResult = await hubService.UserLoging(new LoginData() { Username = username, Password = password }, true);

        EventLoggedIn?.Invoke(this, EventArgs.Empty);
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