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
using wtPayModel.ConfigModel;
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGasGoldenCardStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSecStep04 : UserControl
    {
        public FormPropSecStep04()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPropSecStep02");
        }

        private void 充值_Click(object sender, RoutedEventArgs e)
        {
            try {
                string qiNum = inputBox.Text;
                if ((qiNum.Length > 0) && (qiNum.Length <= 4))
                {
                    if (qiNum.Substring(0, 1).Equals("0"))
                    {
                        return;
                    }
                    Payment.propSecPayParam.UserInputMoney = qiNum;
                    SysBLL.payCostType = 12;
                    Util.JumpUtil.jumpCommonPage("FormReadCard");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep04:充值_Click"+ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (ConfigSysParam.gifBusiness)
                {
                    case GifBusiness.prop2Water_IC:
                        rqrqhints.Text = PayPromptAccess.getPrompt("5_2_2"); 
                        emName.Text = "     方";
                        break;
                    case GifBusiness.prop2Water_RFID:
                        rqrqhints.Text = PayPromptAccess.getPrompt("5_2_1");
                        emName.Text = "     方";
                        break;
                    case GifBusiness.prop2Elec_IC:
                        rqrqhints.Text = PayPromptAccess.getPrompt("5_2_3");
                        emName.Text = "     度";
                        break;
                    case GifBusiness.prop2Elec_RFID:
                        rqrqhints.Text = PayPromptAccess.getPrompt("5_2_4");
                        emName.Text = "     度";
                        break;
                }
                SysBLL.Player("请输入充值数量.wav");
                keyboard.textBox = inputBox;
                this.inputBox.Text = string.Empty;
                rqrqhints.Text = PayPromptAccess.getPrompt(GcManage.gcType);
            }
            catch (Exception ex) { log.Write("error:" + ex.Message); }
        }
    }
}
