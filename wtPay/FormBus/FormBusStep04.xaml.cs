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
using wtPayModel.PaymentModel;

namespace wtPay.FormBus
{
    /// <summary>
    /// FormBusStep04.xaml 的交互逻辑
    /// </summary>
    public partial class FormBusStep04 : UserControl
    {
        public FormBusStep04()
        {
            InitializeComponent();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (amountTxt.Text.Length == 0)
                {
                    showInfoTxt.Text = "提示：请输入缴费金额";
                    return;
                }
                if (amountTxt.Text.Substring(0, 1).Equals("0"))
                {
                    showInfoTxt.Text = "提示：缴费金额不正确";
                    return;
                }
                int price = Convert.ToInt32(amountTxt.Text);
                if (price > 5000)
                {
                    showInfoTxt.Text = "提示：缴费金额超出限制";
                    return;
                }
                Payment.BusPayParam.UserInputMoney = amountTxt.Text;
                SysBLL.payCostType = 9;
                Util.JumpUtil.jumpCommonPage("FormReadCard");
            }catch(Exception ex)
            {
                log.Write("error:FormBusStep04:确定_Click:"+ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请输入充值金额.wav");
                amountTxt.Text = "";
                keyboard.textBox = amountTxt;
            }
            catch(Exception ex)
            {
                log.Write("error:FormBusStep04:load():"+ex.Message);
            }
        }
    }
}
