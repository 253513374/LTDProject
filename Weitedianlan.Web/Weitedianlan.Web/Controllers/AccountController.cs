using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weitedianlan.Service;
using Weitedianlan.Model.Entity;
using Weitedianlan.Web.Application;

namespace Weitedianlan.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountModel model)
        {
            //验证模型是否正确
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //调用服务验证用户名密码
            if (!_userService.Login(model.UserName, model.UserPwd))
            {
                ModelState.AddModelError(nameof(model.UserPwd), "用户名或密码错误");
                return View(model);
            }
            //加密用户名写入cookie中，AdminAuthorizeAttribute特性标记取出cookie并解码除用户名
            var encryptValue = _userService.LoginEncrypt(model.UserName, ApplicationKeys.User_Cookie_Encryption_Key);

            HttpContext.Response.Cookies.Append(ApplicationKeys.User_Cookie_Key, encryptValue, new CookieOptions()
            {
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            }) ;
            
            return Redirect("/");
        }
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(ApplicationKeys.User_Cookie_Key);
            return Redirect(WebContext.LoginUrl);
        }
    }
}