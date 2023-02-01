using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Weitedianlan.Service;

namespace Weitedianlan.Web.Application
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }

        public static T GetService<T>() where T : class
        {
            return Instance.GetService<T>();
        }
    }

    public static class WebContext
    {
        public static string AdminName
        {
            get
            {
                //获取cookie
                var hasCookie = ServiceLocator.GetService<IHttpContextAccessor>().HttpContext.Request.Cookies.TryGetValue(ApplicationKeys.User_Cookie_Key, out string encryptValue);

                if (!hasCookie || string.IsNullOrEmpty(encryptValue))
                    return null;
                var adminName = ServiceLocator.GetService<IUserService>().LoginDecrypt(encryptValue, ApplicationKeys.User_Cookie_Encryption_Key);
                return adminName;
            }
        }

        public const string LoginUrl = "/Account/Login";
    }

    public static class ConfigApp
    {
        private static IConfigurationSection _appSection = null;

        public static string AppSetting(string key)
        {
            string str = string.Empty;
            if (_appSection.GetSection(key) != null)
            {
                str = _appSection.GetSection(key).Value;
            }
            return str;
        }

        public static void SetAppSetting(IConfigurationSection section)
        {
            _appSection = section;
        }

        public static string GetSite(string apiName)
        {
            return AppSetting(apiName);
        }
    }
}