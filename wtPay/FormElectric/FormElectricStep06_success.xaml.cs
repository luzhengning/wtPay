using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace wtPay.FormElectric
{
    /// <summary>
    /// FormElectricStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormElectricStep06_success : UserControl
    {
        public FormElectricStep06_success()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
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
                SysBLL.Player("交易完成.wav");
                lblBalance.Text = Payment.elecPayParam.Account;
                lblAmount.Text = Payment.elecPayParam.UserInputMoney + "元";
                lblpayPrice.Text = Payment.elecPayParam.OrderInfo.msgrsp.realAmout + "元";
            }
            catch(Exception ex)
            {
                log.Write("error:电力成功页面load事件异常："+ex.Message);
            }

        }
        void print()
        {
            try {
                PrintParam printParam = new PrintParam();
                printParam.payType = "电力";
                printParam.account = Payment.elecPayParam.Account;
                printParam.cardNo = Payment.elecPayParam.CardNo;
                printParam.amout = Payment.elecPayParam.OrderInfo.msgrsp.realAmout;
                printParam.orderno = Payment.elecPayParam.OrderInfo.msgrsp.orderNo;
                printParam.resqn = Payment.elecPayParam.OrderInfo.msghead.reqsn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:电力打印异常："+ex.Message);
            }
        }
    }
}
