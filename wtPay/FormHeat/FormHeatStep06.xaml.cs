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
using wtPayModel.HeatModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormHeat
{
    /// <summary>
    /// FormHeatStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormHeatStep06 : UserControl
    {
        HeatQueryOrderlist info = null;
        string pwd = null;

        //支付线程
        Thread payThread = null;

        object senderLoad;
        EventArgs eLoad;

        WantongBLL wt = new WantongBLL();
        Dictionary<string, string> icParams = null;

        DispatcherTimer timerLoad;
        public FormHeatStep06()
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
                this.info = Payment.heatPayParam.HeatQueryOrderlist;
                this.pwd = Payment.heatPayParam.Pwd;
                this.icParams = Payment.heatPayParam.IcParams;
                SysBLL.Player("交易处理中，请稍后.wav");
                payThread = new Thread(pay);
                payThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormHeatStep06:load():" + ex.Message);
            }
        }
        private void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：热力");
            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单结果
            HeatOrderInfo orderinfo = null;
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
                    Util.JumpUtil.jumpCommonPage("FormHeatStep06_success");
                }
                return;
            }
            catch (Exception ex5)
            {
                log.Write("交易异常:" + ex5.Message);
                exit("缴费失败，请重新缴费，或退卡");
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
                log.Write("error:FormHeatStep06:unloaded:" + ex.Message);
            }
        }
        public void orderInfo(ref HeatOrderInfo orderinfo,PayParam p)
        {
            //热力订单
            HeatOrderParam orderParam = new HeatOrderParam();
            orderParam.paymentno = info.custNo; //查询的custNo
            orderParam.realAmout = info.amout; //查询的amout
            orderParam.paymentAmout = info.amout;
            orderParam.billDate = info.billDate;
            orderParam.shopType = PayAccess.isWtLkl(p.icParams);

            log.Write("发起订单：用户编号：" + orderParam.paymentno + ",缴费金额：" + info.amout);
            orderinfo = HeatAccess.HeatOrder(orderParam);
            if ("9999".Equals(orderinfo.msgrsp.retcode)) return;
            if ("1234".Equals(orderinfo.msgrsp.retcode)) return;
            if (!"0000".Equals(orderinfo.msgrsp.retcode)) return;
            //获取商户号
            p.MERCHANTNO_shopNo = orderinfo.msgrsp.MERCHANTNO;
            //获取终端号
            p.TERMINALNO_clientNo = orderinfo.msgrsp.TERMINALNO;
            //交易金额
            p.rechageAmount = orderinfo.msgrsp.realAmout;
            //交易订单号
            p.orderNo = orderinfo.msgrsp.orderNo;
            Payment.heatPayParam.RechageAmount = orderinfo.msgrsp.realAmout;
            Payment.heatPayParam.HeatOrderInfo = orderinfo;
            log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);
        }
    }
}
