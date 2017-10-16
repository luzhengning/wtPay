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

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep06_success : UserControl
    {
        public FormMobileStep06_success()
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
        void print()
        {
            try {

                PrintParam printParam = new PrintParam();
                //交易类型
                if (Payment.mobilePayParam.IcParams == null)
                {
                    //甘肃一卡通交易
                    printParam.tradingType = "甘肃一卡通";
                }
                else
                {
                    //银联交易
                    printParam.tradingType = "银行卡";
                }
                printParam.payType = "移动";
                printParam.account = Payment.mobilePayParam.PhoneOn;
                printParam.cardNo = Payment.mobilePayParam.CardNo;
                printParam.amout = Payment.mobilePayParam.RechageAmount;
                printParam.orderno = Payment.mobilePayParam.OrderInfo.msgrsp.orderNo;
                printParam.resqn = Payment.mobilePayParam.Resqn;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep06_success:print():"+ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                //初始化方法
                load();
            }
            catch(Exception ex)
            {
                log.Write("error:FormMobileStep06_success:UserControl_Loaded:"+ex.Message);
            }
        }

        private void load()
        {
            try {
                isPrint = false;
                SysBLL.Player("交易完成.wav");
                this.lblBalance.Text = Payment.mobilePayParam.PhoneOn;
                this.lblAmount.Text = Payment.mobilePayParam.UserInputMoney + ".00元";
                this.lblPayMoney.Text = Payment.mobilePayParam.RechageAmount + "元";
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep06_success:load():"+ex.Message);
            }
        }
    }
}
