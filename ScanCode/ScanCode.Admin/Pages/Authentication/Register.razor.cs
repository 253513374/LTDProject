using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Register
    {
        //private FluentValidationValidator _fluentValidationValidator = new FluentValidationValidator();

        // private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private RegisterModel _registerUserModel = new();

        private async Task SubmitAsync()
        {
            //var response = await _userManager.RegisterUserAsync(_registerUserModel);
            //if (response.Succeeded)
            //{
            //    _snackBar.Add(response.Messages[0], Severity.Success);
            //    _navigationManager.NavigateTo("/login");
            //    _registerUserModel = new RegisterRequest();
            //}
            //else
            //{
            //    foreach (var message in response.Messages)
            //    {
            //        _snackBar.Add(message, Severity.Error);
            //    }
            //}
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        public class RegisterModel
        {
            public string UserName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }
    }
}