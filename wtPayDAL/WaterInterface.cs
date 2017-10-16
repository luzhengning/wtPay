using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using wtPayModel.WaterModel;
using wtPayBLL;
using System.Configuration;

namespace wtPayDAL
{
    /// <summary>
    /// 水务接口
    /// </summary>
    public class WaterInterface
    {
        /// <summary>
        /// 水务认证登录接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WaterLoginInfo WaterLogin(WaterLoginParam param)
        {
            WaterLoginInfo waterLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WaterLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            waterLoginInfo = JsonConvert.DeserializeObject<WaterLoginInfo>(jsonText);
            return waterLoginInfo;
        }
        /// <summary>
        /// 获取水务缴费信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WaterQueryInfo WaterQuery(WaterQueryParam param)
        {
            WaterQueryInfo waterQueryInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WaterQueryName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            waterQueryInfo = JsonConvert.DeserializeObject<WaterQueryInfo>(jsonText);
            return waterQueryInfo;
        }
        /// <summary>
        /// 水务提交订单接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WaterOrderInfo WaterOrder(WaterOrderParam param)
        {
            WaterOrderInfo waterOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("billdate", param.billdate);
            parameters.Add("paymentamout", param.paymentamout);
            parameters.Add("loginId", param.loginId);
            parameters.Add("shopType", param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderWater"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            waterOrderInfo = JsonConvert.DeserializeObject<WaterOrderInfo>(jsonText);
            return waterOrderInfo;
        }
        public WaterPayresInfo WaterPayres(WaterPayresParam param)
        {
            WaterPayresInfo waterPayresInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("orderno", param.Orderno);
            parameters.Add("realAmout", param.RealAmout);
            parameters.Add("payCode", param.PayCode);
            parameters.Add("loginId", param.LoginId);
            parameters.Add("trandeNo", param.TrandeNo);
            parameters.Add("billDate", param.BillDate);
            parameters.Add("terminalNo", param.TerminalNo);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WaterPayResName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            waterPayresInfo = JsonConvert.DeserializeObject<WaterPayresInfo>(jsonText);
            return waterPayresInfo;
        }
    }
}
