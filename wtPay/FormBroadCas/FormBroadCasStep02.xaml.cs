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
using wtPayModel;
using wtPayModel.BroadCas;
using wtPayModel.PaymentModel;

namespace wtPay.FormBroadCas
{
    /// <summary>
    /// FormBroadCasStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormBroadCasStep02 : UserControl
    {
        //查询线程
        Thread queryThread = null;
        //广电信息
        BoadCasQueryOrderlist list = null;

        //广电列表页数
        int pageCount = 0;
        //当前页数
        int page = 0;

        DispatcherTimer timerLoad;

        private delegate void isShowGridDelegate(Grid grid,bool isShow);
        private delegate void isShowButtonDelegate(Button button,bool isShow);
        private delegate void isShowLabelDelegate(Label label,bool isShow);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);

        public FormBroadCasStep02()
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
        private void isShowButton(Button button,bool isShow)
        {
            if (isShow)
            {
                button.Visibility = Visibility.Visible;
            }else
            {
                button.Visibility = Visibility.Hidden;
            }
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
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Payment.broadCasPayParam.List = list;
                if (PayStaticParam.isHaveRMB == true)
                {
                    //识币器
                    Util.JumpUtil.jumpCommonPage("FormPayType");
                }
                else
                {
                    Util.JumpUtil.jumpCommonPage("FormBroadCasStep03");
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormBroadCasStep02:确定_Click:" + ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormBroadCasStep01");
        }
        //load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                panel12.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid), this.panel12, false);
                btnRecharge.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),this.btnRecharge,false);
                //查询户号线程
                queryThread = new Thread(delegate () { query(); });
                queryThread.Start();
                SysBLL.Player("正在查询.wav");
                lblAccountInfo.Text = "正在查询...";
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, true);

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormBroadCasStep02:load():"+ex.Message);
            }
        }
        //查询广电
        public void query()
        {
            try
            {
                BroadCasQueryInfo info = BroadCasAccess.query(Payment.broadCasPayParam.Account);
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    //隐藏加载按钮
                    loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel),this.loadlbl,false);
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        //显示提示信息
                        lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.lblAccountInfo,info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.lblAccountInfo, "查询失败，请稍后再试");
                        return;
                    }
                }
                PrintInfo("您的账单信息");
                list = info.msgrsp.guangDianData;
                //账户名称
                lblBalance.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.lblBalance, list.CUSTNAME);
                //账单余额
                if (Convert.ToDouble(list.BANLANCE) >= 0)
                {
                    lblAmountText.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAmountText,"账户余额");
                }
                else
                {
                    lblAmountText.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblAmountText, "欠费金额");
                }
                string price = list.BANLANCE;
                lblAmount.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.lblAmount, price);
                lblPayMentAmout.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.lblPayMentAmout, list.PAYMENTAMOUNT);
                btnRecharge.Dispatcher.Invoke(new isShowButtonDelegate(isShowButton),this.btnRecharge,true);
                panel12.Dispatcher.Invoke(new isShowGridDelegate(isShowGrid),this.panel12,true);
                SysBLL.Player("继续缴费请点击确定按钮.wav");
            }
            catch (ThreadAbortException ae) { }
            catch (WtException e)
            {
                PrintInfo(e);
            }
            catch (Exception e)
            {
                //弹出异常提示页面
                string msg = WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY, e.Message);//异常信息
                PrintInfo(msg);
            }
            finally
            {
            }
        }
        private void PrintInfo(WtException e)
        {
            PrintInfo(e.getMsg());
        }
        private void PrintInfo(string error)
        {
            lblAccountInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.lblAccountInfo,error);
            loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), this.loadlbl, false);
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
            }
            catch(Exception ex)
            {
                log.Write("error:FormBroadCasStep02:UserControl_Unloaded"+ex.Message);
            }
        }
    }
}
