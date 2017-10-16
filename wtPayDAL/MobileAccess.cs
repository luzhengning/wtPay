using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.Mobile;
using wtPayBLL;
using System.Configuration;
using Newtonsoft.Json;

namespace wtPayDAL
{
    public class MobileAccess
    {
       static MobileInterface mobileInterface = new MobileInterface();
        public static string MobileLogin()
        {
            MobileLoginParam loginParam = new MobileLoginParam();
            loginParam.trandateTime=SysBLL.getYYYYMMDDHHMMSSTime();
            loginParam.servicename="DL001";
            loginParam.reqsn=SysBLL.getSerialNum();
            loginParam.loginId=SysBLL.getCpuNo();
            //loginParam.loginId = "92820921";
            
            return mobileInterface.BroadCasLogin(loginParam);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static MobileQueryInfo query(string mobile)
        {
            SysBLL.Authcode = MobileLogin();
            MobileQueryParam param = new MobileQueryParam();
            param.authcode = SysBLL.Authcode;
            param.servicename = "YD001";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            param.mobile = mobile;
            return mobileInterface.query(param);
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static MobileOrderInfo order(MobileOrderParam param,MobileQueryInfo info)
        {
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "YD100";
            param.reqsn = SysBLL.getSerialNum(); 
            param.loginId = SysBLL.getCpuNo();
            //param.mobile =;
            param.authcode = SysBLL.Authcode;
            //param.paymentAmout =;
            return mobileInterface.order(param, info);
        }
        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public static MobilePayresInfo payres(MobilePayParam param,string paycode,string terminalNo)
        //{
        //    Dictionary<String, String> parameters = new Dictionary<String, String>();
        //    MobilePayresParam payresParam = new MobilePayresParam();
        //    payresParam.loginId = SysBLL.getCpuNo();
        //    payresParam.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
        //    payresParam.reqsn = SysBLL.getSerialNum(); ;
        //    payresParam.payCode = paycode;
        //    payresParam.trandNo = param.trandNo;
        //    payresParam.channelNo = "002";
        //    payresParam.servicename = "DD004";
        //    payresParam.mobile = param.queryInfo.msgrsp.mobile;

        //    payresParam.prepaidBalance = param.queryInfo.msgrsp.prepaidBalance;
        //    payresParam.homeOffice = param.queryInfo.msgrsp.homeOffice;
        //    payresParam.orderno = param.orderInfo.msgrsp.orderNo;
        //    payresParam.homeRegion = param.queryInfo.msgrsp.homeRegion;
        //    payresParam.contractNo = param.queryInfo.msgrsp.contractNo;
        //    payresParam.authcode = SysBLL.Authcode;
        //    payresParam.realAmout = param.orderInfo.msgrsp.realAmout;
        //    payresParam.changeBalance = param.queryInfo.msgrsp.changeBalance;
        //    payresParam.overdueMoney = param.queryInfo.msgrsp.overdueMoney;
        //    payresParam.terminalNo = terminalNo;

        //    return mobileInterface.payres(payresParam);
        //}
    }
}
