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
using wtPayModel.SocialSecurityModel;

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurityMedicalAccount.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityMedicalAccount : UserControl
    {
        MedicalAccountInfo info = null;

        public FormSocialSecurityMedicalAccount()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityMedicalAccountConsume");
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //数据加载
            load();
        }
        private void load()
        {
            try
            {
                info = (MedicalAccountInfo)Util.JumpUtil.ParamsMap["info"];
                grcard.Text = info.data.grcard;
                expense.Text = info.data.expense;
                income.Text = info.data.income;
                year.Text = info.data.year;
            }
            catch (Exception ex)
            {
                log.Write("error：医保账户信息加载异常："+ex.Message);
            }
        }
    }
}
