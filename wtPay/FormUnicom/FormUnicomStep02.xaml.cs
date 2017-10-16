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
using wtPayModel.PaymentModel;
using wtPayModel.UnicomModel;

namespace wtPay.FormUnicom
{
    /// <summary>
    /// FormMobileStep2.xaml 的交互逻辑
    /// </summary>
    public partial class FormUnicomStep02 : UserControl
    {
        //查询线程
        Thread queryThread =null;
        //提示信息
        private delegate void showInfoDelegate(string value);
        //显示电话号码
        private delegate void balanceDelegate(string value);
        //显示余额
        private delegate void amountDelegate(string value);
        //是否显示面板
        private delegate void panelShowDelegate(bool isShow);
        //是否显示按钮
        private delegate void isShowBtnDelegate(Button btn,bool isShow);
        //是否显示图片
        private delegate void isShowLabelDelegate(Label label,bool isShow);

        private delegate void setTextBlockTextDelegate(TextBlock textBlockm,string value);

        DispatcherTimer timerLoad;

        public FormUnicomStep02()
        {
            InitializeComponent();
        }
        private void setTextBlockText(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }
        private void isShowLabel(Label label,bool isShow)
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
            Util.JumpUtil.jumpCommonPage("FormMobileStep");
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
                //电子现金
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
                this.lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "正在查询，请稍后...");
                //显示图片
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, true);
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
                log.Write("error:FormUnicomStep02:load():"+ex.Message);
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
        //查询
        private void query()
        {
            try
            {
                UnicomQueryInfo info = UnicomAccess.query(Payment.unicomPayParam.PhoneOn);
                if (!"0000".Equals(info.msgrsp.retcode))
                {
                    if (info.msgrsp.retshow.Length > 0)
                    {
                        lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), info.msgrsp.retshow);
                        return;
                    }
                    else
                    {
                        lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "查询失败，请稍后再试");
                        return;
                    }
                }
                Payment.unicomPayParam.Msgrsp = info.msgrsp;
                Payment.unicomPayParam.AccountNo = info.msgrsp.ACCOUNT_NO;
                //显示电话号码
                this.lblBalance.Dispatcher.Invoke(new balanceDelegate(setBalanceText), Payment.unicomPayParam.PhoneOn);
                //显示当月消费金额
                this.lblAmount.Dispatcher.Invoke(new amountDelegate(setAmountText), info.msgrsp.PRESENT_AMOUNT);
                //账户余额
                this.lblyue.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),this.lblyue,info.msgrsp.PAYMENT_AMOUNT);
                //显示面板
                this.panel.Dispatcher.Invoke(new panelShowDelegate(setPanelVisibility), true);
                //显示充值按钮
                this.OkBtn.Dispatcher.Invoke(new isShowBtnDelegate(setBtnVisibility),OkBtn,true);
                //提示信息
                lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "您的话费信息");
                SysBLL.Player("继续充值请点积确定按钮.wav");
            }
            catch (WtException wte)
            {
                lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "查询失败，请稍后再试");
            }
            catch (Exception e)
            {
                lblAccountInfo.Dispatcher.Invoke(new showInfoDelegate(setShowInfo), "查询失败，请稍后再试");
                log.Write("error:FormUnicomStep02:query():"+e.Message);
            }
            finally
            {
                //隐藏动态图片
                loadlbl.Dispatcher.Invoke(new isShowLabelDelegate(isShowLabel), loadlbl, false);
            }


        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="value"></param>
        private void setShowInfo(string value)
        {
            this.lblAccountInfo.Text = value;
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
                if (queryThread != null)
                {
                    if (queryThread.IsAlive)
                        queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                }
            }catch(Exception ex)
            {
                log.Write("error：FormUnicomStep02：UserControl_Unloaded:" + ex.Message);
            }
        }
    }
}
