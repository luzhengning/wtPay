using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace wtPay.usercontrol
{
    /// <summary>
    /// BottomContent.xaml 的交互逻辑
    /// </summary>
    public partial class BottomContent : UserControl
    {
        public BottomContent()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //软件版本号
            this.lblVersion.Text = SysConfigHelper.readerNode("currentVersion");
            //设置页面终端号 
            this.lblMechineNo.Text = "NO：" + ConfigurationManager.AppSettings["MechineNo"];
        }
    }
}
