﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ScanCode.Web.Admin.Authenticated;
using ScanCode.Web.Admin.Authenticated.IdentityModel;

namespace ScanCode.Web.Admin.Pages.Authentication
{
    public partial class Profile
    {
        //  private FluentValidationValidator _fluentValidationValidator;
        // private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private char _firstLetterOfName;
        private WtdlUser _profileModel = new();

        public string UserId { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject] private AccountService Service { get; set; }

        [Inject] private CustomAuthenticationService AuthenticationService { get; set; }

        private async Task UpdateProfileAsync()
        {
            var user = AuthenticationStateTask.Result.User;
            var response = await Service.UpdateAsync(_profileModel);
            if (response.Succeeded)
            {
                await AuthenticationService.LogOut();
                _snackBar.Add("你的个人资料已经更新。请登录访问更多内容", Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in response.Errors)
                {
                    _snackBar.Add(message.Description, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var user = AuthenticationStateTask.Result.User;
            //var state = await _stateProvider.GetAuthenticationStateAsync();
            //var user = state.User;
            //_profileModel.Email = user.GetEmail();
            //_profileModel.FirstName = user.GetFirstName();
            //_profileModel.LastName = user.GetLastName();
            //_profileModel.PhoneNumber = user.GetPhoneNumber();
            UserId = user.Identity.Name;

            var data = _profileModel = await Service.GetUserAsync(user);
            if (data is not null)
            {
                ImageDataUrl = data.Avatar;
            }
            if (data.UserName.Length > 0)
            {
                _firstLetterOfName = data.UserName[0];
            }
        }

        private IBrowserFile _file;

        [Parameter]
        public string ImageDataUrl { get; set; }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            //_file = e.File;
            //if (_file != null)
            //{
            //    var extension = Path.GetExtension(_file.Name);
            //    var fileName = $"{UserId}-{Guid.NewGuid()}{extension}";
            //    var format = "image/png";
            //    var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
            //    var buffer = new byte[imageFile.Size];
            //    await imageFile.OpenReadStream().ReadAsync(buffer);
            //    var request = new UpdateProfilePictureRequest { Data = buffer, FileName = fileName, Extension = extension, UploadType = Application.Enums.UploadType.ProfilePicture };
            //    var result = await _accountManager.UpdateProfilePictureAsync(request, UserId);
            //    if (result.Succeeded)
            //    {
            //        await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, result.Data);
            //        _snackBar.Add(_localizer["Profile picture added."], Severity.Success);
            //        _navigationManager.NavigateTo("/account", true);
            //    }
            //    else
            //    {
            //        foreach (var error in result.Messages)
            //        {
            //            _snackBar.Add(error, Severity.Error);
            //        }
            //    }
            //}
        }

        private async Task DeleteAsync()
        {
            //var parameters = new DialogParameters
            //{
            //    {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), $"{string.Format(_localizer["Do you want to delete the profile picture of {0}"], _profileModel.Email)}?"}
            //};
            //var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            //var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    var request = new UpdateProfilePictureRequest { Data = null, FileName = string.Empty, UploadType = Application.Enums.UploadType.ProfilePicture };
            //    var data = await _accountManager.UpdateProfilePictureAsync(request, UserId);
            //    if (data.Succeeded)
            //    {
            //        await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
            //        ImageDataUrl = string.Empty;
            //        _snackBar.Add(_localizer["Profile picture deleted."], Severity.Success);
            //        _navigationManager.NavigateTo("/account", true);
            //    }
            //    else
            //    {
            //        foreach (var error in data.Messages)
            //        {
            //            _snackBar.Add(error, Severity.Error);
            //        }
            //    }
            //}
        }
    }
}