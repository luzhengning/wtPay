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

namespace wtPay.FormPublicFund
{

    /// <summary>
    /// FormPublicFundInput.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFundInput : UserControl
    {
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));

        public string number = "";
        public string password = "";
        public FormPublicFundInput()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (checkTextIsNull()) return;
            if (SysBLL.customerParam.queryType == 4)
            {
                //个人公积金明细查询
                SysBLL.customerParam.selcnt = this.inputBox.Text;
                SysBLL.customerParam.pwd = this.passwordBox2.Text;
            }
            else
            {
                //其他查询
                SysBLL.customerParam.number = this.inputBox.Text;
                SysBLL.customerParam.password = this.passwordBox2.Text;
            }
               
            if (SysBLL.customerParam.queryType == 4)
            {
                Util.JumpUtil.jumpCommonPage("FormPublicFundWait");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormPublicFund");
            }
        }
        //输入检查
        bool checkTextIsNull()
        {
            bool isbool = false;
            if (inputBox.Text.Length == 0)
            {
                imgAccount.Source = errorImage;
                isbool = true;
            }
            if (passwordBox2.Text.Length == 0)
            {
                imgPwd.Source = errorImage;
                isbool = true;
            }
            return isbool;
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            if (SysBLL.customerParam.queryType == 4)
            {
                Util.JumpUtil.jumpCommonPage("FormPublicFund");
            }
            else
            {
                Util.JumpUtil.jumpMainPage();
            }
            
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.keyboard.textBox = this.inputBox;
            imgAccount.Source = succcesImage;
            imgPwd.Source = null;

        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.keyboard.textBox = this.passwordBox2;
            imgAccount.Source = null;
            imgPwd.Source = succcesImage;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请输入证件号码和密码.wav");
                //初始化公积金查询参数
                if (SysBLL.customerParam == null)
                {
                    SysBLL.customerParam = null;
                    SysBLL.customerParam = new wtPayModel.PublicFundModel.CustomerParam();
                }
                else
                {
                    if (SysBLL.customerParam.queryType != 4)
                    {
                        SysBLL.customerParam = null;
                        SysBLL.customerParam = new wtPayModel.PublicFundModel.CustomerParam();
                    }
                    else
                    {
                        //个人公积金明细查询
                    }
                }
                //身份证查询
                SysBLL.customerParam.seltype = "2";
                //隐藏密码框
                this.passwordBox2.Visibility = Visibility.Hidden;
                //输入焦点
                this.keyboard.textBox = this.inputBox;
                imgAccount.Source = succcesImage;
                imgPwd.Source = null;
                accountlbl.Content = "证件号码";
                if (SysBLL.customerParam.queryType == 4) {
                    accountlbl.Content = "个人编号:";
                }else
                {
                    if ("1".Equals(SysBLL.customerParam.seltype)) accountlbl.Content = "个人编号:";
                    if ("2".Equals(SysBLL.customerParam.seltype)) accountlbl.Content = "身份证号:";
                    if ("3".Equals(SysBLL.customerParam.seltype)) accountlbl.Content = "联名卡号:";
                }

                this.inputBox.Text = "";
                this.passwordBox2.Text = "";
                this.passwordBox.Text = "";
            }
            catch (Exception ex)
            {
                log.Write("error:FormPublicFundInput:load():"+ex.Message);
            }
        }
        //string password = "";
        private void passwordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // password = passwordBox2.Text;
        }

        private void passwordBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordBox.Text = "";
            for (int i=1;i<= passwordBox2.Text.Length; i++)
            {
                passwordBox.Text += "*";
            }
        }

        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (inputBox.Text.Length >= 20)
                {
                    inputBox.Text = inputBox.Text.Remove(inputBox.Text.Length - 1, 1);
                    return;
                }
            }catch(Exception ex)
            {
                log.Write("error:公积金输入站好页面异常："+ex.Message);
            }
        }
    }
}
