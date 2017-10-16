using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;

namespace wtPay.FormTraffic
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormTrafficViolation : System.Windows.Controls.UserControl
    {
        public FormTrafficViolation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendKeys.SendWait("1");
            Thread.Sleep(100);
            SendKeys.Flush();
            //Util.JumpUtil.jumpMainPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            webPage.Source = new Uri("http://gs.122.gov.cn/views/inquiry.html");
           
        }
    }
}
