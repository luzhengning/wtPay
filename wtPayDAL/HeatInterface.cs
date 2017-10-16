using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.HeatModel;

namespace wtPayDAL
{
    /// <summary>
    /// 热力访问接口
    /// </summary>
    public class HeatInterface
    {
        /// <summary>
        /// 热力登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HeatLoginInfo HeatLogin(HeatLoginParam param)
        {
            HeatLoginInfo heatLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("HeatLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            heatLoginInfo = JsonConvert.DeserializeObject<HeatLoginInfo>(jsonText);
            return heatLoginInfo;
        }
        /// <summary>
        /// 热力查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HeatQueryInfo HeatQuery(HeatQueryParam param)
        {
            HeatQueryInfo heatQueryInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("HeatQueryName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            heatQueryInfo = JsonConvert.DeserializeObject<HeatQueryInfo>(jsonText);
            return heatQueryInfo;
        }
        /// <summary>
        /// 热力订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HeatOrderInfo HeatOrder(HeatOrderParam param)
        {
            HeatOrderInfo heatOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("realAmout ", param.realAmout);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("loginId", param.loginId);
            parameters.Add("billDate", param.billDate);
            parameters.Add("shopType", param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderHeat"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            heatOrderInfo = JsonConvert.DeserializeObject<HeatOrderInfo>(jsonText);
            return heatOrderInfo;
        }
        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HeatPayresInfo HeatPayres(HeatPayresParam param)
        {
            HeatPayresInfo heatPayresInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("loginId", param.loginId);
            parameters.Add("payCode", param.payCode);
            parameters.Add("terminalNo",param.terminalNo);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("HeatPayresName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            heatPayresInfo = JsonConvert.DeserializeObject<HeatPayresInfo>(jsonText);
            return heatPayresInfo;
        }
    }
}
