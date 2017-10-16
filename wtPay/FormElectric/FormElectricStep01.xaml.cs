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
using wtPayDAL.SysAccessDAL;
using wtPayModel.PaymentModel;

namespace wtPay.FormElectric
{
    /// <summary>
    /// FormElectricStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormElectricStep01 : UserControl
    {
        public FormElectricStep01()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0)
            {
                Payment.elecPayParam.Account = inputBox.Text;
                Util.JumpUtil.jumpCommonPage("FormElectricStep02");
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("缴费账号.wav");
                keyboard.textBox = this.inputBox;
                inputBox.Text = "";
                //初始化电力支付参数
                SysBLL.payCostType = 4;
                Payment.elecPayParam = null;
                Payment.elecPayParam = new ElecPayParam();
                Payment.elecPayParam.UserInputMoney = "0";
                showinfo.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }catch(Exception ex)
            {
                log.Write("error:电力输入账号页面：load()："+ex.Message+ex.InnerException);
            }
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length > 10)
                {
                    inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length-1,1);
                }
            }catch(Exception ex)
            {
                log.Write("error:"+ex.Message);
            }
        }
    }
}
