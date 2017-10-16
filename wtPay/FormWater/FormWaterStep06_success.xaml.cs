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
using wtPayModel.PaymentModel;

namespace wtPay.FormWater
{
    /// <summary>
    /// FormWaterStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormWaterStep06_success : UserControl
    {
        public FormWaterStep06_success()
        {
            InitializeComponent();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
        }
        private void print()
        {
            try
            {
                WantongBLL wt = new WantongBLL();
                //万通卡号
                string wtCard = wt.GetCardNo();

                PrintParam printParam = new PrintParam();
                printParam.payType = "水务";
                printParam.account = Payment.waterPayParam.OrderInfo.msgrsp.paymentNo;
                printParam.cardNo = Payment.waterPayParam.CardNo;
                printParam.amout = Payment.waterPayParam.OrderInfo.msgrsp.realAmout;
                printParam.orderno = Payment.waterPayParam.OrderInfo.msgrsp.orderNo;
                printParam.resqn = Payment.waterPayParam.OrderInfo.msghead.reqsn;
                PrintBLL.print(printParam);
            }
            catch (Exception ex)
            {
                log.Write("error:FormMobileStep06_successprint:"+ex.Message);
            }
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
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
                isPrint = true;
                SysBLL.Player("交易完成.wav");
                //缴费金额
                lblBalance.Text = Payment.waterPayParam.OrderInfo.msgrsp.realAmout;
                //账单日期
                lblAmount.Text = Payment.waterPayParam.OrderInfo.msgrsp.billDate;
                //用户名
                lblTotal.Text = Payment.waterPayParam.OrderInfo.msgrsp.paymentNo;
            }catch(Exception ex)
            {
                log.Write("error:FormWaterStep06_success:load():"+ex.Message);
            }
        }
    }
}
