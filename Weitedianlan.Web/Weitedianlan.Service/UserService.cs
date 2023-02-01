using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Response;

namespace Weitedianlan.Service
{
    public class UserService : IUserService
    {
        private Db _Db;

        public UserService(Db db)
        {
            _Db = db;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public bool Login(string userName, string userPwd)
        {
            var useraccount = _Db.User.AsNoTracking().Where(w => w.UserID.Trim() == userName.Trim()).FirstOrDefault(); ;
            var pwd = SecurityHelper.Encrypt(userPwd);
            if (pwd.Trim() == useraccount.PWD.Trim())
            {
                return true;
            }
            else
            {
                return false;
            }
            //密码得加密啊
            // return userName == "admin" && userPwd == "123456";
        }

        /// <summary>
        /// 获取账户列表集合
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetUserList()
        {
            var response = new ResponseModel();
            try
            {
                var banners = _Db.User.ToList().OrderByDescending(c => c.UserID);
                response.Code = 200;
                response.Status = "出库集合获取成功";
                response.Data = new List<User>();

                response.Data = banners;
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Code = 400,
                    Status = ex.Message,
                    Data = new List<User>()
                };
            }

            return response;
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        public ResponseModel AddUser(User banner)
        {
            //var ba = new Banner { AddTime = DateTime.Now, Image = banner.Image, Url = banner.Url, Remark = banner.Remark };
            banner.PWD = SecurityHelper.Encrypt(banner.PWD);
            _Db.User.Add(banner);
            int i = _Db.SaveChanges();
            if (i > 0)
                return new ResponseModel { Code = 200, Status = "AddUser添加成功" };
            return new ResponseModel { Code = 0, Status = "AddUser添加失败" };
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        public ResponseModel DeleteUser(int userId)
        {
            var user = _Db.User.Find(userId);
            if (user == null)
                return new ResponseModel { Code = 0, Status = "账号不存在" };
            if (user.UserID.Trim() == "admin")
            {
                return new ResponseModel { Code = 0, Status = "账号无法删除" };
            }
            _Db.User.Remove(user);
            int i = _Db.SaveChanges();
            if (i > 0)
                return new ResponseModel { Code = 200, Status = "账号删除成功" };
            return new ResponseModel { Code = 0, Status = "账号删除失败" };
        }

        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public string LoginEncrypt(string encryptString, string encryptKey)
        {
            //return SecurityHelper.Encrypt(encryptString);
            return SecurityHelper.EncryptDES(encryptString, encryptKey);
        }

        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public string LoginDecrypt(string decryptString, string encryptKey)
        {
            return SecurityHelper.DecryptDES(decryptString, encryptKey);

            //return SecurityHelper.DecryptDES(decryptString, encryptKey);
        }
    }
}