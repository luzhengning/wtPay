using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using wtPayModel.SystemModel;
using wtPayBLL;
using System.Configuration;

namespace wtPayDAL
{
    public class SystemOrderAccess
    {
        public static string id = "0";
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public static SystemOrder getSystemOrder()
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("id", id);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("systemOrder"), parameters, null);
            log.Write("指令："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            SystemOrder systemOrder = JsonConvert.DeserializeObject<SystemOrder>(jsonText);
            id = systemOrder.data.id;
            if (SysBLL.IsTest.Equals("正式")) // GCResourceAccess.adv_id = SysConfigHelper.readerNode("GcAdv_id");
                ComputerBLL.WriteOrderID(id);
            else
                 SysConfigHelper.writerNode("sysOrderId", id);
            return systemOrder;
        }
        /// <summary>
        /// 执行结果通知
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SaveResualt(string info)
        {
            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("log.t_id", ConfigurationManager.AppSettings["MechineNo"]);
                parameters.Add("log.cmd_id", id);
                parameters.Add("log.cmd_result", info);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("systemOrderSaveResualt"), parameters, null);
                if (SysBLL.IsTest.Equals("正式")) // GCResourceAccess.adv_id = SysConfigHelper.readerNode("GcAdv_id");
                    ComputerBLL.WriteOrderID(id);
                else
                    SysConfigHelper.writerNode("sysOrderId", id);
                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                return true;
            }catch(Exception ex)
            {
                log.Write("error:SystemOrderAccess:SaveResualt(string info):"+ex.Message);
            }
            return false;
        }
    }
}
