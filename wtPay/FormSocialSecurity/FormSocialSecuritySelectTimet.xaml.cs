using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using wtPayModel.WintopModel;

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecuritySelectTimet : UserControl
    {
        string[] years = new string[12];

        public FormSocialSecuritySelectTimet()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请选择查询参保日期.wav");
                int year = DateTime.Now.Year;
                year = year - 11;
                int count = 1;
                for (int y = year; year <= DateTime.Now.Year; y++)
                {
                    years[(count - 1)] = (y + "01");
                    ((Label)(this.FindName("name" + count))).Content = (y.ToString() + "年");
                    count++;
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormSocialSecuritySelectTimet:load():" + ex.Message);
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Write("error:FormSocialSecuritySelectTimet:Button_Click_1:" + ex.Message);
            }
        }
        private void btn(Label lable)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }

        private void name1_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[0];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[1];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[2];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[3];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[4];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[5];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[6];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[7];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name9_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[8];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name10_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[9];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name11_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[10];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name12_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysBLL.socialSecurityParam.qstime = years[11];
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityWait");
        }

        private void name9_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
