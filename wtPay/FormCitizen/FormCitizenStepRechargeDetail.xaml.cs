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
using wtPayModel.WintopModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormWaterStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStepRechargeDetail : UserControl
    {

        //查询线程
        Thread queryThread = null;

        WintopRechargeDetailParam param = null;
        //查询结果
        WintopRechargeDetailInfo wintopRechargeDetailInfo = null;

        bool isQuery = true;

        //水务信息
        List<WintopRechargeDetailInfoMsgwTRechargeInfo> list = null;

        int nextPage = 1;

        int totalPage = 0;
        //热力列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        string custNo1 = "";
        string custNo2 = "";  
        string custNo3 = "";
         
        private delegate void setShowGridDelegate(Grid grid,bool isShow);
        private delegate void setShowButtonDelegate(Button button,bool isShow);
        private delegate void setShowTextBlockDelegate(TextBlock textBlock,bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        private delegate void setShowLabelDelegate(Label label,bool isShow);


        DispatcherTimer timerLoad;

        public FormCitizenStepRechargeDetail()
        {
            InitializeComponent();
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
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                totalPage = 9999;
                wintopRechargeDetailInfo = null;
                pageCount = 0;
                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();

                loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, true);
                lblAccountInfo.Text="正在查询,请稍后...";

                page = -1;
                nextPage = 1;
                list = null;
                yingcang();
                button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, false);
                button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, false);
                SysBLL.Player("正在查询.wav");
                clear();
                //查询户号线程
                queryThread = new Thread(delegate() { query("1","3"); });
                queryThread.Start();
            }
            catch(Exception ex) { log.Write("error:FormCitizenStepRechargeDetail:load():"+ex.Message); }
        }
        void yingcang()
        {
            //panel6.Visible = false;
            button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, false);
            button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, false);
            //lblUserName.Visible=false;
            lblUserName1.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock),lblUserName1,false);
            lblDatee.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblDatee, false);
            blancelbl.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), blancelbl, false);
        }
        void xianshi()
        {
            button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, true);
            button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, true);
            //lblUserName.Visible = true;
            lblUserName1.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblUserName1, true);
            lblDatee.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), lblDatee, true);
            blancelbl.Dispatcher.Invoke(new setShowTextBlockDelegate(setShowTextBlock), blancelbl, true);
        }
        //查询水务
        public void query(string pageNo,string pageSize)
        {
            try
            {
                page++;
                if (page >= totalPage)
                {
                    page = totalPage;
                    setPage(page);
                    return;
                }
                if (page < pageCount)
                {
                    setPage(page);
                    return;
                }
                page--;
                loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl,true);
                printInfo("正在查询,请稍后...");
                isQuery = false;
                param = new WintopRechargeDetailParam();
                param.Password = Payment.wintopReChargeParam.Md5Pwd;
                param.Wtcardid = Payment.wintopReChargeParam.WtCardNo;
                param.PageNo = pageNo;
                param.PageSize = pageSize;
                
                wintopRechargeDetailInfo = WintopAccess.RechargeDetail(param);
                if (!"0000".Equals(wintopRechargeDetailInfo.msgrsp.retcode))
                {
                    //激活按钮
                    if (wintopRechargeDetailInfo.msgrsp.retshow.Length > 0)
                    {
                        printInfo(wintopRechargeDetailInfo.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        printInfo("查询失败，请稍后再试");
                        return;  
                    }
                }
                if (wintopRechargeDetailInfo.msgrsp.wTRechargeInfo.Count > 0)
                { 
                    xianshi();
                    printInfo("您的账单信息");
                    if (list == null)
                    {
                        list = new List<WintopRechargeDetailInfoMsgwTRechargeInfo>();
                    }
                    totalPage = Convert.ToInt32(wintopRechargeDetailInfo.msgrsp.totalPage);
                    list.AddRange(wintopRechargeDetailInfo.msgrsp.wTRechargeInfo);
                    pageCount = list.Count / 3;
                    if ((list.Count % 3) != 0)
                    {
                        pageCount++;
                    }
                    page++;
                    setPage(page);
                    button4.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button4, true);
                    button3.Dispatcher.Invoke(new setShowButtonDelegate(setShowButton), button3, true);
                }
            }
            catch (ThreadAbortException ae) {
                log.Write("error:FormWaterStep02:query():" + ae.Message);
                printInfo("查询失败，请稍后再试");
            }
            catch (WtException e)
            {
                printInfo(e.Message);
            }
            catch (Exception e)
            {
                log.Write("error:FormWaterStep02:query():"+e.Message);
                printInfo("查询失败，请稍后再试");
            }
            finally
            {
                isQuery = true;
                loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, false);
            }
        }
        private void printInfo(string info)
        {
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),lblAccountInfo,info);
        }
        //设置页面
        void setPage(int page)
        {
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
        void setRow1(WintopRechargeDetailInfoMsgwTRechargeInfo info)
        {
            //custNo1 = info.custNo;
            if ("大额联机账户".Equals(info.TYPE))
                info.TYPE = "消费缴费钱包";
            type1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),type1, info.TYPE);
            date1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date1, info.LASTTIME);
            balance1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance1, info.AMOUNT);
            panel1.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel1, true);
        }
        void setRow2(WintopRechargeDetailInfoMsgwTRechargeInfo info)
        {
            //custNo2 = info.custNo;
            if ("大额联机账户".Equals(info.TYPE))
                info.TYPE = "消费缴费钱包";
            type2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), type2, info.TYPE);
            date2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date2, info.LASTTIME);
            balance2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance2, info.AMOUNT);
            panel2.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel2, true);
        }
        void setRow3(WintopRechargeDetailInfoMsgwTRechargeInfo info)
        {
            //custNo3 = info.custNo;
            if ("大额联机账户".Equals(info.TYPE))
                info.TYPE = "消费缴费钱包";
            type3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), type3, info.TYPE);
            date3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), date3, info.LASTTIME);
            balance3.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), balance3, info.AMOUNT);
            panel3.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), panel3, true);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                nextPage = nextPage - 1;
                setPage(page);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (!isQuery) return;
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;
                nextPage += 1;
                //查询户号线程
                queryThread = new Thread(delegate () { query(nextPage.ToString(), "3"); });
                queryThread.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStepSpendDetail:button3_Click:"+ex.Message);
            }
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                list.Clear();
                list = null;
                queryThread = null;

                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch(Exception ex)
            {
                log.Write("error:FormCitizenStepRechargeDetail:UserControl_Unloaded:"+ex.Message);
            }
        }
    }
}
