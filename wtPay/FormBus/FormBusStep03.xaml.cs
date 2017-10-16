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
using System.Windows.Threading;
using wtPayBLL;
using wtPayDAL;
using wtPayModel.BusModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormBus
{
    /// <summary>
    /// FormBusStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormBusStep03 : UserControl
    {
        private delegate void isShowButtonDelegate(Button button,bool isShow);
        private delegate void isShowGridDelegate(Grid grid,bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        private delegate void isShowLabelDelegate(Label label,bool isShow);


        Thread queryThread = null;
        DispatcherTimer timerLoad;
        public FormBusStep03()
        {
            InitializeComponent();
        }
        private void isShowLabel(Label label,bool isShow)
        {
            if (isShow)
            {
                label.Visibility = Visibility.Visible;
            }else
            {
                label.Visibility = Visibility.Hidden;
            }
        }
        private void setTextBlockText(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }
        private void isShowButton(Button button, bool isShow)
        {
            if (isShow)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Hidden;
            }
        }
        private void isShowGrid(Grid grid,bool isShow)
        {
            if (isShow) grid.Visibility = Visibility.Visible;
            else grid.Visibility = Visibility.Hidden;
        }
        private void 充值_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormBusStep04");
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
                panel2.Visibility = Visibility.Hidden;
                SysBLL.Player("正在查询.wav");
                lblAccountInfo.Text = "正在查询，请稍候...";
                btnRecharge.Visibility = Visibility.Hidden;
                loadlbl.Visibility = Visibility.Visible;
                queryThread = new Thread(delegate() { query(); });
                queryThread.Start();
                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormBusStep03:load():"+ex.Message);
            }
        }
       
        private void query()
        {
            try
            {
                BusQueryThatInfo busQueryThatInfo = BusAccess.QueryBus();
                if (!"0000".Equals(busQueryThatInfo.msgrsp.retcode))
                {
                    if (busQueryThatInfo.msgrsp.retshow.Length > 0)
                    {
                        PrintInfo( busQueryThatInfo.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        PrintInfo("查询失败，请稍后再试...");
                        return;
                    }
                }

                panel2.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), panel2, true);
                lblAccountInfo.Text = "您的账户信息";
                lblname.Text = busQueryThatInfo.cpumsg.OUTPUT.ONAME;

                string balance = busQueryThatInfo.cpumsg.OUTPUT.WMONEY;
                lblAmount.Text = balance.Insert(balance.Length - 2, ".") + "元";
                
                Payment.BusPayParam.Output = busQueryThatInfo.cpumsg.OUTPUT;
            }
            catch (ThreadAbortException ae) { }
            catch (Exception e)
            {
                PrintInfo("查询失败，请稍后再试...");
            }
            finally
            {
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel),loadlbl,false);
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

        private void PrintInfo(String msg)
        {
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAccountInfo, msg);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
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
                log.Write("error:FormBroadCasStep02:unload:"+ex.Message);
            }
        }
    }
}
