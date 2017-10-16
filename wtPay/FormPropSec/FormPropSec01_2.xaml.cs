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

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGas.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSec01_2 : UserControl
    {
        Button[] btns = new Button[4];
        public FormPropSec01_2()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btns[0] = btn00;
                btns[1] = btn01;
                btns[2] = btn02;
                btns[3] = btn03;
                btnGroup.Children.Remove(btn00);
                btnGroup.Children.Remove(btn01);
                btnGroup.Children.Remove(btn02);
                btnGroup.Children.Remove(btn03);

                for(int i=0;i< Payment.propSecPayParam.propMeterInfo.msgrsp.meterLists.Count; i++)
                {
                    btns[i].Uid = Payment.propSecPayParam.propMeterInfo.msgrsp.meterLists[i].g0402+ Payment.propSecPayParam.propMeterInfo.msgrsp.meterLists[i].g1912;
                    btnGroup.Children.Add(btns[i]);
                }
            }
            catch(Exception ex) { }

        }

        private void Button_Click_00(object sender, RoutedEventArgs e)
        {
            jumpNext(0);
        }

        private void Button_Click_01(object sender, RoutedEventArgs e)
        {
            jumpNext(1);
        }

        private void Button_Click_02(object sender, RoutedEventArgs e)
        {
            jumpNext(2);
        }

        private void Button_Click_03(object sender, RoutedEventArgs e)
        {
            jumpNext(3);
        }
        private void jumpNext(int i)
        {
            try
            {
                Payment.propSecPayParam.PrimaryKey = Payment.propSecPayParam.propMeterInfo.msgrsp.meterLists[i].primaryKey;
                Util.JumpUtil.jumpCommonPage("FormPropSecStep04");
            }catch(Exception ex) { }
        }
    }
}
