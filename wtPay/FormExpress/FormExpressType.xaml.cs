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
using wtPayModel.ExpressModel;

namespace wtPay.FormExpress
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormExpressType : UserControl
    {
        public FormExpressType()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StaticParam.expressQueryParam = null;
            StaticParam.expressQueryParam = new wtPayModel.ExpressModel.ExpressQueryParam();
        }

        private void button1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.youzhengAppId;
            StaticParam.expressQueryParam.conName = "邮政快递查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
        }

        private void button2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.yuantonAppId;
            StaticParam.expressQueryParam.conName = "圆通快递查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
        }

        private void button3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.shentonAppId;
            StaticParam.expressQueryParam.conName = "申通快递查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void button4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.huishengAppId;
            StaticParam.expressQueryParam.conName = "汇升快递查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void button5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.zhaijisongAppId;
            StaticParam.expressQueryParam.conName = "宅急送查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void button6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StaticParam.expressQueryParam.appId = ExpressClass.zhongtieAppId;
            StaticParam.expressQueryParam.conName = "中铁快运查询";
            Util.JumpUtil.jumpCommonPage("FormExpressInput");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
