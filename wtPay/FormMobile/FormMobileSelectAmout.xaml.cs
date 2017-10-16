using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.RegistrationModel;
using wtPayModel.WintopModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileSelectAmout : UserControl
    {
        public FormMobileSelectAmout()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            SysBLL.Player("请选择充值金额.wav");
            if ("测试".Equals(SysBLL.IsTest))
            {
                button8.Visibility = Visibility.Visible;
            }else
            {
                button8.Visibility = Visibility.Hidden;
            }
        }
        
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
           
        }

       
        private void Button_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
        }

        private void name1_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            btn(10);
        }

        private void name2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(20);
        }

        private void name3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(50);
        }

        private void name4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(100);
        }

        private void name5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(200);
        }

        private void name6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(300);
        }

        private void name7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(500);
        }

        private void name8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(1);
        }
        private void btn(int amount)
        {
            if (SysBLL.MobilePayType == 1)
            {
                //输入金额
                Payment.unicomPayParam.UserInputMoney = amount.ToString();
            }
            else
            {
                //输入金额
                Payment.mobilePayParam.UserInputMoney = amount.ToString();
            }
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }
    }
}
