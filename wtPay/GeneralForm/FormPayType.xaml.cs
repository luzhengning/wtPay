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
    public partial class FormPayType : UserControl
    {
        
        
        
        public FormPayType()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (SysBLL.payCostType)
                {
                    case 1:
                        //移动
                        Util.JumpUtil.jumpCommonPage("FormMobileSelectAmout");
                        break;
                    case 2:
                        //联通
                        Util.JumpUtil.jumpCommonPage("FormUnicomStep03");
                        break;
                    case 4:
                        //电力
                        Util.JumpUtil.jumpCommonPage("FormElectricStep03");
                        break;
                    case 6:
                        //广电
                        Util.JumpUtil.jumpCommonPage("FormBroadCasStep03");
                        break;

                }
            }
            catch (Exception ex) { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCashPay");
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
        }
    }
}
