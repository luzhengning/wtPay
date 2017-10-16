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
using wtPayModel;

namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormMaintainSign.xaml 的交互逻辑
    /// </summary>
    public partial class FormMaintainSign : UserControl
    {
        //资源图片
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));
        public FormMaintainSign()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keyboard.textBox = accountTxt;
            img1.Source = succcesImage;
            img2.Source = null;
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keyboard.textBox = password;
            img1.Source = null;
            img2.Source = succcesImage;
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (checkTextIsNull()) return;
                SysBLL.maintainSignParam.Account = accountTxt.Text.ToUpper();
                SysBLL.maintainSignParam.Passsword = password.Text.ToUpper();
                Util.JumpUtil.jumpCommonPage("FormMaintainSignWait");
            }catch(Exception ex)
            {
                log.Write("error:维护人员键盘登录异常："+ex.Message);
            }
        }
        bool checkTextIsNull()
        {
            bool isbool = false;
            if (accountTxt.Text.Length == 0)
            {
                img1.Source = errorImage;
                isbool = true;
            }
            if (pwdTxt.Text.Length == 0)
            {
                img2.Source = errorImage;
                isbool = true;
            }
            return isbool;
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            password.Visibility = Visibility.Hidden;
            accountTxt.Text = "";
            password.Text = "";
            pwdTxt.Text = "";
            //输入账号焦点
            keyboard.textBox = accountTxt;
            img1.Source = succcesImage;
            img2.Source = null;
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            pwdTxt.Text = "";
            for (int i = 1; i <= password.Text.Length; i++)
            {
                pwdTxt.Text += "*";
            }
        }
    }
}
