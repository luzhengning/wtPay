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
using wtPayModel.ElecModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormElectric
{
    /// <summary>
    /// FormElectricStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormElectricStep06 : UserControl
    {
        //万通卡读卡器
        WantongBLL wt = new WantongBLL();
        //支付信息
        ElecPayParam payParam = null;
        //支付线程
        Thread payThread = null;

        Dictionary<string, string> icParams = null;
        DispatcherTimer timerLoad;
        public FormElectricStep06()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormElectricStep06_success");
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

                payParam = Payment.elecPayParam;


                icParams = Payment.elecPayParam.IcParams;
                payThread = new Thread(delegate () { pay(); });
                payThread.Start();
                SysBLL.Player("交易处理中，请稍后.wav");
                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:电力支付异常：" + ex.Message);
                exit("缴费失败，请稍后再试...");
            }
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
        public void pay()
        {
            TradeRecord tradeRecord = new TradeRecord();

            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：电力");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单
            ElecOrderInfo orderinfo = null;
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
                    Util.JumpUtil.jumpCommonPage("FormElectricStep06_success");
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
            FormTip.FormFailShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFail"); }));
        }
        void exitFund()
        {
            log.Write("--------------------交易结束--------------------");
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFailRefund"); }));
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
                log.Write("error:FormElecticStep06:UserControl_Unloaded:" + ex.Message);
            }
        }
        private void orderInfo( ref ElecOrderInfo orderinfo,PayParam p)
        {
            log.Write("发起订单：用户编号：" + payParam.Account + ",充值金额：" + payParam.UserInputMoney);
            payParam.ShopType = PayAccess.isWtLkl(p.icParams);
            orderinfo = ElecAccess.ElecOrder(payParam);

            if("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //交易金额
            p.rechageAmount = orderinfo.msgrsp.realAmout;
            //订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            Payment.elecPayParam.OrderInfo = orderinfo;
            Payment.elecPayParam.RechageAmount = orderinfo.msgrsp.realAmout;
            log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);
            
        }
    }
}
