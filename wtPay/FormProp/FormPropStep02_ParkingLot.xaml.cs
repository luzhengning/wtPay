using Newtonsoft.Json;
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
    /// FormPropStep02_ParkingLot.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStep02_ParkingLot : UserControl
    {
        //查询户号
        string paymentno = "";

        //查询水务户号的线程
        Thread queryThread = null;

        //水务信息
        List<wyDataParkList> list = null;

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
        public FormPropStep02_ParkingLot()
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
            Util.JumpUtil.jumpCommonPage("FormPropStep");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
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
        private void load()
        {
            try
            {
                yingcang();
                pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),pagedn,false);
                pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, false);
                SysBLL.Player("正在查询.wav");
                lblAccountInfo.Text = "正在查询...";
                clear();
                //查询户号线程
                queryThread = new Thread(query);
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();

                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, true);
            }
            catch(Exception ex)
            {
                log.Write("error:FormPropStep02_ParkingLot：load："+ex.Message+ex.InnerException);
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void yingcang()
        {
            pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, false);
            pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, false);
            //lblUserName.Visible=false;
            lblUserName1.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblUserName1, false);
        }
        private void xianshi()
        {
            pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, true);
            pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, true);
            //lblUserName.Visible = true;
            lblUserName1.Dispatcher.Invoke(new isShowTextBlockDelegate(isShowTextBlock), lblUserName1, true);
        }
        public void query()
        {
            try
            {
                ParkingLotQueryInfo info = PropAccess.ParkingLotQuery(Payment.PropPayParam.Mobile);
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //激活按钮
                    //btnRecharge.Enabled = true;
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        setTextBlock(lblAccountInfo,info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        setTextBlock(lblAccountInfo,"查询失败，请稍后再试");
                        return; 
                    }
                }
                if (info.msgrsp.wyDataParkList.Count > 0)
                {
                    xianshi();
                    setTextBlock(lblAccountInfo,"您的车位信息");
                    list = info.msgrsp.wyDataParkList;
                    pageCount = list.Count / 3;
                    if (list.Count <= 3)
                    {
                        pageCount = 0;
                    }
                    if ((list.Count % 3) != 0)
                    {
                        pageCount++;
                    }
                    setPage(page);
                    pagedn.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pagedn, true);
                    pageup.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton), pageup, true);
                }
            }
            catch (ThreadAbortException ae) { }
            catch (Exception e)
            {
                setTextBlock(lblAccountInfo,"查询失败...");
            }
            finally
            {
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, false);
            }
        }
        public void setTextBlock(TextBlock textBlock,string value)
        {
            textBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), textBlock, value);
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
        private void clear()
        {
            panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, false);
            panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, false);
            panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, false);
        }
        //行一
        private void setRow1(wyDataParkList info)
        {
            try
            {
                setTextBlock(address1, info.parkingmsg);
                panel1.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel1, true);
            }catch(Exception ex)
            {
                log.Write("");
            }
        }
        private void setRow2(wyDataParkList info)
        {
            try { 
                setTextBlock(address2, info.parkingmsg);
                panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, true);

            }
            catch (Exception ex)
            {
                log.Write("");
            }
        }
        private void setRow3(wyDataParkList info)
        {
            try { 
                setTextBlock(address3, info.parkingmsg);
                panel3.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel3, true);
            }
            catch (Exception ex)
            {
                log.Write("");
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }catch(Exception ex)
            {
                log.Write("error:FormPropStep02_ParkingLot:"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ParkInfo = list[ilbl1];
            Util.JumpUtil.jumpCommonPage("FormPropStep02");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ParkInfo = list[ilbl2];
            Util.JumpUtil.jumpCommonPage("FormPropStep02");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Payment.PropPayParam.ParkInfo = list[ilbl3];
            Util.JumpUtil.jumpCommonPage("FormPropStep02");
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

        private void pageup_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
            {
                page = page - 1;
                setPage(page);
            }
        }
    }
}
