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
    /// FormPublicFundCardType.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFundCardType : UserControl
    {
        public FormPublicFundCardType()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFundCustomerInfo");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SysBLL.customerParam.seltype = "1";
            Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SysBLL.customerParam.seltype = "2";
            Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SysBLL.customerParam.seltype = "3";
            Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化公积金查询参数
            SysBLL.customerParam = null;
            SysBLL.customerParam = new wtPayModel.PublicFundModel.CustomerParam();
        }
    }
}
