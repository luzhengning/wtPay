using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using wtPayModel.WintopModel;
using wtPayBLL;
using System.Configuration;
using wtPayModel.PaymentModel;

namespace wtPayDAL
{
    public class WintopInterface
    {
        /// <summary>
        /// 万通卡登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopLoginInfo WintopLogin(WintopLoginParam param)
        {
            WintopLoginInfo wintopLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            wintopLoginInfo = JsonConvert.DeserializeObject<WintopLoginInfo>(jsonText);
            return wintopLoginInfo;
        }
        /// <summary>
        /// 万通卡查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopQueryInfo WintopQuery(WintopQueryparam param)
        {
            WintopQueryInfo wintopQueryInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("loginId", param.loginId);
            parameters.Add("password", param.md5Pwd);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopQueryName"), parameters, null);
           
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            wintopQueryInfo = JsonConvert.DeserializeObject<WintopQueryInfo>(jsonText);
            
            return wintopQueryInfo;
        }

        /// <summary>
        /// 万通卡提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopOrderInfo WintopOrder(WintopOrderParam param)
        {
            WintopOrderInfo wintopOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("wtcardid", param.wtcardid);
            parameters.Add("wtuserid", param.wtuserid);
            parameters.Add("type", param.type);
            parameters.Add("money", Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00017);
            parameters.Add("realAmout", Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00030);// parameters.Add("realAmout", Payment.wintopReChargeParam.wintopDiscountInfoMsgrspList.CZ00030);
            parameters.Add("reduceid", Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00016);
            parameters.Add("loginId", param.loginId);
            parameters.Add("shopType", param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderSmartCard"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            wintopOrderInfo = JsonConvert.DeserializeObject<WintopOrderInfo>(jsonText);
            return wintopOrderInfo;
        }

        /// <summary>
        /// 万通支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopPayresInfo payres(WintopPayresParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("wtcardid", param.wtcardid);
            parameters.Add("wtuserid", param.wtuserid);
            parameters.Add("type", param.type);
            parameters.Add("terminal", param.terminalNo);
            parameters.Add("terminalno", param.terminalno);
            parameters.Add("operator", param.operators);
            parameters.Add("deptno", param.deptno);
            
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopPayresName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            WintopPayresInfo wintopPayresInfo = JsonConvert.DeserializeObject<WintopPayresInfo>(jsonText);
            return wintopPayresInfo;
        }
        /// <summary>
        /// 万通卡挂失
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopLossReportInfo LossReport(WintopLossReportParam param)
        {
            WintopLossReportInfo wintopLossReportInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("wtcardid", param.Wtcardid);
            parameters.Add("loginId", param.LoginId);
            parameters.Add("validatecode", param.Validatecode);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintoplossReport"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            wintopLossReportInfo = JsonConvert.DeserializeObject<WintopLossReportInfo>(jsonText);

            return wintopLossReportInfo;
        }
        /// <summary>
        /// 万通卡发送短信
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopSendValidateCodeInfo sendValidateCode(WintopSendValidateCodeParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("loginId", param.LoginId);
            parameters.Add("wtcardid", param.Wtcardid);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopsendValidateCode"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopSendValidateCodeInfo>(jsonText); 
        }
        /// <summary>
        /// 万通卡消费明细查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopSpendDetailInfo SpendDetail(WintopSpendDetailParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("loginId", param.LoginId);

            parameters.Add("wtcardid", param.Wtcardid);
            parameters.Add("password", param.Password);
            parameters.Add("pageNo", param.PageNo);
            parameters.Add("pageSize", param.PageSize);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopquerySpendDetail"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopSpendDetailInfo>(jsonText); 
        }

        /// <summary>
        /// 万通卡充值明细查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopRechargeDetailInfo RechargeDetail(WintopRechargeDetailParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("loginId", param.LoginId);

            parameters.Add("wtcardid", param.Wtcardid);
            parameters.Add("password", param.Password);
            parameters.Add("pageNo", param.PageNo);
            parameters.Add("pageSize", param.PageSize);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopqueryRechargeDetail"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopRechargeDetailInfo>(jsonText);
        }
        /// <summary>
        /// 万通卡密码修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopUpdateWtPwdInfo updateWtPwd(WintopUpdateWtPwdParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.Authcode);
            parameters.Add("servicename", param.Servicename);
            parameters.Add("trandateTime", param.TrandateTime);
            parameters.Add("reqsn", param.Reqsn);
            parameters.Add("loginId", param.LoginId);

            parameters.Add("wtcardid", param.Wtcardid);
            parameters.Add("newpassword", param.Newpassword);
            parameters.Add("password", param.Password);
            parameters.Add("type", param.Type);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WintopupdateWtPwd"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopUpdateWtPwdInfo>(jsonText);
        }
        /// <summary>
        /// 查询一卡通状态
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public WintopStatusInfo queryCardStatus(string card,string Authcode)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", Authcode);
            parameters.Add("servicename", "WT007");
            parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("loginId", SysBLL.getCpuNo());

            parameters.Add("wtcardid", card);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryCardStatus"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopStatusInfo>(jsonText);
        }
        /// <summary>
        /// 充值优惠信息查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public WintopDiscountInfo queryDiscount(WintopDiscountParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", "WT103");
            parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("wtcardid", param.wtcardid);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryDiscountName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopDiscountInfo>(jsonText);
        }
        public WintopMessage findHintSpec()//findHintSpec
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("code", "TTTT");
            parameters.Add("serviceType", "4");
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("findHintSpec"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<WintopMessage>(jsonText);
        }
    }
}
