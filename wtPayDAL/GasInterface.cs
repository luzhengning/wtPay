using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using wtPayModel.GasModel;
using wtPayBLL;
using System.Configuration;

namespace wtPayDAL
{
    /// <summary>
    /// 燃气数据访问
    /// </summary>
    public class GasInterface
    {
        /// <summary>
        /// 燃气卡认证登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public string GasLogin()
        {
            
            GasLoginInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            try
            {
                parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());
                parameters.Add("servicename", "DL001");
                parameters.Add("reqsn", SysBLL.getSerialNum());
                parameters.Add("loginId", SysBLL.getCpuNo());

                string jsonText = jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("GasLoginName"), parameters, null);

                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                info = JsonConvert.DeserializeObject<GasLoginInfo>(jsonText);
                //同步系统时间
                SysBLL.SetSystemTime(info.msghead.trandatetime);
                GasShowInfo.rqhints = info.msgtext.rqhints;
                if (info != null)
                {
                    if (info.msgrsp != null)
                    {
                        if (info.msgrsp.authcode != null)
                        {
                            return info.msgrsp.authcode;
                        }
                    }
                }
            }catch(Exception ex) { log.Write("error:燃气登录认证异常:" + ex.Message); }
            return "";
        }

        /// <summary>
        /// 燃气卡查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public GasQueryInfo GasQuery(GasQueryParam param)
        {
            GasQueryInfo gasQueryInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
           
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("chargeAmount", param.chargeAmount);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("GasQueryName"), parameters, null);
                
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            gasQueryInfo = JsonConvert.DeserializeObject<GasQueryInfo>(jsonText);
                
            return gasQueryInfo;
        }

        /// <summary>
        /// 燃气卡提交订单接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public GasOrderInfo GasOrder(GasOrderParam param)
        {
            GasOrderInfo gasOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("shopType", param.shopType);
            parameters.Add("servicename", "RQ002");
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("authcode",SysBLL.Authcode);
            parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("chargeAmount", param.chargeAmount);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderNoNewName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            gasOrderInfo = JsonConvert.DeserializeObject<GasOrderInfo>(jsonText);
            return gasOrderInfo;
        }
        /// <summary>
        /// 燃气支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public GasPayresInfo Payres(GasPayresParam param)
        {
            GasPayresInfo gasPayresInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("loginId", param.loginId);
            parameters.Add("terminalNo",param.terminalNo);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("GasPayresName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            gasPayresInfo = JsonConvert.DeserializeObject<GasPayresInfo>(jsonText);
            return gasPayresInfo;
        }
    }
}
