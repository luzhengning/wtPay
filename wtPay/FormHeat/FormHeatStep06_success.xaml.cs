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

namespace wtPay.FormHeat
{
    /// <summary>
    /// FormHeatStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormHeatStep06_success : UserControl
    {
        public FormHeatStep06_success()
        {
            InitializeComponent();
        }
        private void print()
        {
            try
            {
                WantongBLL wt = new WantongBLL();
                //万通卡号
                string wtCard = wt.GetCardNo();

                PrintParam printParam = new PrintParam();
                printParam.payType = "热力";
                printParam.account = Payment.heatPayParam.HeatQueryOrderlist.custNo;
                printParam.cardNo = Payment.heatPayParam.CardNo;
                printParam.amout = Payment.heatPayParam.HeatOrderInfo.msgrsp.realAmout;
                printParam.orderno = Payment.heatPayParam.HeatOrderInfo.msgrsp.orderNo;
                printParam.resqn = Payment.heatPayParam.HeatOrderInfo.msghead.resqn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormHeatStep06_success:print():"+ex.Message+ex.InnerException);
            }
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
                lblUsername.Text = Payment.heatPayParam.HeatQueryOrderlist.custName;
                lblAmount.Text = Payment.heatPayParam.HeatOrderInfo.msgrsp.realAmout;
                lblDate1.Text = Payment.heatPayParam.HeatQueryOrderlist.billDate;
            }
            catch(Exception ex)
            {
                log.Write("error:FormHeatStep06_success:load():"+ex.Message);
            }
        }
    }
}
