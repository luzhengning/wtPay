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

namespace wtPay.FormGas.FormGasGoldenCard
{
    /// <summary>
    /// FormGasGoldenCardStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasGoldenCardStep03 : UserControl
    {
        public FormGasGoldenCardStep03()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep04");
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
                SysBLL.Player("继续请点积充值按钮.wav");
                this.lblshow.Visibility = Visibility.Hidden;
                btnOkBg.Visibility = Visibility.Hidden;
                //用户编号
                lblBalance.Text = Payment.GasPayParam.GasCard.CardNo+"  ";
                //剩余燃气量
                lblGasVolume.Text = Payment.GasPayParam.GasCard.GasValue.ToString()+"  ";
                if (Payment.GasPayParam.GasCard.GasValue != 0)
                {
                    this.lblshow.Visibility = Visibility.Visible;
                    lblshow.Text = "提示:请将卡内剩余气量充入气表后，再进行充值";
                    btnOkBg.Visibility = Visibility.Hidden;
                    return;
                }
                btnOkBg.Visibility = Visibility.Visible;
            }
            catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep03:load():"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGas");
        }
    }
}
