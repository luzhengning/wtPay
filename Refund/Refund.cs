using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayModel;
using wtPayModel.BusModel;

namespace RefundUtils
{
    public class Refund
    {

        public void ExeRefund(int refundType)
        {
            TradeRecord tradeRecord = new TradeRecord();
            tradeRecord.cloud_state = false;
            tradeRecord.order_type = "3";


            //退费申请
            XmlDocument xmlDoc = new XmlDocument();
            string refundFilePath = "";
            if (refundType == 0)
            {
                tradeRecord.shop_type = "0";
                refundFilePath =  System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml";

            }
            else if (refundType == 1)
            {
                tradeRecord.shop_type = "1";
                refundFilePath =  System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml";
            }
            xmlDoc.Load(refundFilePath);
            XmlNode root = xmlDoc.SelectSingleNode("refunds");
            XmlNodeList list = root.ChildNodes;
            foreach (XmlNode xn in list)
            {

                if (EnableRefund(xn, xmlDoc, refundFilePath, refundType))
                {

                    log.Write("可以退款，申请退款订单号：" + xn.ChildNodes[6].InnerText);
                    string state = xn.ChildNodes[8].InnerText;
                    //int num = Convert.ToInt32(xn.ChildNodes[10].InnerText);

                    string id = xn.ChildNodes[10].InnerText;

                    bool refundSuccess = false;
                    if (refundType == 0)
                    {//拉卡拉退款
                        refundSuccess = ExeBankRefund(xn, tradeRecord);

                    }
                    else if (refundType == 1)
                    {
                        refundSuccess = ExeWantonRefund(xn, tradeRecord);

                    }
                    if (refundSuccess)
                    {
                        //删除此退款账单
                        root.RemoveChild(xn);
                        xmlDoc.Save(refundFilePath);
                        //如果退款成功，那么向后台发送退款成功通知
                        //SendRefundSuccessNotice(id);
                        //如果退款成功，那么向云缴费发送异步通知
                        //SendSanweiRefundSuccessNotice(xn,"0");
                    }
                    else
                    {
                        string num = xn.ChildNodes[9].InnerText;
                        if (num != null && !"".Equals(num))
                        {
                            int times = Convert.ToInt32(num);
                            if (times >= 3)
                            {
                                //删除此退款账单
                                root.RemoveChild(xn);
                            }
                            else
                            {
                                ++times;
                                xn.ChildNodes[9].InnerText = times.ToString();
                                Thread.Sleep(wtPayUtils.Fibonacci(times) * 60 * 1000);
                            }

                            xmlDoc.Save(refundFilePath);

                        }
                        //如果退款成功，那么向后台发送退款成功通知
                        //SendRefundSuccessNotice(id);
                        //如果退款成功，那么向云缴费发送异步通知
                        //SendSanweiRefundSuccessNotice(xn,"1");
                    }

                }


            }
        }

        private bool SendSanweiRefundSuccessNotice(XmlNode xn, string refundStatus)
        {
            try
            {
                string url = SysConfigHelper.readerNode("RefundAsyNotice");

                string conName = xn.ChildNodes[2].InnerText;
                string refundRequestState = "";
                if (xn.ChildNodes[5] != null)
                {
                    refundRequestState = xn.ChildNodes[5].InnerText;

                }

                string refundOrderNo = xn.ChildNodes[6].InnerText;
                string refundPrice = xn.ChildNodes[7].InnerText;

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("refund_status", refundRequestState);
                parameters.Add("refund_fee", refundPrice);
                parameters.Add("out_trade_no", refundOrderNo);
                parameters.Add("conName", conName);
                parameters.Add("refundStatus", refundStatus);
                parameters.Add("rqFlushesCode", SysBLL.GasReadState);
                string jsonResult = "";
                JObject jobject = null;
                int i = 0;
                while (true)
                {
                    jsonResult = HttpHelper.getHttp(url, parameters, null);
                    jobject = JObject.Parse(jsonResult);
                    if (jobject["asyResult"] == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if ("success".Equals(jobject["asyResult"].ToString().Trim().ToLower()) || "fail".Equals(jobject["asyResult"].ToString().Trim().ToLower()))
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    i++;
                    if (i >= 3)
                    {
                        break;
                    }
                }

                string noticeResult = jobject["result"].ToString();
                string asyResult = jobject["asyResult"].ToString();

                if ("success".Equals(asyResult.Trim()))
                {
                    log.Write("向云缴费发送退款异步通知成功,退款订单号：" + refundOrderNo);

                    return true;
                }
                log.Write("向云缴费发送退款异步通知失败,退款订单号：" + refundOrderNo + "," + asyResult);

                return false;
            }
            catch (Exception e)
            {

                log.Write("向云缴费发送退款异步通知异常：" + e.Message);

            }
            return false;
        }

        private bool SendRefundSuccessNotice(string id)
        {
            string url = SysConfigHelper.readerNode("UpdatePaymentLog");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("id", id);
            parameters.Add("state", "1");//1为成功
            HttpHelper.getHttp(url, parameters, null);
            log.Write("向后台发起退款订单成功通知");
            return true;
        }

        /// <summary>
        /// 执行万通退款
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        private bool ExeWantonRefund(XmlNode xn, TradeRecord tradeRecord)
        {
            try
            {
                if (xn.ChildNodes[11] == null || xn.ChildNodes[12] == null || xn.ChildNodes[13] == null)
                {
                    return false;
                }
                string _2 = xn.ChildNodes[13].InnerText;
                string _4 = xn.ChildNodes[14].InnerText;
                string _59 = xn.ChildNodes[15].InnerText;
                string wantonOrderNo = xn.ChildNodes[16].InnerText;
                string cloud_no = xn.ChildNodes[17].InnerText;

                tradeRecord.relation_order = wantonOrderNo;
                tradeRecord.cloud_no = cloud_no;


                string connName = xn.ChildNodes[2].InnerText;
                string secondShopNo = "";
                string secondClientNo = "";


                if (connName.Contains("移动"))
                {
                    secondShopNo = WtShopNoInfo.MobileWtShopNo;
                }
                else if (connName.Contains("联通"))
                {
                    secondShopNo = WtShopNoInfo.UnionWtShopNo;
                }
                else if (connName.Contains("电力"))
                {
                    secondShopNo = WtShopNoInfo.ElectricWtShopNo;

                }
                else if (connName.Contains("广电"))
                {
                    secondShopNo = WtShopNoInfo.BoardWtShopNo;
                }
                else if (connName.Contains("燃气"))
                {
                    secondShopNo = WtShopNoInfo.GasWtShopNo;
                }
                else if (connName.Contains("水务"))
                {
                    secondShopNo = WtShopNoInfo.WaterWtShopNo;
                }
                else if (connName.Contains("热力"))
                {
                    secondShopNo = WtShopNoInfo.HeatWtShopNo;
                }
                else if (connName.Contains("公交"))
                {
                    secondShopNo = WtShopNoInfo.BusWtShopNo;
                }


                Dictionary <string, string> map = Wanton.refund(_2, _4, _59, tradeRecord, secondShopNo);
                if ("00".Equals(map["39"]))
                {
                    log.Write("万通退款成功");
                    SendSanweiRefundSuccessNotice(xn, "0");
                    return true;
                }
                else if ("".Equals(map["recode"].Trim()) || map["recode"] == null)
                {

                    return false;

                }
                else
                {

                    SendSanweiRefundSuccessNotice(xn, "1");
                    return false;
                }
                //return false;
            }
            catch (Exception e)
            {
                log.Write("万通退款异常：" + e.Message);
            }
            return false;
        }
        private bool ExeBankRefund(XmlNode xn, TradeRecord tradeRecord)
        {
            try
            {
                if (xn.ChildNodes[11] == null || xn.ChildNodes[12] == null || xn.ChildNodes[13] == null || xn.ChildNodes[14] == null || xn.ChildNodes[15] == null || xn.ChildNodes[16] == null || xn.ChildNodes[17] == null || xn.ChildNodes[18] == null || xn.ChildNodes[19] == null)
                {

                    return false;

                }

                string _2 = xn.ChildNodes[13].InnerText;
                string _4 = xn.ChildNodes[14].InnerText;
                string _11 = xn.ChildNodes[15].InnerText;
                string _23 = xn.ChildNodes[16].InnerText;
                string _35 = xn.ChildNodes[17].InnerText;
                string _36 = xn.ChildNodes[18].InnerText;
                string _55 = xn.ChildNodes[19].InnerText;
                string pwd = xn.ChildNodes[20].InnerText;
                string batchNo = xn.ChildNodes[21].InnerText;
                string cloudNo = xn.ChildNodes[22].InnerText;


                string connName = xn.ChildNodes[2].InnerText;
                string secondShopNo = "";
                string secondClientNo = "";
            

                    tradeRecord.cloud_no = cloudNo;
                Dictionary<string, string> map = LKLProcedure.payRevoke(_2, _23, _35, _36, _55, pwd, _4, _11, batchNo, tradeRecord);

                if ("00".Equals(map["recode"]))
                {
                    log.Write("退款成功");
                    SendSanweiRefundSuccessNotice(xn, "0");





                    return true;
                }
                else if ("".Equals(map["recode"].Trim()) || map["recode"] == null)
                {

                    return false;

                }
                else
                {

                    SendSanweiRefundSuccessNotice(xn, "1");
                    return false;
                }

                log.Write("退款失败");
                return false;
            }
            catch (Exception e)
            {
                log.Write("退款异常" + e.Message);

            }
            return false;
        }



        private static void AddRefundBaseInfo(XmlDocument xmlDoc, XmlElement refund, string state, string orderNo, string transType, string conName, string serviceName, string price)
        {
            createXmlNode(refund, "orderNo", orderNo, xmlDoc);
            createXmlNode(refund, "transType", transType, xmlDoc);
            createXmlNode(refund, "conName", conName, xmlDoc);
            createXmlNode(refund, "serviceName", serviceName, xmlDoc);

            createXmlNode(refund, "refundRequestResult", "false", xmlDoc);
            createXmlNode(refund, "refundRequestState", "", xmlDoc);
            createXmlNode(refund, "refundOrderNo", orderNo, xmlDoc);
            createXmlNode(refund, "refundPrice", price, xmlDoc);

            createXmlNode(refund, "state", state, xmlDoc);
            createXmlNode(refund, "num", "0", xmlDoc);
            createXmlNode(refund, "id", "1", xmlDoc);
        }
        private static void AddRefundBaseInfo(XmlDocument xmlDoc, XmlElement refund, string state, string orderNo, string transType, string conName, string serviceName, string price, string reqsn, string trandno)
        {
            createXmlNode(refund, "orderNo", orderNo, xmlDoc);
            createXmlNode(refund, "transType", transType, xmlDoc);
            createXmlNode(refund, "conName", conName, xmlDoc);
            createXmlNode(refund, "serviceName", serviceName, xmlDoc);



            createXmlNode(refund, "refundRequestResult", "false", xmlDoc);
            createXmlNode(refund, "refundRequestState", "", xmlDoc);
            createXmlNode(refund, "refundOrderNo", orderNo, xmlDoc);
            createXmlNode(refund, "refundPrice", price, xmlDoc);

            createXmlNode(refund, "state", state, xmlDoc);
            createXmlNode(refund, "num", "0", xmlDoc);
            createXmlNode(refund, "id", "1", xmlDoc);

            createXmlNode(refund, "reqsn", reqsn, xmlDoc);
            createXmlNode(refund, "trandeNo", trandno, xmlDoc);
        }
        /// <summary>
        /// 测试
        /// </summary>
        public static bool AddBankRefund(string cloudNo, string _2, string _23, string _35, string _36, string _55, string pwd, string money, string _11, string batchNo, string state, string orderNo, string transType, string conName, string serviceName)
        {
            try
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                string reqsn = SysBLL.getSerialNum();
                string trandeNo = "";
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, money, reqsn, trandeNo);

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", money, xmlDoc);
                createXmlNode(refund, "_11", _11, xmlDoc);
                createXmlNode(refund, "_23", _23, xmlDoc);
                createXmlNode(refund, "_35", _35, xmlDoc);
                createXmlNode(refund, "_36", _36, xmlDoc);
                createXmlNode(refund, "_55", _55, xmlDoc);
                createXmlNode(refund, "pwd", pwd, xmlDoc);
                createXmlNode(refund, "batchNo", batchNo, xmlDoc);
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);


                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                log.Write("添加银行卡待退款纪录成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款纪录出错：" + e.Message);
            }

            return false;
        }
        /// <summary>
        /// 向银行卡退款文件添加一条退款记录
        /// </summary>
        /// <param name="cloudNo">云缴费订单号</param>
        /// <param name="_2"></param>
        /// <param name="_23"></param>
        /// <param name="_35"></param>
        /// <param name="_36"></param>
        /// <param name="_55"></param>
        /// <param name="pwd"></param>
        /// <param name="money">交易金额</param>
        /// <param name="_11">流水号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="state">后台退款记录状态</param>
        /// <param name="id">后台退款记录ID</param>
        /// <param name="orderNo">云缴费订单号</param>
        /// <param name="transType"></param>
        /// <param name="conName"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool AddBankRefund(string cloudNo, string _2, string _23, string _35, string _36, string _55, string pwd, string money, string _11, string batchNo, string state, string orderNo, string transType, string conName, string serviceName, string trandNo)
        {
            try
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                string reqsn = SysBLL.getSerialNum();
                string trandeNo = trandNo;
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, money, reqsn, trandeNo);

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", money, xmlDoc);
                createXmlNode(refund, "_11", _11, xmlDoc);
                createXmlNode(refund, "_23", _23, xmlDoc);
                createXmlNode(refund, "_35", _35, xmlDoc);
                createXmlNode(refund, "_36", _36, xmlDoc);
                createXmlNode(refund, "_55", _55, xmlDoc);
                createXmlNode(refund, "pwd", pwd, xmlDoc);
                createXmlNode(refund, "batchNo", batchNo, xmlDoc);
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);


                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                log.Write("添加银行卡待退款纪录成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款纪录出错：" + e.Message);
            }

            return false;
        }
        /// <summary>
        /// 向银行卡退款文件添加一条退款记录
        /// </summary>
        /// <param name="cloudNo">云缴费订单号</param>
        /// <param name="_2"></param>
        /// <param name="_23"></param>
        /// <param name="_35"></param>
        /// <param name="_36"></param>
        /// <param name="_55"></param>
        /// <param name="pwd"></param>
        /// <param name="money">交易金额</param>
        /// <param name="_11">流水号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="state">后台退款记录状态</param>
        /// <param name="id">后台退款记录ID</param>
        /// <param name="orderNo">云缴费订单号</param>
        /// <param name="transType"></param>
        /// <param name="conName"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool AddBankRefund(string cloudNo, string _2, string _23, string _35, string _36, string _55, string pwd, string money, string _11, string batchNo, string state, string orderNo, string transType, string conName, string serviceName, string trandNo, string GasReadState)
        {
            try
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                string reqsn = SysBLL.getSerialNum();
                string trandeNo = trandNo;
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, money, reqsn, trandeNo);

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", money, xmlDoc);
                createXmlNode(refund, "_11", _11, xmlDoc);
                createXmlNode(refund, "_23", _23, xmlDoc);
                createXmlNode(refund, "_35", _35, xmlDoc);
                createXmlNode(refund, "_36", _36, xmlDoc);
                createXmlNode(refund, "_55", _55, xmlDoc);
                createXmlNode(refund, "pwd", pwd, xmlDoc);
                createXmlNode(refund, "batchNo", batchNo, xmlDoc);
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);
                createXmlNode(refund, "rqFlushesCode", GasReadState, xmlDoc);


                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "bankRefund.xml");
                log.Write("添加银行卡待退款纪录成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款纪录出错：" + e.Message);
            }

            return false;
        }
        private static XmlElement createXmlNode(XmlElement fatherNode, string nodeName, string value, XmlDocument xmlDoc)
        {
            XmlElement sonNode = xmlDoc.CreateElement(nodeName);
            sonNode.InnerText = value;
            fatherNode.AppendChild(sonNode);
            return sonNode;
        }

        /// <summary>
        /// 向云平台发起退款申请，成功返回对象，失败返回Null
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="transType"></param>
        /// <param name="conName"></param>
        /// <param name="serviceName"></param>
        public bool EnableRefund(XmlNode xn, XmlDocument xmlDoc, string refundFilePath, int refundType)
        {
            try
            {
                if (hasRefundRequest(xn))
                {
                    return true;
                }
                else
                {
                    string orderNo = xn.ChildNodes[0].InnerText;
                    string transType = xn.ChildNodes[1].InnerText;
                    string conName = xn.ChildNodes[2].InnerText;
                    string serviceName = xn.ChildNodes[3].InnerText;

                    string reqsn = xn.ChildNodes[11].InnerText;
                    string trandNo = xn.ChildNodes[12].InnerText;
                    string rqFlushesCode = "";

                    if (refundType == 0)
                    {
                        //银行卡
                        if (xn.ChildNodes[23] != null)
                        {
                            rqFlushesCode = xn.ChildNodes[23].InnerText;
                        }

                    }
                    else if (refundType == 1)
                    {
                        //万通卡
                        if (xn.ChildNodes[18] != null)
                        {
                            rqFlushesCode = xn.ChildNodes[18].InnerText;
                        }

                    }


                    string authcode = RefundRequestLogin();
                    if (conName.Contains("公交"))
                    {
                        authcode = RefundRequestLogin("bus");
                    }

                    string jsonResult = SendRefundRequest(authcode, serviceName, orderNo, transType, conName, reqsn, trandNo, rqFlushesCode);

                    JObject jobject = JObject.Parse(jsonResult);

                    if ("0000".Equals(jobject["retcode"].ToString()) && "0".Equals(jobject["refund_status"].ToString()))
                    {
                        log.Write("向云缴费退款申请成功，交易订单号：" + orderNo + ",退款订单号：" + jobject["out_trade_no"].ToString());
                        xn.ChildNodes[4].InnerText = "true";
                        xn.ChildNodes[5].InnerText = jobject["refund_status"].ToString();//申请状态，0为成功，1为失败

                        if (jobject["out_trade_no"] != null)
                        {
                            xn.ChildNodes[6].InnerText = jobject["out_trade_no"].ToString();//退款订单号

                        }
                        if (jobject["refund_fee"] != null)
                        {
                            xn.ChildNodes[7].InnerText = jobject["refund_fee"].ToString();//退款金额

                        }


                        xmlDoc.Save(refundFilePath);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                log.Write("判断是否可以退款出错" + e.Message+e.InnerException);
            }
            return false;
        }

        private string SendRefundRequest(string authcode, string serviceName, string orderNo, string transType, string conName, string reqsn, string trandNo, string rqFlushesCode)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("servicename", serviceName);// TK001 not null
            parameters.Add("loginId", SysBLL.getCpuNo());//交互终端的设备编号（用于自助终端）not null
            parameters.Add("authcode", authcode);// 认证码 not null
            //parameters.Add("reqsn", SysBLL.getSerialNum());//请求流水号 not null
            parameters.Add("trandateTime", SysBLL.getYYYYMMDDHHMMSSTime());//  交易时间 格式：YYYYMMDDHHMMSS not null
            parameters.Add("orderno", orderNo);//订单编号 not null
            parameters.Add("transType", transType);//线上线下 not null
            parameters.Add("conName", conName);
            parameters.Add("reqsn", reqsn);
            parameters.Add("trandeNo", trandNo);
            parameters.Add("rqFlushesCode", rqFlushesCode);
            return HttpHelper.getHttp(SysConfigHelper.readerNode("refund"), parameters, null);

        }

        private string RefundRequestLogin()
        {
            log.Write("准备发起云缴费退款申请");

            return SysBLL.Authcode;
        }
        private string RefundRequestLogin(string type)
        {
            if ("bus".Equals(type))
            {
                BusInterface access = new BusInterface();
                BusLoginParam param = new BusLoginParam();
                param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
                param.servicename = "DL001";
                param.resqn = SysBLL.getSerialNum();
                param.loginId = SysBLL.getCpuNo();  //设备ID
                BusLoginInfo info = access.BusLogin(param);
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
            else
            {
                return SysBLL.Authcode;
            }

        }
        /// <summary>
        /// 判断当前一条退款，是否已经发送过退款申请
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        public static bool hasRefundRequest(XmlNode xn)
        {
            string refundRequestResult = xn.ChildNodes[4].InnerText;
            string refundRequestState = xn.ChildNodes[5].InnerText;
            string refundOrderNo = xn.ChildNodes[6].InnerText;
            string refundPrice = xn.ChildNodes[7].InnerText;

            if ("true".Equals(refundRequestResult))
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        public static bool AddRefund(string cloudNo, string _2, string _4, string _59, string state, string orderNo, string transType, string conName, string serviceName, string wantonOrderNo, string trandNo, string GasReadState)
        {
            try
            {

                string filePath =  System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, _4, SysBLL.getSerialNum(), trandNo);

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", _4, xmlDoc);//金额
                createXmlNode(refund, "_59", _59, xmlDoc);
                createXmlNode(refund, "wantonOrderNo", wantonOrderNo, xmlDoc);//关联的支付流水号
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);//云缴费订单号
                createXmlNode(refund, "rqFlushesCode", GasReadState, xmlDoc);
                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml");
                log.Write("添加待退款订单成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款订单异常：" + e.Message);
            }

            return false;
        }
        public static bool AddRefund(string cloudNo, string _2, string _4, string _59, string state, string orderNo, string transType, string conName, string serviceName, string wantonOrderNo, string trandNo)
        {
            try
            {

                string filePath =  System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, _4, SysBLL.getSerialNum(), trandNo);

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", _4, xmlDoc);//金额
                createXmlNode(refund, "_59", _59, xmlDoc);
                createXmlNode(refund, "wantonOrderNo", wantonOrderNo, xmlDoc);//关联的支付流水号
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);//云缴费订单号

                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml");
                log.Write("添加待退款订单成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款订单异常：" + e.Message);
            }

            return false;
        }
        /// <summary>
        /// 测试
        /// </summary>
        public static bool AddRefund(string cloudNo, string _2, string _4, string _59, string state, string orderNo, string transType, string conName, string serviceName, string wantonOrderNo)
        {
            try
            {

                string filePath =  System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("refunds");
                XmlElement refund = xmlDoc.CreateElement("refund");
                AddRefundBaseInfo(xmlDoc, refund, state, orderNo, transType, conName, serviceName, _4, SysBLL.getSerialNum(), "");

                createXmlNode(refund, "_2", _2, xmlDoc);
                createXmlNode(refund, "_4", _4, xmlDoc);//金额
                createXmlNode(refund, "_59", _59, xmlDoc);
                createXmlNode(refund, "wantonOrderNo", wantonOrderNo, xmlDoc);//关联的支付流水号
                createXmlNode(refund, "cloudNo", cloudNo, xmlDoc);//云缴费订单号

                root.AppendChild(refund);
                xmlDoc.Save( System.AppDomain.CurrentDomain.BaseDirectory + "dataFile.xml");
                log.Write("添加待退款订单成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待退款订单异常：" + e.Message);
            }

            return false;
        }
    }

}
