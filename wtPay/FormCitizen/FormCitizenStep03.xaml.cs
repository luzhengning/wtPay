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
    /// FormCitizenStep03.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep03 : UserControl
    {
        Thread queryThread = null;

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        private delegate void setGridShowDelegate(Grid grid,bool b);
        private delegate void isShowLabelDelegate(Label label,bool isShow);

        WintopQueryInfo wintopQueryInfo = null;


        //查询结果
        List<WintopQueryResult> list = null;

        //脱机
        WintopQueryResult result01 = null;
        //大额联机
        WintopQueryResult result02 = null;
        //小额联机
        WintopQueryResult result03 = null;

        DispatcherTimer timerLoad;
        public FormCitizenStep03()
        {
            InitializeComponent();
        }
        
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                //交通钱包
                if (result01 != null)
                {
                    Payment.wintopReChargeParam.WintopQueryResult = result01;
                    Util.JumpUtil.jumpCommonPage("FormNot");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStep03:Button_Click:"+ex.Message);
            }
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try {
                SysBLL.Player("正在查询.wav");
                result01 = null;
                result02 = null;
                result03 = null;
                list = null;
                wintopQueryInfo = null;
                visble();
                lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "正在查询，请稍后...");
                //显示正在加载中
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel),this.loadlbl,true);
                queryThread = new Thread(delegate () { query(); });
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormCitizenStep03:load():"+ex.Message);
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
        private void visble()
        {
            panel6.Dispatcher.Invoke(new setGridShowDelegate(setGridShow), panel6, false);
        }
        void show()
        {
            panel6.Dispatcher.Invoke(new setGridShowDelegate(setGridShow),panel6,true);
        }
        void query()
        {
            try
            {
                wintopQueryInfo = WintopAccess.WintopQuery(Payment.wintopReChargeParam.WtCardNo);

                if (!"0000".Equals(wintopQueryInfo.msgrsp.retcode))
                {
                    //pictureBox4.Visible = false;
                    if (wintopQueryInfo.msgrsp.retshow.Length > 0)
                    {
                        lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),lblAccountInfo, wintopQueryInfo.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),lblAccountInfo, "查询失败，请稍后再试");
                        return;
                    }
                }
                
                if (wintopQueryInfo.msgrsp.wTCardInfoList.Count > 0)
                {
                    show();
                    list = wintopQueryInfo.msgrsp.wTCardInfoList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if ("01".Equals(list[i].TYPE))
                        {
                            USERID1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), USERID1, list[i].WTCARDID);
                            AMOUNT1.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),AMOUNT1, list[i].AMOUNT);
                            result01 = list[i];
                        }
                        if ("02".Equals(list[i].TYPE))
                        {
                            USERID2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), USERID2, list[i].WTCARDID);
                            AMOUNT2.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), AMOUNT2,list[i].AMOUNT);
                            result02 = list[i];
                        }
                        /*if ("03".Equals(list[i].TYPE))
                        {
                            USERID3.Text = list[i].USERID;
                            AMOUNT3.Text = list[i].AMOUNT;
                            result03 = list[i];
                            button3.Enabled = true;
                        }*/
                    }
                    lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "您的账号信息");
                    SysBLL.Player("继续缴费请点击充值按钮.wav");
                }
                else
                {
                    lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "没有记录");
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch (WtException e)
            {
                lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, e.Message);
            }
            catch (Exception e)
            {
                lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, "查询失败，请稍候再试...");
            }
            finally
            {
                //隐藏加载
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, false);
            }
        }

        private void setTextBlockText(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }

        private void setGridShow(Grid grid,bool b)
        {
            if (b) grid.Visibility = Visibility.Visible;
            else grid.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try {
                //消费缴费钱包  
                if (result02 != null)
                {
                    Payment.wintopReChargeParam.WintopQueryResult = result02;
                    Util.JumpUtil.jumpCommonPage("FormSelectAmout");
                    //Util.JumpUtil.jumpCommonPage("FormCitizenStep04");
                }
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStep03:Button_Click_1:"+ex.Message);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;
                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStep03:UserControl_Unloaded:"+ex.Message);
            }
        }
    }
}
