using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using wtPayBLL;
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.PropModel;

namespace wtPay.FormProp
{
    /// <summary>
    /// FormPropStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStep02 : UserControl
    {
        //查询户号
        string paymentno = "";

        //查询水务户号的线程
        Thread queryThread = null;


        List<wyDataChargeList> list = null;

        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        private delegate void isShowLabelDelegate(Label label, bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock, string value);
        private delegate void isShowGridDelegate(Grid grid, bool isShow);
        private delegate void isShowButtonDelegate(Button button, bool isShow);
        private delegate void isShowTextBlockDelegate(TextBlock textBlock, bool isShow);

        DispatcherTimer timerLoad;
        public FormPropStep02()
        {
            InitializeComponent();
        }
        private void isShowTextBlock(TextBlock textBlock, bool isShow)
        {
            if (isShow) textBlock.Visibility = Visibility.Visible;
            else textBlock.Visibility = Visibility.Hidden;
        }
        private void isShowButton(Button button, bool isShow)
        {
            if (isShow) button.Visibility = Visibility.Visible;
            else button.Visibility = Visibility.Hidden;
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void isShowGrid(Grid grid, bool isShow)
        {
            if (isShow) grid.Visibility = Visibility.Visible;
            else grid.Visibility = Visibility.Hidden;

        }
        private void isShowLabel(Label label, bool isShow)
        {
            if (isShow)
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Hidden;
            }
        }
        private void 确定_Click(object sender, RoutedEventArgs e)
        {

        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            if (Payment.PropPayParam.PropType == 1) Util.JumpUtil.jumpCommonPage("FormPropStep02_house");
            if (Payment.PropPayParam.PropType == 2) Util.JumpUtil.jumpCommonPage("FormPropStep02_ParkingLot");
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (loadlbl.Content.ToString().Length >= 6)
            {
                loadlbl.Content = "";
            }
            else
            {
                loadlbl.Content += ".";
            }
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
                SysBLL.payCostType = 10;
                yingcang();
                pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),pagedn,false);
                pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, false);
                SysBLL.Player("正在查询.wav");
                setTextBlock(lblAccountInfo, "正在查询，请稍后...");
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, true);
                clear();
                //查询户号线程
                queryThread = new Thread(query);
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormPropStep02:load:" + ex.Message);
            }
        }
        private void yingcang()
        {
            pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, false);
            pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, false);
            titlePanel.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), titlePanel, false);
        }
        private void xianshi()
        {
            pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, true);
            pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, true);
            titlePanel.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid),titlePanel,true);
        }
        public void query()
        {
            PropCostsQueryInfo info = null;
            PropCostsQueryParam param = new PropCostsQueryParam();
            try
            {
                if (Payment.PropPayParam.PropType == 1)
                {
                    param.queryid = Payment.PropPayParam.HouseInfo.houseid;
                    param.communityid = Payment.PropPayParam.HouseInfo.communityid;
                    param.isPark = "1";
                    //房屋查询
                }
                else if (Payment.PropPayParam.PropType == 2)
                {
                    //车位查询
                    param.queryid = Payment.PropPayParam.ParkInfo.parkingid;
                    param.communityid = Payment.PropPayParam.ParkInfo.communityid;
                    param.isPark = "0";
                }
                info = PropAccess.CostQuery(param);
                Payment.PropPayParam.MerchantNo = info.msgrsp.merchantNo;
                //param.queryid", param.queryid);// 房屋编码【houseid】或者车位编码【parkingid】	String 否
                //param.communityid", param.communityid);// 小区编码【communityid】	String 否
                //param.isPark="";// 费用编号  String 房屋费用 1，车位费用 0
                //if (!"0000".Equals(info.msgrsp.retcode))
                //{
                //    wtPayUtils.PrintInfo(WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY), lblAccountInfo, loadPct);
                //    return;
                //}
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //激活按钮
                    //btnRecharge.Enabled = true;
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        setTextBlock(lblAccountInfo, info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        setTextBlock(lblAccountInfo, "查询失败，请稍后再试...");
                        return;
                    }
                }
                if (info.msgrsp.wyDataChargeList.Count > 0)
                {
                    xianshi();
                    setTextBlock(lblAccountInfo, "您的账单信息");
                    list = info.msgrsp.wyDataChargeList;
                    pageCount = list.Count / 3;
                    if ((list.Count % 3) != 0)
                    {
                        pageCount++;
                    }
                    setPage(page);
                    pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, true);
                    pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, true);
                    SysBLL.Player("继续缴费请点击充值按钮.wav");
                }
                else
                {
                    setTextBlock(lblAccountInfo, "您的已完成本项缴费");
                    return;
                }
            }
            catch (ThreadAbortException ae) { }
            catch (Exception e)
            {
                log.Write("error:FormPropStep02:query:"+e.Message+e.InnerException);
                setTextBlock(lblAccountInfo,"查询失败...");
            }
            finally
            {
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, false);
            }
        }
        private void setTextBlock(TextBlock textBlock,string value)
        {
            textBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),textBlock,value);
        }
        int ilbl1 = 0, ilbl2 = 0, ilbl3 = 0;
        //设置页面
        private void setPage(int page)
        {
            if (list == null) return;
            clear();
            int count = 0;
            for (int i = page * 3; i < list.Count; i++)
            {
                count++;
                if (count == 1) { setRow1(list[i]); ilbl1 = i; }
                if (count == 2) { setRow2(list[i]); ilbl2 = i; }
                if (count == 3) { setRow3(list[i]); ilbl3 = i; }
                if (count == 3)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 隐藏3个面
        /// </summary>
        private void clear()
        {
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, false);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, false);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, false);
        }
        //行一
        private void setRow1(wyDataChargeList info)
        {
            setTextBlock(name1,info.standardname);
            setTextBlock(amount1, info.money);
            setTextBlock(balance1, info.late_fees);
            setTextBlock(chargetypename1, info.chargetypename);
            //setTextBlock(adjustMoney1, info.adjustMoney);
            //setTextBlock(chargetypename1, "");
            //setTextBlock(adjustMoney1, "");
            //
            setTextBlock(date1, info.stime + "-" + info.etime);
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, true);
        }
        private void setRow2(wyDataChargeList info)
        {
            setTextBlock(name2, info.standardname);
            setTextBlock(amount2, info.money);
            setTextBlock(balance2, info.late_fees);
            setTextBlock(chargetypename2, info.chargetypename);
            //setTextBlock(adjustMoney2, info.adjustMoney);
            //setTextBlock(chargetypename2, "");
            //setTextBlock(adjustMoney2, "");
            //
            setTextBlock(date2, info.stime + "-" + info.etime);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, true);
        }
        private void setRow3(wyDataChargeList info)
        {
            setTextBlock(name3, info.standardname);
            setTextBlock(amount3, info.money);
            setTextBlock(balance3, info.late_fees);
            setTextBlock(chargetypename3, info.chargetypename);
            //setTextBlock(adjustMoney3, info.adjustMoney);
            //setTextBlock(chargetypename3,"");
            //setTextBlock(adjustMoney3, "");
            //
            setTextBlock(date3, info.stime + "-" + info.etime);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, true);
        }

        private void pageup_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }

        private void pagedn_Click(object sender, RoutedEventArgs e)
        {
            int count = page;
            count++;
            if (count < pageCount)
            {
                page = page + 1;
                setPage(page);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ChargeList = list[ilbl1];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch(Exception ex)
            {
                log.Write("error:FormPropStep02:Unloaded:"+ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ChargeList = list[ilbl2];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ChargeList = list[ilbl3];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }
    }
}
