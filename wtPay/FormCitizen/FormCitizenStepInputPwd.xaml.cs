using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStepInputPwd : UserControl
    {
        private delegate void setTextBlockDelegate(TextBlock textBlock, string value);
        public FormCitizenStepInputPwd()
        {
            InitializeComponent();
        }
        private void setTextBlock(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.pwdtxt.Text.Length == 0) return;
                Payment.wintopReChargeParam.Md5Pwd= SysBLL.md5(this.pwdtxt.Text.Trim());
                if (Payment.wintopReChargeParam.QueryType == 1) Util.JumpUtil.jumpCommonPage("FormCitizenStep03");
                if (Payment.wintopReChargeParam.QueryType == 2) Util.JumpUtil.jumpCommonPage("FormCitizenStepSpendDetail");
                if (Payment.wintopReChargeParam.QueryType == 3) Util.JumpUtil.jumpCommonPage("FormCitizenStepRechargeDetail");
            }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepInputPwd:确定_Click:" + ex.Message);
            }
        }

        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
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
                SysBLL.Player("请输入一卡通密码.wav");
                if (Payment.wintopReChargeParam.QueryType == 1)
                {
                    step2.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step2, "2.核对信息");
                    step3.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step3, "3.输入密码");
                }
                if (Payment.wintopReChargeParam.QueryType == 2)
                {
                    step2.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step2, "2.消费查询");
                    step3.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step3, "3.输入密码");
                }
                if (Payment.wintopReChargeParam.QueryType == 3)
                {
                    step2.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step2, "2.充值查询");
                    step3.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step3, "3.输入密码");
                }

                pwdtxt.Visibility = Visibility.Hidden;
                pwdtxt.Text = "";
                inputBox.Text = "";

                keyboard.textBox = pwdtxt;
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStepInputPwd:load():"+ex.Message);
            }
        }

        private void pwdtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                if (pwdtxt.Text.Length > 6)
                {
                    pwdtxt.Text = pwdtxt.Text.Remove(pwdtxt.Text.Length-1,1);
                }
                inputBox.Text = "";
                for (int i = 1; i <= pwdtxt.Text.Length; i++)
                {
                    inputBox.Text += "*";
                }
            }catch(Exception ex)
            {
                log.Write("error:"+ex.Message);
            }
        }
    }
}
