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
using wtPay.pay;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel;
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormGas.FormGasGoldenCard
{
    /// <summary>
    /// FormGasGoldenCardStep07.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasGoldenCardStep07 : UserControl
    {
        BitmapImage jinImage = new BitmapImage(new Uri("/cut-2/金卡.png", UriKind.Relative));
        BitmapImage xianfengImage = new BitmapImage(new Uri("/cut-2/先锋.png", UriKind.Relative));

        PasswordBLL pwdBLL = new PasswordBLL();

        //甘肃一卡通卡读卡器
        WantongBLL wt = new WantongBLL();
        //读卡器类
        JinCardBLL jinCard = new JinCardBLL();
        //燃气卡信息
        GasCard card = new GasCard();
        //先锋卡读卡器
        XianfengBLL xf = new XianfengBLL();

        Dictionary<string, string> icParams = null;
        //支付线程
        Thread payThread = null;
        public FormGasGoldenCardStep07()
        {
            InitializeComponent();
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
                SysBLL.Player("燃气卡写入.wav");
                card = Payment.GasPayParam.GasCard;
                icParams = Payment.GasPayParam.IcParams;

                if (card.cardType == 1)
                {
                    //金卡
                    img1.Source = jinImage;
                }
                else
                {
                    //先锋卡
                    img1.Source = xianfengImage;
                }

                payThread = new Thread(delegate () { pay(); });
                payThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep07:load():" + ex.Message);
                exit("充值失败，请稍后再试...");
            }
        }
        public void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：燃气");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单结果
            GasOrderInfo orderinfo = null;
            //初始化参数
            payAccess.InitPayParam(ref p);
            PayBLL.payCode_terminalNo(ref p);
            try
            {
                //获取订单
                orderInfo(ref orderinfo, p);
                //获取订单结果处理
                if (pay.orderInfoResult(orderinfo.msgrsp.retcode, orderinfo.msgrsp.retshow) == false) return;
                //支付
                if (pay.payResult(ref p))
                {
                    bool isCarsWrite = false;
                    WriteCardParam writeCardParam = new WriteCardParam();
                    writeCardParam.write_card_status = "3";
                    //金卡卡内充值
                    if (card.cardType == 1)
                    {
                        int rechageNum = Convert.ToInt32(card.rechargeNum) + card.GasValue;
                        if (jinCard.Recharge(card.CardNo, rechageNum))
                        {
                            isCarsWrite = true;
                            writeCardParam.write_card_status = "1";
                        }
                    }
                    //先锋卡内充值
                    if (card.cardType == 2)
                    {
                        int rechageNum = Convert.ToInt32(card.rechargeNum);
                        if (xf.Recharge(card.CardNo, rechageNum))
                        {
                            isCarsWrite = true;
                            writeCardParam.write_card_status = "1";
                        }
                    }
                    //写卡状态
                    writeCardParam.serviceType = p.serviceType;
                    writeCardParam.rqFlushesCode = isCardTypeRq(card);
                    writeCardParam.shop_type = PayAccess.isWtLkl(p.icParams);
                    writeCardParam.cloud_no = orderinfo.msgrsp.orderNo;
                    writeCardParam.data = pay.refundStr(p.payResultInfo, p);
                    payAccess.WriteCardAcc(writeCardParam);
                    if (isCarsWrite)
                    {
                        log.Write("--------------------交易结束--------------------");
                        Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep08_success");
                        return;
                    }
                    else
                    {
                        log.Write("error:燃气卡写卡失败");
                        exitRefund("缴费失败，退款成功");
                        return;
                    }
                }
                return;
            }
            catch (Exception ex) { log.Write("error:" + ex.Message); }
            exit("缴费失败，退款成功");
            return;
        }
        private void exit(string info)
        {
            FormTip.FormFailShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFail");
        }
        private void exitRefund(string info)
        {
            FormTip.FormFailRefundShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFailRefund");
        }
        private string isCardTypeRq(GasCard card)
        {
            //金卡卡内充值
            if (card.cardType == 1)
            {
                return "RQ005";
            }
            //先锋卡内充值
            else if (card.cardType == 2)
            {
                return "RQ005";
            }
            else
            {
                return "";
            }
        }

        public void orderInfo(ref GasOrderInfo orderinfo, PayParam p)
        {
            log.Write("发起订单：燃气卡号：" + card.CardNo + ",充值气量：" + card.rechargeNum);
            GasOrderParam gasOrderParam = new GasOrderParam();
            gasOrderParam.paymentno = card.CardNo;
            gasOrderParam.chargeAmount = card.rechargeNum;
            gasOrderParam.paymentAmout = card.price;
            gasOrderParam.shopType = PayAccess.isWtLkl(p.icParams);
            //发送燃气订单
            orderinfo = GasAccess.gasOrder(gasOrderParam);
            if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if ("11111".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //交易金额
            p.rechageAmount = orderinfo.msgrsp.realAmout;
            //交易订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            if (!orderinfo.msgrsp.retcode.Equals("0000"))
            {
                throw new Exception();
            }
            Payment.GasPayParam.GasOrderInfo = orderinfo;
        }
    }
}
