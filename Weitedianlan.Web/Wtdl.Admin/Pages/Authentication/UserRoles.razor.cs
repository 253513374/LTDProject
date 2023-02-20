using System;

using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Wtdl.Admin.Authenticated;
using Wtdl.Admin.Pages.Authentication.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Wtdl.Admin.Pages.Authentication
{
    public partial class UserRoles
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }
        public List<UserRoleModel> UserRolesList { get; set; } = new();
        public List<UserRoleModel> UserRolesListMark { get; set; } = new();

        private UserRoleModel _userRole = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        [Inject] private AccountService service { set; get; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private ClaimsPrincipal _currentUser;
        private bool _canEditUsers;
        private bool _canSearchRoles;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = (await authenticationStateTask).User;//await _authenticationManager.CurrentUser();
                                                                // _canEditUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Edit)).Succeeded;
                                                                //// _canSearchRoles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Search)).Succeeded;

            var userId = Id;
            var user = await service.GetUserByIdAsync(userId);//_userManager.GetAsync(userId);
            if (user is not null)
            {
                //var user = result.Data;
                //if (user != null)
                //{
                Title = $"{user.UserName} 的角色";
                Description = $"管理 {user.UserName} 的 角色";
                UserRolesList = await service.GetRolesByUserIdAsync(user.Id);
                UserRolesListMark.AddRange(UserRolesList);
            }

            _loaded = true;
        }

        private async Task SaveAsync()
        {
            var oldrole = UserRolesListMark.FirstOrDefault(f => f.RoleName.Contains(BaseRole.Aministrator));

            var newrole = UserRolesList.FirstOrDefault(f => f.RoleName.Contains(BaseRole.Aministrator));
            if (oldrole.Selected != newrole.Selected)
            {
                _snackBar.Add("不允许添加或删除管理员角色");
                return;
            }

            var request = new UpdateUserRoles()
            {
                UserId = Id,
                UserRoles = UserRolesList,
            };
            var result = await service.UpdateUserByRolesAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add("成功更新用户角色", Severity.Success);
                _navigationManager.NavigateTo("/identity/users");
            }
            else
            {
                foreach (var identityError in result.Errors)
                {
                    _snackBar.Add(identityError.Description, Severity.Error);
                }
            }
        }

        private bool Search(UserRoleModel userRole)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (userRole.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (userRole.RoleDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}