//using BlazorHero.CleanArchitecture.Client.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Wtdl.Admin.Authenticated;

namespace Wtdl.Admin.Shared.Components
{
    public partial class UserCard
    {
        [Parameter] public string Class { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        [Parameter] public string ImageDataUrl { get; set; }

        [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject] private CustomAuthenticationService Service { set; get; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            //  var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = authenticationStateTask.Result.User;
            if (user.Identity.IsAuthenticated)
            {
                this.Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                this.FirstName = user.Identity.Name;
                this.SecondName = "";
                if (this.FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }

                // var UserId = user.GetUserId();
                var imageResponse = ""; //await _localStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
                if (!string.IsNullOrEmpty(imageResponse))
                {
                    ImageDataUrl = imageResponse;
                }

                StateHasChanged();
            }

            StateHasChanged();
        }
    }
}