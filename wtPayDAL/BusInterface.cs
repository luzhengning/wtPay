using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using wtPayModel.BusModel;
using wtPayBLL;
using System.Configuration;
using Newtonsoft.Json.Linq;
using wtPayModel.PaymentModel;

namespace wtPayDAL
{
    public class BusInterface
    {
        /// <summary>
        /// 公交卡登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BusLoginInfo BusLogin(BusLoginParam param)
        {
            BusLoginInfo  busLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("loginId", param.loginId);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusLoginName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            busLoginInfo = JsonConvert.DeserializeObject<BusLoginInfo>(jsonText);
            return busLoginInfo;
        }

        /// <summary>
        /// 公交卡证书签到
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BusRegisterInfo BusRegister(BusRegisterParam param)
        {
            BusRegisterInfo busRegisterInfo = new BusRegisterInfo();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode",param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("btype", param.btype);
            parameters.Add("terno", param.terno);
            parameters.Add("userNo",param.userNo);
            parameters.Add("tradeno", param.tradeno);
            if(param.opno!=null) parameters.Add("opno", param.opno);
            parameters.Add("loginId", param.loginId);
            if(param.random!=null) parameters.Add("random",param.random);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusRegisterName"), parameters, null);

            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject msgrsp = (JObject)JsonConvert.DeserializeObject(jsonObj["msgrsp"].ToString());
            JObject msghead = (JObject)JsonConvert.DeserializeObject(jsonObj["msghead"].ToString());
            JObject cpumsg = (JObject)JsonConvert.DeserializeObject(jsonObj["cpumsg"].ToString());
            if(msgrsp!=null) busRegisterInfo.msgrsp = JsonConvert.DeserializeObject<BusRegisterMsgrsp>(msgrsp.ToString());
            if(msghead!=null) busRegisterInfo.msghead = JsonConvert.DeserializeObject<BusRegisterMsghead>(msghead.ToString());
            if (cpumsg!=null)
                busRegisterInfo.cpumsg = JsonConvert.DeserializeObject<BusRegisterCpuMsg>(cpumsg.ToString());
            return busRegisterInfo;
        }
        /// <summary>
        /// 公交卡充值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BusCpuCardInfo BusCpuCard(BusCpuCardParam param)
        {
            BusCpuCardInfo busCpuCardInfo = new BusCpuCardInfo();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.resqn);
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("billDate", param.billDate);
            parameters.Add("merchantNo", param.merchantNo);
            parameters.Add("orderno", param.orderno);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);
            parameters.Add("inapdu", param.inapdu);
            parameters.Add("step", param.step);
            parameters.Add("money", param.money);
            parameters.Add("serno", param.serno);
            parameters.Add("appsid", param.appsid);
            parameters.Add("btype", param.btype);
            parameters.Add("opno", param.opno);
            parameters.Add("scode", param.scode);
            parameters.Add("terno", param.terno);
            parameters.Add("tradeno", param.tradeno);
            parameters.Add("loginId", param.loginId);
            parameters.Add("CMTYPE", param.CMTYPE);
            parameters.Add("WMONEY", param.WMONEY);
            parameters.Add("APDUSUM", param.APDUSUM);
            parameters.Add("APDUDATA", param.APDUDATA);
            parameters.Add("APDUSW", param.APDUSW);
            parameters.Add("RETDATA", param.RETDATA);

            string jsonText = jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusCpuCardName"), parameters, null);
                
            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject msgrsp = (JObject)JsonConvert.DeserializeObject(jsonObj["msgrsp"].ToString());
            JObject cpumsg = (JObject)JsonConvert.DeserializeObject(jsonObj["cpumsg"].ToString());
            JObject msghead = (JObject)JsonConvert.DeserializeObject(jsonObj["msghead"].ToString());
                
            busCpuCardInfo.msgrsp = JsonConvert.DeserializeObject<BusCpuCardMsgrsp>(msgrsp.ToString());
            busCpuCardInfo.msghead = JsonConvert.DeserializeObject<BusCpuCardMsghead>(msghead.ToString());
            busCpuCardInfo.cpumsg = JsonConvert.DeserializeObject<BusCpuCardCpumsg>(cpumsg.ToString());

            BusPayParam.busShopNo = busCpuCardInfo.msgrsp.MERCHANTNO;
            BusPayParam.busClient = busCpuCardInfo.msgrsp.TERMINALNO;


            return busCpuCardInfo;
        }

        /// <summary>
        /// 公交查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BusQueryInfo BusQuery1(BusQueryParam param)
        {
            BusQueryInfo busQueryInfo = new BusQueryInfo();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode",param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("opno", param.opno);
            parameters.Add("scode", param.scode);
            parameters.Add("terno", param.terno);
            parameters.Add("tradeno", param.tradeno);
            parameters.Add("btype", param.btype);
            parameters.Add("inapdu", param.inapdu);
            parameters.Add("step", param.step);
            parameters.Add("faccard", param.faccard);
            parameters.Add("appsid", param.appsid);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusQueryName"), parameters, null);
            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject msgrsp = null;
            JObject msghead = null;
            JObject cpumsg = null;
            if (jsonObj["msgrsp"] != null) msgrsp = (JObject)JsonConvert.DeserializeObject(jsonObj["msgrsp"].ToString());
            if (jsonObj["msghead"] != null) msghead = (JObject)JsonConvert.DeserializeObject(jsonObj["msghead"].ToString());
            if (jsonObj["cpumsg"] != null) cpumsg = (JObject)JsonConvert.DeserializeObject(jsonObj["cpumsg"].ToString());
            busQueryInfo.msgrsp = JsonConvert.DeserializeObject<BusQueryMsgrsp>(msgrsp.ToString());
            busQueryInfo.msghead = JsonConvert.DeserializeObject<BusQueryMsghead>(msghead.ToString());
            busQueryInfo.cpumsg = JsonConvert.DeserializeObject<BusQueryCpumsg>(cpumsg.ToString());
                    
            return busQueryInfo;
        }
        /// <summary>
        /// 公交查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BusQueryThatInfo BusQuery2(BusQueryParam param)
        {
            BusQueryThatInfo busQueryInfo = new BusQueryThatInfo();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("opno", param.opno);
            parameters.Add("scode", param.scode);
            parameters.Add("terno", param.terno);
            parameters.Add("tradeno", param.tradeno);
            parameters.Add("btype", param.btype);
            parameters.Add("inapdu", param.inapdu);
            parameters.Add("step", param.step);
            parameters.Add("loginId", param.loginId);
            parameters.Add("APDUSUM", param.APDUSUM);
            parameters.Add("APDUDATA", param.APDUDATA);
            parameters.Add("APDUSW", param.APDUSW);
            parameters.Add("RETDATA", param.RETDATA);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusQueryName"), parameters, null);
                
            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject msgrsp = (JObject)JsonConvert.DeserializeObject(jsonObj["msgrsp"].ToString());
            JObject msghead = (JObject)JsonConvert.DeserializeObject(jsonObj["msghead"].ToString());
            JObject cpumsg = (JObject)JsonConvert.DeserializeObject(jsonObj["cpumsg"].ToString());
            busQueryInfo.msgrsp = JsonConvert.DeserializeObject<BusQueryThatMsgrsp>(msgrsp.ToString());
            busQueryInfo.msghead = JsonConvert.DeserializeObject<BusQueryThatMsghead>(msghead.ToString());
            busQueryInfo.cpumsg = JsonConvert.DeserializeObject<BusQueryThatCpumsg>(cpumsg.ToString());
                
            return busQueryInfo;
        }
        public BusPayresInfo BusPayres(BusPayresParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout+"00");
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);
            string jsonText = null;
            int count = 2;
            for (int i = 1; i <= count; i++)
            {
                jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("BusPayresName"), parameters, null);
                if (jsonText == null) continue;
                break;
            }
            if (jsonText == null) return null;

            BusPayresInfo busPayresInfo = JsonConvert.DeserializeObject<BusPayresInfo>(jsonText);
            return busPayresInfo;
        }
    }
}
