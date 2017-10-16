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

namespace wtPay.FormWater
{
    /// <summary>
    /// FormWaterStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormWaterStep01 : UserControl
    {
        public FormWaterStep01()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no = inputBox.Text;
                if (no.Length > 0)
                {
                    //执行下一步
                    Payment.waterPayParam.Account = inputBox.Text;
                    Util.JumpUtil.jumpCommonPage("FormWaterStep02");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormWaterStep01:确定_Click:"+ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("缴费账号.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                Payment.waterPayParam = null;
                Payment.waterPayParam = new WaterPayParam();
                this.showinfo.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }catch(Exception ex)
            {
                log.Write("error:水务输入账号页面：load():"+ex.Message+ex.InnerException);
            }
        }
    }
}
