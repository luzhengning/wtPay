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

namespace wtPay.FormWater
{
    /// <summary>
    /// FormWaterStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormWaterStep03 : UserControl
    {
        double price = 0;
        public FormWaterStep03()
        {
            InitializeComponent();
        }
        
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length == 0)
                {
                    showInfoTxt.Text = "提示：请输入缴费金额";
                    return;
                }
                if (inputBox.Text.Substring(0, 1).Equals("0"))
                {
                    showInfoTxt.Text = "提示：缴费金额不正确";
                    return;
                }
                if (inputBox.Text.Length > 4)
                {
                    return;
                }
                double amout = Convert.ToDouble(inputBox.Text);
                if (price <= amout)
                {
                    Payment.waterPayParam.WaterQueryInfo.msgrsp.orderlist[0].amout = inputBox.Text;
                    SysBLL.payCostType = 7;
                    Util.JumpUtil.jumpCommonPage("FormReadCard");
                    return;
                }
                else
                {
                    showInfoTxt.Text = "提示：充值金额必须大于账单金额";
                    return;
                }
            }catch(Exception ex) { }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormWaterStep02");
        }

        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请输入充值金额.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                lblShowInfo2.Foreground = Brushes.White;
                price = Convert.ToDouble(Payment.waterPayParam.WaterQueryInfo.msgrsp.orderlist[0].amout);
                showInfoTxt.Text = "充值金额：" + price + "元";
            }
            catch (Exception ex){ }
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length > 5)
                {
                    inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1, 1);
                }
                if (Convert.ToInt32(inputBox.Text) > 50000)
                {
                    lblShowInfo2.Foreground = Brushes.Red;
                    return;
                }
            }
            catch (Exception ex) { }
        }
    }
}