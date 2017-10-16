using AnalysisBase;
using Newtonsoft.Json;
using RefundUtils;
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
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep07.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep07 : UserControl
    {
        //充值线程
        Thread payThread = null;

        PasswordBLL pwdBLL = new PasswordBLL();
        //退款
        TradeRecord tradeRecord = null;
        //支付参数
        WintopPayParam witonPay = null;
        Dictionary<string, string> map = null;

        public FormCitizenStep07()
        {
            InitializeComponent();
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
                SysBLL.Player("交易处理中，请稍后.wav");
                payThread = new Thread(delegate () { pay(); });
                payThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStep07:load():" + ex.Message);
            }
        }
        private void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：甘肃一卡通充值");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单
            WintopOrderInfo orderinfo = null;
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
                    log.Write("--------------------交易结束--------------------");
                    Util.JumpUtil.jumpCommonPage("FormCitizenStep08_success");
                }
                return;
            }
            catch (Exception ex9)
            {
                log.Write("交易异常:" + ex9.Message);
                exit(SysConfigHelper.readerNode("payFailInfo"));
                return;
            }
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
            //new FormJump().openForm(senderLoad, eLoad, this, new FormMobileStep06_fail(info));
            FormTip.FormFailShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFail"); }));
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }
        private void orderInfo(ref WintopOrderInfo orderinfo, PayParam p)
        {
            log.Write("发起订单：甘肃一卡通卡号：" + Payment.wintopReChargeParam.WtCardNo + "，充值金额：" + Payment.wintopReChargeParam.UserInputMoney);
            Payment.wintopReChargeParam.ShopType = PayAccess.isWtLkl(p.icParams);
            orderinfo = WintopAccess.WintopOrder(Payment.wintopReChargeParam);
            if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //交易金额
            p.rechageAmount = Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00030;
            //订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            Payment.wintopReChargeParam.RechageAmount = Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00030;
            Payment.wintopReChargeParam.WintopOrderInfo = orderinfo;
            log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);

        }
    }
}
