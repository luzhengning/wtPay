using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel;

namespace wtPayDAL
{
    public class TerminalAccess
    {
        /// <summary>
        /// 设备是否在线
        /// </summary>
        /// <returns></returns>
        public string isOnline()
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("findOnlineStatus"), parameters, null);

                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                OnLineInfo info = JsonConvert.DeserializeObject<OnLineInfo>(jsonText);
                if (info != null)
                {
                    if (info.data != null)
                    {
                        if (info.data.online)
                        {
                            return "在线";
                        }
                    }
                }
                return "离线";
            }catch(Exception ex)
            {
                log.Write("error:isOnline:" + ex.Message);
                return "离线";
            }
        }
    }
}
