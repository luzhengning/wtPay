using Newtonsoft.Json;
using RefundUtils;
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
using System.Windows.Threading;
using wtPay.pay;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel;
using wtPayModel.Mobile;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep6.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep06 : UserControl
    {
        //万通卡读卡器
        WantongBLL wt = new WantongBLL();
        //支付信息
        MobilePayParam payParam = null;
        //支付线程
        Thread payThread = null;
        DispatcherTimer timerLoad;


        public FormMobileStep06()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("交易处理中，请稍后.wav");
                load();
            }
            catch (Exception ex)
            {
                log.Write("error:FormMobileStep06:UserControl_Loaded:" + ex.Message);
                exit("充值失败，请稍后再试...");
            }
        }
        private void load()
        {
            //支付参数
            payParam = Payment.mobilePayParam;
            //启动缴费
            payThread = new Thread(delegate () { pay(); });
            payThread.Start();

            timerLoad = new DispatcherTimer();
            timerLoad.Interval = TimeSpan.FromMilliseconds(400);
            timerLoad.Tick += new EventHandler(timer_Tick);
            timerLoad.Start();
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
        void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：移动");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单结果
            MobileOrderInfo orderinfo = null;
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
                    Util.JumpUtil.jumpCommonPage("FormMobileStep06_success");
                }
                return;
            }
            catch (Exception ex9)
            {
                log.Write("交易异常:" + ex9.Message);
                exit(SysConfigHelper.readerNode("payUnknownInfo"));
                return;
            }

        }
        private void exit(string info)
        {
            log.Write("--------------------交易结束--------------------");
            //new FormJump().openForm(senderLoad, eLoad, this, new FormMobileStep06_fail(info));
            FormTip.FormFailShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFail"); }));
        }
        private void exitRefund(string info)
        {
            FormTip.FormFailRefundShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFailRefund");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormMobileStep06:UserControl_Unloaded:" + ex.Message);
            }
        }

        public void orderInfo(ref MobileOrderInfo orderinfo,PayParam p)
        {
            try
            {
                MobileOrderParam mobileOrderParam = new MobileOrderParam();
                //获取订单
                log.Write("发起订单：电话号码：" + payParam.PhoneOn + ",缴费金额：" + payParam.UserInputMoney);
                mobileOrderParam.mobile = payParam.PhoneOn;
                mobileOrderParam.paymentAmout = p.userInputAmount;
                mobileOrderParam.shopType = PayAccess.isWtLkl(p.icParams);
                orderinfo = MobileAccess.order(mobileOrderParam, payParam.QueryInfo);
                if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
                if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
                if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
                //获取商户号
                p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
                //获取终端号
                p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
                if (PayStaticParam.payType == 0) {
                    //交易金额
                    p.rechageAmount = p.userInputAmount;
                    Payment.mobilePayParam.RechageAmount = p.userInputAmount;
                }
                else
                {
                    //交易金额
                    p.rechageAmount = orderinfo.msgrsp.realAmout;
                    Payment.mobilePayParam.RechageAmount = orderinfo.msgrsp.realAmout;
                }
                //交易订单号
                p.orderNo = orderinfo.msgrsp.orderNo;
                Payment.mobilePayParam.OrderInfo = orderinfo;
                log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);
            }
            catch (Exception ex)
            {
                log.Write("获取缴费订单失败:" + ex.Message + Environment.NewLine + ex.InnerException);
                //获取订单的失败 
                exit("充值失败，请稍后再试...");
                return;
            }
        }
    }
}
