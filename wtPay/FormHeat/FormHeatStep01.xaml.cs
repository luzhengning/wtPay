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
using wtPayDAL.SysAccessDAL;
using wtPayModel.PaymentModel;

namespace wtPay.FormHeat
{
    /// <summary>
    /// FormHeatStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormHeatStep01 : UserControl
    {
        public FormHeatStep01()
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
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("缴费账号.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                SysBLL.payCostType = 8;
                Payment.heatPayParam = null;
                Payment.heatPayParam = new HeatPayParam();
                showinfo.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }
            catch (Exception ex)
            {
                log.Write("error：FormHeatStep01：load（）：" + ex.Message + ex.InnerException);
            }
        }
    }
}
