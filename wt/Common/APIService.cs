using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{

    public class APIService
    {

        /// <summary>
        /// 调用WEBAPI方法
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="body">参数</param>
        /// <returns></returns>
        public static string HttpPostInterface(string InterfaceName, string body, string method = "POST")
        {
            try
            {

                string url = PubConstant.VirtualPath + InterfaceName;
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToUpper().ToString();
                request.Accept = "text/html, application/xhtml+xml,text/plain, */*";
                request.ContentType = "application/json";
                request.UserAgent = "app";



                //if (token != "")
                //{
                //    request.Headers.Add("token", token);
                //}
                //body = "{\"Account\": \"chenjian\",\"Passwords\": \"aaaaa\"}";
                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;
                if (method.Equals("POST"))
                {
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                HttpWebResponse response;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    try
                    {
                        string respmsg = System.Web.HttpUtility.UrlDecode(reader.ReadToEnd(), System.Text.Encoding.UTF8);

                        return Regex.Replace(Regex.Unescape(respmsg), @"[\r\n]", "");
                    }
                    catch (WebException ex)
                    {
                        throw new Exception("XX" + ex.Message);
                    }
                }

            }
            catch (WebException ex)
            {

                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    Console.WriteLine("Error code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default))
                    {
                        string text = reader.ReadToEnd();

                        int start = text.IndexOf("System.Exception:");
                        string s = text.Substring(start, 2000);
                        throw new Exception("XX" + s);
                    }
                }
                else
                {
                    throw new Exception("XX" + ex.Message);
                }

            }
        }

        /// <summary>
        /// POST提交
        /// </summary>
        /// <param name="InterfaceName">地址</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static string HttpPost(string InterfaceName, string body)
        {
            try
            {
                //把用户传过来的数据转成“UTF-8”的字节流
                Encoding encoding = Encoding.UTF8;
                //先根据用户请求的uri构造请求地址
                string url = PubConstant.VirtualPath + InterfaceName;
                //创建Web访问对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                //把用户传过来的数据转成“UTF-8”的字节流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(body);

                myRequest.Method = "POST";
                myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json;charset=utf-8";
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;
                //发送请求
                Stream stream = myRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();

                //获取接口返回值
                //通过Web访问对象获取响应内容
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                reader.Close();
                myResponse.Close();
                return returnXml;
            }
            catch (WebException ex)
            {
                return "";
            }
        }

        /// <summary>
        /// POST提交
        /// </summary>
        /// <param name="InterfaceName">地址</param>
        /// <param name="body">内容</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static string HttpPost(string InterfaceName, string body, string token)
        {
            try
            {
                //把用户传过来的数据转成“UTF-8”的字节流
                Encoding encoding = Encoding.UTF8;
                //先根据用户请求的uri构造请求地址
                string url = PubConstant.VirtualPath + InterfaceName;
                //创建Web访问对象
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                //把用户传过来的数据转成“UTF-8”的字节流
                byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(body);
                myRequest.Headers.Add("Authorization", "Bearer " + token);
                myRequest.Method = "POST";
                myRequest.ContentLength = buf.Length;
                myRequest.ContentType = "application/json;charset=utf-8";
                myRequest.MaximumAutomaticRedirections = 1;
                myRequest.AllowAutoRedirect = true;
                //发送请求
                Stream stream = myRequest.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();

                //获取接口返回值
                //通过Web访问对象获取响应内容
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
                reader.Close();
                myResponse.Close();
                return returnXml;
            }
            catch (WebException ex)
            {
                return "";
            }
        }
        /// <summary>
        /// GET获取文件内容
        /// </summary>
        /// <param name="Url">地址</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static string GetFunction(string Url, string token)
        {
            try
            {
                string serviceAddress = PubConstant.VirtualPath + Url;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Method = "GET";

                // request.ContentType = "text/html;charset=UTF-8";
                request.ContentType = "application/json;charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                myResponseStream.WriteTimeout = 90000;
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (WebException ex)
            {
                return "";
            }
        }

        /// <summary>
        /// GET获取文件内容
        /// </summary>
        /// <param name="Url">地址</param>
        /// <returns></returns>
        public static string GetFunction(string Url)
        {
            try
            {
                string serviceAddress = PubConstant.VirtualPath + Url;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

                request.Method = "GET";

                // request.ContentType = "text/html;charset=UTF-8";
                request.ContentType = "application/json;charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                myResponseStream.WriteTimeout = 90000;
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (WebException ex)
            {
                return "";
            }
        }



    }
}
