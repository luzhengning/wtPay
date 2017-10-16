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

namespace wtPay.FormProp
{
    /// <summary>
    /// FormPropStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStep06_success : UserControl
    {
        public FormPropStep06_success()
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
                PrintParam printParam = new PrintParam();
                printParam.payType = "物业";
                printParam.account = Payment.PropPayParam.Mobile;
                printParam.cardNo = Payment.PropPayParam.CardNo;
                printParam.amout = Payment.PropPayParam.PropOrderInfo.msgrsp.realAmout;
                printParam.orderno = Payment.PropPayParam.PropOrderInfo.msgrsp.orderNo;
                PrintBLL.print(printParam);
            }catch(Exception ex)
            {
                log.Write("error:FormPropStep06_success:print:" + ex.Message);
            }
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
                SysBLL.Player("交易完成.wav");
                //缴费金额
                lblBalance.Text = Payment.PropPayParam.PropOrderInfo.msgrsp.realAmout;

                if (Payment.PropPayParam.PropType == 1) lblPropType.Text = "房屋";
                if (Payment.PropPayParam.PropType == 2) lblPropType.Text = "车位";
                //用户名
                lblmobile.Text = Payment.PropPayParam.Mobile;
            }
            catch(Exception ex)
            {
                log.Write("error:FormPropStep06_success:load:" + ex.Message);
            }
        }
    }
}
