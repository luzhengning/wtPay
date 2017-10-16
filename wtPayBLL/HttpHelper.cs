using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Encryption;
using System.Collections;
using wtPayCommon;
namespace wtPayBLL
{
   public  class HttpHelper
    {

        public static string getHttp(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            string retString = null;
            Hashtable ht = new Hashtable();

            foreach (string key in parameters.Keys)
            {
                //if (!key.Equals("orderText"))
                    //ht.Add(key.ToLower(), parameters[key]);
            }
            //string mac = EncryptionMD5.MD5(ht, SysConfigHelper.readerNode("macKey"));

            //if (!parameters.ContainsKey("mac"))
            //{
            //    parameters.Add("mac".ToLower(), mac);
            //}
            //else
            //{
            //    parameters["mac"] = mac;
            //}
            url = url + "?";
            StringBuilder buffer = new StringBuilder();
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }

            retString = getHttp(url, buffer.ToString(), charset);
            return retString;
        }


        public static string getHttp(string url, string parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            string retString = null;
            try
            {
               
                request = (HttpWebRequest)WebRequest.Create(url);//"http://192.168.31.154/webbus/"
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 1000*50;

                if (charset == null)
                {
                    charset = Encoding.GetEncoding("utf-8");
                }
                byte[] data = charset.GetBytes(parameters);
                //请求发送时间
                DateTime start = System.DateTime.Now;
            
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                //请求响应时间
                DateTime end = System.DateTime.Now;


                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, charset);
                retString = myStreamReader.ReadToEnd();
                return retString;
            }
            catch (Exception e)
            {
                throw new Exception("网络异常");
            }
            finally
            {
                myStreamReader.Close();
                myResponseStream.Close();
            }
        }

        public static string getHttp( IDictionary<string, string> parameters, Encoding charset)
        {
            string retString = null;
            Hashtable ht = new Hashtable();

            foreach (string key in parameters.Keys)
            {
                if (!key.Equals("orderText"))
                    ht.Add(key.ToLower(), parameters[key]);
            }
            string mac = EncryptionMD5.MD5(ht, SysConfigHelper.readerNode("macKey"));

            if (!parameters.ContainsKey("mac"))
            {
                parameters.Add("mac".ToLower(), mac);
            }
            else
            {
                parameters["mac"] = mac;
            }
            string url = SysConfigHelper.readerNode("mechineSign");

            url = url + "?";
            StringBuilder buffer = new StringBuilder();
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            retString = getHttp2(url, buffer.ToString(), charset);
            return retString;
        }



        public static string getHttp2(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            string retString = null;
            Hashtable ht = new Hashtable();

            foreach (string key in parameters.Keys)
            {
                if (!key.Equals("orderText"))
                    ht.Add(key.ToLower(), parameters[key]);
            }
            //string mac = EncryptionMD5.MD5(ht, SysConfigHelper.readerNode("macKey"));

            //if (!parameters.ContainsKey("mac"))
            //{
            //    parameters.Add("mac".ToLower(), mac);
            //}
            //else
            //{
            //    parameters["mac"] = mac;
            //}
            url = url + "?";
            StringBuilder buffer = new StringBuilder();
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
            }
            retString = getHttp2(url, buffer.ToString(), charset);
            return retString;
        }


        public static string getHttp2(string url, string parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            string retString = null;
            try
            {
                string str = url;
                request = (HttpWebRequest)WebRequest.Create(str);//"http://192.168.31.154/webbus/"
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 1000 * 50;

                if (charset == null)
                {
                    charset = Encoding.GetEncoding("utf-8");
                }
                byte[] data = charset.GetBytes(parameters);
                //请求发送时间
                DateTime start = System.DateTime.Now;
              

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                //请求响应时间
                DateTime end = System.DateTime.Now;


                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, charset);
                retString = myStreamReader.ReadToEnd();
                return retString;
            }
            catch (WebException e)
            {
                DateTime end = System.DateTime.Now;
              
                string msg = e.Message;
                throw new WtException(WtExceptionCode.Sys.NETWORK, e.Message);
            }
            finally
            {
                myStreamReader.Close();
                myResponseStream.Close();
            }
        }
        public static string sendPaymentLog(string parameters1, string parameters2, string parameters3, string shopType)
        {
            //Dictionary<string, string> map = new Dictionary<string, string>();
            //map.Add("orderText", parameters1);
            //if (parameters2 != null) map.Add("reconcStr", parameters2);
            //map.Add("shopNo", parameters3);
            //map.Add("shopType", shopType);

            //return "{\"data\":\"1\"}";
            ////return getHttp(SysConfigHelper.readerNode("savePaymentLog"), map, null);//"payment/savePaymentLog?"
            return null;
        }
        public static string sendPaymentLog(string parameters1, string parameters2, string parameters3, string shopType,string state)
        {
            //Dictionary<string, string> map = new Dictionary<string, string>();
            //map.Add("orderText", parameters1);
            //if (parameters2 != null) map.Add("reconcStr", parameters2);
            //map.Add("shopNo", parameters3);
            //map.Add("shopType", shopType);
            //map.Add("orderState",state);
            //return "{\"data\":\"1\"}";
            //return getHttp(SysConfigHelper.readerNode("savePaymentLog"), map, null);//"payment/savePaymentLog?"
            return null;
        }
       
    }
}
