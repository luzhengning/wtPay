using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;

namespace wtPayDAL
{
    public class WtPayAccess
    {
        /// <summary>
        /// 上送版本号
        /// </summary>
       public static void insertVersion()
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
                parameters.Add("version", SysConfigHelper.readerNode("currentVersion"));
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("insertVersionName"), parameters, null);
                log.Write("上送版本号："+jsonText);
            }catch(Exception ex)
            {
                log.Write("error: 上送版本号:" + ex.Message);
            }
        }
    }
}
