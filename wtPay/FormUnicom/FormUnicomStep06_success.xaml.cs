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

namespace wtPay.FormUnicom
{
    /// <summary>
    /// FormMobileStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormUnicomStep06_success : UserControl
    {
        public FormUnicomStep06_success()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        Boolean isPrint = false;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint == false)
            {
                print();
                isPrint = true;
            }
        }
        //窗体load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化方法
            load();
        }
        private void load()
        {
            try {
                isPrint = false;
                SysBLL.Player("交易完成.wav");
                lblBalance.Text = Payment.unicomPayParam.PhoneOn;
                lblAmount.Text = Payment.unicomPayParam.RechageAmount + "元";
                realTimeBalanceLbl.Text = (Convert.ToDouble(Payment.unicomPayParam.Msgrsp.PAYMENT_AMOUNT)+ Convert.ToDouble(Payment.unicomPayParam.RechageAmount)).ToString()+".00";
            }catch(Exception ex)
            {
                log.Write("error:FormUnicomStep06_success:load():"+ex.Message);
            }
        }
        void print()
        {
            try {
                PrintParam printParam = new PrintParam();
                printParam.payType = "联通";
                if (Payment.unicomPayParam.IcParams == null)
                {
                    //万通交易
                    printParam.tradingType = "万通";
                }
                else
                {
                    printParam.tradingType = "银行卡";
                }
                printParam.account = Payment.unicomPayParam.PhoneOn;
                printParam.cardNo = Payment.unicomPayParam.CardNo;
                printParam.amout = Payment.unicomPayParam.RechageAmount;
                printParam.orderno = Payment.unicomPayParam.Orderinfo.msgrsp.orderNo;
                printParam.resqn = Payment.unicomPayParam.Resqn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormUnicomStep06_success:print():" + ex.Message);
            }
        }
    }
}
