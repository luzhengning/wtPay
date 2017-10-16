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
using wtPayBLL;
using wtPayModel.PaymentModel;

namespace wtPay.FormProp
{
    /// <summary>
    /// FormGasGoldenCardStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStepTemp02 : UserControl
    {
        public FormPropStepTemp02()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPropStepTemp03");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                ////用户编号
                lblBalance.Text = Payment.propPayTempParam.AccountNo;
            }
            catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep03:load():"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
    }
}
