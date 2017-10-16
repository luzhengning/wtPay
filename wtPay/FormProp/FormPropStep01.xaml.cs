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

namespace wtPay.FormProp
{
    /// <summary>
    /// FormPropStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStep01 : UserControl
    {
        public FormPropStep01()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keyboard.textBox = inputBox;
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPropStep");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (inputBox.Text.Length > 0&&inputBox.Text.Length==11)
            {
                Payment.PropPayParam.Mobile = inputBox.Text;
                if (Payment.PropPayParam.PropType==1) Util.JumpUtil.jumpCommonPage("FormPropStep02_house");
                if (Payment.PropPayParam.PropType == 2) Util.JumpUtil.jumpCommonPage("FormPropStep02_ParkingLot");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("缴费账号.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                showinfo.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }catch(Exception ex)
            {
                log.Write("error："+ex.Message+ex.InnerException);
            }
        }
    }
}
