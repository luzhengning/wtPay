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

namespace wtPay.FormGas
{
    /// <summary>
    /// FormGas.xaml 的交互逻辑
    /// </summary>
    public partial class FormGas : UserControl
    {
        public FormGas()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep02");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGasPioneerCardStep02");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep02");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Payment.GasPayParam = null;
            Payment.GasPayParam = new GasPayParam();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
