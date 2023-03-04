namespace Weitedianlan.SqlServer.Service
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
      
        public bool ShouldRemember { get; set; }
    }
}