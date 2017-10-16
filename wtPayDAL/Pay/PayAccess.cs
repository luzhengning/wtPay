using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.PayParamModel;

namespace wtPayDAL.Pay
{
    public class PayAccess
    {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo PayResNewAcc(PayResNewParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("payType", param.payType);
            parameters.Add("authcode", param.authcode);
            parameters.Add("orderno", param.orderno);
            parameters.Add("payMsg", param.payMsg);
            parameters.Add("remarks", param.remarks);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo RefundAcc(RefundParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("refundType", param.refundType);
            parameters.Add("authcode", param.authcode);
            parameters.Add("orderno", param.orderno);
            parameters.Add("refundMsg", param.refundMsg);
            parameters.Add("remarks", param.remarks);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo CorrectAcc(CorrectParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("correctMsg", param.correctMsg);
            parameters.Add("remarks", param.remarks);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
        /// <summary>
        /// 写卡状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo WriteCardAcc(WriteCardParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("write_card_status", param.write_card_status);
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("refundType", param.refundType);
            parameters.Add("authcode", param.authcode);
            parameters.Add("orderno", param.orderno);
            parameters.Add("refundMsg", param.refundMsg);
            parameters.Add("remarks", param.remarks);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
    }
}
