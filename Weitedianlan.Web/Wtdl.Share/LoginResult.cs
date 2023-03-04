namespace Wtdl.Share
{
    public class LoginResult
    {
        public LoginResult()
        {
        }

        public bool Succeeded { get; set; }

        public string Error { get; set; }

        public string Token { get; set; }

        public string Username { get; set; }

        public string UserId { get; set; }

        public static LoginResult Failure(string error)
        {
            return new LoginResult()
            {
                Succeeded = false,
                Error = error
            };
        }

        public static LoginResult Success(string token, string username = "", string userid = "")
        {
            return new LoginResult()
            {
                Succeeded = true,
                Token = token,
                Username = username,
                UserId = userid
            };
        }
    }
}