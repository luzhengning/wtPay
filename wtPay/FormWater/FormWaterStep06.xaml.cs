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
using wtPayBLL;
using WtPayBLL;
using wtPayDAL;
using wtPayModel.WaterModel;
using wtPayModel;
using wtPayModel.WintopModel;
using System.Windows.Threading;
using wtPayModel.PayParamModel;
using wtPayDAL.Pay;
using wtPay.pay;
using wtPayModel.PaymentModel;
using wtPayModel.SystemModel;

namespace wtPay.FormWater
{
    /// <summary>
    /// FormWaterStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormWaterStep06 : UserControl
    {
        //万通卡读卡器
        WantongBLL wt = new WantongBLL();
        //缴费列表
        WaterQueryInfo info = null;

        PasswordBLL pwdBLL = new PasswordBLL();
        //支付线程
        Thread payThread = null;

        //Dictionary<string, string> icParams = null;
        //支付密码
        string pwd = "";

        DispatcherTimer timerLoad;
        public FormWaterStep06()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {

        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {

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
                info = Payment.waterPayParam.WaterQueryInfo;
                this.pwd = Payment.waterPayParam.Pwd;
                payThread = new Thread(pay);
                payThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormWaterStep06:load():" + ex.Message);
            }
        }
        private void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：水务");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单结果
            WaterOrderInfo orderinfo = null;
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
                    Util.JumpUtil.jumpCommonPage("FormWaterStep06_success");
                }
                return;
            }
            catch (Exception ex6)
            {
                log.Write("交易异常:" + ex6.Message);
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
                log.Write("error:FormWaterStep06:unloaded:" + ex.Message);
            }
        }
        private void orderInfo(ref WaterOrderInfo orderinfo,PayParam p)
        {
            WaterOrderParam param = new WaterOrderParam();
            param.paymentno = info.msgrsp.orderlist[0].custNo;
            param.billdate = info.msgrsp.orderlist[0].billDate;
            param.paymentamout = info.msgrsp.orderlist[0].amout;
            param.shopType = PayAccess.isWtLkl(p.icParams);
            log.Write("发起订单：用户编号：" + param.paymentno + ",账单金额：" + param.paymentamout);
            orderinfo = WaterAccess.WaterOrder(param);
            
            if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //订单实际支付金额
            p.rechageAmount = Convert.ToDouble(orderinfo.msgrsp.realAmout).ToString();
            //交易订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            Payment.waterPayParam.OrderInfo = orderinfo;
            log.Write("订单提交成功：云平台订单号：" + orderinfo.msgrsp.orderNo);
        }
    }
}
