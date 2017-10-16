using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.ConfigModel;
using wtPayModel.PropSecModel;

namespace wtPayDAL
{
    public class PropSecInterface
    {
        /// <summary>
        /// 物业2登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropSecLoginInfo login(PropSecLoginParam param)
        {
            PropSecLoginInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("propSecLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropSecLoginInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 物业2读卡
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropSecQueryInfo query(PropSecQueryParam param)
        {
            PropSecQueryInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("authcode", param.authcode);
            parameters.Add("SC10009", param.SC10009);
            parameters.Add("SC10010", param.SC10010);
            parameters.Add("SC10007",param.ResidentialNo);
            parameters.Add("SC10011", param.SC10011);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("propSecQueryName"), parameters, null);
            log.Write("物业2读卡："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropSecQueryInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 物业2提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropSecOrderInfo order(PropSecOrderParam param)
        {
            PropSecOrderInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("authcode", param.authcode);
            parameters.Add("shopType", param.shopType);
            parameters.Add("AMOUNT", param.AMOUNT);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("SC10009", param.SC10009);
            parameters.Add("SC10010", param.SC10010);
            parameters.Add("SC10007", param.SC10007);
            parameters.Add("SC10008", param.SC10008);
            parameters.Add("SC10014", param.SC10014);
            parameters.Add("merchantNo",param.merchantNo);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("propSecOrderNoNew"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropSecOrderInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 获取表具列表
        /// </summary>
        /// <param name="SC10007"></param>
        /// <param name="SC10008"></param>
        /// <returns></returns>
        public PropMeterInfo queryMeter(PropSecOrderParam param,string SC10007, string SC10008)
        {
            PropMeterInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("authcode", param.authcode);

            parameters.Add("SC10007", SC10007);
            parameters.Add("SC10008", SC10008);
            parameters.Add("SC10009", ConfigPropParam.Prop2ManufacturerNum);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("propSecQueryName"), parameters, null);
            log.Write("获取表具列表:"+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropMeterInfo>(jsonText);
            return info;
        }
    }
}
//