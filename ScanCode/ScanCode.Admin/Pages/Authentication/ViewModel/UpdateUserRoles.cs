namespace ScanCode.Web.Admin.Pages.Authentication.ViewModel
{
    public class UpdateUserRoles
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}