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
using System.Windows.Threading;
using wtPay.pay;
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel;
using wtPayModel.BroadCas;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormBroadCas
{
    /// <summary>
    /// FormBroadCasStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormBroadCasStep06 : UserControl
    {
        //万通卡读卡器
        WantongBLL wt = new WantongBLL();
        //缴费
        BoadCasQueryOrderlist list = null;

        Dictionary<string, string> icParams = null;

        //支付线程
        Thread payThread = null;
        //支付密码
        string pwd = "";
        public FormBroadCasStep06()
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
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                list = Payment.broadCasPayParam.List;
                pwd = Payment.broadCasPayParam.Pwd;
                icParams = Payment.broadCasPayParam.IcParams;
                SysBLL.Player("交易处理中，请稍后.wav");
                payThread = new Thread(delegate () { pay(); });
                payThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep06:load():" + ex.Message);
            }
        }
        public void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：广电");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            //订单结果
            BroadCasOrderInfo orderinfo = null;
            Pay pay = new Pay();
            //初始化银行卡交易参数
            payAccess.InitPayParam(ref p);
            //设置交易流水
            PayBLL.payCode_terminalNo(ref p);
            try
            {
                //获取订单
                orderInfo(ref orderinfo, p);
                //获取订单结果处理
                if (pay.orderInfoResult(orderinfo.msgrsp.retcode, orderinfo.msgrsp.retshow) == false) return;
                //支付
                if(pay.payResult(ref p))
                {
                    log.Write("--------------------交易结束--------------------");
                    Util.JumpUtil.jumpCommonPage("FormBroadCasStep06_success");
                }return;
            }
            catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch (Exception ee)
            {
                log.Write("交易异常:" + ee.Message);
                exit("缴费失败，请重新缴费，或退卡");
                return;
            }

        }
        private void exit(string info)
        {
            log.Write("--------------------交易结束--------------------");
            FormTip.FormFailShowinfo = info;
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormFail"); }));
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
                log.Write("error:FormBroadCasStep06:UserControl_Unloaded:" + ex.Message);
            }
        }
        DispatcherTimer timerLoad;

        public void orderInfo(ref BroadCasOrderInfo orderinfo,PayParam p)
        {
            //获取订单
            BroadCasOrderParam param = new BroadCasOrderParam();
            param.paymentno = list.CUSTNO;
            param.paymentAmout = list.BANLANCE;
            //param.balenceNO = list.balenceNO;
            param.shopType = PayAccess.isWtLkl(p.icParams);
            log.Write("获取订单：用户编号：" + param.paymentno + ",账单金额：" + param.paymentAmout + "，余额账本编码：" + param.balenceNO);
            //发送缴费订单
            orderinfo = BroadCasAccess.order(param);
            if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //交易金额
            p.rechageAmount = orderinfo.msgrsp.realAmout;
            Payment.broadCasPayParam.RechageAmount = orderinfo.msgrsp.realAmout;
            //交易订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            Payment.broadCasPayParam.BroadCasOrderInfo = orderinfo;
            log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);
            
        }
    }
}
