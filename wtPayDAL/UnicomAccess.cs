using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.UnicomModel;
using wtPayBLL;
using wtPayCommon;
using System.Configuration;
using wtPayModel.ElecModel;
using Newtonsoft.Json;

namespace wtPayDAL
{
    public static class UnicomAccess
    {
        private static UnicomInterface access = new UnicomInterface();

        public static string UnicomLogin()
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            ElecLoginInfo elecLoginInfo = null;
            parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());
            parameters.Add("servicename", "DL001");
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("loginId", SysBLL.getCpuNo());

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("UnicomLoginName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecLoginInfo = JsonConvert.DeserializeObject<ElecLoginInfo>(jsonText);
            //同步系统时间
            SysBLL.SetSystemTime(elecLoginInfo.msghead.trandatetime);
            return elecLoginInfo.msgrsp.authcode;
        }
        /// <summary>
        /// 联通查询
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public static UnicomQueryInfo query(string phoneNo)
        {
            UnicomQueryInfo info = null;
            try {
                UnicomQueryParam param = new UnicomQueryParam();
                SysBLL.Authcode = UnicomLogin();
                param.authcode = SysBLL.Authcode;
                param.servicename = "LT001";
                param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
                param.reqsn = SysBLL.getSerialNum(); ;
                param.loginId = SysBLL.getCpuNo(); ;
                param.phoneNo = phoneNo;

                info= access.query(param);
                string temp=info.msgrsp.retcode;
                return info;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Bus.BUS_QUERY, e.Message);
            }
        }

        /// <summary>
        /// 联通提交订单
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="amout"></param>
        /// <returns></returns>
        public static UnicomOrderInfo order(string phoneNo,string amout,string account,string shoptype)
        {
            UnicomOrderInfo info = null;
            try
            {
                UnicomOrderParam param = new UnicomOrderParam();
                param.authcode = SysBLL.Authcode;
                param.servicename = "LT002";
                param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
                param.reqsn = SysBLL.getSerialNum();
                param.loginId = SysBLL.getCpuNo();
                param.phoneNo = phoneNo;
                param.paymentAmout = amout;
                param.shopType = shoptype;//*************************************
                param.account_no = account;
                info = access.order(param);
                string temp = info.msgrsp.retcode;
                return info;
            }
            catch (Exception e) { throw new WtException(WtExceptionCode.Bus.BUS_QUERY, e.Message); }

        }

        /// <summary>
        /// 订单通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static UnicomPayResInfo payres(UnicomPayResParam param)
        {
            param.authcode = SysBLL.Authcode;
            param.servicename = "DD004";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum(); 
            param.loginId = SysBLL.getCpuNo();
            //param.trandeNo = SysBLL.getSerialNum();
            //param.payCode = "Z000000004";
            return access.payres(param);
        }
     }
}
