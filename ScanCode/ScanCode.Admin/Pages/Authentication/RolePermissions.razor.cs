using Microsoft.AspNetCore.Components;
using MudBlazor;
using ScanCode.Web.Admin.Authenticated.Services;
using ScanCode.Web.Admin.Pages.Authentication.ViewModel;
using System.Security.Claims;

namespace ScanCode.Web.Admin.Pages.Authentication
{
    public partial class RolePermissions
    {
        // [Inject] private IRoleManager RoleManager { get; set; }

        // [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public string Id { get; set; }

        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        private RolePermission _model = new();
        private Dictionary<string, List<RoleClaimModel>> GroupedRoleClaims { get; } = new();

        //private IMapper _mapper;
        private RoleClaimModel _roleClaims = new();

        private RoleClaimModel _selectedItem = new();

        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canEditRolePermissions;
        private bool _canSearchRolePermissions;
        private bool _loaded;

        [Inject] private RoleClaimService Service { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ///获取权限
            //_currentUser = await _authenticationManager.CurrentUser();
            //_canEditRolePermissions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.Edit)).Succeeded;
            //_canSearchRolePermissions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.Search)).Succeeded;

            await GetRolePermissionsAsync();
            _loaded = true;
            //HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            //if (HubConnection.State == HubConnectionState.Disconnected)
            //{
            //    await HubConnection.StartAsync();
            //}
        }

        private async Task GetRolePermissionsAsync()
        {
            // _mapper = new MapperConfiguration(c => { c.AddProfile<RoleProfile>(); }).CreateMapper();
            var roleId = Id;
            ///获取角色所有权限
            var result = await Service.GetRoleClaimAsync(roleId);  //RoleManager.GetPermissionsAsync(roleId);
            if (result.IsSuccess)
            {
                _model.RoleId = roleId;
                _model.RoleClaims = result.Data as List<RoleClaimModel>;

                GroupedRoleClaims.Add("所有权限", _model.RoleClaims);
                foreach (var claim in _model.RoleClaims)
                {
                    if (GroupedRoleClaims.ContainsKey(claim.Group))
                    {
                        GroupedRoleClaims[claim.Group].Add(claim);
                    }
                    else
                    {
                        GroupedRoleClaims.Add(claim.Group, new List<RoleClaimModel> { claim });
                    }
                }
                if (_model != null)
                {
                    Description = $"管理用户角色 {roleId} 的权限";
                }
            }
            else
            {
                _snackBar.Add(result.Message, Severity.Error);
                _navigationManager.NavigateTo("/identity/roles");
            }
        }

        private async Task SaveAsync()
        {
            // var request = _mapper.Map<PermissionResponse, PermissionRequest>(_model);

            // var sss = GroupedRoleClaims;
            _model.RoleClaims = _model.RoleClaims.Where(w => w.Selected == true).ToList();

            var result = await Service.UpdateRoleClaimAsync(_model);// RoleManager.UpdatePermissionsAsync(request);
            if (result.IsSuccess)
            {
                _snackBar.Add(result.Message, Severity.Success);
                // await HubConnection.SendAsync(ApplicationConstants.SignalR.SendRegenerateTokens);
                // await HubConnection.SendAsync(ApplicationConstants.SignalR.OnChangeRolePermissions, _currentUser.GetUserId(), request.RoleId);
                _navigationManager.NavigateTo("/identity/roles");
            }
            else
            {
                //foreach (var error in result.Messages)
                //{
                _snackBar.Add(result.Message, Severity.Error);
                //}
            }
        }

        private bool Search(RoleClaimModel roleClaims)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (roleClaims.Value?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (roleClaims.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }
    }
}