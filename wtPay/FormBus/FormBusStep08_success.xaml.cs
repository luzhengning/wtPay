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

namespace wtPay.FormBus
{
    /// <summary>
    /// FormBusStep08_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormBusStep08_success : UserControl
    {
        public FormBusStep08_success()
        {
            InitializeComponent();
        }
        private void print()
        {
            try
            {
                PrintParam printParam = new PrintParam();
                printParam.payType = "公交";
                printParam.account = Payment.BusPayParam.BusNo;
                printParam.cardNo = Payment.BusPayParam.CardNo;
                printParam.amout = Payment.BusPayParam.UserInputMoney + ".00";
                printParam.orderno = Payment.BusPayParam.BusCpuCardInfo.msgrsp.orderno;
                printParam.resqn = Payment.BusPayParam.BusCpuCardInfo.msghead.reqsn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormBusStep08_success:print():"+ex.Message);
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
        /// <summary>
        /// Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                isPrint = true;
                SysBLL.Player("交易已完成，退卡.wav");
                lblBalance.Text = Payment.BusPayParam.UserInputMoney + ".00 元";
                lblName.Text = Payment.BusPayParam.Output.ONAME;
            }
            catch(Exception ex)
            {
                log.Write("error:FormBusStep08_success:load():"+ex.Message);
            }
        }
    }
}
