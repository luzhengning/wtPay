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
using System.Threading;
using wtPayCommon;
using wtPayModel;
using wtPayModel.PaymentModel;

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormMobileStep06_failback.xaml 的交互逻辑
    /// </summary>
    public partial class FormCashPay : UserControl
    {
        private delegate void setTextBoxTextDelegate(TextBox tb,string value);
        private delegate void setTextBlcokTextDelegate(TextBlock tb,string value);

        private bool isClose = false;

        //识币器线程
        Thread cashThread = null;
        public FormCashPay()
        {
            InitializeComponent();
        }
        private void setTextBoxText(TextBox tb,string value)
        {
            tb.Text = value;
        }
        private void setTextBlock(TextBlock tb,string value)
        {
            tb.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            if (money > 0) return;
            isClose = true;
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (money == 0) return;
            isClose = true;
            try
            {
                switch (SysBLL.payCostType)
                {
                    case 1:
                        //移动
                        Payment.mobilePayParam.UserInputMoney = money.ToString();
                        Util.JumpUtil.jumpCommonPage("FormMobileStep06");
                        break;
                    case 2:
                        //联通
                        Payment.unicomPayParam.UserInputMoney = money.ToString();
                        Util.JumpUtil.jumpCommonPage("FormUnicomStep06");
                        break;
                    case 4:
                        //电力
                        Payment.elecPayParam.UserInputMoney = money.ToString();
                        Util.JumpUtil.jumpCommonPage("FormElectricStep06");
                        break;
                    case 6:
                        //广电
                        Payment.broadCasPayParam.UserInputMoney = money.ToString();
                        Payment.broadCasPayParam.List.BANLANCE = money.ToString();
                        Util.JumpUtil.jumpCommonPage("FormBroadCasStep06");
                        break;

                }
            }
            catch (Exception ex) { }
            DeviceState.sendRmdStatu();
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
                isClose = false;
                PayStaticParam.payType = 0;
                money = 0;
                rmbCountTxt.Text = "";
                moneylbl.Text = "0.0";
                cashThread = new Thread(delegate() { cash(); });
                cashThread.Start();
            }
            catch (ThreadAbortException ae) { }
            catch(Exception ex) { }
        }
        int money = 0;
        private void cash()
        {
            
            int rmbCount = 0;
            try
            {
                StringBuilder info = new StringBuilder(1024);
                //CashRMB.TT_OpenDevice(new StringBuilder("COM11"), new StringBuilder("9600"),info);
                //设置可接收金额范围
                CashRMB.TT_SetBillType(120,info);
                //允许投币
                CashRMB.TT_EnableCash(60,info);
                int tempMoney = 0;
                
                while(true)
                {
                    if (isClose) break;
                    //取得接收金额
                    tempMoney = CashRMB.TT_GetMoney(info);
                    if (tempMoney > 0)
                    {
                        money = money + tempMoney;
                        tempMoney = 0;
                        rmbCount++;
                        rmbCountTxt.Dispatcher.Invoke(new setTextBoxTextDelegate(setTextBoxText), rmbCountTxt, rmbCount.ToString());
                        moneylbl.Dispatcher.Invoke(new setTextBlcokTextDelegate(setTextBlock), moneylbl, money.ToString());
                    }
                    Thread.Sleep(100); //延时１００毫秒
                }
                //禁止投币
                CashRMB.TT_DisableCash(info);
                //现金模块停止工作后延时２秒，防止在停止时纸币正在入钞箱而漏收纸币
                Thread.Sleep(2000);
                CashRMB.TT_GetMoney(info);
                if (Convert.ToInt32(info) > 0)
                {
                    money = Convert.ToInt32(info) + money;
                    rmbCount++;
                    rmbCountTxt.Dispatcher.Invoke(new setTextBoxTextDelegate(setTextBoxText), rmbCountTxt, rmbCount.ToString());
                    moneylbl.Dispatcher.Invoke(new setTextBlcokTextDelegate(setTextBlock), moneylbl, money.ToString());
                }
            }
            catch (ThreadAbortException ae) { }
            catch (Exception ex) { }
        }
    }
}
