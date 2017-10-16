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
using wtPayCommon;
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.WaterModel;

namespace wtPay.FormWater
{
    /// <summary>
    /// FormWaterStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormWaterStep02 : UserControl
    {

        //查询水务户号的线程
        Thread queryThread = null;

        //水务信息
        List<WaterQueryOrderList> list = null;

        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";
        string custNo3 = "";

        private delegate void setShowGridDelegate(Grid grid, bool isShow);
        private delegate void setShowButtonDelegate(Button button, bool isShow);
        private delegate void setShowTextBlockDelegate(TextBlock textBlock, bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock, string value);
        private delegate void setShowLabelDelegate(Label label, bool isShow);




        public FormWaterStep02()
        {
            InitializeComponent();
        }
        private void setShowGrid(Grid grid, bool isShow)
        {
            if (isShow) grid.Visibility = Visibility.Visible;
            else grid.Visibility = Visibility.Hidden;
        }
        private void setShowButton(Button button, bool isShow)
        {
            if (isShow)
                button.Visibility = Visibility.Visible;
            else
                button.Visibility = Visibility.Hidden;
        }
        private void setShowTextBlock(TextBlock textBlock, bool isShow)
        {
            if (isShow)
                textBlock.Visibility = Visibility.Visible;
            else
                textBlock.Visibility = Visibility.Hidden;
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void setShowLabel(Label label, bool isShow)
        {
            if (isShow)
                label.Visibility = Visibility.Visible;
            else
                label.Visibility = Visibility.Hidden;
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
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                lblAccountInfo.Text = "正在查询...";
                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();

                yingcang();
                button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, false);
                button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, false);
                pagelbl.Visibility = Visibility.Hidden;
                SysBLL.Player("正在查询.wav");
                clear();
                //查询户号线程
                queryThread = new Thread(delegate () { query(); });
                queryThread.Start();
            }
            catch (Exception ex) { log.Write("error:FormWaterStep02:load():" + ex.Message); }
        }
        void yingcang()
        {
            //panel6.Visible = false;
            button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, false);
            button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, false);
            //lblUserName.Visible=false;
            lblUserName1.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblUserName1, false);
            lblDatee.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblDatee, false);
            lblMoney.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblMoney, false);
            blancelbl.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), blancelbl, false);
        }
        void xianshi()
        {
            button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, true);
            button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, true);
            //lblUserName.Visible = true;
            lblUserName1.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblUserName1, true);
            lblDatee.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblDatee, true);
            lblMoney.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblMoney, true);
            blancelbl.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), blancelbl, true);
        }
        //查询水务
        public void query()
        {
            try
            {
                loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, true);
                WaterQueryInfo info = WaterAccess.WaterQuery(Payment.waterPayParam.Account);
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //激活按钮
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        printInfo(info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        printInfo("查询失败，请稍后再试");
                        return;
                    }
                }
                if (info.msgrsp.orderlist.Count > 0)
                {
                    xianshi();
                    printInfo("您的账单信息");
                    list = info.msgrsp.orderlist;
                    pageCount = list.Count / 3;
                    if ((list.Count % 3) != 0)
                    {
                        pageCount++;
                    }
                    setPage(page);
                    pagelbl.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock),pagelbl,true);
                    button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, true);
                    button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, true);
                    SysBLL.Player("继续缴费请点击充值按钮.wav");
                }
            }
            catch (ThreadAbortException ae)
            {
                log.Write("error:FormWaterStep02:query():" + ae.Message);
                printInfo("查询失败，请稍后再试");
            }
            catch (WtException e)
            {
                printInfo(e.Message);
            }
            catch (Exception e)
            {
                log.Write("error:FormWaterStep02:query():" + e.Message);
                printInfo("查询失败，请稍后再试");
            }
            finally
            {
                loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, false);
            }
        }
        private void printInfo(string info)
        {
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, info);
            loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, false);
        }
        //设置页面
        void setPage(int page)
        {
            pagelbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), pagelbl, "第" + (page+1) + "页");
            if (list == null) return;
            clear();
            int count = 0;
            for (int i = page * 3; i < list.Count; i++)
            {
                count++;
                if (count == 1) setRow1(list[i]);
                if (count == 2) setRow2(list[i]);
                if (count == 3) setRow3(list[i]);
                if (count == 3)
                {
                    break;
                }
            }
        }
        void clear()
        {
            panel1.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel1, false);
            panel2.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel2, false);
            panel3.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel3, false);
        }
        //行一
        void setRow1(WaterQueryOrderList info)
        {
            custNo1 = info.custNo;
            username1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username1, info.custName);
            date1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date1, info.billDate);
            balance1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance1, info.balance);
            price1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), price1, info.amout);
            panel1.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel1, true);
        }
        void setRow2(WaterQueryOrderList info)
        {
            custNo2 = info.custNo;
            username2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username2, info.custName);
            date2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date2, info.billDate);
            balance2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance2, info.balance);
            price2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), price2, info.amout);
            panel2.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel2, true);
        }
        void setRow3(WaterQueryOrderList info)
        {
            custNo3 = info.custNo;
            username3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), username3, info.custName);
            date3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date3, info.billDate);
            balance3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance3, info.balance);
            price3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), price3, info.amout);
            panel3.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel3, true);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
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
            List<WaterQueryOrderList> orderlist = new List<WaterQueryOrderList>();
            orderlist.Add(new WaterQueryOrderList());
            orderlist[0].custNo = custNo1;
            orderlist[0].custName = username1.Text;
            orderlist[0].billDate = date1.Text;
            orderlist[0].amout = price1.Text;
            WaterQueryInfo info = new WaterQueryInfo();
            info.msgrsp = new WaterQueryMsgreq();
            info.msgrsp.orderlist = orderlist;
            //new FormJump().openForm(sender, e, this, new FormWaterStep03(info));
            Payment.waterPayParam.WaterQueryInfo = info;
            Util.JumpUtil.jumpCommonPage("FormWaterStep03");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<WaterQueryOrderList> orderlist = new List<WaterQueryOrderList>();
            orderlist.Add(new WaterQueryOrderList());
            orderlist[0].custNo = custNo2;
            orderlist[0].custName = username2.Text;
            orderlist[0].billDate = date2.Text;
            orderlist[0].amout = price2.Text;
            WaterQueryInfo info = new WaterQueryInfo();
            info.msgrsp = new WaterQueryMsgreq();
            info.msgrsp.orderlist = orderlist;
            //new FormJump().openForm(sender, e, this, new FormWaterStep03(info));
            Payment.waterPayParam.WaterQueryInfo = info;
            Util.JumpUtil.jumpCommonPage("FormWaterStep03");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<WaterQueryOrderList> orderlist = new List<WaterQueryOrderList>();
            orderlist.Add(new WaterQueryOrderList());
            orderlist[0].custNo = custNo3;
            orderlist[0].custName = username3.Text;
            orderlist[0].billDate = date3.Text;
            orderlist[0].amout = price3.Text;
            WaterQueryInfo info = new WaterQueryInfo();
            info.msgrsp = new WaterQueryMsgreq();
            info.msgrsp.orderlist = orderlist;
            //new FormJump().openForm(sender, e, this, new FormWaterStep03(info));
            Payment.waterPayParam.WaterQueryInfo = info;
            Util.JumpUtil.jumpCommonPage("FormWaterStep03");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormWaterStep01");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormWaterStep02:UserControl_Unloaded:" + ex.Message);
            }
        }
        DispatcherTimer timerLoad;
    }
}
