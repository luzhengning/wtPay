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

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenInputNo : UserControl
    {
        public FormCitizenInputNo()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length == 0)
                {
                    return;
                }
                if (inputBox.Text.Substring(0, 1).Equals("0"))
                {
                    return;
                }
                //payParam.rechageAmount = inputBox.Text;

                Payment.wintopReChargeParam.WtCardNo = inputBox.Text;
                Util.JumpUtil.jumpCommonPage("FormCitizenStepValidatecode_1");
            }
            catch (Exception ex)
            {
                log.Write("error:FormMobileStep03:确定_Click:" + ex.Message);
            }
        }

        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SysBLL.Player("请输入一卡通号码.wav");
            keyboard.textBox = inputBox;
            //缴费类型为万通卡充值
            SysBLL.payCostType = 3;

            inputBox.Text = string.Empty;
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputBox.Text.Length >= 17)
            {
                inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1, 1);
                return;
            }
        }
    }
}
