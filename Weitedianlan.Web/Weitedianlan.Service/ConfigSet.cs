using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Weitedianlan.Service
{
    public class SqlContext
    {
        public SqlContext()
        { }

        public string Sqlcomandtext { set; get; }
    }

    public static class ConfigSet
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

        public static string GetConfigjson()
        {
            string path = Assembly.GetEntryAssembly().Location;
            var paths = path.Split(@"\");
            string temp = paths[paths.Length - 1].ToString();
            var str = path.Substring(0, path.Length - temp.Length).ToString() + "appsettings.json";
            using (StreamReader reader = new StreamReader(str))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                JsonReader jsonReader = new JsonTextReader(reader);

                var sql = jsonSerializer.Deserialize<SqlContext>(jsonReader);//.DeserializeObject<SqlContext>(jsonReader);

                return sql.Sqlcomandtext.Trim();
            }
        }

        public static string GetSite(string apiName)
        {
            return AppSetting(apiName);
        }
    }
}