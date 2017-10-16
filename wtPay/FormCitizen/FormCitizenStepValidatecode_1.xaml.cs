using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.WintopModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStepValidatecode_1 : UserControl
    {
        WintopSendValidateCodeParam param = null;

        Thread sendThread = null;
        public FormCitizenStepValidatecode_1()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        private void inputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (inputBox.Text.Length == 0) return;
                Payment.wintopReChargeParam.WintopLossReportParam = new WintopLossReportParam();
                Payment.wintopReChargeParam.WintopLossReportParam.Wtcardid = Payment.wintopReChargeParam.WtCardNo;
                Payment.wintopReChargeParam.WintopLossReportParam.Validatecode = inputBox.Text;
                //操作类型，挂失
                Payment.wintopReChargeParam.ExcuteType = 1;
                Util.JumpUtil.jumpCommonPage("FormCitizenStepLoad");
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStepValidatecode_1:Button_Click:"+ex.Message);
            }
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
                SysBLL.Player("请输入验证码.wav");
                inputBox.Text = "";
                keyboard.textBox = inputBox;
                param = null;
                param = new WintopSendValidateCodeParam();
                param.Wtcardid = Payment.wintopReChargeParam.WtCardNo;
                if (sendThread != null)
                {
                    sendThread.Abort();
                    sendThread.DisableComObjectEagerCleanup();
                }
                sendThread = new Thread(delegate() { WintopAccess.sendValidateCode(param); });
                sendThread.Start();

            }
            catch(Exception ex)
            {
                log.Write("error:FormCitizenStepValidatecode_1:load():"+ex.Message);
            }
        }
    }
}
