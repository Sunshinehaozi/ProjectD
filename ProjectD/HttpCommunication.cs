using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace HttpCommunication
{
    public class HttpPost
    {
        /// <summary>
        /// Json方式 查询用户名和密码
        /// </summary>
        /// <returns></returns>
        public string GetUserByJson(string ReqURL,string name)
        {
            string requestData = "{'OrderCode':'GetUser','UserName':'"+ name + "'}";

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("RequestData", WebUtility.UrlEncode(requestData));

            string result = sendPost(ReqURL, param);
            return result;
        }

        /// <summary>
        /// Json方式 提交用户名和密码
        /// </summary>
        /// <returns></returns>
        public string PostUserByJson(string ReqURL, string name, string password)
        {
            string requestData = "{'OrderCode':'PostUser','UserName':'" + name + "','PassWord': '" + password + "'}";

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("RequestData", WebUtility.UrlEncode(requestData));

            string result = sendPost(ReqURL, param);
            return result;
        }
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>
        private string sendPost(string url, Dictionary<string, string> param)
        {
            string result = "";
            StringBuilder postData = new StringBuilder();
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    if (postData.Length > 0)
                    {
                        postData.Append("&");
                    }
                    postData.Append(p.Key);
                    postData.Append("=");
                    postData.Append(p.Value);
                }
            }
            byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = url;
                request.Accept = "*/*";
                request.Timeout = 30 * 1000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.Method = "POST";
                request.ContentLength = byteData.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Flush();
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream backStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                result = sr.ReadToEnd();
                sr.Close();
                backStream.Close();
                response.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
