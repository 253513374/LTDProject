using PPK_Logistics.DataSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace PPK_Logistics.Config
{
    public class Tools
    {

        public  static string[] _ComName = SerialPort.GetPortNames();
        /// <summary>
        /// 堆垛同步失败的数据
        /// </summary>
        public static ObservableCollection<Checkin_ltjyStack> Checkin_ltjyStack_Error = new ObservableCollection<Checkin_ltjyStack>();

        /// <summary>
        /// listbox入库错误索引
        /// </summary>
        public static int Errorint = 0;

        public static string HproseUrl = ConfigurationManager.AppSettings["HpUrl"].ToString();

        public static int SFCount = 0;

        /// <summary>
        /// 几级包装关系
        /// </summary>
        public static int Norms = 0;

        /// <summary>
        /// 同步失败的数据
        /// </summary>
        public static ObservableCollection<PackData_Simple> PackData_Simpl_Error = new ObservableCollection<PackData_Simple>();

        /// <summary>
        /// 已经同步完成的数据
        /// </summary>
        public static int SendCount = 0;

        /// <summary>
        /// 数据同步失败数量
        /// </summary>
        public static int SendError = 0;

        public static string AppPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string AppPathDate
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(T _object)
        {
            try
            {
                //T t = null;

                if (!File.Exists(AppPathDate + _object.GetType().ToString()))
                {
                    return _object;
                }
                using (var file = System.IO.File.Open(AppPathDate + _object.GetType().ToString(), FileMode.OpenOrCreate))
                {
                    T t = ProtoBuf.Serializer.Deserialize<T>(file);
                    return t;
                }
            }
            catch (System.Exception ex)
            {
                return _object;
            }
        }

        public static T DeSerializeShared<T>(T _object)
        {
            try
            {
                //T t = null;
                if (!File.Exists(AppPath + _object.GetType().ToString()))
                {
                    return _object;
                }
                using (var file = System.IO.File.Open(AppPath + _object.GetType().ToString(), FileMode.OpenOrCreate))
                {
                    return ProtoBuf.Serializer.Deserialize<T>(file);
                }
            }
            catch (System.Exception ex)
            {
                return _object;
            }
        }

        public static int GetSendStatus(bool statusbool)
        {
            if (statusbool)
            {
                IEnumerable<Checkin_ltjyStack> items = from objdataitem in App.ListCheckin_ltjyStack
                                                       where objdataitem.Status.ToString().Contains("成功")
                                                       select objdataitem;
                return items.ToList().Count;
            }
            else
            {
                IEnumerable<Checkin_ltjyStack> items = from objdataitem in App.ListCheckin_ltjyStack
                                                       where objdataitem.Status.ToString() != "成功"
                                                       select objdataitem;
                return items.ToList().Count;
            }
        }

        /// <summary>
        /// 获取12位序号正则
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ReplaceScanWord(string v)
        {
            string pattern = @"^[\w\W]*?(\d{12})$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);//正则表达式
            Match match = regex.Match(v);
            if (string.IsNullOrEmpty(match.Groups[1].Value)) return "";
            return match.Groups[1].Value;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T _object)
        {
            try
            {
                using (var file = System.IO.File.Create(AppPathDate + _object.GetType().ToString()))
                {
                    ProtoBuf.Serializer.Serialize(file, _object);
                    return "OK";
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        public static string SerializeShared<T>(T _object)
        {
            try
            {
                using (var file = System.IO.File.Create(AppPath + _object.ToString()))
                {
                    ProtoBuf.Serializer.Serialize(file, _object);
                    return "OK";
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 返回数据库连接串
        /// </summary>
        /// <param name="_v"></param>
        /// <returns></returns>
        public static string ConnectionString(string _v)
        {
            return ConfigurationManager.ConnectionStrings[_v].ToString().Trim();
        }

        /// <summary>
        /// 返回其他设置字符串
        /// </summary>
        /// <param name="_v"></param>
        /// <returns></returns>
        public static string GetAppSettings(string _v)
        {
            return ConfigurationManager.AppSettings[_v].ToString().Trim();
        }

        public static bool RegexValidate(string validateString)
        {
            string regexString = "^[0-9]*$";

            Regex regex = new Regex(regexString);

            return regex.IsMatch(validateString.Trim());
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out Point pt);



        public static int indexRowColmn=0;

        public static Point[] _point = new Point[] {new Point(1,0), new Point(1, 2), new Point(2, 0), new Point(2, 2) };



    }
}