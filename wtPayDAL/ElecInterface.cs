using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using wtPayModel.ElecModel;
using wtPayBLL;
using System.Configuration;


namespace wtPayDAL
{
    public class ElecInterface
    {
        /// <summary>
        /// 电力登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ElecLoginInfo ElecLogin(ElecLoginParam param)
        {
            ElecLoginInfo elecLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("ElecLoginName"), parameters, null);
               
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecLoginInfo = JsonConvert.DeserializeObject<ElecLoginInfo>(jsonText);
              
            return elecLoginInfo;
          
        }

        /// <summary>
        /// 获取电网用户资料
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ElecQueryUserInfo ElecQueryUser(ElecQueryUserParam param)
        {
            ElecQueryUserInfo elecQueryUserInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
           
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("yhbh", param.yhbh);
            parameters.Add("queryId", param.queryId);
            parameters.Add("loginId", param.loginId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("ElecQueryUserName"), parameters, null);
                
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecQueryUserInfo = JsonConvert.DeserializeObject<ElecQueryUserInfo>(jsonText);
                
            return elecQueryUserInfo;
           
        }

        /// <summary>
        /// 电网查询电费接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ElecQueryElecInfo ElecQueryElec(ElecQueryElecParam param)
        {
            ElecQueryElecInfo elecQueryElecInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
          
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("yhbh", param.yhbh);
            parameters.Add("loginId", param.loginId);

            string jsonText =  HttpHelper.getHttp(SysConfigHelper.readerNode("ElecQueryElecName"), parameters, null);
               

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecQueryElecInfo = JsonConvert.DeserializeObject<ElecQueryElecInfo>(jsonText);
              
            return elecQueryElecInfo;
         
        }

        /// <summary>
        /// 电网获取订单接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ElecOrderInfo ElecOrder(ElecOrderParam param)
        {
            ElecOrderInfo elecOrderInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
         
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("paymentno", param.paymentno);
            parameters.Add("paymentamout", param.paymentamout);
            parameters.Add("shopType", param.shopType);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string  jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderElec"), parameters, null);
            log.Write("电力获取订单返回："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecOrderInfo = JsonConvert.DeserializeObject<ElecOrderInfo>(jsonText);
            return elecOrderInfo;
        }
        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ElecPayresInfo HeatPayres(ElecPayresParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
           
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("orderno", param.orderno);  //订单编号
            parameters.Add("realAmout", param.realAmout); //账单金额
            parameters.Add("payCode", param.payCode);  //支付渠道编码
            parameters.Add("trandeNo", param.trandeNo);  //支付渠道交易流水号
            parameters.Add("dzpc", param.dzpc);   //
            parameters.Add("yhbh", param.yhbh);
            parameters.Add("ysje", param.realAmout);
            parameters.Add("bankDate", param.bankDate);
            parameters.Add("isPrint", param.isPrint);
            parameters.Add("jfbs", param.jfbs);
            parameters.Add("bz", param.bz);
            parameters.Add("jfmx", param.jfmx);
            parameters.Add("terminalNo", param.terminalNo);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("ElecPayresName"), parameters, null);
          
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<ElecPayresInfo>(jsonText);

           
        }
        public ElecPerPayresInfo PerPayres(ElecPerPayresParam param)
        {
            ElecPerPayresInfo elecPerPayresInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
         
            parameters.Add("authcode", param.authcode);
            parameters.Add("servicename", param.servicename);
            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            parameters.Add("orderno", param.orderno);  //订单编号
            parameters.Add("realAmout", param.realAmout); //账单金额
            parameters.Add("payCode", param.payCode);  //支付渠道编码
            parameters.Add("trandeNo", param.trandeNo);  //支付渠道交易流水号
            parameters.Add("dzpc", param.dzpc);   //
            parameters.Add("yhbh", param.yhbh);
            parameters.Add("ysje", param.realAmout);
            parameters.Add("isPrint", param.isPrint);
            parameters.Add("jfbs", param.jfbs);
            parameters.Add("pre", param.pre);
            parameters.Add("terminalNo", param.terminalNo);

            string   jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("ElecPayresName"), parameters, null);
               
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecPerPayresInfo = JsonConvert.DeserializeObject<ElecPerPayresInfo>(jsonText);
                
            return elecPerPayresInfo;
            
        }

    }
}
