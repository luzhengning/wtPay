using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel;

namespace wtPayDAL
{
    public class MaintainSignAccess
    {
        /// <summary>
        /// 维护人员签到
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static MaintainSignInfo MaintainSign(string account,string pwd)
        {
            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("userCode", account);
                parameters.Add("userPwd", pwd);
                parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("MaintainSign"), parameters, null);

                return JsonConvert.DeserializeObject<MaintainSignInfo>(jsonText);
            }
            catch(Exception ex) { log.Write("维护人员签到异常："+ex.Message);return null; }
        }


        /// <summary>
        /// 维护人员更新设备异常
        /// </summary>
        /// <returns></returns>
        public static string upException(string account)
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("login_name", account);
                parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("upExcption"), parameters, null);
                log.Write("更新设备异常返回："+jsonText);
                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                return json["message"].ToString();
            }
            catch (Exception ex) { log.Write("维护人员更新设备异常：" + ex.Message); return "提交失败"; }
        }


    }
}
