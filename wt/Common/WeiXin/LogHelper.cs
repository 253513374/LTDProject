using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WeiXin
{
    public class LogHelper
    {

        public static void CreateWebLog(string logStr)
        {
            try
            {
                string dir = System.Web.HttpContext.Current.Server.MapPath("~/log");

                if (Directory.Exists(dir) == false)
                {

                    Directory.CreateDirectory(dir);

                }

                string strFilePath = System.Web.HttpContext.Current.Server.MapPath("~/log/log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

                FileInfo logFile = new FileInfo(strFilePath);

                System.IO.FileStream fs;

                if (logFile.Exists)
                {

                    fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Append);

                }

                else
                {

                    fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create);

                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.Default);

                sw.WriteLine("---------------------------------------------------------------------------------------");

                sw.WriteLine("-----------------------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------------------");

                sw.WriteLine("---------------------------------------------------------------------------------------");

                sw.WriteLine(logStr);

                sw.Close();

                fs.Close();

            }

            catch (Exception)
            {

            }

        }

    }
}
