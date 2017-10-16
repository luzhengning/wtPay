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

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGasGoldenCardStep8_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSecStep08_success : UserControl
    {
        public FormPropSecStep08_success()
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
                //缴费量
                lblBanlance.Text = Payment.propSecPayParam.UserInputMoney;
                //支付金额
                lblGasVolume.Text = Payment.propSecPayParam.RechageAmount;
            }
            catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep8_success:load():"+ex.Message);
            }
        }
        void print()
        {
            try {
                //WantongBLL wt = new WantongBLL();
                //万通卡号
                //string wtCard = wt.GetCardNo();

                PrintParam printParam = new PrintParam();
                printParam.payType = "物业";
                printParam.account = "";
                printParam.amout = Payment.propSecPayParam.RechageAmount;
                printParam.cardNo = Payment.propSecPayParam.CardNo;
                printParam.orderno = Payment.propSecPayParam.orderInfo.msgrsp.orderNo;
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
