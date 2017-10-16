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
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormMobileStep06_fail.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSecStep09 : UserControl
    {
        public FormPropSecStep09()
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
            try
            {
                if (SysBLL.payCostType == 5)
                {
                    if (GasPayParam.Showinfo.Length == 0)
                    {
                        lblShowinfo.Text = "业务正忙，请稍后再试";
                    }
                    else
                    {
                        lblShowinfo.Text = GasPayParam.Showinfo;
                    }
                }
                if (SysBLL.payCostType == 11)
                {
                    lblShowinfo.Text = "缴费户号不存在，请联系物业公司";
                }
            }catch(Exception ex)
            {
                lblShowinfo.Text = "业务正忙，请稍后再试";
            }
        }
    }
}
