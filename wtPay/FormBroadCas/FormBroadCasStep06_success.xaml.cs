using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using wtPayModel.BroadCas;
using wtPayModel.PaymentModel;

namespace wtPay.FormBroadCas
{
    /// <summary>
    /// FormBroadCasStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormBroadCasStep06_success : UserControl
    {
        BoadCasQueryOrderlist list = null;
        BroadCasOrderInfo orderInfo = null;
        public FormBroadCasStep06_success()
        {
            InitializeComponent();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
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
                isPrint = true;
                list = Payment.broadCasPayParam.List;
                orderInfo = Payment.broadCasPayParam.BroadCasOrderInfo;
                SysBLL.Player("交易完成.wav");
                //缴费金额
                lblBalance.Text = list.BANLANCE + "元";
                //用户名
                lblTotal.Text = list.CUSTNAME;
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep06_success:load():" + ex.Message);
            }
        }
        private void print()
        {
            try
            {
                WantongBLL wt = new WantongBLL();

                PrintParam printParam = new PrintParam();
                printParam.payType = "广电";
                printParam.account = list.CUSTNO;
                printParam.cardNo = Payment.broadCasPayParam.CardNo;
                printParam.amout = orderInfo.msgrsp.realAmout;
                printParam.orderno = orderInfo.msgrsp.orderNo;
                printParam.resqn = orderInfo.msghead.reqsn;
                PrintBLL.print(printParam);
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep06_success:print():" + ex.Message);
            }
        }
    }
}
