using System.Text.Json.Serialization;

namespace   Wtdl.Share
{
    public class LoginResult
    {
        public LoginResult()
        {
        }

        [JsonPropertyName("succeeded")]
        public bool Succeeded { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("userId")]
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