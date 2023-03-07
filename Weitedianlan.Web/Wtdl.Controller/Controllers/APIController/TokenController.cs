using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wtdl.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        // POST api/<TokenController>
        /// <summary>
        /// 生成Token。
        /// </summary>
        /// <param name="model">用户名与密码</param>
        /// <returns></returns>
        [HttpPost(Name = "Token")]
        public IActionResult Post([FromForm] LoginViewModel model)
        {
            if (model.Username != "apiuser" || model.Password != "apipassword")
            {
                return BadRequest();
            }
            //添加用户信息。可以中数据库中查询用户权限或者角色信息添加到Claim中
            //这样在使用Token的时候就可以根据Claim中的信息来判断用户是否有权限访问
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a94c36eaf8bf49f68099d8db3e28372e"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "apiissuer",//_config["Jwt:Issuer"],
                audience: "apiaudience",//_config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
            });
        }

        //// PUT api/<TokenController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<TokenController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}