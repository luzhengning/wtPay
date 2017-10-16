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
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurity : UserControl
    {
        public FormSocialSecurity()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityInfo");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityInput");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SysBLL.socialSecurityParam.Id = "YLBX002";
            SysBLL.socialSecurityParam.conName = "个人参保信息查询";
            SysBLL.socialSecurityParam.type = 2;
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SysBLL.socialSecurityParam.Id = "YLBX005";
            SysBLL.socialSecurityParam.conName = "医疗账户消费信息";
            SysBLL.socialSecurityParam.type = 5;
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SysBLL.socialSecurityParam.Id = "YLBX003";
            SysBLL.socialSecurityParam.conName = "医保账户信息查询";
            SysBLL.socialSecurityParam.type = 3;
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SysBLL.socialSecurityParam.Id = "YLBX001";
            SysBLL.socialSecurityParam.conName = "养老发放信息查询";
            SysBLL.socialSecurityParam.type = 1;
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SysBLL.socialSecurityParam.Id = "YLBX004";
            SysBLL.socialSecurityParam.conName = "养老月账户信息查询";
            SysBLL.socialSecurityParam.type = 4;
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SysBLL.Player("请选择服务项目.wav");
        }
    }
}
