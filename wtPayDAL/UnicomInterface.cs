using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.UnicomModel;
using wtPayBLL;
using System.Configuration;
using Newtonsoft.Json;

namespace wtPayDAL
{
    public class UnicomInterface
    {
        /// <summary>
        /// 联通查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UnicomQueryInfo query(UnicomQueryParam param)
        {
            UnicomQueryInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("phoneNo", param.phoneNo);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("UnicomQueryName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<UnicomQueryInfo>(jsonText);

            return info;
        }
        /// <summary>
        /// 发送订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UnicomOrderInfo order(UnicomOrderParam param)
        {
            UnicomOrderInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("phoneNo", param.phoneNo);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("shopType", param.shopType);
            parameters.Add("account_no",param.account_no);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderUnicom"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<UnicomOrderInfo>(jsonText);

            return info;
        }

        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public UnicomPayResInfo payres(UnicomPayResParam param)
        {
            UnicomPayResInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);

            parameters.Add("orderno", param.orderno);
            parameters.Add("phoneNo", param.phoneNo);
            parameters.Add("accountNo", param.accountNo);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("payCode", param.payCode);
            parameters.Add("terminalNo", param.terminalNo);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("UnicomPayresName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<UnicomPayResInfo>(jsonText);

            return info;
        }
    }
}
