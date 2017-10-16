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
using wtPayDAL;
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurityInput.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationUndoInput : UserControl
    {
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));
        BitmapImage weixuanzhong= new BitmapImage(new Uri("/cut-2/textBlockBack.png", UriKind.Relative));
        BitmapImage xuanzhong = new BitmapImage(new Uri("/cut-2/xuanzhong.png", UriKind.Relative));
        public FormRegistrationUndoInput()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 文本框验证
        /// </summary>
        /// <returns></returns>
        bool checkTextIsNull()
        {
            if (appointIdTxt.Text.Length == 0)
            {
                return false;
            }
            return true;
        }
      
        private void load()
        {
            try {
                appointIdTxt.Text = "";
                keyboard38.textBox = appointIdTxt;
            }
            catch(Exception ex)
            {
                log.Write("error:FormSocialSecurityInput:load():"+ex.Message);
            }
        }
        private void phoneTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            flow_noTxtGotFocus();
        }
        private void phoneTxtGotFocus()
        {
        }
        private void flow_noTxtGotFocus()
        {
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkTextIsNull()) return;
                //取消预约挂号
                if (RegistrationClass.RegistrationType == -1)
                {
                    RegistrationClass.undoRegistrationParam = new UndoRegistrationParam();
                    RegistrationClass.undoRegistrationParam.appId = RegistrationClass.appId;
                    RegistrationClass.undoRegistrationParam.conName = "撤销挂号预约";
                    //RegistrationClass.undoRegistrationParam.tel = phoneTxt.Text;
                    RegistrationClass.undoRegistrationParam.tel ="";
                    RegistrationClass.undoRegistrationParam.operate_type = "1";
                    //RegistrationClass.undoRegistrationParam.flow_no = flow_noTxt.Text;
                    RegistrationClass.undoRegistrationParam.flow_no = "";
                    RegistrationClass.undoRegistrationParam.appointId = appointIdTxt.Text;
                    RegistrationClass.RegistrationType = 1;
                    StaticParam.FormIsContinueType = 1;
                    Util.JumpUtil.jumpCommonPage("FormIsContinue"); 
                    return;
                }
                //挂号记录查询
                if (RegistrationClass.RegistrationType == -2)
                {
                    RegistrationClass.registrationRecordQueryParam = new RegistrationRecordQueryParam();
                    RegistrationClass.registrationRecordQueryParam.appId = RegistrationClass.registrationRecordQueryAppId;
                    RegistrationClass.registrationRecordQueryParam.conName = "撤销挂号预约";
                    //RegistrationClass.registrationRecordQueryParam.tel = phoneTxt.Text;
                    //RegistrationClass.registrationRecordQueryParam.flow_no = flow_noTxt.Text;
                    RegistrationClass.registrationRecordQueryParam.tel = "";
                    RegistrationClass.registrationRecordQueryParam.flow_no = "";
                    RegistrationClass.registrationRecordQueryParam.appointId = appointIdTxt.Text;
                    Util.JumpUtil.jumpCommonPage("FormRegistrationRecord");
                }
            }
            catch(Exception ex)
            {
                log.Write("error:"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistration");
        }
        private void flow_noTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void appointIdTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (appointIdTxt.Text.Length >= 21)
            {
                appointIdTxt.Text = appointIdTxt.Text.Remove(appointIdTxt.Text.Length - 1, 1);
                return;
            }
        }
    }
}
