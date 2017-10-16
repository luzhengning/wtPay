using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPay.pay;
using wtPayBLL;
using wtPayDAL;
using wtPayModel;

namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormMechineState.xaml 的交互逻辑
    /// </summary>
    public partial class FormRefund : UserControl
    {
        public FormRefund()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btn1.Visibility = Visibility.Hidden;
            btn2.Visibility = Visibility.Hidden;
            btn3.Visibility = Visibility.Hidden;
            btn4.Visibility = Visibility.Hidden;
            btn5.Visibility = Visibility.Hidden;
            cardNo1.Text = "";
            price1.Text = "";
            cardNo2.Text = "";
            price2.Text = "";
            cardNo3.Text = "";
            price3.Text = "";
            cardNo4.Text = "";
            price4.Text = "";
            cardNo5.Text = "";
            price5.Text = "";
            for (int i = 0; i < PayStaticParam.refundTest.Count; i++)
            {
                if (i == 0)
                {
                    cardNo1.Text = PayStaticParam.refundTest[i].refundPayParam.WtNo;
                    price1.Text= PayStaticParam.refundTest[i].refundPayParam.rechageAmount;
                    btn1.Visibility = Visibility.Visible;
                }
                if (i == 1)
                {
                    cardNo2.Text = PayStaticParam.refundTest[i].refundPayParam.WtNo;
                    price2.Text = PayStaticParam.refundTest[i].refundPayParam.rechageAmount;
                    btn2.Visibility = Visibility.Visible;
                }
                if (i == 2)
                {
                    cardNo3.Text = PayStaticParam.refundTest[i].refundPayParam.WtNo;
                    price3.Text = PayStaticParam.refundTest[i].refundPayParam.rechageAmount;
                    btn3.Visibility = Visibility.Visible;
                }
                if (i == 3)
                {
                    cardNo4.Text = PayStaticParam.refundTest[i].refundPayParam.WtNo;
                    price4.Text = PayStaticParam.refundTest[i].refundPayParam.rechageAmount;
                    btn4.Visibility = Visibility.Visible;
                }
                if (i == 4)
                {
                    cardNo5.Text = PayStaticParam.refundTest[i].refundPayParam.WtNo;
                    price5.Text = PayStaticParam.refundTest[i].refundPayParam.rechageAmount;
                    btn5.Visibility = Visibility.Visible;
                }
            }
        }
        private void checkState()
        {
           
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            pay.testRefund(ref PayStaticParam.refundTest[0].refundPayResultInfo, PayStaticParam.refundTest[0].refundPayParam);
            btn1.Visibility = Visibility.Hidden;
            PayStaticParam.refundTest.RemoveAt(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            pay.testRefund(ref PayStaticParam.refundTest[1].refundPayResultInfo, PayStaticParam.refundTest[1].refundPayParam);
            btn2.Visibility = Visibility.Hidden;
            PayStaticParam.refundTest.RemoveAt(1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            pay.testRefund(ref PayStaticParam.refundTest[2].refundPayResultInfo, PayStaticParam.refundTest[2].refundPayParam);
            btn3.Visibility = Visibility.Hidden;
            PayStaticParam.refundTest.RemoveAt(2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            pay.testRefund(ref PayStaticParam.refundTest[3].refundPayResultInfo, PayStaticParam.refundTest[3].refundPayParam);
            btn4.Visibility = Visibility.Hidden;
            PayStaticParam.refundTest.RemoveAt(3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            pay.testRefund(ref PayStaticParam.refundTest[4].refundPayResultInfo, PayStaticParam.refundTest[4].refundPayParam);
            btn5.Visibility = Visibility.Hidden;
            PayStaticParam.refundTest.RemoveAt(4);
        }
        bool isSendStatu = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
           
        }
        private void showInfo(string value)
        {
            
        }

        private void setTextBlockText(TextBlock textBlock, string value)
        {
           
        }
        private void setTextBlockText2(TextBlock textBlock, string value)
        {
            
        }
       
        
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
