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
    /// FormPropStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStepTemp06_success : UserControl
    {
        public FormPropStepTemp06_success()
        {
            InitializeComponent();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
        }
        private void print()
        {
            try
            {
                string payType = "";
                if ("01".Equals(Payment.propPayTempParam.PropType)) payType = "水费";
                if ("02".Equals(Payment.propPayTempParam.PropType)) payType = "电费";
                if ("03".Equals(Payment.propPayTempParam.PropType)) payType = "燃气费";
                PrintParam printParam = new PrintParam();
                printParam.payType = "物业" + payType;
                printParam.account = Payment.propPayTempParam.AccountNo;
                printParam.cardNo = Payment.propPayTempParam.CardNo;
                printParam.amout = Payment.propPayTempParam.PropOrderInfo.msgrsp.realAmout;
                printParam.orderno = Payment.propPayTempParam.PropOrderInfo.msgrsp.orderNo;
                PrintBLL.print(printParam);
            }
            catch (Exception ex)
            {
                log.Write("error:FormPropStepTemp06_success:print:" + ex.Message);
            }
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
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
                isPrint = true;
                SysBLL.Player("交易已完成，退卡.wav");
                ////缴费金额
                //lblBalance.Text = Payment.propPayTempParam.PropOrderInfo.msgrsp.realAmout+"元";

                //if (Payment.propPayTempParam.PropType.Equals("01")) lblPropType.Text = "水费";
                //if (Payment.propPayTempParam.PropType.Equals("02")) lblPropType.Text = "电费";
                //if (Payment.propPayTempParam.PropType.Equals("03")) lblPropType.Text = "燃气费";
                ////用户名
                //lblmobile.Text = Payment.propPayTempParam.AccountNo;

                //测试
                //缴费金额
                lblBalance.Text = Payment.propPayTempParam.UserInputMoney + "元";
                lblPropType.Text = "水费";
                //用户名
                lblmobile.Text = Payment.propPayTempParam.AccountNo;
            }
            catch (Exception ex)
            {
                log.Write("error:FormPropStepTemp06_success:load:" + ex.Message);
            }
        }
    }
}