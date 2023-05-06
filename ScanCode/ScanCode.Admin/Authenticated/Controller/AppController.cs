using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScanCode.Share;
using ScanCode.Web.Admin.Pages.Authentication.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanCode.Web.Admin.Authenticated.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly AccountService _accountService;

        private readonly IConfiguration _configuration;

        public AppController(AccountService service, IConfiguration configuration)
        {
            _accountService = service;
            _configuration = configuration;
        }

        //// GET: api/<ValuesController>
        //[HttpGet("gettt")]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet("Login")]
        public async Task<LoginResult> LoginApp(string username, string password)
        {
            var loginresult = await _accountService.LoginUserAsync(new LoginModel()
            {
                UserName = username,
                Password = password
            });

            if (loginresult.Succeeded)
            {
                var claims = loginresult.Claims;

                var token = GenerateJwtToken(claims);

                return LoginResult.Success(token, username, loginresult.UserIdentifier);
            }

            return LoginResult.Failure($"登录失败:{loginresult.Error}-{loginresult.ErrorDescription}");
        }

        [HttpGet("GetToken")]
        public async Task<LoginResult> LoginAppAsync()
        {
            var token = GenerateJwtToken(new List<Claim>());

            return LoginResult.Success(token);
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var jwtclaims = new[]
            {
               // new Claim(ClaimTypes.)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };

            claims = claims.Union(jwtclaims).ToList();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddYears(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}