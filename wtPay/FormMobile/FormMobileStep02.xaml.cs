using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.Mobile;
using wtPayModel.PaymentModel;

namespace wtPay.FormMobile
{
    /// <summary>
    /// FormMobileStep2.xaml 的交互逻辑
    /// </summary>
    public partial class FormMobileStep02 : UserControl
    {
        //查询线程
        Thread queryThread =null;
        //提示信息
        private delegate void showInfoDelegate(string value);
        //显示电话号码
        private delegate void balanceDelegate(string value);
        //显示余额
        private delegate void amountDelegate(string value);
        //显示欠费金额
        private delegate void overdueMoneyDelegate(string value);
        //是否显示面板
        private delegate void panelShowDelegate(bool isShow);
        //是否显示按钮
        private delegate void isShowBtnDelegate(Button btn,bool isShow);
        //是否显示图片
        private delegate void isShowPctDelegate(Image image,bool isShow);
        //是否显示加载
        private delegate void isShowLableDelegate(Label label, bool isShow);

        DispatcherTimer timerLoad;

        public FormMobileStep02()
        {
            InitializeComponent();
        }
        private void isShowLable(Label label, bool isShow)
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
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormMobileStep01");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (PayStaticParam.isHaveRMB == true)
            {
                //识币器
                Util.JumpUtil.jumpCommonPage("FormPayType");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormMobileSelectAmout");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try {
                SysBLL.Player("正在查询.wav");
                //初始化提示信息
                this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "正在查询，请稍后...");
                //隐藏面板
                panel.Visibility = Visibility.Hidden;
                //隐藏充值按钮
                this.OkBtn.Dispatcher.Invoke(new isShowBtnDelegate(setBtnVisibility), OkBtn, false);
                //查询
                queryThread = new Thread(delegate () { query(); });
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormMobileStep02:load():"+ex.Message);
            }
        }
        //查询
        private void query()
        {
            try
            {
                //显示图片
                this.loadlbl.Dispatcher.Invoke(new isShowLableDelegate(isShowLable), this.loadlbl, true);
                MobileQueryInfo info = MobileAccess.query(Payment.mobilePayParam.PhoneOn);

                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "查询失败，请稍后再试");
                        return;
                    }
                }
                Payment.mobilePayParam.QueryInfo = info;
                //格式金额，小数
                double amout = 0;
                double price = 0;
                amout = Convert.ToDouble(info.msgrsp.prepaidBalance);
                price = Convert.ToDouble(info.msgrsp.payableAmout);
                amout = amout / 100;
                price = price / 100;
                //显示电话号码
                this.lblBalance.Dispatcher.Invoke(new balanceDelegate(setBalanceText), Payment.mobilePayParam.PhoneOn);
                //显示余额
                this.lblAmount.Dispatcher.Invoke(new amountDelegate(setAmountText), amout.ToString());
                //显示欠费金额
                this.overdueMoneyLbl.Dispatcher.Invoke(new overdueMoneyDelegate(setOverdueMoneyText), price.ToString());
                //显示面板
                panel.Dispatcher.Invoke(new panelShowDelegate(setPanelVisibility), true);
                //显示充值按钮
                this.OkBtn.Dispatcher.Invoke(new isShowBtnDelegate(setBtnVisibility), OkBtn, true);
                //提示信息
                this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "您的话费信息");
                SysBLL.Player("继续缴费请点击确定按钮.wav");
            }
            catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch (WtException wte)
            {
                //异常处理
                if (wte.getCode() == WtExceptionCode.Sys.NETWORK)
                {
                    this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), WtException.formatMsg(WtExceptionCode.Sys.NETWORK) + wte.Message);
                }
                else
                {
                    this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), WtException.formatMsg(WtExceptionCode.Bus.BUS_QUERY) + wte.Message);
                }
            }
            catch (Exception e)
            {
                this.showInfoTxt.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "查询失败，请稍后再试");
                log.Write("error:FormMobileStep02:query():" + e.Message);
            }
            finally
            {
                //隐藏加载图片
                this.loadlbl.Dispatcher.Invoke(new isShowLableDelegate(isShowLable), this.loadlbl, false);
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
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="value"></param>
        private void setShowInfo(string value)
        {
            this.showInfoTxt.Text = value;
        } 
        /// <summary>
        /// 显示电话号码
        /// </summary>
        /// <param name="text"></param>
        private void setBalanceText(string text)
        {
            this.lblBalance.Text = text;
        }
        /// <summary>
        /// 显示余额
        /// </summary>
        /// <param name="text"></param>
        private void setAmountText(string text)
        {
            this.lblAmount.Text = text;
        }
        /// <summary>
        /// 显示欠费金额
        /// </summary>
        /// <param name="text"></param>
        private void setOverdueMoneyText(string text)
        {
            this.overdueMoneyLbl.Text = text;
        }
        /// <summary>
        /// 是否显示按钮
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="isShow"></param>
        private void setBtnVisibility(Button btn,bool isShow)
        {
            if (isShow) btn.Visibility = Visibility.Visible;
            else btn.Visibility = Visibility.Hidden;
        }
        //是否显示图片
        private void setPctVisibility(Image image,bool isShow)
        {
            if (isShow) image.Visibility = Visibility.Visible;
            else image.Visibility = Visibility.Hidden;
        }
        //设置面板显示或隐藏
        private void setPanelVisibility(bool isShow)
        {
            if(isShow)this.panel.Visibility = Visibility.Visible;
            else this.panel.Visibility = Visibility.Hidden;
        }
        //关闭窗口时触发
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
            }
            catch(Exception ex)
            {
                log.Write("error：FormMobileStep02：UserControl_Unloaded:" + ex.Message);
            }
        }
    }
}
