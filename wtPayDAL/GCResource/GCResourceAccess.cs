using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.MediaResourceModel;

namespace wtPayDAL.GCResource
{
    /// <summary>
    /// 媒体更新
    /// </summary>
    public class GCResourceAccess
    {
        public static string adv_id="";

        /// <summary>
        /// 左侧广告图是否更新
        /// </summary>
        public static bool isUpdateLeftGC = false;

        /// <summary>
        /// 是否更新广告
        /// </summary>
        /// <returns></returns>
        public FindIsUpdateAdvInfo findIsUpdateAdv()
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("adv_id", adv_id);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("findIsUpdateAdvName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<FindIsUpdateAdvInfo>(jsonText);
        }
        /// <summary>
        /// 广告详情获取,1、缴费系统主界面轮播图2、缴费系统左侧轮播图3、缴费系统顶部系统提示消息4、顶部广告屏大屏视频5、顶部广告屏小屏视
        /// </summary>
        /// <returns></returns>
        public FindDownAdvInfo findDownAdv(string type,string busi_type)
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("type", type);
                parameters.Add("adv_id", adv_id);
                parameters.Add("busi_type", busi_type);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("findDownAdvName"), parameters, null);

                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                return JsonConvert.DeserializeObject<FindDownAdvInfo>(jsonText);
            }catch(Exception ex) { return null; }
        }

        /// <summary>
        /// 更新结果通知参数
        /// </summary>
        /// <param name="m_id"></param>
        /// <param name="m_name"></param>
        public void recordAdvUpdateLog(string m_id,string m_name)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("m_id", m_id);
            parameters.Add("m_name", m_name);
            parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("recordAdvUpdateLogName"), parameters, null);
            log.Write("广告更新结果通知：" + jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            //return JsonConvert.DeserializeObject<FindDownAdvInfo>(jsonText);
        }
        /// <summary>
        /// 广告播放状态通知
        /// </summary>
        public void controlAdvPlayStatus(string m_id, string m_name,string play_status)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("m_id", m_id);
            parameters.Add("m_name", m_name);
            parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("play_status", play_status);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("controlAdvPlayStatusName"), parameters, null);
        }
    }
}
