using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ScanCode.WPF
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(SecureString), typeof(PasswordHelper), new FrameworkPropertyMetadata(new SecureString(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));

        public static SecureString GetPassword(DependencyObject d)
        {
            return (SecureString)d.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject d, SecureString value)
        {
            d.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox passwordBox) return;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if (e.NewValue is SecureString secureString)
            {
                string newPassword = SecureStringToString(secureString);
                string oldPassword = passwordBox.Password;

                if (newPassword.Length > oldPassword.Length)
                {
                    passwordBox.Password = oldPassword.Insert(oldPassword.Length, newPassword.Substring(oldPassword.Length));
                }
                else if (newPassword.Length < oldPassword.Length)
                {
                    passwordBox.Password = oldPassword.Remove(newPassword.Length);
                }
            }
            else
            {
                passwordBox.Password = string.Empty;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetPassword(passwordBox, StringToSecureString(passwordBox.Password));
            }
        }

        private static SecureString StringToSecureString(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var secure = new SecureString();
            foreach (var c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string SecureStringToString(SecureString input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            var ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(input);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }
    }
}