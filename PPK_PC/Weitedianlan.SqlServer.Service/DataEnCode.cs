using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.SqlServer.Service
{
    public class DataEnCode
    {
        private byte[] desIV = new byte[] { 0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08 };
        private byte[] desKey = new byte[] { 0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08 };

        public string ChangeContent(int ReType, string strChar, int subLenght)
        {
            if (ReType == 1)
            {
                strChar = strChar.Replace("\r", "");        //Keycode 13 (Enter key)
                strChar = strChar.Replace(" ", "&nbsp;");        //Space
                strChar = strChar.Replace("\t", "    ");    //Tab
                strChar = strChar.Replace("<", "&lt;");
                strChar = strChar.Replace(">", "&gt;");
                strChar = strChar.Replace("\'", "&#39;");
                strChar = strChar.Replace("&", "&");
                strChar = strChar.Replace("\n", "<br>");
            }
            else if (ReType == 0)
            {
                strChar = strChar.Replace("&nbsp;", " ");
                strChar = strChar.Replace("<br />", "\n");
                strChar = strChar.Replace("<br>", "\n");
                strChar = strChar.Replace("<BR>", "\n");
                strChar = strChar.Replace("<Br>", "\n");
                strChar = strChar.Replace("<bR>", "\n");
                strChar = strChar.Replace("&lt;", "<");
                strChar = strChar.Replace("&gt;", ">");
                strChar = strChar.Replace("&#39;", "\'");
            }
            else
            {
                strChar = strChar.Replace("&lt;", "<");
                strChar = strChar.Replace("&gt;", ">");
                strChar = strChar.Replace("&nbsp;", " ");
                strChar = strChar.Replace("<br />", " ");
                strChar = strChar.Replace("<br>", " ");
                strChar = strChar.Replace("<BR>", " ");
                strChar = strChar.Replace("<Br>", " ");
                strChar = strChar.Replace("<bR>", " ");
                strChar = strChar.Replace("&#39;", "\'");
                if (subLenght > 0 && strChar.Length > subLenght)
                {
                    strChar = strChar.Substring(0, subLenght);
                }
            }
            return strChar;
        }

        public DataEnCode()
        {

        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密字符串</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = desKey;
            des.IV = desIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            byte[] ret = ms.ToArray();

            ms.Close();
            cs.Close();
            des.Clear();

            return Encoding.Default.GetString(ret);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string pToEncrypt)
        {
            if (pToEncrypt == null || pToEncrypt.ToString() == "")
            {
                return "";
            }
            else
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = desKey;
                des.IV = desIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                    ret.AppendFormat("{0:X2}", b);//格式化为十六进制

                ms.Close();
                cs.Close();
                des.Clear();
                return ret.ToString();
            }
        }

        public string NumberChangeChinese(int i)
        {
            string strReturn = "";
            switch (i)
            {
                case 0: strReturn = "无"; break;
                case 1: strReturn = "一"; break;
                case 2: strReturn = "二"; break;
                case 3: strReturn = "三"; break;
                case 4: strReturn = "四"; break;
                case 5: strReturn = "五"; break;
                case 6: strReturn = "六"; break;
                case 7: strReturn = "七"; break;
                case 8: strReturn = "八"; break;
                case 9: strReturn = "九"; break;
                case 10: strReturn = "十"; break;
            }
            return strReturn;
        }

        public string ReplaceBadChar(string strChar)
        {
            if (strChar == null || strChar.Trim() == "")
            {
                return "";
            }
            else
            {
                strChar = strChar.Replace("'", "''");
                strChar = strChar.Replace(";", "");
                strChar = strChar.Replace("*", "");
                strChar = strChar.Replace("?", "");
                strChar = strChar.Replace("(", "");
                strChar = strChar.Replace(")", "");
                strChar = strChar.Replace("<", "");
                strChar = strChar.Replace("=", "");
                strChar = strChar.Replace(">", "");
                return strChar.Trim();
            }
        }
    }
}
