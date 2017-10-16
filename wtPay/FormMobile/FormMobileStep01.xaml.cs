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
using wtPayDAL.SysAccessDAL;
using wtPayModel.Mobile;
using wtPayModel.PaymentModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep1.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep01 : UserControl
    {
        BitmapImage errorImage=new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));

        public FormMobileStep01()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormMobileStep");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try {
                if ((phoneNumber.Text.Length != 11) && (phoneNumberAgain.Text.Length != 11))
                {
                    showInfoLbl.Content = "请输入11位手机号码";
                    image1.Source = errorImage;
                    image2.Source = errorImage;
                    return;
                }
                if (!phoneNumber.Text.Equals(phoneNumberAgain.Text))
                {
                    showInfoLbl.Content = "输入的手机号码不一致，请检查";
                    image1.Source = errorImage;
                    image2.Source = errorImage;
                    return;
                }
                if (SysBLL.MobilePayType == 1)
                {
                    //联通
                    Payment.unicomPayParam.PhoneOn = phoneNumberAgain.Text;
                    Util.JumpUtil.jumpCommonPage("FormUnicomStep02");
                }
                else
                {
                    //移动
                    Payment.mobilePayParam.PhoneOn = phoneNumberAgain.Text;
                    Util.JumpUtil.jumpCommonPage("FormMobileStep02");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep01:确定_Click:"+ex.Message);
            }
        }
        private void phoneNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            keyboard.textBox = phoneNumber;
            image1.Source = succcesImage;
            image2.Source = null;

        }

        private void phoneNumberAgain_GotFocus(object sender, RoutedEventArgs e)
        {
            keyboard.textBox = phoneNumberAgain;
            image1.Source = null;
            image2.Source = succcesImage;
        }
        private void UserControl_Loaded_2(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("缴费账号.wav");
                this.showInfoLbl.Content = "欢迎使用话费充值业务";
                //输入焦点
                keyboard.textBox = phoneNumber;
                image1.Source = succcesImage;
                image2.Source = null;

                //初始化文本框
                phoneNumber.Text = "";
                phoneNumberAgain.Text = "";

                this.showInfo.Text= PayPromptAccess.getPrompt(GcManage.gcType);
            }
            catch (Exception ex)
            {
                log.Write("error：FormMobileStep01：load():" + ex.Message);
            }
        }

        private void phoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                if (phoneNumber.Text.Length == 11)
                {
                    //输入焦点
                    keyboard.textBox = phoneNumberAgain;
                    image1.Source = null;
                    image2.Source = succcesImage;
                }
                if (phoneNumber.Text.Length > 11)
                {
                    phoneNumber.Text = phoneNumber.Text.Remove(phoneNumber.Text.Length - 1, 1);
                    return;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep01:phoneNumber_TextChanged：" + ex.Message);
            }
        }

        private void phoneNumberAgain_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                if (phoneNumberAgain.Text.Length == 11) return;
                if (phoneNumberAgain.Text.Length > 11)
                {
                    phoneNumberAgain.Text = phoneNumberAgain.Text.Remove(phoneNumberAgain.Text.Length - 1, 1);
                    return;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep01：phoneNumberAgain_TextChanged:"+ex.Message);
            }
        }
    }
}
