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
using wtPayCommon;
using wtPayModel.PaymentModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep08_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep08_success : UserControl
    {
        public FormCitizenStep08_success()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
            isPrint = true;
        }
        private void load()
        {
            try
            {
                SysBLL.Player("交易完成.wav");
                //缴费账号
                lblBalance.Text = Payment.wintopReChargeParam.WintopQueryResult.WTCARDID;
                //账号类型
                if ("01".Equals(Payment.wintopReChargeParam.WintopQueryResult.TYPE)) lblType.Text = "脱机";
                if ("02".Equals(Payment.wintopReChargeParam.WintopQueryResult.TYPE)) lblType.Text = "消费缴费钱包";
                if ("03".Equals(Payment.wintopReChargeParam.WintopQueryResult.TYPE)) lblType.Text = "小额联机";
                //缴费金额
                lblAmount.Text = Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00030;
                lblRechage.Text = Payment.wintopReChargeParam.UserInputMoney;
            }
            catch(Exception ex)
            {
                log.Write("error:FormCitizenStep08_success:load():"+ex.Message);
            }
        }
        private void print()
        {
            try
            {
                StringBuilder outMsg = new StringBuilder();
                Print.TT_OpenDevice(new StringBuilder("COM" + SysConfigHelper.readerNode("PrintPort")), new StringBuilder("38400"), outMsg);
                Print.TT_GetDeviceStatus(outMsg);
                // Print.TT_PrintText(new StringBuilder("三维终端 快捷支付 便利生活\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("  三维终端交易凭条\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("----------------------"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("交易类型:甘肃一卡通充值\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("一卡通卡号:" + PrintBLL.hideCardNo(Payment.wintopReChargeParam.WtCardNo) + "****" + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("银行卡号:" + PrintBLL.hideCardNo(Payment.wintopReChargeParam.IcParams["cardNo"]) + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("交易时间:" + SysBLL.getTimeFormat() + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("交易金额：￥" + Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList.CZ00030 + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("订单编号：" + Payment.wintopReChargeParam.WintopOrderInfo.msgrsp.orderNo + "\n"), outMsg);
                //Print.TT_PrintText(new StringBuilder("流水号：" + orderInfo.msghead.resqn + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("备注：\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("-----持卡人存根-----\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_CutPaper(1, outMsg);
                Print.TT_CloseDevice(outMsg);
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStep08_success:print():"+ex.Message+ex.InnerException);
            }
        }
    }
}
