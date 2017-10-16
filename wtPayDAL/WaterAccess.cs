using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.WaterModel;

namespace wtPayDAL
{
    /// <summary>
    /// 水务接口
    /// </summary>
    public class WaterAccess
    {
        /// <summary>
        /// 水务登录认证
        /// </summary>
        /// <returns></returns>
        public static string WaterLogin()
        {
            //水务登录认证
            WaterInterface access = new WaterInterface();
            WaterLoginParam param = new WaterLoginParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); //交易时间  格式：YYYYMMDDHHMMSS 
            param.servicename = "DL001";    //交易号 not null
            param.reqsn = SysBLL.getSerialNum();   //请求流水号 not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            WaterLoginInfo info= access.WaterLogin(param);
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
        /// 查询水务
        /// </summary>
        /// <param name="paymentno">户号</param>
        /// <returns></returns>
        public static WaterQueryInfo WaterQuery(string paymentno)
        {
            SysBLL.Authcode = WaterLogin();
            WaterInterface access = new WaterInterface();
            WaterQueryParam param = new WaterQueryParam();
            WaterQueryInfo info = null;
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "SW001";//交易编号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();//请求流水号 not null
            param.paymentno = paymentno;//缴费户号 not null
            param.loginId = SysBLL.getCpuNo();//交互终端的设备编号（用于自助终端） not null
            info = access.WaterQuery(param);
            return info;
        }

        /// <summary>
        /// 水务缴费
        /// </summary>
        /// <param name="waterOrderParam"></param>
        /// <returns></returns>
        public static WaterOrderInfo WaterOrder(WaterOrderParam waterOrderParam)
        {
            WaterInterface access = new WaterInterface();
            WaterOrderParam param = new WaterOrderParam();
            WaterOrderInfo info = null;
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "SW002";//交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();//请求流水号 not null【请求发起方流水号】
            param.paymentno = waterOrderParam.paymentno;//缴费户号 not null【水务用户编号】
            param.billdate = waterOrderParam.billdate;//账单日期  not null 【根据查询产生的账单日期】
            param.paymentamout = waterOrderParam.paymentamout;//账单金额  not null【根据查询产生的账单金额】
            param.loginId = SysBLL.getCpuNo();//交互终端的设备编号（用于自助终端） not null
            param.shopType = waterOrderParam.shopType;
            info = access.WaterOrder(param);
            return info;
        }
        public static WaterPayresInfo WaterPayres(WaterPayresParam param)
        {
            WaterPayresInfo info = null;
            WaterInterface access = new WaterInterface();
            param.Authcode = SysBLL.Authcode;
            param.Servicename = "DD004";
            param.TrandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.Reqsn = SysBLL.getSerialNum();
            param.LoginId = SysBLL.getCpuNo();
            //param.trandeNo= SysBLL.getSerialNum();
            info = access.WaterPayres(param);
            return info;
        }
    }
}
