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
    /// FormBroadCasStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStepTemp03 : UserControl
    {
        public FormPropStepTemp03()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length == 0)
                {
                    //lblShowInfo.Content = "提示：充值金额不能为空";
                    return;
                }
                if ("0".Equals(inputBox.Text.Substring(0, 1)))
                {
                    //lblShowInfo.Content = "提示：金额第一位不能为0";
                    return;
                }
                if (inputBox.Text.Length > 4)
                {
                    //lblShowInfo.Content = "提示：充值金额不能为空";
                    return;
                }
                if (inputBox.Text.Length > 4) return;
                Payment.propPayTempParam.UserInputMoney = inputBox.Text;
                SysBLL.payCostType = 11;
                Util.JumpUtil.jumpCommonPage("FormReadCard");
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep03:确定_Click:" + ex.Message);
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
                SysBLL.Player("请输入充值金额.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                if (Payment.propPayTempParam.PropType.Equals("01"))
                {
                    danwei.Content = "吨";
                }else
                {
                    danwei.Content = "度";
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep03:load():" + ex.Message);
            }
        }
    }
}
