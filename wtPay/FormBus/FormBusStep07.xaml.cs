using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayModel;
using wtPayModel.BusModel;
using wtPayModel.PaymentModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormBus
{
    /// <summary>
    /// FormBusStep07.xaml 的交互逻辑
    /// </summary>
    public partial class FormBusStep07 : UserControl
    {
        BusPayParam payParam = null;
        

        Dictionary<string, string> icParams = null;

        Thread payThread = null;

        DispatcherTimer timerLoad;
        public FormBusStep07()
        {
            InitializeComponent();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (loadlbl.Content.ToString().Length >= 6)
            {
                loadlbl.Content = "";
            }
            else
            {
                loadlbl.Content += ".";
            }
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("交易处理中，请稍后.wav");
                this.payParam = Payment.BusPayParam;
                this.icParams = Payment.BusPayParam.IcParams;
                payThread = new Thread(delegate() { pay(); });
                payThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormBusStep07:load():"+ex.Message);
            }
        }
        private void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：公交");
            try
            {
                WantongBLL wt = new WantongBLL();
                string wtCard = "";
                string icParamsJson = "";

                string order = "";
                string orderparam = "";
                string serialStr = "";
                string reconcStr = "";
                string result = "";
                string jsonPayParam = "";

                string busCard = "";

                string batchNo = "";
                string payMoney = "";

                //交易日志，订单编号
                string orderNo = "";
                //交易日志，商户类型
                string shopType = "";

                string shopNo = "";

                string trandNo = "";

                PasswordBLL pwdBLL = new PasswordBLL();
                Dictionary<string, string> map = null;
                string payCode = "Z000000003";//payCode = "Z000000005";

                string ClientNo = "";

                string terminalNo = SysConfigHelper.readerNode("ClientNo");

                if (icParams != null)
                {
                    payCode = "Z000000005";
                    terminalNo = SysConfigHelper.readerNode("LklClientNo");
                }
                
                WintopPayParam witonPay = new WintopPayParam();
                
                witonPay.price = payParam.UserInputMoney;
                witonPay.pwd = payParam.Pwd;
                busCard = Payment.BusPayParam.BusNo;
                BusReChangeParam busReChangeParam = new BusReChangeParam();
                busReChangeParam.appsid = payParam.Output.APPSID;
                busReChangeParam.paymentAmout = payParam.UserInputMoney;
                busReChangeParam.paymentno = busCard;
                busReChangeParam.wmoney = payParam.Output.WMONEY;
                busReChangeParam.trandeNo = SysBLL.getHHMMSSITime10();
                //busReChangeParam.orderno = orderNo;
                log.Write("发起公交充值");

                BusCpuCardParam param = new BusCpuCardParam();
                BusCpuCardInfo busCpuCardInfo = BusAccess.GetOrder(busReChangeParam, param, payCode);
                Payment.BusPayParam.BusCpuCardInfo = busCpuCardInfo;
                string cloudNo = busCpuCardInfo.msgrsp.orderno;
                  
                //向后台发送订单记录
                if (icParams == null)
                {
                    orderNo = Wanton.GetOrderNo();
                    //tradeRecord.branch_termail_no = SysConfigHelper.readerNode("ClientNo");
                    ClientNo = BusPayParam.busShopNo + "|" + SysConfigHelper.readerNode("ClientNo");
                }
                else
                {
                    orderNo = LKLProcedure.GetOrderNo();
                    ClientNo = BusPayParam.busShopNo + "|" + BusPayParam.busClient;
                }
                //支付
                try
                {
                    if (icParams == null)
                    {
                        log.Write("交易类型：万通交易");
                        //万通卡号
                        //orderNo = SysBLL.GetRnd(6, true, false, false, false, "");
                        shopNo = orderNo + "|" + SysConfigHelper.readerNode("ClientNo") + "|" + SysConfigHelper.readerNode("ShopNo");
                        witonPay.orderNo = orderNo;
                        witonPay.price = payParam.UserInputMoney;
                        wtCard = wt.GetCardNo();
                        witonPay.wintopNo = wtCard;
                        pwdBLL.SetCryptMode(1);//万通卡加密方式
                        log.Write("万通卡号：" + witonPay.wintopNo);
                        log.Write("交易金额：" + witonPay.price);
                        log.Write("终端交易流水号" + orderNo);
                        log.Write("开始支付");
                        witonPay.pwd = payParam.Pwd;
                        map = Wanton.pay(witonPay, BusPayParam.busShopNo);
                        shopType = "1";
                        jsonPayParam = JsonConvert.SerializeObject(witonPay);
                        map.Add("payParam", jsonPayParam);
                    }
                    else
                    {
                        log.Write("交易类型：拉卡拉交易");
                        //银行卡
                        //orderNo = SysBLL.GetOrderNo();
                        shopNo = orderNo + "|" + SysConfigHelper.readerNode("ClientNo") + "|" + SysConfigHelper.readerNode("ShopNo");
                        batchNo = SysBLL.GetBatchNo();
                        pwdBLL.SetCryptMode(0);//银行卡加密方式
                        payMoney = payParam.UserInputMoney;
                        log.Write("银行卡号：" + icParams["cardNo"]);
                        log.Write("交易金额：" + payMoney);
                        log.Write("终端交易流水号" + orderNo);
                        log.Write("开始支付");
                        map = wtPayBLL.LKLProcedure.PayOrder(payParam.Pwd, payMoney, icParams, orderNo, batchNo, BusPayParam.busShopNo, BusPayParam.busClient);
                        shopType = "0";
                        icParamsJson = JsonConvert.SerializeObject(icParams);
                        map.Add("bankParam", icParamsJson);
                    }
                    map.Add("orderType", "5");//1、广电 2、水务3、燃气4、电力5、公交6、万通卡7、热力
                    map.Add("payType", shopType);//拉卡拉是0，万通是1
                    map.Add("payOrderNo", orderNo);
                    map.Add("busCardNo", busCard);

                    //如果万通支付成功
                    if (!map["recode"].Equals("00"))
                    {
                        if ("55".Equals(map["recode"]))
                        {
                            SysBLL.PasswordErrorInfo = "密码错误，请重新输入！";
                            log.Write("密码错误");
                            log.Write("--------------------交易结束--------------------");
                            if (icParams == null)
                            {
                                pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", wtCard);
                                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormInputPassword"); }));
                                return;
                            }
                            else
                            {
                                pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", icParams["cardNo"]);
                                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormInputPassword"); }));
                                return;
                            }
                        }
                        else if ("51".Equals(map["recode"]))
                        {
                            log.Write("余额不足");
                            exit("余额不足");
                            return;
                        }
                        else
                        {
                            log.Write("支付失败：返回码：" + map["recode"]);
                            exit("充值失败，请重新缴费，或退卡");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Write("支付异常，发起冲正:" + ex.Message);
                    //冲正
                    if (icParams == null)
                    {
                        //万通冲正
                        for (int i = 1; i <= 3; i++)
                        {
                            try
                            {
                                Dictionary<string, string> mapCor = Wanton.correct(witonPay, BusPayParam.busShopNo);
                                if (mapCor == null) mapCor = new Dictionary<string, string>();
                                mapCor.Add("payParam", jsonPayParam);
                                mapCor.Add("orderType", "5");//1、广电 2、水务3、燃气4、电力5、公交6、万通卡7、热力
                                mapCor.Add("payType", shopType);//拉卡拉是0，万通是1
                                mapCor.Add("payOrderNo", orderNo);
                                mapCor.Add("busCardNo", busCard);


                                if (mapCor["recode"].Equals("00"))
                                {
                                    log.Write("冲正成功");
                                    break;
                                }
                                else
                                {
                                    log.Write("冲正失败,返回码：" + mapCor["recode"]);
                                }
                            }
                            catch (Exception)
                            {
                                log.Write("冲正失败");
                            }
                            finally { Thread.Sleep(wtPayUtils.Fibonacci(i) * 1000); }
                        }
                        exit("充值失败，请重新缴费，或退卡");
                        return;
                    }
                    else
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            try
                            {
                                //拉卡拉冲正
                                Dictionary<string, string> ret = LKLProcedure.correct(icParams, payMoney, orderNo, batchNo );
                                if (ret == null) ret = new Dictionary<string, string>();
                                ret.Add("payParam", jsonPayParam);
                                ret.Add("orderType", "5");//1、广电 2、水务3、燃气4、电力5、公交6、万通卡7、热力
                                ret.Add("payType", shopType);//拉卡拉是0，万通是1
                                ret.Add("payOrderNo", orderNo);
                                ret.Add("busCardNo", busCard);

                                if (ret["recode"].Equals("00"))
                                {
                                    log.Write("冲正成功");
                                    break;
                                }
                                else
                                {
                                    log.Write("冲正失败，返回码：" + ret["recode"]);
                                }
                            }
                            catch (Exception ex8) { log.Write("冲正失败"); }
                            finally { Thread.Sleep(wtPayUtils.Fibonacci(i) * 1000); }
                        }
                        exit("充值失败，请重新缴费，或退卡");
                        return;
                    }

                }
                string busOrderNo = "";
                string recode = "";
                //发送订单，充值
                try
                {
                    trandNo = map["cloudOrderNo"];
                    busCpuCardInfo = BusAccess.ReCharge(busCpuCardInfo, param);


                    if (busCpuCardInfo.cpumsg.OUTPUT != null)
                    {
                        try
                        {
                            order = JsonConvert.SerializeObject(busCpuCardInfo);
                            map.Add("payres", order);
                            orderparam = JsonConvert.SerializeObject(busReChangeParam);
                            map.Add("payresparam", orderparam);
                            map.Add("state", "消费成功");
                            map.Add("type", "1");//1消费，2冲正，3消费撤销
                            serialStr = JsonConvert.SerializeObject(map);
                            reconcStr = busCpuCardInfo.msgrsp.orderno + "|" + busCard + "|" + busCpuCardInfo.msghead.reqsn + "|" + orderNo + "|" + busCpuCardInfo.msgrsp.paymentAmout + "|" + SysBLL.getYYYYMMDDHHMMSSTime() + "|" + busCpuCardInfo.msgrsp.Tac + "|" + busCpuCardInfo.msghead.reqsn + "|" + payParam.Output.WMONEY + "";


                            result = HttpHelper.sendPaymentLog(serialStr, reconcStr, shopNo, shopType, "1");
                        }
                        catch (Exception ex) { log.Write("发送对账信息异常:" + ex.Message); }
                        finally
                        {
                            log.Write("交易成功，交易信息：" + reconcStr);
                            log.Write("--------------------交易结束--------------------");
                            //支付成功
                            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormBusStep08_success"); }));
                        }
                        return;
                    }
                    throw new Exception();
                }
                catch (Exception ex3)
                {
                    log.Write("支付通知交易异常:" + ex3.Message);
                    //string refund = map["_2"] + "|" + map["_4"] + "|" + map["_59"] + "|" + "";
                    //退款
                    string conName = "公交卡退费申请";
                    //string payOrderNo = busCpuCardInfo.msgrsp.orderno;
                    //tring payOrderNo = busOrderNo.ToString();
                    string payOrderNo = cloudNo;
                    string transType = "02";
                    string serviceName = "TK001";

                    try
                    {
                        if (icParams == null)
                        {
                            //string sendPaymentLogResult = HttpHelper.sendPaymentLog(serialStr, reconcStr, shopNo, shopType);
                            //JObject paymentLogObject = JObject.Parse(sendPaymentLogResult);//用于获取后台生成的ID
                            string refundRequestState = "0";//是否需要向云平台发起退款申请
                            
                        }
                        else
                        {
                            
                            //string sendPaymentLogResult = HttpHelper.sendPaymentLog(serialStr, reconcStr, shopNo, shopType);
                            //JObject paymentLogObject = JObject.Parse(sendPaymentLogResult);//用于获取后台生成的ID
                            string refundRequestState = "1";//是否需要向云平台发起退款申请
                           
                        }

                    }
                    catch (Exception ex) { log.Write("退款异常：" + ex.Message); }
                    finally
                    {
                        payFail("充值失败，请重新缴费，或退卡");
                    }
                    return;
                }
            }
            catch (Exception ex5)
            {
                log.Write("交易异常:" + ex5.Message);
                exit("充值失败，请重新缴费，或退卡");
                return;
            }
        }
        private void exit(string info)
        {
            log.Write("--------------------交易结束--------------------");
            FormTip.FormFailRefundShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFail"); }));
        }
        private void payFail(string info)
        {
            log.Write("--------------------交易结束--------------------");
            //new FormJump().openForm(senderLoad, eLoad, this, new FormWaterStep06_fail(info));
            FormTip.FormFailRefundShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFailRefund"); }));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }catch(Exception ex)
            {
                log.Write("error:FormBusStep07:unload:"+ex.Message);
            }
        }
    }
}
