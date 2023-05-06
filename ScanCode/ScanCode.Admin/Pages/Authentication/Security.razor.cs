using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using ScanCode.Web.Admin.Authenticated;
using ScanCode.Web.Admin.Pages.Authentication.Fluent;
using ScanCode.Web.Admin.Pages.Authentication.ViewModel;

namespace ScanCode.Web.Admin.Pages.Authentication
{
    public partial class Security
    {
        private UpdatePasswordValidator _fluentValidationValidator = new();

        //private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private readonly UpdatePassword _passwordModel = new();

        [Inject] private AccountService Service { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private MudForm mudForm;

        private async Task ChangePasswordAsync()
        {
            await mudForm.Validate();
            if (mudForm.IsValid)
            {
                var usermodel = mudForm.Model as UpdatePassword;
                var user = AuthenticationStateTask.Result.User;

                _passwordModel.User = user;
                var response = await Service.ChangePasswordAsync(usermodel);
                if (response.Succeeded)
                {
                    _snackBar.Add("密码更新成功", Severity.Success);
                    _passwordModel.Password = string.Empty;
                    _passwordModel.ConfirmPassword = string.Empty;
                    _passwordModel.CurrentPassword = string.Empty;
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        _snackBar.Add(error.Description, Severity.Error);
                    }
                }
            }
        }

        private bool _currentPasswordVisibility;
        private InputType _currentPasswordInput = InputType.Password;
        private string _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool _newPasswordVisibility;
        private InputType _newPasswordInput = InputType.Password;
        private string _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility(bool newPassword)
        {
            if (newPassword)
            {
                if (_newPasswordVisibility)
                {
                    _newPasswordVisibility = false;
                    _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    _newPasswordInput = InputType.Password;
                }
                else
                {
                    _newPasswordVisibility = true;
                    _newPasswordInputIcon = Icons.Material.Filled.Visibility;
                    _newPasswordInput = InputType.Text;
                }
            }
            else
            {
                if (_currentPasswordVisibility)
                {
                    _currentPasswordVisibility = false;
                    _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    _currentPasswordInput = InputType.Password;
                }
                else
                {
                    _currentPasswordVisibility = true;
                    _currentPasswordInputIcon = Icons.Material.Filled.Visibility;
                    _currentPasswordInput = InputType.Text;
                }
            }
        }
    }
}