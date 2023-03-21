using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web;

namespace Common.WeiXin
{
    public class DataCache
    {

        public static void Insert(string key, object obj)
        {
            int expires = int.Parse(ConfigurationManager.AppSettings["TimeCache"]);
            DataCache.Insert(key, obj, expires);
        }

        public static void Insert(string key, object obj, int expires)
        {

            HttpContext.Current.Cache.Insert(key, obj, (CacheDependency)null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
        }

        public static bool IsExist(string strKey)
        {
            return HttpContext.Current.Cache[strKey] != null;
        }

        public static object Get(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }

        public static T Get<T>(string argKey)
        {

            if (HttpContext.Current.Cache.Get(argKey) != null)
                return (T)HttpContext.Current.Cache.Get(argKey);
            return default(T);
        }
        public static void RemoveAllCache(string CacheKey)
        {
            HttpRuntime.Cache.Remove(CacheKey);
        }

        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
                cache.Remove(enumerator.Key.ToString());
        }
    }
}
