using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.HeatModel;

namespace wtPayDAL
{
    public class HeatAccess
    {
        /// <summary>
        /// 热力登录认证
        /// </summary>
        /// <returns></returns>
        public static string HeatLogin()
        {
            HeatInterface access = new HeatInterface();//asdfa
            HeatLoginParam param = new HeatLoginParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "DL001";
            param.resqn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();  //设备ID
            HeatLoginInfo info= access.HeatLogin(param);
            //同步系统时间
            SysBLL.SetSystemTime(info.msghead.trandatetime);
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
            return null;
        }

        /// <summary>
        /// 热力查询
        /// </summary>
        /// <param name="paymentno"></param>
        /// <returns></returns>
        public static HeatQueryInfo HeatQuery(string paymentno)
        {
            SysBLL.Authcode = HeatLogin();
            HeatInterface access = new HeatInterface();
            HeatQueryParam param = new HeatQueryParam();
            param.authcode = SysBLL.Authcode;
            param.servicename = "RL001";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            param.paymentno = paymentno;
            HeatQueryInfo info = access.HeatQuery(param);
            return info;
        }
        /// <summary>
        /// 订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HeatOrderInfo HeatOrder(HeatOrderParam param) 
        {
            HeatInterface access = new HeatInterface();
            param.authcode = SysBLL.Authcode;
            param.servicename = "RL002";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            //param.paymentno = "";
            //param.realAmout = "";
            //param.paymentAmout = "";
            //param.billDate = "";
            HeatOrderInfo info = access.HeatOrder(param);
            return info;
        }
        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HeatPayresInfo HeatPayres(HeatPayresParam param)
        {
            HeatInterface access = new HeatInterface();
            param.authcode = SysBLL.Authcode;
            param.servicename = "DD004";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            //param.orderno = "";
            //param.realAmout = "";
            //param.payCode = "";
            //param.trandeNo = "";
            HeatPayresInfo info = access.HeatPayres(param);
            return info;
        }
    }
}
