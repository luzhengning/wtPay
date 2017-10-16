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

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormCitizenStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormNot : UserControl
    {
        public FormNot()
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                showInfo.Text = SqlLiteHelper.SqlLiteHelper.query("formNot")[0].FormalValue;
            }
            catch { }
        }
    }
}
