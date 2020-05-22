using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Response;
using Weitedianlan.Service;


namespace Weitedianlan.Web.Controllers
{
    public class UserController : Controller
    {
        private UserService  UserService;
        public UserController(UserService userService)
        {
            UserService = userService;
        }
        // GET: User
        public ActionResult Index()
        {
            var userlist = UserService.GetUserList();
            return View(userlist);
        }

        // GET: User/Details/5
        public ActionResult UserAdd()
        {
            return View();
        }

        /// <summary>
        /// 账号添加
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddUser(User user)
        {

           //var re= UserService.AddUser(user);
           // if(re.Code==200)
           // {
           //     Json(new ResponseModel { Code = 0, Status = "请输入账号" });
           // }
            return Json(UserService.AddUser(user));
        }


        [HttpPost]
        public JsonResult DeleteUsers(int id)
        {
            if (id <= 0)
                return Json(new ResponseModel { Code = 0, Status = "参数有误" });
            return Json(UserService.DeleteUser(id));
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}