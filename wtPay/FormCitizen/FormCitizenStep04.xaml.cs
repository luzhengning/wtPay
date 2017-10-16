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
    public partial class FormCitizenStep04 : UserControl
    {
        public FormCitizenStep04()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
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
                if (inputBox.Text.Length > 5)
                {
                    return;
                }
                //payParam.rechageAmount = inputBox.Text;

                int userPay = Convert.ToInt32(inputBox.Text);
                if (userPay > 500)
                {
                    return;
                }
                else
                {
                    //输入金额


                    Payment.wintopReChargeParam.UserInputMoney = inputBox.Text;
                    Util.JumpUtil.jumpCommonPage("FormReadCard");
                }
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
            SysBLL.Player("请输入充值金额.wav");
            keyboard.textBox = inputBox;
            //缴费类型为万通卡充值
            SysBLL.payCostType = 3;

            inputBox.Text = string.Empty;
        }
    }
}
