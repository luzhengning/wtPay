using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel.BroadCas;

namespace wtPayDAL
{
    public class BroadCasAccess
    {
        /// <summary>
        /// 广电登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string BroadCasLogin()
        {
            log.Write("XXXXXXXXXXXXXX广电登录认证");
            try {
                BroadCasInterface access = new BroadCasInterface();
                BroadCasLoginParam param = new BroadCasLoginParam();
                param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
                param.servicename = "DL001";
                param.reqsn = SysBLL.getSerialNum();
                param.loginId = SysBLL.getCpuNo();  //设备ID
                BroadCasLoginInfo info = access.BroadCasLogin(param);
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
            catch(Exception ex) { log.Write("广电登录失败："+ex.Message); return null; }
        }

        public static string getSystemTime()
        {
            log.Write("XXXXXXXXXXXXXX广电登录认证：获取时间");
            try
            {
                BroadCasInterface access = new BroadCasInterface();
                BroadCasLoginParam param = new BroadCasLoginParam();
                param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
                param.servicename = "DL001";
                param.reqsn = SysBLL.getSerialNum();
                param.loginId = SysBLL.getCpuNo();  //设备ID
                BroadCasLoginInfo info = access.BroadCasLogin(param);
                if (info != null)
                {
                    if (info.msgrsp != null)
                    {
                        if (info.msgrsp.authcode != null)
                        {
                            return info.msghead.trandatetime;
                        }
                    }
                }
            }catch(Exception ex) { log.Write("error:getSystemTime" + ex.Message); }
            return "";
        }

        /// <summary>
        /// 广电查询
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static BroadCasQueryInfo query(string account)
        {

            BroadCasInterface access = new BroadCasInterface();
            BroadCasQueryParam param = new BroadCasQueryParam();
            SysBLL.Authcode = BroadCasLogin();
            param.authcode = SysBLL.Authcode;
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "GD001";
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();  //设备ID
            param.paymentno = account;
            return access.BroadCasQuery(param);

        }
        /// <summary>
        /// 广电提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static BroadCasOrderInfo order(BroadCasOrderParam param)
        {
            BroadCasInterface access = new BroadCasInterface();
            param.authcode = SysBLL.Authcode;
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "GD002";
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();  //设备ID
            param.shopType = param.shopType;//***********************************
            return access.BroadCasOrder(param);
        }

        /// <summary>
        /// 广电支付通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static BroadCasPayresInfo payres(BroadCasPayresParam param)
        {
            BroadCasInterface access = new BroadCasInterface();
            param.authcode = SysBLL.Authcode;
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "DD004";
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();  //设备ID
            //param.payCode= param.payCode;
            //param.trandeNo=SysBLL.getSerialNum();
            return access.BroadCasPayres(param);
        }


    }
}
