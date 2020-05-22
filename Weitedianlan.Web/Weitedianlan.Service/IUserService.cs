using System;
using System.Collections.Generic;
using System.Text;

namespace Weitedianlan.Service
{
    public interface IUserService
    {
        bool Login(string userName, string userPwd);

        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        string LoginEncrypt(string encryptString, string encryptKey);
        /// <summary>
        /// 登录时使用的加密方法
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        string LoginDecrypt(string decryptString, string encryptKey);

    }
}
