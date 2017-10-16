using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.RegistrationModel;
using wtPayModel.WintopModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormSelectAmout : UserControl
    {
        //页码
        int pageNo = 0;
        //科室列表
        WintopDiscountInfo discountList = null;
        //是否正在查询
        bool isQuery = false;
        private delegate void setShowGridDelegate(FormSelectAmout from, string Gridname, bool isShow);
        private delegate void setButtonValueDelegate(FormSelectAmout form, string gridName, string TextBlockName, string value,string amountTextBlockName,string amountValue);
        private delegate void setTextBlockDelegate(TextBlock tb, string value);

        private Thread queryThread = null;
        public FormSelectAmout()
        {
            InitializeComponent();
        }
        private void setTextBlock(TextBlock tb, string value)
        {
            tb.Text = value;
        }
        private void setShowGrid(FormSelectAmout from, string Gridname, bool isShow)
        {
            if (isShow) ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Visible;
            else ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Hidden;
        }
        private void setButtonValue(FormSelectAmout form, string gridName, string TextBlockName, string value, string amountTextBlockName, string amountValue)
        {
            ((Grid)(form.FindName(gridName))).Visibility = Visibility.Visible;
            ((Label)(form.FindName(TextBlockName))).Content = value;
            ((Label)(form.FindName(amountTextBlockName))).Content = amountValue;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                SysBLL.Player("请选择充值金额.wav");
                showInfo1.Text = "";
                //缴费类型为万通卡充值
                SysBLL.payCostType = 3;
                pageNo = 0;
                //清空面板
                clear();
                queryThread = new Thread(delegate () { query(pageNo); });
                queryThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:load():" + ex.Message);
            }
        }
        //查询
        private void query(int pageNo)
        {
            try
            {
                resultInfo("正在查询，请稍后...");
                isQuery = true;
                //清空面板
                clear();
                discountList = null;
                WintopAccess accexss = new WintopAccess();
                discountList = accexss.queryDiscount(Payment.wintopReChargeParam.WtCardNo);
                try
                {
                    showInfo1.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), showInfo1, accexss.findHintSpec().msg);
                }catch(Exception ex) { log.Write("error:query:" + ex.Message); }
                if (!"0000".Equals(discountList.msgrsp.retcode))
                {
                    resultInfo(discountList.msgrsp.retshow);
                    return;
                }
                if (discountList.msgrsp.RECHARGEINFO.Count == 0)
                {
                    resultInfo("无优惠信息");
                    return;
                }
                setPage(pageNo);
                resultInfo("请选择充值金额：");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                resultInfo("无优惠信息");
                log.Write("error:FormRegistrationDepartment_2:query():" + ex.Message);
            }
            finally
            {
                resultInfo("请选择充值金额：");
                isQuery = false;
            }
        }

        private void setPage(int page)
        {
            clear();
            int count = (discountList.msgrsp.RECHARGEINFO.Count / 9);
            if ((discountList.msgrsp.RECHARGEINFO.Count % 9) > 0)
            {
                count++;
            }
            if (page>=count)
            {
                page--;
                pageNo--;
            }
            for (int i = (page * 9),j=1; i < discountList.msgrsp.RECHARGEINFO.Count; i++,j++)
            {
                setButton(this, ("button" + (j)), ("name" + (j)),  discountList.msgrsp.RECHARGEINFO[i].CZ00017 + "元", ("amount" + (j)), "售价:"+discountList.msgrsp.RECHARGEINFO[i].CZ00030+"元");
                if (j == 9) return;
            }
        }
        private void resultInfo(String value)
        {
            showInfo.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), showInfo, value);
        }
        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="form"></param>
        /// <param name="gridName"></param>
        /// <param name="TextBlockName"></param>
        /// <param name="value"></param>
        private void setButton(FormSelectAmout form, string gridName, string TextBlockName, string value, string amountTextBlockName, string amountValue)
        {
            this.Dispatcher.Invoke(new setButtonValueDelegate(setButtonValue), form, gridName, TextBlockName, value,  amountTextBlockName,  amountValue);
        }
        //清空面板
        private void clear()
        {
            try
            {
                for (int i = 1; i <= 9; i++)
                {
                    this.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), this, ("button" + i), false);
                }
                //resultInfo("正在查询，请稍后");
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:clear():" + ex.Message);
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                //上一页
                if (pageNo > 0)
                {
                    pageNo--;
                }
                setPage(pageNo);
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:Button_Click_1:" + ex.Message);
            }
        }

        private void name1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn((Label)sender);
        }
        private void btn(Label lable)
        {
            try
            {
                for (int i = 0; i <= discountList.msgrsp.RECHARGEINFO.Count; i++)
                {
                    if (lable.Content.ToString().Split('元')[0].Equals(discountList.msgrsp.RECHARGEINFO[i].CZ00017))
                    {
                        Payment.wintopReChargeParam.UserInputMoney = discountList.msgrsp.RECHARGEINFO[i].CZ00017;
                        Payment.wintopReChargeParam.WintopDiscountInfoMsgrspList = discountList.msgrsp.RECHARGEINFO[i];
                        Util.JumpUtil.jumpCommonPage("FormReadCard");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }
        private void Button_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                pageNo++;
                setPage(pageNo);
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:Button_Click:" + ex.Message);
            }
        }

        private void name1_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            btn(name1);
        }

        private void name2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name2);
        }

        private void name3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name3);
        }

        private void name4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name4);
        }

        private void name5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name5);
        }

        private void name6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name6);
        }

        private void name7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name7);
        }

        private void name8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name8);
        }

        private void name9_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn(name9);
        }
        
    }
}
