using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL.Pay;
using wtPayModel;
using wtPayModel.ElecModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;

namespace wtPay.pay
{
    /// <summary>
    /// 交易处理过程
    /// </summary>
    public class Pay
    {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PayResultInfo payStart(PayParam p)
        {
            //密码键盘工具类
            PasswordBLL pwdBLL = new PasswordBLL();
            //交易数据访问类
            PayAccess payAccess = new PayAccess();
            //支付结果
            PayResultInfo payResultInfo = null;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                //缴费类型
                parameters.Add("serviceType", p.serviceType);
                //交易金额
                parameters.Add("realAmout", p.rechageAmount);
                //交易类型，纸币，银行卡，万通卡
                parameters.Add("tr.shop_type", isWtLkl(p.icParams));
                if (PayStaticParam.payType == -1)
                {//电子现金支付相关参数
                    //获取批次号
                    p.batchNo = LKLProcedure.GetBatchNo();
                    //获取支付报文
                    parameters.Add("tr.data", PayBLL.PayMessage(ref p));
                    //批次流水号
                    parameters.Add("dzls", p.batchNo + p.WtLklorderNo);
                }
                //云平台交易流水号
                parameters.Add("tr.cloud_no", p.orderNo);
                if ("4".Equals(p.serviceType)) {
                    //万通卡支付相关参数
                    if (SysBLL.IsTest.Equals("正式"))
                    {
                        parameters.Add("terminalno", "XNJ00002");
                        parameters.Add("terminal", ConfigurationManager.AppSettings["MechineNo"]);
                    }else
                    {
                        parameters.Add("terminalno","UU000002");
                        parameters.Add("terminal", ConfigurationManager.AppSettings["MechineNo"]);
                    }
                }else
                {
                        parameters.Add("terminalNo", p.terminalNo);
                }
                //设置其余参数
                initParam(parameters,p);
                //发起支付
                payResultInfo = payAccess.PayResNewAcc(parameters);
                //纸币交易到此为止
                if (PayStaticParam.payType == 0)
                {
                    if ("0000".Equals(payResultInfo.code))
                    {
                        //支付成功
                        log.Write("--------------------交易结束--------------------");
                        return payResultInfo;
                    }else
                    {
                        payResultInfo.code = "9999";
                        log.Write("--------------------交易结束--------------------");
                        return payResultInfo;
                    }
                }
                if (isNull(payResultInfo) || ("9991".Equals(payResultInfo.code)))
                {
                    //冲正
                    CorrectParam correctParam = new CorrectParam();
                    //缴费类型
                    correctParam.serviceType = p.serviceType;
                    //支付类型
                    correctParam.shop_type = isWtLkl(p.icParams);
                    //云平台交易流水
                    correctParam.cloud_no = p.orderNo;
                    for (int i = 1; i <= 3; i++)
                    {
                        //获取冲正报文
                        correctParam.data = Util.JumpUtil.CorrectJump(p);
                        //发起冲正
                        PayResultInfo payResultInfoacc = payAccess.CorrectAcc(correctParam);
                        if ("0000".Equals(payResultInfoacc.code))
                        {
                            break;
                        }
                    }
                    payResultInfo.code = "9991";
                    return payResultInfo;
                }
                else if ("9955".Equals(payResultInfo.code))
                {
                    //密码错误
                    SysBLL.PasswordErrorInfo = "密码错误，请重新输入！";
                    log.Write("密码错误");
                    log.Write("--------------------交易结束--------------------");
                    if (p.icParams == null)
                    {
                        //打开键盘，同时交易页面跳转到输入密码页面,万通
                        pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", p.WtNo);
                        return payResultInfo;
                    }
                    else
                    {
                        //打开键盘，同时交易页面跳转到输入密码页面,银行卡
                        pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", p.icParams["cardNo"]);
                        return payResultInfo;
                    }
                }
                else if ("9951".Equals(payResultInfo.code))
                {
                    //余额不足
                    log.Write("余额不足");
                    return payResultInfo;
                }
                else if ("9900".Equals(payResultInfo.code))
                {
                    //发起退款
                    refund(ref payResultInfo, p);
                    return payResultInfo;
                }
                else if("0000".Equals(payResultInfo.code))
                {
                    //支付成功
                    log.Write("--------------------交易结束--------------------");
                    //银行卡给万通卡充值成功后，测试环境下会在其他服务页面中显示银行卡退款按钮，可进行退款操作
                    if (SysBLL.IsTest.Equals("测试"))
                    {
                        if (p.serviceType.Equals("4"))
                        {
                            //发起退款
                            RefundTest refundTest = new RefundTest();
                            refundTest.refundPayResultInfo = payResultInfo;
                            refundTest.refundPayParam = p;
                            PayStaticParam.refundTest.Add(refundTest);
                        }
                    }
                    return payResultInfo;
                }else
                {
                    return payResultInfo;
                }
            }
            catch (Exception ex)
            {
                log.Write("error:99:支付失败：" + ex.Message+ex.InnerException);
                return payResultInfo;
            }
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="orderinfo"></param>
        /// <param name="payResultInfo"></param>
        /// <param name="p"></param>
        public void refund( ref PayResultInfo payResultInfo, PayParam p)
        {
            try
            {
                RefundParam refundParam = new RefundParam();
                //交易类型
                refundParam.serviceType = p.serviceType;
                //是否写卡成功
                refundParam.rqFlushesCode = p.rqFlushesCode;
                //交易类型
                refundParam.shop_type = isWtLkl(p.icParams);
                //云平台订单号
                refundParam.cloud_no = p.orderNo;
                //交易金额
                payResultInfo.map["4"] = p.rechageAmount;
                //退款
                if (p.icParams == null)
                {
                    //万通退款
                    refundParam.data = Wanton.wtRefund(payResultInfo.map, p.MERCHANTNO_shopNo);
                }
                else
                {
                    //银行卡退款
                    refundParam.data = LKLProcedure.wtPayRevoke(payResultInfo.map, p);
                }
                PayAccess payAccess = new PayAccess();
                //发起退款
                PayResultInfo result=payAccess.RefundAcc(refundParam);
                if ("0000".Equals(result.code))
                {
                    log.Write("退款成功！");
                }else
                {
                    log.Write("退款失败！");
                }
            }
            catch (Exception ex)
            {
                log.Write("error:退款异常：" + ex.Message+ex.InnerException);
            }
        }
        public void testRefund(ref PayResultInfo payResultInfo, PayParam p)
        {
            try
            {
                RefundParam refundParam = new RefundParam();
                refundParam.serviceType = p.serviceType;
                refundParam.rqFlushesCode = p.rqFlushesCode;
                refundParam.shop_type = isWtLkl(p.icParams);
                refundParam.cloud_no = p.orderNo;
                payResultInfo.map["4"] = p.rechageAmount;
                //退款
                if (p.icParams == null)
                {
                    //万通退款
                    refundParam.data = Wanton.wtRefund(payResultInfo.map, p.MERCHANTNO_shopNo);
                }
                else
                {
                    //银行卡退款
                    refundParam.data = LKLProcedure.wtPayRevoke(payResultInfo.map, p);
                }
                PayAccess payAccess = new PayAccess();
                PayResultInfo result = payAccess.testRefundAcc(refundParam);
                if ("0000".Equals(result.code))
                {
                    log.Write("退款成功！");
                }
                else
                {
                    log.Write("退款失败！");
                }
            }
            catch (Exception ex)
            {
                log.Write("error:退款异常：" + ex.Message + ex.InnerException);
            }
        }
        public string refundStr(PayResultInfo payResultInfo, PayParam p)
        {
            try
            {
                //退款
                if (p.icParams == null)
                {
                    //万通退款
                    return Wanton.wtRefund(payResultInfo.map, p.MERCHANTNO_shopNo);
                }
                else
                {
                    //银行卡退款
                    return LKLProcedure.wtPayRevoke(payResultInfo.map, p);
                }
            }
            catch (Exception ex)
            {
                log.Write("error:退款异常：" + ex.Message + ex.InnerException);
                return null;
            }
        }
        /// <summary>
        /// 判断缴费类型，拉卡拉or万通
        /// </summary>
        /// <param name="icParam"></param>
        /// <returns></returns>
        public string isWtLkl(Dictionary<string, string> icParam)
        {
            if (PayStaticParam.payType == 0) return  "2";
            if (icParam == null)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public bool isNull(object o)
        {
            if (o == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 设置缴费参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="p"></param>
        private void initParam(Dictionary<string, string> param, PayParam p)
        {
            switch (p.serviceType)
            {
                case "1":
                    param.Add("ipAddress", SysConfigHelper.readerNode("payResNewName"));
                    break;
                case "2":
                    //水务
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    param.Add("billDate", Payment.waterPayParam.WaterQueryInfo.msgrsp.orderlist[0].billDate);
                    break;
                case "3":
                    //联通
                    param.Add("phoneNo", Payment.unicomPayParam.PhoneOn);// 手机号 varchar2(20)    否 由LT001接口返回
                    param.Add("accountNo", Payment.unicomPayParam.AccountNo);// 账户号 varchar2(20)    否 由LT001接口返回
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    break;
                case "4":
                    //万通卡
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    param.Add("operator", "162");
                    param.Add("deptno", "0108");
                    // param.Add("pursedetail", );
                    param.Add("wtcardid", Payment.wintopReChargeParam.WtCardNo);
                    param.Add("wtuserid", Payment.wintopReChargeParam.WintopQueryResult.USERID);
                    param.Add("type", Payment.wintopReChargeParam.WintopQueryResult.TYPE);
                    break;
                case "5":
                    //物业
                    if (SysBLL.payCostType == 10)
                    {
                        param.Add("mobile", Payment.PropPayParam.Mobile);
                        param.Add("chargeseids", Payment.PropPayParam.ChargeList.chargeid);
                        param.Add("isOld", "0");
                    }

                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    break;
                case "6":
                    //电力
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    if (Payment.elecPayParam.IsArrearage)
                    {
                        //预缴费
                        param.Add("pre", "yes");
                        param.Add("dzpc", "0");
                        param.Add("yhbh", Payment.elecPayParam.Account);
                        param.Add("ysje", Payment.elecPayParam.RechageAmount);
                        param.Add("isPrint", "1");
                        param.Add("jfbs", "0");
                    }
                    else
                    {
                        //欠费缴纳
                        param.Add("pre", "no");
                        param.Add("dzpc", Payment.elecPayParam.Param.dzpc);
                        param.Add("yhbh", Payment.elecPayParam.Account);
                        param.Add("ysje", Payment.elecPayParam.RechageAmount);
                        param.Add("isPrint", "1");
                        param.Add("jfbs", Payment.elecPayParam.Param.jfbs);
                        ElecQueryDianFeiDetail list = Payment.elecPayParam.ElecQueryDianFeiDetail;
                        Payment.elecPayParam.Param.jfmx = list.yhbh + "-" + list.ysbz + "-" + Payment.elecPayParam.RechageAmount + "-" + list.dfje + "-" + list.wyjje + "-" + list.sctw + "-" + list.bctw;
                        log.Write("电力缴费明细："+Payment.elecPayParam.Param.jfmx);
                        param.Add("jfmx", Payment.elecPayParam.Param.jfmx);
                    }
                    break;
                case "7":
                    //广电
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    break;
                case "8":
                    //热力
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    break;
                case "9":
                    //移动
                    param.Add("mobile", Payment.mobilePayParam.PhoneOn);
                    param.Add("homeRegion", Payment.mobilePayParam.QueryInfo.msgrsp.homeRegion);
                    param.Add("homeOffice", Payment.mobilePayParam.QueryInfo.msgrsp.homeOffice);
                    param.Add("contractNo", Payment.mobilePayParam.QueryInfo.msgrsp.contractNo);
                    param.Add("overdueMoney", Payment.mobilePayParam.QueryInfo.msgrsp.overdueMoney);
                    param.Add("prepaidBalance", Payment.mobilePayParam.QueryInfo.msgrsp.prepaidBalance);
                    param.Add("changeBalance", Payment.mobilePayParam.QueryInfo.msgrsp.changeBalance);
                    param.Add("channelNo", "002");
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    break;
                case "10":
                    break;
                case "5_2":
                    //物业2
                    param.Add("ipAddress", SysConfigHelper.readerNode("PayName"));
                    param.Add("SC10014",Payment.propSecPayParam.PrimaryKey);
                    break;
            }
        }
        /// <summary>
        /// 开始支付过程并响应支付结果，由支付页面发起
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool payResult(ref PayParam p)
        {
            //发起支付
            PayResultInfo payResultInfo = payStart(p);
            p.payResultInfo = payResultInfo;
            try
            {
                p.propSecSC20003 = payResultInfo.SC20003.ToString();
            }catch(Exception e)
            {
                log.Write("error:100:"+e.Message+e.InnerException);
            }
            //纸币交易到此为止
            if (PayStaticParam.payType == 0)
            {
                if ("0000".Equals(payResultInfo.code))
                {
                    //支付成功
                    return true;
                }
                else
                {
                    //支付失败，异常
                    exit(payResultInfo.msg);
                    return false;
                }
            }
            string recode = payResultInfo.code;
            log.Write("恒篮支付返回码："+recode);
            log.Write("云平台返回码："+payResultInfo.msgCode);
            if ("0000".Equals(recode))
            {
                //支付成功
                return true;
            }
            else if ("9999".Equals(payResultInfo.msgCode)&&"9900".Equals(recode))
            {
                //退款
                exitRefund(payResultInfo.msg);
                return false;
            }
            else if ("0028".Equals(payResultInfo.msgCode)&&"9900".Equals(recode))
            {
                //退款
                exitRefund(payResultInfo.msg);
                return false;
            }
            else if ("9991".Equals(recode))
            {
                //发起冲正
                exit(payResultInfo.msg);
                return false;
            }
            else if (("17".Equals(payResultInfo.map["39"]))|| ("18".Equals(payResultInfo.map["39"]))|| ("32".Equals(payResultInfo.map["39"]))|| ("45".Equals(payResultInfo.map["39"]))|| ("46".Equals(payResultInfo.map["39"]))|| ("47".Equals(payResultInfo.map["39"]))|| ("48".Equals(payResultInfo.map["39"]))|| ("50".Equals(payResultInfo.map["39"])))
            {
                exitRefund("此卡已挂失，无法缴费");
                return false;
            }
            else if ("9999".Equals(recode))
            {
                //支付失败，异常
                exit(payResultInfo.msg);
                return false;
            }
            else if ("9955".Equals(recode))
            {
                //密码错误
                Util.JumpUtil.jumpCommonPage("FormInputPassword");
                return false;
            }
            else if ("9951".Equals(recode))
            {
                //余额不足
                exit(payResultInfo.msg);
                return false;
            }
            //退款成功 9999
            //正在缴费 其他
            exit("缴费失败，请重新缴费或退卡");
            return false; 
        }
        public bool payResult1(ref PayParam p)
        {
            PayResultInfo payResultInfo = payStart(p);
            p.payResultInfo = payResultInfo;
            string recode = payResultInfo.code;
            log.Write("我方支付返回码：" + recode);
            if ("0000".Equals(recode))
            {
                //支付成功
                return true;
            }
            else if ("9999".Equals(payResultInfo.msgCode) && "9900".Equals(recode))
            {
                //退款
                exitRefund(SqlLiteHelper.SqlLiteHelper.query("refund9999")[0].FormalValue);
                return false;
            }
            else if ("0028".Equals(payResultInfo.msgCode) && "9900".Equals(recode))
            {
                //退款
                exitRefund(SqlLiteHelper.SqlLiteHelper.query("refund0028")[0].FormalValue);
                return false;
            }
            else if ("9991".Equals(recode))
            {
                //发起冲正
                exit("缴费失败，请重新缴费或退卡");
                return false;
            }
            else if ("9999".Equals(recode))
            {
                //支付失败，异常
                exit("缴费失败，请重新缴费或退卡");
                return false;
            }
            else if ("9955".Equals(recode))
            {
                //密码错误
                Util.JumpUtil.jumpCommonPage("FormInputPassword");
                return false;
            }
            else if ("9951".Equals(recode))
            {
                //余额不足
                exit("余额不足");
                return false;
            }
            //退款成功 9999
            //正在缴费 其他
            exit("缴费失败，请重新缴费或退卡");
            return false;
        }
        /// <summary>
        /// 处理获取订单结果
        /// </summary>
        /// <param name="retcode"></param>
        /// <returns></returns>
        public bool orderInfoResult(string retcode,string msgInfo)
        {
            if ("0000".Equals(retcode))
            {
                return true;
            }else if ("11111".Equals(retcode))
            {
                //上笔交易或写卡异常
                Util.JumpUtil.jumpCommonPage("FormCardFail");
                return false;
            }
            else if ("1234".Equals(retcode))
            {
                exit("暂不支持此类型卡业务");
                return false;
            }
            else if ("0048".Equals(retcode))
            {
                exit("不在营业时间范围内");
                return false;
            }
            else if ("7777".Equals(retcode))
            {
                exit(msgInfo);
                return false;
            }
            else if ("9999".Equals(retcode))
            {
                throw new Exception("获取缴费订单失败:" + retcode);
            }
            exit("缴费失败，请重新缴费或退卡");
            return false;
        }
        private void exitRefund(string info)
        {
            FormTip.FormFailRefundShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFailRefund");
        }
        private void exit(string info)
        {
            log.Write("--------------------交易结束--------------------");
            FormTip.FormFailShowinfo = info;
            Util.JumpUtil.jumpCommonPage("FormFail");
        }
    }
}
