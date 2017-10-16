using System;
using System.Collections.Generic;
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

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep02 : UserControl
    {
        public FormCitizenStep02()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep03");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
    }
}
