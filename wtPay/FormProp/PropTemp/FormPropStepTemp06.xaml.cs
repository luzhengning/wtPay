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
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.PropModel;
using wtPayModel.SystemModel;

namespace wtPay.FormProp
{
    /// <summary>
    /// FormPropStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStepTemp06 : UserControl
    {
        //支付线程
        Thread payThread = null;
        PropPayParam payParam = null;
        DispatcherTimer timerLoad;
        public FormPropStepTemp06()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {

        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }catch(Exception ex)
            {
                log.Write("error:FormPropStep06:Unloaded:"+ex.Message);
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
                payParam = Payment.PropPayParam;
                SysBLL.Player("交易处理中，请稍后.wav");
                payThread = new Thread(delegate() { pay(); } );
                payThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormPropStep06:load:" + ex.Message);
            }
        }

        private void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：小区物业");
            Thread.Sleep(3000);
            //测试
            Util.JumpUtil.jumpCommonPage("FormPropStepTemp06_success");
            return;








            PayAccess payAccess = new PayAccess();
            PayParam p = new PayParam();
            Pay pay = new Pay();
            //订单结果
            PropOrderInfo orderinfo = null;
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
                    Util.JumpUtil.jumpCommonPage("FormPropStepTemp06_success");
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

        private void orderInfo(ref PropOrderInfo orderinfo,PayParam p)
        {
            PropOrderParam param = new PropOrderParam();
            //获取订单
            try
            {
                param.paymentno = Payment.propPayTempParam.AccountNo;
                param.merchantNo = Payment.propPayTempParam.MerchantNo;
                param.AMOUNT = Payment.propPayTempParam.UserInputMoney;
                param.HOUSEID = Payment.propPayTempParam.House;
                param.paymentAmout = Payment.propPayTempParam.RechageAmount;
                param.shopType = PayAccess.isWtLkl(p.icParams);
                param.TYPE = Payment.propPayTempParam.PropType;
                //log.Write("发起订单：用户电话号码：" + payParam.Mobile + ",账单金额：" + payParam.ChargeList.money);
                orderinfo = PropAccess.getPropOrder(param);
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
                Payment.propPayTempParam.PropOrderInfo = orderinfo;
                log.Write("订单提交成功：订单号：" + orderinfo.msgrsp.orderNo);
            }
            catch (Exception ex1)
            {
                log.Write("error:物业获取缴费订单失败！"+ex1.Message+ex1.InnerException);
                return;
            }
        }
    }
}
