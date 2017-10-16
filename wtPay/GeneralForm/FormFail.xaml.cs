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
using wtPayModel.SystemModel;

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormMobileStep06_fail.xaml 的交互逻辑
    /// </summary>
    public partial class FormFail : UserControl
    {
        public FormFail()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            wtPay.Util.JumpUtil.jumpMainPage();
        }
        //窗体load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化方法
            load();
        }
        private void load()
        {
            panel.Visibility = Visibility.Visible;
            if (FormTip.FormFailShowinfo.Equals("暂不支持此类型卡业务")){
                panel.Visibility = Visibility.Hidden;
            }
            if (FormTip.FormFailShowinfo.Length > 0)
            {
                lblShowinfo.Text = "  "+FormTip.FormFailShowinfo;
            }
            else
            {
                lblShowinfo.Text = "  缴费失败，请稍后再试...";//
                
            }
        }
    }
}
