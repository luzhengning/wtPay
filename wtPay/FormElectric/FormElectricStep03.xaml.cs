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

namespace wtPay.FormElectric
{
    /// <summary>
    /// FormElectricStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormElectricStep03 : UserControl
    {
        public FormElectricStep03()
        {
            InitializeComponent();
        }

        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length > 0)
                {
                    if (inputBox.Text.Length > 4)
                    {
                        return;
                    }
                    amount = Convert.ToInt32(inputBox.Text);
                    if (amount > 50000)
                    {
                        lblShowInfo1.Foreground = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    if (inputBox.Text.Substring(0, 1).Equals("0"))
                    {
                        return;
                    }
                    if (!"0".Equals(Payment.elecPayParam.UserInputMoney))
                    {
                        if (amount < Convert.ToDouble(Payment.elecPayParam.UserInputMoney))
                        {
                            lblShowInfo1.Text = "提示:充值金额必须大于欠费金额";
                            return;
                        }
                    }
                    Payment.elecPayParam.UserInputMoney = inputBox.Text;

                    SysBLL.payCostType = 4;
                    Util.JumpUtil.jumpCommonPage("FormReadCard");
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormElectricStep03:确定_Click:" + ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormElectricStep02");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("请输入充值金额.wav");
                keyboard.textBox = this.inputBox;       //键盘绑定输入框
                inputBox.Text = "";
                lblShowInfo1.Foreground = new SolidColorBrush(Colors.White);
                lblShowInfo2.Visibility = Visibility.Hidden;
                if (!"0".Equals(Payment.elecPayParam.UserInputMoney))
                {
                    lblShowInfo1.Text = "提示：欠费用户的充值金额必须大于欠费金额";
                    lblShowInfo2.Content = "欠费金额：" + Payment.elecPayParam.UserInputMoney + "元";
                    lblShowInfo2.Visibility = Visibility.Visible;
                }else
                {
                    //预交费
                    lblShowInfo1.Text = "提示：甘肃一卡通最大充值金额为5000元";
                    lblShowInfo2.Content = "提示：银行卡最大充值金额为50000元";
                    lblShowInfo2.Visibility = Visibility.Visible;
                }
                //lblShowInfo2.Foreground = new SolidColorBrush(Colors.White);
            }catch(Exception ex)
            {
                log.Write("error:"+ex.Message+ex.InnerException);
            }
        }
        int amount = 0;
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length > 5)
                {
                    inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1, 1);
                }
                if (inputBox.Text.Length > 1)
                {
                    amount = Convert.ToInt32(inputBox.Text);
                    if (amount > 50000)
                    {
                        lblShowInfo1.Foreground = new SolidColorBrush(Colors.Red);
                        //lblShowInfo2.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }
    }
}
