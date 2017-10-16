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
using wtPayModel.Mobile;
using wtPayModel.PaymentModel;
using wtPayModel.UnicomModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep : UserControl
    {
        

        public FormMobileStep()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //联通
            GcManage.gcType = "3";
            //缴费类型
            SysBLL.payCostType = 2;
            //联通缴费
            Payment.unicomPayParam = new UnicomPayParam();
            Payment.unicomPayParam.PasswordInfo = "重要提示";
            //缴费类型
            SysBLL.MobilePayType = 1;
            Util.JumpUtil.jumpCommonPage("FormMobileStep01");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //移动
            GcManage.gcType = "9";
            //缴费类型
            SysBLL.payCostType = 1;
            //移动缴费
            Payment.mobilePayParam = new MobilePayParam();
            Payment.mobilePayParam.PasswordInfo = "重要提示";
            //缴费类型
            SysBLL.MobilePayType = 2;
            Util.JumpUtil.jumpCommonPage("FormMobileStep01");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
