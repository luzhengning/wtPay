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

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurityInput.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityInput : UserControl
    {
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));
        public FormSocialSecurityInput()
        {
            InitializeComponent();
        }

        private void credentialNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //证件号码选中
            keyboard38.textBox = credentialNumberTextBox;
            img1.Source = null;
            img2.Source = succcesImage;
            img3.Source = null;
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //密码框选中
            keyboard38.textBox = passwordBox2;
            img1.Source = null;
            img2.Source = null;
            img3.Source = succcesImage;
        }
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (checkTextIsNull()) return;
                SysBLL.socialSecurityParam.grcard = socialSecurityNumberTextBox.Text.ToUpper();
                SysBLL.socialSecurityParam.idcard = credentialNumberTextBox.Text.ToUpper();
                SysBLL.socialSecurityParam.password = passwordBox2.Text.ToUpper();
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
            }catch(Exception ex)
            {
                log.Write("error:FormSocialSecurityInput:确定_Click:"+ex.Message);
            }
        }
        /// <summary>
        /// 文本框验证
        /// </summary>
        /// <returns></returns>
        bool checkTextIsNull()
        {
            bool isbool = false;
            if (credentialNumberTextBox.Text.Length == 0)
            {
                img1.Source = errorImage;
                isbool = true;
            }
            if (socialSecurityNumberTextBox.Text.Length == 0)
            {
                img2.Source = errorImage;
                isbool = true;
            }
            if (passwordBox2.Text.Length == 0)
            {
                img3.Source = errorImage;
                isbool = true;
            }
            return isbool;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void socialSecurityNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //社保卡号选中
            keyboard38.textBox = socialSecurityNumberTextBox;
            img1.Source = succcesImage;
            img2.Source = null;
            img3.Source = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();   
        }
        private void load()
        {
            try {
                SysBLL.socialSecurityParam = null;
                SysBLL.socialSecurityParam = new wtPayModel.SocialSecurityModel.SocialSecurityParam();
                SysBLL.Player("请输入社保卡号.wav");
                socialSecurityNumberTextBox.Text = "";
                credentialNumberTextBox.Text = "";
                passwordBox2.Text = "";
                passwordBox.Text = "";
                this.passwordBox2.Visibility = Visibility.Hidden;
                //社保卡号选中
                keyboard38.textBox = socialSecurityNumberTextBox;
                img1.Source = succcesImage;
                img2.Source = null;
                img3.Source = null;
            }catch(Exception ex)
            {
                log.Write("error:FormSocialSecurityInput:load():"+ex.Message);
            }
        }

        private void passwordBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordBox.Text = "";
            for (int i = 1; i <= passwordBox2.Text.Length; i++)
            {
                passwordBox.Text += "*";
            }
        }

        private void socialSecurityNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (socialSecurityNumberTextBox.Text.Length >= 20)
                {
                    socialSecurityNumberTextBox.Text = socialSecurityNumberTextBox.Text.Remove(socialSecurityNumberTextBox.Text.Length - 1, 1);
                    return;
                }
            }catch(Exception ex)
            {
                log.Write("error:社保账号输入页面异常："+ex.Message);
            }
        }

        private void credentialNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (credentialNumberTextBox.Text.Length >= 20)
            {
                credentialNumberTextBox.Text = credentialNumberTextBox.Text.Remove(credentialNumberTextBox.Text.Length - 1, 1);
                return;
            }
        }
    }
}
