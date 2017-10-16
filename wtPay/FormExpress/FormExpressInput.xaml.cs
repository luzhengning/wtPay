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

namespace wtPay.FormExpress
{
    /// <summary>
    /// FormHeatStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormExpressInput : UserControl
    {
        public FormExpressInput()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length == 0)
            {
                return;
            }
            Payment.heatPayParam.AccountNo = inputBox.Text;
            Util.JumpUtil.jumpCommonPage("FormHeatStep02");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        
        private  void load()
        {
            //SysBLL.Player("缴费账号.wav");
            inputBox.Text = "";
            keyboard.textBox = inputBox;
        }
        //关闭
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                StaticParam.expressQueryParam.billcode = inputBox.Text;
                Util.JumpUtil.jumpCommonPage("FormExpressResult");
            }
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
    }
}
