using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wtdl.Admin.Authenticated;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wtdl.Admin.Pages.Authentication
{
    public partial class UserProfile
    {
        private bool _active;
        private char _firstLetterOfName;
        private string _userName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;

        private bool _loaded;

        [Inject] private AccountService service { set; get; }

        private async Task ToggleUserStatus()
        {
            //var request = new ToggleUserStatusRequest { ActivateUser = _active, UserId = Id };
            var result = await service.UpdateUserStatusAsync(_active, Id);
            if (result.Succeeded)
            {
                _snackBar.Add("成功更新用户状态", Severity.Success);
                _navigationManager.NavigateTo("/identity/users");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _snackBar.Add(error.Description, Severity.Error);
                }
                //  _snackBar.Add($"更新状态失败：{result.Errors.}", Severity.Error);
            }
        }

        [Parameter] public string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = Id;
            var user = await service.GetUserByIdAsync(userId);
            if (user is not null)
            {
                _userName = user.UserName;
                _email = user.Email;
                _phoneNumber = user.PhoneNumber;
                _active = user.IsActive.Value;
                //var data = await _accountManager.GetProfilePictureAsync(userId);
                //if (data.Succeeded)
                //{
                //    ImageDataUrl = data.Data;
                //}
                // }
                Title = $"{_userName} 的个人账户资料";
                Description = _email;
                if (_userName.Length > 0)
                {
                    _firstLetterOfName = _userName[0];
                }
            }

            _loaded = true;
        }
    }
}