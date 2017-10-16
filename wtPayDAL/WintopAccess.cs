using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.PaymentModel;
using wtPayModel.WintopModel;

namespace wtPayDAL
{
    public class WintopAccess
    {
        /// <summary>
        /// 万通卡登陆认证
        /// </summary>
        /// <returns></returns>
        public static string WintopLogin()
        {
            //万通卡登录认证
            WintopInterface access = new WintopInterface();
            WintopLoginParam param = new WintopLoginParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();    //交易时间  格式：YYYYMMDDHHMMSS 
            param.servicename = "DL001"; //交易号 not null
            param.resqn = SysBLL.getSerialNum(); //请求流水号 not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            WintopLoginInfo info= access.WintopLogin(param);
            //同步系统时间
            SysBLL.SetSystemTime(info.msghead.trandatetime);
            if (info != null)
            {
                if (info.msgrsp != null)
                {
                    if (info.msgrsp.authcode != null)
                    {
                        SysBLL.Authcode = info.msgrsp.authcode;
                        return info.msgrsp.authcode;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 万通卡查询
        /// </summary>
        /// <param name="wtCardNo"></param>
        /// <returns></returns>
        public static WintopQueryInfo WintopQuery(string wtCardNo)
        {
            SysBLL.Authcode = WintopLogin();
            WintopQueryInfo info = null;
            //万通卡查询
            WintopInterface access = new WintopInterface();
            WintopQueryparam param = new WintopQueryparam();
            param.authcode = SysBLL.Authcode;   // 认证码 not null
            param.servicename = "WT001"; //交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易时间  格式：YYYYMMDDHHMMSS
            param.resqn = SysBLL.getSerialNum();   //请求流水号 not null
            param.paymentno = wtCardNo;   //缴费户号 not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            param.md5Pwd = Payment.wintopReChargeParam.Md5Pwd;
            info = access.WintopQuery(param);
            return info;
        }

        /// <summary>
        /// 万通卡提交订单
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static WintopOrderInfo WintopOrder(WintopReChargeParam result)
        {
            WintopOrderInfo info = null;
            //万通卡订单提交
            WintopInterface access = new WintopInterface();
            WintopOrderParam param = new WintopOrderParam();
            param.authcode = SysBLL.Authcode;   // 认证码 not null
            param.servicename = "WT002";  //交易号
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); //交易时间
            param.resqn = SysBLL.getSerialNum(); //请求流水号
            param.wtcardid = result.WintopQueryResult.WTCARDID;//万通卡号
            param.wtuserid = result.WintopQueryResult.USERID; //用户编号
            param.type = result.WintopQueryResult.TYPE;    //账户类型
            param.money = result.UserInputMoney; //充值金额
            param.loginId = SysBLL.getCpuNo();  //设备ID
            param.shopType = result.ShopType;//****************************************
            info = access.WintopOrder(param);
            return info;
        }
        public static WintopPayresInfo payres(WintopReChargeParam wintopReChargeParam)
        {
            WintopPayresInfo info = null;
            WintopInterface access = new WintopInterface();
            WintopPayresParam wintopPayresParam = new WintopPayresParam();
            wintopPayresParam.authcode = SysBLL.Authcode;   // 认证码 not null
            wintopPayresParam.servicename = "DD004";  //交易号
            wintopPayresParam.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); //交易时间
            wintopPayresParam.reqsn = SysBLL.getSerialNum(); //请求流水号
            wintopPayresParam.loginId = SysBLL.getCpuNo();  //设备ID

            wintopPayresParam.orderno = wintopReChargeParam.WintopOrderInfo.msgrsp.orderNo; //订单编号 not null
            wintopPayresParam.realAmout = wintopReChargeParam.WintopOrderInfo.msgrsp.realAmout; // 账单金额 Not null 【根据查询产生的账单金额】
            wintopPayresParam.payCode = wintopReChargeParam.PayCode;
            wintopPayresParam.trandeNo = wintopReChargeParam.TradeNo; // 支付渠道交易流水号 not null【银行支付流水号】

            wintopPayresParam.wtcardid = wintopReChargeParam.WintopOrderInfo.msgrsp.wtcardid; //万通卡号【获取万通卡信息接口方法返回即query方法返回】
            wintopPayresParam.wtuserid = wintopReChargeParam.WintopQueryResult.USERID; //用户编号【获取万通卡信息接口方法返回即query方法返回】
            wintopPayresParam.type = wintopReChargeParam.WintopQueryResult.TYPE; //账户类型【获取万通卡信息接口方法返回即query方法返回】
            wintopPayresParam.terminalNo = wintopReChargeParam.TerminalNo; //终端编号 not null
            wintopPayresParam.terminalno = wintopReChargeParam.Terminalno; //终端编号 not null
            wintopPayresParam.operators = "162"; //操作员编号 not null
            wintopPayresParam.deptno = "0108"; //网点编号 not null

            info = access.payres(wintopPayresParam);
            return info;
        }
        /// <summary>
        /// 万通挂失
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static WintopLossReportInfo LossReport(WintopLossReportParam param)
        {
            WintopInterface access = new WintopInterface();
            WintopLossReportInfo wintopLossReportInfo = null;
            param.Authcode= WintopLogin();
            param.Servicename= "WT004";
            param.TrandateTime= SysBLL.getYYYYMMDDHHMMSSTime();
            param.Reqsn= SysBLL.getSerialNum();
            param.LoginId= SysBLL.getCpuNo();
            //parameters.Add("wtcardid", param.Wtcardid);
            //parameters.Add("validatecode", param.Validatecode);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            wintopLossReportInfo=access.LossReport(param);
            return wintopLossReportInfo;
        }
        /// <summary>
        /// 万通卡发送短信验证码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static WintopSendValidateCodeInfo sendValidateCode(WintopSendValidateCodeParam param)
        {
            WintopInterface access = new WintopInterface();
            param.Authcode = WintopLogin();
            param.Servicename = "WT005";
            param.TrandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.Reqsn = SysBLL.getSerialNum();
            param.LoginId = SysBLL.getCpuNo();
            
            return access.sendValidateCode(param); ;
        }

        /// <summary>
        /// 万通卡消费明细查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static WintopSpendDetailInfo SpendDetail(WintopSpendDetailParam param)
        {
            WintopInterface access = new WintopInterface();
            param.Authcode =WintopLogin();
            param.Servicename = "WT008";
            param.TrandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.Reqsn = SysBLL.getSerialNum();
            param.LoginId = SysBLL.getCpuNo();
            //wtcardid
            //password
            //pageNo
            //pageSize
            return access.SpendDetail(param); ;
        }

        /// <summary>
        /// 万通卡充值明细查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static WintopRechargeDetailInfo RechargeDetail(WintopRechargeDetailParam param)
        {
            WintopInterface access = new WintopInterface();
            param.Authcode = WintopLogin();
            param.Servicename = "WT009";
            param.TrandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.Reqsn = SysBLL.getSerialNum();
            param.LoginId = SysBLL.getCpuNo();
            //wtcardid
            //password
            //pageNo
            //pageSize
            return access.RechargeDetail(param); ;
        }
        /// <summary>
        /// 万通卡密码修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static WintopUpdateWtPwdInfo updateWtPwd(WintopUpdateWtPwdParam param)
        {
            WintopInterface access = new WintopInterface();
            param.Authcode = WintopLogin();
            param.Servicename = "WT101";
            param.TrandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.Reqsn = SysBLL.getSerialNum();
            param.LoginId = SysBLL.getCpuNo();
            //wtcardid
            //newpassword
            //password
            //type
            return access.updateWtPwd(param); ;
        }
        /// <summary>
        /// 查询一卡通状态
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public WintopStatusInfo queryCardStatus(string card)
        {
            try
            {
                WintopInterface access = new WintopInterface();
                return access.queryCardStatus(card,WintopLogin());
            }catch(Exception ex) { log.Write("error:queryCardStatus:" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 充值优惠信息查询
        /// </summary>
        /// <param name="wtid"></param>
        /// <returns></returns>
        public WintopDiscountInfo queryDiscount(string wtid)
        {
            WintopDiscountParam param = new WintopDiscountParam();
            param.authcode = SysBLL.Authcode;
            param.wtcardid = wtid;
            WintopInterface access = new WintopInterface();
            return access.queryDiscount(param);
        }
        public WintopMessage findHintSpec()//findHintSpec{
        {
            WintopInterface access = new WintopInterface();
            return access.findHintSpec();
        }
    } 
}
