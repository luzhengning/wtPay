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
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistration : UserControl
    {
        public FormRegistration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RegistrationClass.registrationParam = new wtPayModel.RegistrationModel.RegistrationParam();
            RegistrationClass.registrationAddress = new RegistrationAddress();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null == DeviceState.SendPrintState())
                {
                    Util.JumpUtil.jumpCommonPage("FormRegistrationHospital_1");
                    return;
                }
                else
                {
                    Util.JumpUtil.jumpCommonPage("FormPrintError");
                    return;
                }
            }
            catch(Exception ex)
            {
                log.Write("error:FormRegistration:Button_Click_1:" + ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RegistrationClass.RegistrationType = -1;
            Util.JumpUtil.jumpCommonPage("FormRegistrationUndoInput");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RegistrationClass.RegistrationType = -2;
            Util.JumpUtil.jumpCommonPage("FormRegistrationUndoInput");
        }
    }
}
