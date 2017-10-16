using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayCommon;
using wtPayModel.BroadCas;
using Newtonsoft.Json;

namespace wtPayDAL
{
    public class BroadCasInterface
    {
        /// <summary>
        /// 广电登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BroadCasLoginInfo BroadCasLogin(BroadCasLoginParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BroadCasLoginName"), parameters, null);
           
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<BroadCasLoginInfo>(jsonText);
        }
        /// <summary>
        /// 广电查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BroadCasQueryInfo BroadCasQuery(BroadCasQueryParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("paymentno", param.paymentno);
            string jsonText = null;
            int count = 2;
            for (int i = 1; i <= count; i++)
            {
                jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BroadCasQueryName"), parameters, null);
                if (jsonText == null) continue;
                break;
            }
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<BroadCasQueryInfo>(jsonText);
            
        }
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BroadCasOrderInfo BroadCasOrder(BroadCasOrderParam param)
        {
            BroadCasOrderInfo broadCasOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("balenceNO", param.balenceNO);
            parameters.Add("shopType", param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = null;
            int count = 2;
            for (int i = 1; i <= count; i++)
            {
                jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderBroadCas"), parameters, null);
                if (jsonText == null) continue;
                break;
            }
            if (jsonText == null) return null;
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            broadCasOrderInfo = JsonConvert.DeserializeObject<BroadCasOrderInfo>(jsonText);
            if (broadCasOrderInfo == null)
            {
                return null;
            }
            return broadCasOrderInfo;
        }
        /// <summary>
        /// 支付通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BroadCasPayresInfo BroadCasPayres(BroadCasPayresParam param)
        {
            BroadCasPayresInfo broadCasPayresInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("terminalNo", param.terminalNo);
            string jsonText = null;
            int count = 2;
            for (int i = 1; i <= count; i++)
            {
                jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BroadCasPayresName"), parameters, null);
                if (jsonText == null) continue;
                break;
            }
            if (jsonText == null) return null;
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            broadCasPayresInfo = JsonConvert.DeserializeObject<BroadCasPayresInfo>(jsonText);
            if (broadCasPayresInfo == null)
            {
                return null;
            }
            return broadCasPayresInfo;
        }
    }
}
