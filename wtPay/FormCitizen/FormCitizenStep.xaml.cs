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
using wtPayModel.PaymentModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep : UserControl
    {
        public FormCitizenStep()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Payment.wintopReChargeParam.QueryType = 1;
            Util.JumpUtil.jumpCommonPage("FormCitizenStepInputPwd");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Payment.wintopReChargeParam.QueryType = 2;
            Util.JumpUtil.jumpCommonPage("FormCitizenStepInputPwd");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Payment.wintopReChargeParam.QueryType = 3;
            Util.JumpUtil.jumpCommonPage("FormCitizenStepInputPwd");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStepUpdatePwd");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            return;
            Util.JumpUtil.jumpCommonPage("FormCitizenStepValidatecode_1");
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //guashiBtn.Visibility = Visibility.Hidden;
            //xiaofeBtn.Visibility = Visibility.Hidden;
            //chongzhiBtn .Visibility = Visibility.Hidden;
            //pwdBtn.Visibility = Visibility.Hidden;
        }
    }
}
