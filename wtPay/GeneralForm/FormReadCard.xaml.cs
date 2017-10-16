using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel.ConfigModel;
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.PropModel;
using wtPayModel.PropSecModel;
using wtPayModel.WintopModel;

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormMobileStep4.xaml 的交互逻辑
    /// </summary>
    public partial class FormReadCard : UserControl
    {
        private GifImage yhCardImg = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\prop\\yhCard.gif");
        private GifImage gifImg =null;

        //BitmapImage wtImage = new BitmapImage(new Uri("/cut-2/wtcard1.png", UriKind.Relative));
        //BitmapImage yhImage = new BitmapImage(new Uri("/cut-2/yhcard2.png", UriKind.Relative));

        private delegate void showInfoDelegate(string value);
        private delegate void showBankTipDelegate();
        WantongBLL wt = new WantongBLL();
        PasswordBLL pwdBLL = new PasswordBLL();

        //读卡线程
        Thread readThread = null;
        //线程
        Thread setBankTipThread = null;
        Dictionary<string, string> dictionary = null;
        public FormReadCard()
        {
            InitializeComponent();
           
        }
        
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();

        }
        private void load()
        {
            try
            {
                try
                {
                    gifImg = null;
                    if (ConfigSysUtils.getReadCardGif() != null)
                    {
                        gifImg = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + ConfigSysUtils.getReadCardGif());
                        this.gifImg.Height = 500;
                        if (imgGrid.Children.Contains(gifImg)) imgGrid.Children.Remove(gifImg);
                        imgGrid.Children.Add(gifImg);
                        this.gifImg.StartAnimate();
                       
                    }
                    else
                    {
                        this.yhCardImg.Height = 500;
                        if (imgGrid.Children.Contains(yhCardImg)) imgGrid.Children.Remove(yhCardImg);
                        imgGrid.Children.Add(yhCardImg);
                        this.yhCardImg.StartAnimate();
                       
                    }
                }
                catch (Exception ex)
                {

                }
                cardImg.Visibility = Visibility.Hidden;
                if (SysBLL.payCostType == 3)
                {
                    PrintInfo("请将银行卡插入银行读卡器");
                    SysBLL.Player("插入银行卡.wav");
                }
                else
                {
                    PrintInfo("请将甘肃一卡通或银行卡插入银行读卡器");
                    SysBLL.Player("请插入银行卡或惠民卡.wav");
                }
                //if (SysBLL.payCostType == 12)
                //{
                //    if (imgGrid.Children.Contains(yhCardImg)) imgGrid.Children.Remove(prop2WaterImg);
                //    imgGrid.Children.Add(prop2WaterImg);
                //    this.prop2WaterImg.StartAnimate();
                //}
                
                

                readThread = new Thread(delegate () {
                    readCard();
                });
                setBankTipThread = new Thread(delegate () {
                    //设置提示信息
                    //showBankTip();
                });

                readThread.Start();
                setBankTipThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:插卡页面异常：" + ex.Message);
            }
        }
        //关闭窗口时触发
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgGrid.Children.Contains(yhCardImg)) imgGrid.Children.Remove(yhCardImg);
                this.yhCardImg.StopAnimate();
                if (imgGrid.Children.Contains(gifImg)) imgGrid.Children.Remove(gifImg);
                this.gifImg.StopAnimate();
                readThread.Abort();
                readThread.DisableComObjectEagerCleanup();
                readThread = null;

                setBankTipThread.Abort();
                setBankTipThread.DisableComObjectEagerCleanup();
                setBankTipThread = null;

            }
            catch (Exception ex)
            {
                log.Write("error：读取银行卡万通卡页面关闭时：" + ex.Message);
            }
        }
        private void readCard()
        {
            try
            {
                Thread.Sleep(1500);
                //等待用户插卡
                MachCardBLL.waitCard();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:插卡页readCard():" + ex.Message);
            }
            while (true)
            {
                try
                {
                    Thread.Sleep(700);
                    //卡机内是否有卡
                    if (!MachCardBLL.CardUsable()) continue;
                    PrintInfo("正在读卡,请稍后...");
                    Thread.Sleep(1500);
                }
                catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
                catch (Exception ex)
                {
                    log.Write("error:插卡页readCard():" + ex.Message);
                }
                try
                {
                    //签到
                    PayAccess.ReadSign();
                    string wtNo = wt.GetCardNo();
                    if (wtNo != null)
                    {
                        if (wtNo.Length == 16)
                        {
                            if (SysBLL.payCostType == 3)
                            {
                                //退卡
                                MachCardBLL.backCard();
                                //等待用户插卡
                                MachCardBLL.waitCard();
                                PrintInfo("暂不支持此卡,请取卡");
                                return;
                            }
                            else
                            {
                                SysBLL.payCardType = 1;
                                SysBLL.payCardNo = wtNo;
                                if(!pwdBLL.SetCryptMode(1)) throw new Exception("系统异常，请稍后再试...");//万通卡加密方式
                                pwdBLL.closeKey();
                                //pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", wtNo);
                                if (!setPayParam(wtNo, null, wtNo)) {
                                    //PrintInfo("业务异常，请稍后再试...");
                                    //退卡
                                    MachCardBLL.backCard();
                                    //等待用户插卡
                                    MachCardBLL.waitCard();
                                    return;
                                };
                                SysBLL.payTypeName = "甘肃一卡通";
                                WintopStatusInfo statusInfo = new WintopAccess().queryCardStatus(wtNo);
                                if ((statusInfo.msgrsp.WTSTATE == null) || (statusInfo.msgrsp.WTSTATE.Equals("")))
                                {
                                    PrintInfo("业务正忙，请稍后再试");
                                    //退卡
                                    MachCardBLL.backCard();
                                    //等待用户插卡
                                    MachCardBLL.waitCard();
                                    return;
                                }
                                if ("01".Equals(statusInfo.msgrsp.WTSTATE))
                                {
                                    //执行下一步操作
                                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormInputPassword"); }));
                                    return;
                                }
                                else
                                {
                                    PrintInfo("该卡已挂失，无法正常使用");
                                    //退卡
                                    MachCardBLL.backCard();
                                    //等待用户插卡
                                    MachCardBLL.waitCard();
                                    return;
                                }
                            }
                        }
                    }
                    dictionary = null;
                    //银行卡
                    dictionary = BankBLL.ReadBankCard();
                    SysBLL.IcBankDictionary = dictionary;
                    //if (SysBLL.payCostType == 3)
                    //{
                    SysBLL.payCardType = 0;
                    SysBLL.payCardNo = dictionary["cardNo"];
                    if (!pwdBLL.SetCryptMode(0)) throw new Exception("系统异常，请稍后再试...");//拉卡拉加密方式
                    pwdBLL.closeKey();
                    //pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", dictionary["cardNo"]);
                    //if (!setPayParam("", dictionary, dictionary["cardNo"])) throw new Exception("查询失败");
                    if (!setPayParam("", dictionary, dictionary["cardNo"])) return; ;
                    SysBLL.payTypeName = "银行卡";
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormInputPassword"); }));
                    return;
                    //}
                    //else
                    //{
                    //    //退卡
                    //    MachCardBLL.backCard();
                    //    //等待用户插卡
                    //    MachCardBLL.waitCard();
                    //    PrintInfo("暂不支持此卡,请取卡");
                    //}


                }
                catch (ThreadAbortException ae)
                {
                    log.Write("error:读卡失败：" + ae.Message);
                    return;
                }catch(WtException we)
                {
                    log.Write("error：读卡异常");
                    PrintInfo("读取失败");
                    //退卡
                    MachCardBLL.backCard();
                    //等待用户插卡
                    MachCardBLL.waitCard();
                }
                catch (Exception ex)
                {
                    log.Write("error：读卡异常：" + ex.Message);
                    PrintInfo("读取失败");
                    //退卡
                    MachCardBLL.backCard();
                    //等待用户插卡
                    MachCardBLL.waitCard();
                }
                finally
                {
                    //btnOkBg.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 支付参数
        /// </summary>
        /// <param name="wtNo"></param>
        /// <param name="icParams"></param>
        /// <param name="cardNo"></param>
        private bool setPayParam(string wtNo, Dictionary<string, string> icParams, string cardNo)
        {
            try
            {

                switch (SysBLL.payCostType)
                {

                    case 1:
                        //移动缴费
                        Payment.mobilePayParam.WtNo = wtNo;
                        Payment.mobilePayParam.IcParams = icParams;
                        Payment.mobilePayParam.CardNo = cardNo;
                        return true;
                    case 2:
                        //联通缴费
                        Payment.unicomPayParam.WtNo = wtNo;
                        Payment.unicomPayParam.IcParams = icParams;
                        Payment.unicomPayParam.CardNo = cardNo;
                        return true;
                    case 3:
                        //万通卡充值
                        Payment.wintopReChargeParam.IcParams = icParams;
                        Payment.wintopReChargeParam.CardNo = cardNo;
                        return true;
                    case 4:
                        //电力缴费
                        Payment.elecPayParam.WtNo = wtNo;
                        Payment.elecPayParam.IcParams = icParams;
                        Payment.elecPayParam.CardNo = cardNo;
                        return true;
                    case 5:
                        if (!gasQuery())
                        {
                            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardFail");
                            return false;
                        }
                        Payment.GasPayParam.WtNo = wtNo;
                        Payment.GasPayParam.IcParams = icParams;
                        Payment.GasPayParam.CardNo = cardNo;
                        return true;
                    case 6:
                        //广电缴费
                        Payment.broadCasPayParam.WtNo = wtNo;
                        Payment.broadCasPayParam.IcParams = icParams;
                        Payment.broadCasPayParam.CardNo = cardNo;
                        return true;
                    case 7:
                        //水务缴费
                        Payment.waterPayParam.WtNo = wtNo;
                        Payment.waterPayParam.IcParams = icParams;
                        Payment.waterPayParam.CardNo = cardNo;
                        return true;
                    case 8:
                        //热力缴费
                        Payment.heatPayParam.WtNo = wtNo;
                        Payment.heatPayParam.IcParams = icParams;
                        Payment.heatPayParam.CardNo = cardNo;
                        return true;
                    case 9:
                        //公交缴费
                        Payment.BusPayParam.WtNo = wtNo;
                        Payment.BusPayParam.IcParams = icParams;
                        Payment.BusPayParam.CardNo = cardNo;
                        return true;
                    case 10:
                        //物业缴费
                        Payment.PropPayParam.WtNo = wtNo;
                        Payment.PropPayParam.IcParams = icParams;
                        Payment.PropPayParam.CardNo = cardNo;
                        return true;
                    case 11:
                        //小区物业缴费
                        if (!propTempQuery())
                        {
                            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardFail");
                            return false;
                        }
                        Payment.propPayTempParam.WtNo = wtNo;
                        Payment.propPayTempParam.IcParams = icParams;
                        Payment.propPayTempParam.CardNo = cardNo;
                        return true;
                    case 12:
                        //物业二次专供
                        Payment.propSecPayParam.WtNo = wtNo;
                        Payment.propSecPayParam.IcParams = icParams;
                        Payment.propSecPayParam.CardNo = cardNo;
                        if (!propSecOrder())
                        {
                            PrintInfo("订单生成失败，请稍后再试...");
                            return false;
                        }
                        return true;
                    default:
                        throw new Exception("缴费类型不匹配");
                }
            }
            catch (WtException wt) { return false; }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); return false; }
            catch (Exception ex) { log.Write("error:读卡页面设置参数异常：" + ex.Message); return false; }
        }
        /// <summary>
        /// 提交物业2订单
        /// </summary>
        /// <returns></returns>
        private Boolean propSecOrder()
        {
            try
            {
                PropSecAccess access = new PropSecAccess();
                PropSecOrderParam param = new PropSecOrderParam();
                param.shopType = PayAccess.isWtLkl(Payment.propSecPayParam.IcParams);
                param.AMOUNT = Payment.propSecPayParam.UserInputMoney;
                param.paymentAmout = "1";
                param.SC10009 = Payment.propSecPayParam.ManufacturerNum;
                param.SC10010 = Payment.propSecPayParam.CardType;
                param.SC10007 = ConfigSysParam.ResidentialNo;
                param.SC10014 = Payment.propSecPayParam.PrimaryKey;
                param.SC10008 = Payment.propSecPayParam.SC10008;
                param.merchantNo = Payment.propSecPayParam.merchantNo;
                PropSecOrderInfo orderinfo = access.order(param);

                if ("9999".Equals(orderinfo.msgrsp.retcode)) return false;
                if ("1234".Equals(orderinfo.msgrsp.retcode)) return false;
                if (!"0000".Equals(orderinfo.msgrsp.retcode)) return false;
                Payment.propSecPayParam.p= new PayParam();
                //获取商户号
                Payment.propSecPayParam.p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
                //获取终端号
                Payment.propSecPayParam.p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
                //订单实际支付金额
                Payment.propSecPayParam.p.rechageAmount = orderinfo.msgrsp.realAmout;
                //交易订单号
                Payment.propSecPayParam.p.orderNo = orderinfo.msgrsp.orderNo;
                Payment.propSecPayParam.RechageAmount= orderinfo.msgrsp.realAmout;
                Payment.propSecPayParam.orderInfo = orderinfo;
                log.Write("订单提交成功：云平台订单号：" + orderinfo.msgrsp.orderNo);
                return true;
            }
            catch(Exception ex)
            {

            }
            return false;
        }
        private bool gasQuery()
        {
            try
            {
                GasQueryInfo info = GasAccess.query(Payment.GasPayParam.GasCard.CardNo, Payment.GasPayParam.GasCard.rechargeNum);

                //if (!"0000".Equals(info.msgrsp.retcode))
                //{
                //    log.Write("获取燃气支付金额出错：" + info.msgrsp.retcode + "," + info.msgrsp.retcode + "," + info.msgrsp.retcode);
                //    return false;
                //}
                GasPayParam.Showinfo = info.msgrsp.retshow;
                Payment.GasPayParam.GasCard.price = info.msgrsp.orderlist[0].payableAmount;
                return true;
            }
            catch (WtException wt){ return false; }
            catch (Exception e)
            {
                log.Write("error:燃气价格查询："+e.Message);
                return false;
            }

        }

        private bool propTempQuery()
        {
            return true;
            PropFeeQueryParam param = new PropFeeQueryParam();
            param.AMOUNT = Payment.propPayTempParam.UserInputMoney;
            param.paymentno = Payment.propPayTempParam.AccountNo;
            param.TYPE = Payment.propPayTempParam.PropType;
            PropFeeQueryInfo info = PropAccess.PropFeeQuery(param);
            if (!"0000".Equals(info.msgrsp.retcode)) return false;
            Payment.propPayTempParam.RechageAmount = info.msgrsp.result.MONEY;
            Payment.propPayTempParam.MerchantNo = info.msgrsp.merchantNo;
            Payment.propPayTempParam.House = info.msgrsp.result.HOUSEID;
            return true;
        }
        private void PrintInfo(string msg)
        {
            this.lblAccountInfoTip.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), msg);
        }
        private void setShowInfo(string msg)
        {
            this.lblAccountInfoTip.Content = msg;
        }
        private void showBankTip()
        {
            this.lblBankTip.Dispatcher.Invoke(new showBankTipDelegate(setBankTip));
        }
        private void setBankTip()
        {
            if (SysBLL.payCostType == 3)
            {
                this.lblBankTip.Content = "";
            }
            else
            {
                this.lblBankTip.Content = "该业务暂不支持银行卡交易，请使用甘肃一卡通进行";
            }

        }

    }
}
