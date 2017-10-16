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
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormGas.FormGasGoldenCard
{
    /// <summary>
    /// FormGasGoldenCardStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasGoldenCardStep04 : UserControl
    {
        public FormGasGoldenCardStep04()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep03");
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
                    Payment.GasPayParam.GasCard.rechargeNum = qiNum;
                    SysBLL.payCostType = 5;
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
                SysBLL.Player("请输入充值气量.wav");
                keyboard.textBox = inputBox;
                this.inputBox.Text = string.Empty;
                //rqrqhints.Text = GasShowInfo.rqhints;
                rqrqhints.Text= PayPromptAccess.getPrompt(GcManage.gcType); 
            }
            catch (Exception ex) { log.Write("error:" + ex.Message); }
        }
    }
}
