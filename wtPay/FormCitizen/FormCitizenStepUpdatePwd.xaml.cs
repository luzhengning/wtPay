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

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormMobileStep1.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStepUpdatePwd : UserControl
    {
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));
        BitmapImage xuanzhongImage = new BitmapImage(new Uri("/cut-2/xuanzhong.png", UriKind.Relative));
        BitmapImage weixuanzhongImage = new BitmapImage(new Uri("/cut-2/weixuanzhong.png", UriKind.Relative));
        //密码类型
        int type = 0;
        public FormCitizenStepUpdatePwd()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        private void UserControl_Loaded_2(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请输入一卡通密码.wav");
                lblShowinfo.Foreground = new SolidColorBrush(Colors.White);
                lblShowinfo.Content = "提示：  请输入密码";
                //服务密码
                type = 1;
                typeimg1.Source = xuanzhongImage;
                typeimg2.Source = null;

                newPassword.Text = "";
                newPassword2.Text = "";
                oldPassword.Text = "";

                this.oldPassword.Visibility = Visibility.Hidden;
                this.newPassword.Visibility = Visibility.Hidden;
                this.newPassword2.Visibility = Visibility.Hidden;
                //选中旧密码
                keyboard.textBox = oldPassword;
                img1.Source = succcesImage;
                img2.Source = null;
                img3.Source = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepUpdatePwd:load():" + ex.Message);
            }
        }

        private void oldPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            lblShowinfo.Content = "提示：  请输入原始密码";
            //旧密码
            keyboard.textBox = oldPassword;
            img1.Source = succcesImage;
            img2.Source = null;
            img3.Source = null;
        }

        private void newPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            lblShowinfo.Content = "提示：  请输入新密码";
            //新密码
            keyboard.textBox = newPassword;
            img1.Source = null;
            img2.Source = succcesImage;
            img3.Source = null;
        }

        private void newPwd2_GotFocus(object sender, RoutedEventArgs e)
        {
            lblShowinfo.Content = "提示：  请确认密码";
            //确认密码
            keyboard.textBox = this.newPassword2;
            img1.Source = null;
            img2.Source = null;
            img3.Source = succcesImage;
        }

        private void oldPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (oldPassword.Text.Length >= 6)
            {
                //新密码焦点
                lblShowinfo.Content = "提示：  请输入新密码";
                //新密码
                keyboard.textBox = newPassword;
                img1.Source = null;
                img2.Source = succcesImage;
                img3.Source = null;
                if (oldPassword.Text.Length > 6)
                    oldPassword.Text = oldPassword.Text.Remove(oldPassword.Text.Length - 1, 1);
            }
            oldPwd.Text = "";
            for (int i = 1; i <= oldPassword.Text.Length; i++)
            {
                oldPwd.Text += "*";
            }
        }

        private void newPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (newPassword.Text.Length >= 6)
            {
                //确认新密码焦点
                lblShowinfo.Content = "提示：  请确认密码";
                //确认密码
                keyboard.textBox = this.newPassword2;
                img1.Source = null;
                img2.Source = null;
                img3.Source = succcesImage;
                if (newPassword.Text.Length > 6)
                    newPassword.Text = newPassword.Text.Remove(newPassword.Text.Length - 1, 1);
            }
            newPwd.Text = "";
            for (int i = 1; i <= newPassword.Text.Length; i++)
            {
                newPwd.Text += "*";
            }
        }

        private void newPassword2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (newPassword2.Text.Length > 6)
            {
                newPassword2.Text = newPassword2.Text.Remove(newPassword2.Text.Length-1,1);
            }
            newPwd2.Text = "";
            for (int i = 1; i <= newPassword2.Text.Length; i++)
            {
                newPwd2.Text += "*";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkTextIsNull()) return;
                Payment.wintopReChargeParam.WintopUpdateWtPwdParam = new wtPayModel.WintopModel.WintopUpdateWtPwdParam();
                Payment.wintopReChargeParam.WintopUpdateWtPwdParam.Password = SysBLL.md5(oldPassword.Text);
                Payment.wintopReChargeParam.WintopUpdateWtPwdParam.Newpassword = SysBLL.md5(newPassword.Text);
                Payment.wintopReChargeParam.WintopUpdateWtPwdParam.Type = type.ToString();
                Payment.wintopReChargeParam.WintopUpdateWtPwdParam.Wtcardid = Payment.wintopReChargeParam.WtCardNo;
                Payment.wintopReChargeParam.ExcuteType = 2;
                Util.JumpUtil.jumpCommonPage("FormCitizenStepLoad");
            }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepUpdatePwd:确定_Click:" + ex.Message);
            }
        }
        /// <summary>
        /// 文本框验证
        /// </summary>
        /// <returns></returns>
        private bool checkTextIsNull()
        {
            if (oldPassword.Text.Length == 0)
            {
                lblShowinfo.Foreground = new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示:   请输入原始密码";
                img1.Source = errorImage;
                return true;
            }
            if (newPassword.Text.Length == 0)
            {
                lblShowinfo.Foreground = new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示：  请输入新密码";
                img2.Source = errorImage;
                return true;
            }
            if (newPassword2.Text.Length == 0)
            {
                lblShowinfo.Foreground = new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示：  请输入密码";
                img3.Source = errorImage;
                return true;
            }
            if (!newPassword.Text.Equals(newPassword2.Text))
            {
                lblShowinfo.Foreground=new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示：  两次输入的密码不一致";
                return true;
            }
            if (newPassword2.Text.Length > 6)
            {
                lblShowinfo.Foreground = new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示：  万通卡密码必须是6位";
                return true;
            }
            if (newPassword.Text.Equals(oldPassword.Text))
            {
                lblShowinfo.Foreground = new SolidColorBrush(Colors.Red);
                lblShowinfo.Content = "提示：  原始密码与新密码相同";
                return true;
            }
            return false;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //服务密码
            type = 1;
            typeimg1.Source = xuanzhongImage;
            typeimg2.Source = null;
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //应用密码
            type = 2;
            typeimg1.Source = null;
            typeimg2.Source = xuanzhongImage;
        }

        private void oldPwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void newPwd_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
    }
}
