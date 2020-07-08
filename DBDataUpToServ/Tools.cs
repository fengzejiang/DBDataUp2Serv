using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace DBDataUpToServ
{
    public class Tools
    {
        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        /// <summary>
        /// 提交POST请求到服务端
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="postString">请求参数</param>
        /// <returns></returns>
        public static string HttpPostInfo(string url, string postString)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可
            //postString = System.Web.HttpUtility.UrlEncode(postString, Encoding.UTF8);
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
            string res = Encoding.UTF8.GetString(responseData);//解码
            return res;
        }

        /// <summary>
        /// 转义正则特殊字符 （$%()*+.[]?\^{},|）
        /// </summary>
        /// <param name="keyword">要替换的</param>
        /// <returns></returns>
        public static string EscapeExprSpecialWord(string keyword)
        {
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                string[] fbsArr = { "\\","#", "%", "$", "(", ")", "*", "+", ".", "[", "]", "?", "^", "{", "}", "|" };
                Dictionary<string, string> dd = new Dictionary<string, string>
                {
                    { " ", "%20" },
                    { "''", "%22" },
                    { "#", "%23" },
                    { "%", "%25" },
                    { "&", "%26" },
                    { "(", "%28" },
                    { ")", "%29" },
                    { "+", "%2B" },
                    { ",", "%2C" },
                    { "?", "%3F" },
                    { "=", "%3D" },
                    { "@", "%40" },
                    { "\\", "%5C" },
                    { "|", "%7C" }
                };
                foreach (KeyValuePair<string, string> kvp in dd) {
                    if (keyword.Contains(kvp.Key)) {
                        keyword = keyword.Replace(kvp.Key, kvp.Value);
                    }
                }
            }
            return keyword;
        }


        

        public static long DateTimeToLong(DateTime d1)
        {
            TimeSpan delta = new TimeSpan();
            DateTime epoc = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            delta = DateTime.Parse(d1.ToString()).Subtract(epoc);
            long l1 = (long)delta.TotalMilliseconds;
            return l1;
        }

        public static DateTime LongToDateTime(long ticks)
        {
            var date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            date = date.AddMilliseconds(ticks);
            return date;
        }

        ///编码
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        ///解码
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        public static string Now() {
            return DateTime.Now.ToString(ICL.DATE_FMT_L);
        }


    }
}
