using ScanCode.WPF.HubServer.Model;

namespace ScanCode.WPF.HubServer.Services
{
    public class UserResult
    {
        public bool Successed { get; set; }

        public string Message { get; set; } = string.Empty;

        public User User { get; set; }

        public static UserResult Success(User user)
        {
            return new UserResult
            {
                Successed = true,
                User = user
            };
        }

        //失败
        public static UserResult Failed(string message)
        {
            return new UserResult
            {
                Successed = false,
                Message = message
            };
        }
    }
}