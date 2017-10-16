using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormTemp.xaml 的交互逻辑
    /// </summary>
    public partial class FormTemp : UserControl
    {
        
        
        
        public FormTemp()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //维护人员签到
            Util.JumpUtil.jumpCommonPage("FormMaintainSign");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormNetTest");
        }
       
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            refundBtn.Visibility = Visibility.Visible;
            if ("测试".Equals(SysBLL.IsTest))
            {
                refundBtn.Visibility = Visibility.Visible;
            }else
            {
                refundBtn.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRefund");
        }
    }
}
