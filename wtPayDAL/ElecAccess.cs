using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.ElecModel;
using wtPayBLL;
using wtPayModel.PaymentModel;

namespace wtPayDAL
{
    public class ElecAccess
    {
        public static string ElecLogin()
        {
            //电力登录认证
            ElecInterface access = new ElecInterface();
            ElecLoginParam param = new ElecLoginParam();
           
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); //交易时间  格式：YYYYMMDDHHMMSS 
            param.servicename = "DL001";    //交易号 not null
            param.reqsn = SysBLL.getSerialNum();   //请求流水号 not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            ElecLoginInfo info= access.ElecLogin(param);
            //同步系统时间
            SysBLL.SetSystemTime(info.msghead.trandatetime);
            if (info != null)
            {
                if (info.msgrsp != null)
                {
                    if (info.msgrsp.authcode!=null)
                    {
                        return info.msgrsp.authcode;
                    }
                }
            }
            return null;
        }

        public static ElecQueryUserInfo QueryUser(string account)
        {
           
            ElecInterface access = new ElecInterface();
            ElecQueryUserParam param = new ElecQueryUserParam();
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "DW005";// 交易编号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();//请求流水号 not null
            param.yhbh = account;//缴费户号 not null   44646465
            param.queryId = "01";//查询条件 not null【01 - 新户号         02 - 原户号          03 - 户名】
            param.loginId = SysBLL.getCpuNo();//交互终端的设备编号（用于自助终端） not null
            ElecQueryUserInfo info = access.ElecQueryUser(param);
            return info;
           
        }

        /// <summary>
        /// 查询电费
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static ElecQueryElecInfo QueryElec(string account)
        {
            SysBLL.Authcode = ElecLogin();
            ElecInterface access = new ElecInterface();
            ElecQueryElecParam param = new ElecQueryElecParam();
          
            param.authcode = SysBLL.Authcode; ;// 认证码 not null
            param.servicename = "DW001";//交易编号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();//请求流水号 not null
            param.yhbh = account;//用户名 not null
            param.loginId = SysBLL.getCpuNo();//交互终端的设备编号（用于自助终端） not null
            ElecQueryElecInfo info = access.ElecQueryElec(param);
            return info;
           
        }

        public static ElecOrderInfo ElecOrder(ElecPayParam payParam)
        {
            ElecInterface access = new ElecInterface();
            ElecOrderParam param = new ElecOrderParam();
         
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "DW002";//交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();//请求流水号 not null【请求发起方流水号】
            param.paymentno = payParam.Account;//用户名 not null【用户编号】 
            param.paymentamout = payParam.UserInputMoney;//账单金额  not null【电网查询电费接口(即queryElecFees)返回的dfze（应缴电费总额）】
            param.shopType = payParam.ShopType;//*******************************
            ElecOrderInfo info = access.ElecOrder(param);
            return info;
        }
        public static ElecPayresInfo HeatPayres(ElecPayresParam payParam,string payCode)
        {
            ElecInterface access = new ElecInterface();
            ElecPayresParam parameters = new ElecPayresParam();
            
            parameters.authcode = SysBLL.Authcode;
            parameters.servicename = "DD004";
            parameters.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            parameters.reqsn = SysBLL.getSerialNum();
            parameters.loginId = SysBLL.getCpuNo();
            parameters.orderno = payParam.orderno;
            parameters.realAmout = payParam.realAmout; //账单金额
            parameters.payCode =payCode; //支付渠道编码
            parameters.trandeNo = payParam.trandeNo;  //支付渠道交易流水号
            parameters.dzpc = payParam.dzpc;   //
            parameters.yhbh = payParam.yhbh;
            parameters.ysje = payParam.ysje;
            parameters.isPrint = payParam.isPrint;
            parameters.jfbs = payParam.jfbs;
            parameters.jfmx = payParam.jfmx;
            parameters.terminalNo = payParam.terminalNo;
            ElecPayresInfo info = access.HeatPayres(parameters);
            return info;
        }
        public static ElecPerPayresInfo PerPayres(ElecPerPayresParam payParam)
        {
            ElecInterface access = new ElecInterface();
            ElecPerPayresParam parameters = new ElecPerPayresParam();
           
            parameters.authcode = SysBLL.Authcode;
            parameters.servicename = "DD004";
            parameters.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            parameters.reqsn = SysBLL.getSerialNum();
            parameters.loginId = SysBLL.getCpuNo();
            parameters.orderno = payParam.orderno;
            parameters.realAmout = payParam.realAmout; //账单金额
            parameters.payCode = payParam.payCode; //支付渠道编码
            parameters.trandeNo = payParam.trandeNo;  //支付渠道交易流水号
            parameters.dzpc = "0";   //
            parameters.yhbh = payParam.yhbh;
            parameters.ysje = payParam.ysje;
            parameters.isPrint = "0";
            parameters.jfbs = "0";
            parameters.pre = "yes";
            parameters.terminalNo = payParam.terminalNo;
            ElecPerPayresInfo info = access.PerPayres(parameters);
            return info;
            
        }
    }
}
