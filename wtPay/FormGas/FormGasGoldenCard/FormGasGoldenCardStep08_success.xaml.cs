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

namespace wtPay.FormGas.FormGasGoldenCard
{
    /// <summary>
    /// FormGasGoldenCardStep8_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasGoldenCardStep08_success : UserControl
    {
        public FormGasGoldenCardStep08_success()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
            isPrint = true;
        }
        private void load()
        {
            try
            {
                isPrint = true;
                SysBLL.Player("交易完成.wav");
                //缴费金额
                lblBanlance.Text = Payment.GasPayParam.GasCard.price;
                //剩余燃气量
                lblGasVolume.Text = (Payment.GasPayParam.GasCard.GasValue + Convert.ToInt32(Payment.GasPayParam.GasCard.rechargeNum)).ToString();
            }
            catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep8_success:load():"+ex.Message);
            }
        }
        void print()
        {
            try {
                WantongBLL wt = new WantongBLL();
                //万通卡号
                string wtCard = wt.GetCardNo();

                PrintParam printParam = new PrintParam();
                printParam.payType = "燃气";
                printParam.account = Payment.GasPayParam.GasCard.CardNo;
                printParam.amout = Payment.GasPayParam.GasCard.price;
                printParam.cardNo = Payment.GasPayParam.CardNo;
                printParam.orderno = Payment.GasPayParam.GasOrderInfo.msgrsp.orderNo;
                //printParam.resqn = orderInfo.msghead.resqn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep8_success:print():"+ex.Message);
            }
        }
        bool isPrint = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
    }
}
