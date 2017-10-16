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
    /// FormPublicFund.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFund : UserControl
    {
        public FormPublicFund()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFundAccountInfo");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //个人公积金账户查询
            SysBLL.customerParam.Id = "GJJ002";
            SysBLL.customerParam.conName = "个人公积金账户查询";
            SysBLL.customerParam.queryType =2;
            Util.JumpUtil.jumpCommonPage("FormPublicFundWait");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //个人客户信息查询
            SysBLL.customerParam.Id = "GJJ001";
            SysBLL.customerParam.conName = "个人客户信息查询";
            SysBLL.customerParam.queryType =1;
            Util.JumpUtil.jumpCommonPage("FormPublicFundWait");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //个人公积金明细查询
            SysBLL.customerParam.Id = "GJJ003";
            SysBLL.customerParam.conName = "个人公积金明细查询";
            SysBLL.customerParam.queryType =4;
            Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //公积金贷款还款明细
            SysBLL.customerParam.Id = "GJJ005";
            SysBLL.customerParam.conName = "公积金贷款还款明细";
            SysBLL.customerParam.queryType =5;
            Util.JumpUtil.jumpCommonPage("FormPublicFundWait");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //公积金贷款余额查询
            SysBLL.customerParam.Id = "GJJ004";
            SysBLL.customerParam.conName = "公积金贷款余额查询";
            SysBLL.customerParam.queryType =3;
            Util.JumpUtil.jumpCommonPage("FormPublicFundWait");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //gjjdaikuan.Visibility = Visibility.Hidden;
            //gjjhuankuan.Visibility = Visibility.Hidden;
            SysBLL.Player("请选择服务项目.wav");
            SysBLL.customerParam.queryType = -1;
            SysBLL.customerParam.selcnt = SysBLL.customerParam.number;
            SysBLL.customerParam.pwd = SysBLL.customerParam.password;
        }
    }
}
