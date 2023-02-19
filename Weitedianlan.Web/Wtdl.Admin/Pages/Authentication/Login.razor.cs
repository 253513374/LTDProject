//using Blazored.FluentValidation;
//using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Wtdl.Admin.Authenticated;

namespace Wtdl.Admin.Pages.Authentication
{
    public partial class Login
    {
        private FluentValidationValidator _fluentValidationValidator = new FluentValidationValidator();

        //  private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private LoginModel _tokenModel = new();

        private MudForm mudForm;

        // [Inject] private ISnackbar _snackbar { set; get; }
        //   [Inject] private NavigationManager Navigation { get; set; }

        //[Inject] private AccountService _accountService { get; set; }

        //[Inject]
        //private CustomAuthenticationStateProvider authStateProvider { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    //var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //    //if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        //    //{
        //    //    Navigation.NavigateTo("/");
        //    //}
        //}

        //private async Task SubmitAsync()
        //{
        //    await mudForm.Validate();

        //    if (mudForm.IsValid)
        //    {
        //        var model = mudForm.Model as LoginModel;
        //        var result = await _accountService.LoginUserAsync(model);

        //        if (result.Succeeded)
        //        {
        //            // StateHasChanged();
        //            _navigationManager.NavigateTo("/");
        //            _snackbar.Add("欢迎使用系统，", Severity.Success);
        //        }
        //        else
        //        {
        //            _snackbar.Add($"{result.Error}-{result.ErrorDescription}", Severity.Error);
        //        }
        //    }
        //    //var result = await _authenticationManager.Login(_tokenModel);
        //    //if (!result.Succeeded)
        //    //{
        //    //    foreach (var message in result.Messages)
        //    //    {
        //    //        _snackBar.Add(message, Severity.Error);
        //    //    }
        //    //}
        //}

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

        //private void FillAdministratorCredentials()
        //{
        //    _tokenModel.Email = "mukesh@blazorhero.com";
        //    _tokenModel.Password = "123Pa$$word!";
        //}

        //private void FillBasicUserCredentials()
        //{
        //    _tokenModel.Email = "john@blazorhero.com";
        //    _tokenModel.Password = "123Pa$$word!";
        //}
    }

    /// <summary>
    /// 登录表单
    /// </summary>
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}