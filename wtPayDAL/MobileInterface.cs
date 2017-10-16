using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.Mobile;
using wtPayBLL;
using System.Configuration;
using Newtonsoft.Json;
using wtPayModel.BroadCas;

namespace wtPayDAL
{
    /// <summary>
    /// 移动缴费
    /// </summary>
    public class MobileInterface
    {
        /// <summary>
        /// 移动登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string BroadCasLogin(MobileLoginParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("mobileLogin"), parameters, null);
            //同步系统时间
            SysBLL.SetSystemTime(JsonConvert.DeserializeObject<BroadCasLoginInfo>(jsonText).msghead.trandatetime);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<BroadCasLoginInfo>(jsonText).msgrsp.authcode;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MobileQueryInfo query(MobileQueryParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("mobile",param.mobile);
            parameters.Add("authcode",param.authcode);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("mobileQuery"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<MobileQueryInfo>(jsonText);
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MobileOrderInfo order(MobileOrderParam param,MobileQueryInfo info) {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("mobile", param.mobile);
            parameters.Add("authcode", param.authcode);
            parameters.Add("paymentAmout",param.paymentAmout);

            parameters.Add("homeRegion", info.msgrsp.homeRegion);
            parameters.Add("homeOffice", info.msgrsp.homeOffice);
            parameters.Add("contractNo", info.msgrsp.contractNo);
            parameters.Add("overdueMoney", info.msgrsp.overdueMoney);
            parameters.Add("prepaidBalance", info.msgrsp.afterPrepaidBalance);
            parameters.Add("changeBalance", info.msgrsp.changeBalance);
            parameters.Add("channelNo", "002");
            parameters.Add("shopType",param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderMobile"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<MobileOrderInfo>(jsonText);
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public MobilePayresInfo payres(MobilePayresParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", param.loginId);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandNo);
            parameters.Add("channelNo", param.channelNo);
            parameters.Add("servicename", param.servicename);
            parameters.Add("mobile", param.mobile);
            parameters.Add("prepaidBalance", param.prepaidBalance);
            parameters.Add("homeOffice", param.homeOffice);
            parameters.Add("orderno", param.orderno);

            parameters.Add("homeRegion", param.homeRegion);
            parameters.Add("contractNo", param.contractNo);
            parameters.Add("authcode", param.authcode);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("changeBalance", param.changeBalance);
            parameters.Add("overdueMoney", param.overdueMoney);
            parameters.Add("terminalNo", param.terminalNo);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("mobilePayres"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<MobilePayresInfo>(jsonText);
        }
    }
}
