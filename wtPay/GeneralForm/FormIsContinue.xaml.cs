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
using WtPayBLL;

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormMobileStep06_failback.xaml 的交互逻辑
    /// </summary>
    public partial class FormIsContinue : UserControl
    {
        public FormIsContinue()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistration");
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
                if (StaticParam.FormIsContinueType == 1)
                {
                    lblShowinfo.Text = "确认要取消挂号吗？";
                }
            }catch(Exception ex) { log.Write("error:FormPrintError:load:" + ex.Message); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistrationHospital_1");
            return;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (StaticParam.FormIsContinueType == 1)
            {
                Util.JumpUtil.jumpCommonPage("FormRegistrationWait");
                return;
            }
        }
    }
}
