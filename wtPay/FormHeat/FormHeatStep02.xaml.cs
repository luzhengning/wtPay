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
using wtPayModel.HeatModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormHeat
{
    /// <summary>
    /// FormHeatStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormHeatStep02 : UserControl
    {
        private delegate void isShowGridDelegate(Grid gird,bool isShow);
        private delegate void isShowButtonDelegate(Button button,bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        private delegate void isShowLabelDelegate(Label label, bool isShow);
        private delegate void isShowTextBlockDelegate(TextBlock textBlock, bool isShow);


        //查询线程
        Thread queryThread = null;

        //热力缴费列表
        List<HeatQueryOrderlist> heatList = null;

        int list1 = 0;
        int list2 = 0;
        int list3 = 0;
        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        object senderLoad;
        EventArgs eLoad;

        string address1Str = "";
        string address2Str = "";
        string address3Str = "";

        DispatcherTimer timerLoad;
        public FormHeatStep02()
        {
            InitializeComponent();
        }
        private void isShowGrid(Grid grid,bool isShow)
        {
            if (isShow)
            {
                grid.Visibility = Visibility.Visible;
            }else
            {
                grid.Visibility = Visibility.Hidden;
            }
        }
        private void isShowTextBlock(TextBlock textblock,bool isshow)
        {
            if (isshow) textblock.Visibility = Visibility.Visible;
            else textblock.Visibility = Visibility.Hidden;
        }
        private void isShowButton(Button button,bool isShow)
        {
            if (isShow)
            {
                button.Visibility = Visibility.Visible;
            }else
            {
                buttomPageBtn.Visibility = Visibility.Hidden;
            }
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
        private void setTextBlock(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormHeatStep01");
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
                //清空列表
                clear();
                pagelbl.Visibility = Visibility.Hidden;
                lblAccountInfo.Text = "正在查询，请稍后...";
                topPageBtn.Visibility = Visibility.Hidden;
                buttomPageBtn.Visibility = Visibility.Hidden;
                titlePanel.Visibility = Visibility.Hidden;
                loadlbl.Visibility = Visibility.Visible;
                SysBLL.Player("正在查询.wav");
                queryThread = new Thread(query);
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormHeatStep02:load():"+ex.Message);
            }
        }
        //查询
        private void query()
        {
            try
            {
                //根据账号查询热力
                HeatQueryInfo info = HeatAccess.HeatQuery(Payment.heatPayParam.AccountNo);

                //if (!"0000".Equals(info.msgrsp.retcode))
                //{
                //    wtPayUtils.PrintInfo(WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY), lblAccountInfo, pictureBox4);
                //    return;
                //}
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //激活按钮
                    //btnRecharge.Enabled = true;
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
                    panel6.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel6, true);
                    clear();
                    topPageBtn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),topPageBtn,true);
                    buttomPageBtn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), buttomPageBtn, true);
                    titlePanel.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), titlePanel, true);
                    heatList = info.msgrsp.orderlist;
                    pageCount = heatList.Count / 3;
                    if ((heatList.Count % 3) != 0)
                    {
                        pageCount++;
                    }
                    setPage(page);
                    pagelbl.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), pagelbl, true);
                    printInfo("您的账单信息");
                }

            }
            catch (ThreadAbortException ae) { }
            catch (Exception e)
            {
                printInfo("查询失败，请稍后再试");
                log.Write("error:FormHeatStep02:query():"+e.Message);
            }
            finally
            {
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, false);
            }

        }
        private  void printInfo(string info)
        {
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock),lblAccountInfo,info);
        }
        //设置页面
        private void setPage(int page)
        {
            pagelbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock),pagelbl,"第"+(page+1) +"页");
            if (heatList == null) return;
            clear();
            int count = 0;
            for (int i = page * 3; i < heatList.Count; i++)
            {
                count++;
                if (count == 1) { setRow1(heatList[i]); list1 = i; }
                if (count == 2) { setRow2(heatList[i]); list2 = i; }
                if (count == 3) { setRow3(heatList[i]); list3 = i; }
                if (count == 3)
                {
                    break;
                }
            }
        }
        //行一
        private void setRow1(HeatQueryOrderlist info)
        {
            name1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), name1, info.custName);
            address1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), address1, info.address);
            address1Str = info.address;
            date1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), date1, info.billDate);
            price1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), price1, info.amout);
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, true);
        }
        private void setRow2(HeatQueryOrderlist info)
        {
            name2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), name2, info.custName); 
            date2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), date2, info.billDate);
            address2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), address2, info.address);
            address2Str = info.address;
            price2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), price2, info.amout);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, true);
        }
        private void setRow3(HeatQueryOrderlist info)
        {
            name3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), name3, info.custName);
            date3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), date3, info.billDate);
            price3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), price3, info.amout);
            address3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlock), address3, info.address);
            address3Str = info.address;
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, true);
        }
        private void clear()
        {
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, false);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, false);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, false);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Payment.heatPayParam.HeatQueryOrderlist = heatList[list1];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Payment.heatPayParam.HeatQueryOrderlist = heatList[list2];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Payment.heatPayParam.HeatQueryOrderlist = heatList[list3];
            Util.JumpUtil.jumpCommonPage("FormReadCard");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            int count = page;
            count++;
            if (count < pageCount)
            {
                page = page + 1;
                setPage(page);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try{ 
                if (queryThread.IsAlive)
                {
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                }
                queryThread = null;

                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }catch(Exception ex)
            {
                log.Write("error:FormHeatStep02:unloaded:"+ex.Message);
            }
        }
    }
}
