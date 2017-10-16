using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading;
using wtPayBLL;
using wtPayModel.PublicFundModel;
using wtPayDAL;
using System.IO;
using System.Windows.Threading;

namespace wtPay.FormPublicFund
{
    /// <summary>
    /// FormPublicFundWait.xaml 的交互逻辑
    /// </summary>
    public partial class FormPublicFundWait : System.Windows.Controls.UserControl
    {

        //查询线程
        Thread queryThread = null;
        //查询结果
        Dictionary<string, object> paramList = new Dictionary<string, object>();
        //页面是否关闭
        bool isCloseForm = false;

        DispatcherTimer timerLoad;


        private delegate void labelTextDelegate(TextBlock label, string value);

        private void setLabelText(TextBlock label, string value)
        {
            label.Text = value;
        }
        public FormPublicFundWait()
        {
            InitializeComponent();
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPublicFund");
        }

        //窗体load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loaded();
            }
            catch (Exception ex)
            {
                log.Write("error:公积金查询异常：" + ex.Message);
            }
        }
        private void loaded()
        {
            SysBLL.Player("正在查询.wav");
            lblIsChargingTip.Text = "正在查询，请稍后...";
            isCloseForm = false;
            queryThread = new Thread(delegate () { Query(); });
            queryThread.Start();

            timerLoad = new DispatcherTimer();
            timerLoad.Interval = TimeSpan.FromMilliseconds(400);
            timerLoad.Tick += new EventHandler(timer_Tick);
            timerLoad.Start();
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
        public void Query()
        {
            try
            {
                paramList.Clear();
                query();
            }
            catch (ThreadAbortException ae)
            {
                log.Write("error:FormPublicFundWait:Query():" + ae.Message);
                return;
            }
            catch (Exception ex)
            {
                if (isCloseForm) return;
                log.Write("error:公积金查询异常：" + ex.Message);
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "查询失败，请稍后再试...");
                Thread.Sleep(1000 * 10);
                if (isCloseForm) return;
                Util.JumpUtil.jumpMainPage();
                return;
            }
        }
        public void query()
        {
            switch (SysBLL.customerParam.queryType)
            {
                case 1:
                    //个人客户信息查询
                    QueryCustomerInfo();
                    break;
                case 2:
                    // 个人公积金账户查询
                    QueryAccountInfo();
                    break;
                case 3:
                    // 公积金贷款余额查询
                    QueryLoanBalance();
                    break;
                case 4:
                    // 个人公积金明细查询
                    QueryDetailedInfo();
                    break;
                case 5:
                    //公积金贷款还款明细
                    QueryLoanDetail();
                    break;
                default:
                    throw new Exception();
            }
        }

        void QueryCustomerInfo()
        {
            CustomerInfo info = PublicFundAccess.QueryCustomerInfo(SysBLL.customerParam);
            if ("9999".Equals(info.recode))
            {
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            else if ((info.data == null) || ("false".Equals(info.success)))
            {
                if (info.msg != null)
                {
                    if (info.msg.Length > 0)
                    {
                        lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                    }
                    else lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");

                }
                else
                {
                    lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            //输出info
            paramList.Add("info", info);
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFundCustomerInfo", paramList); }));
        }

        void QueryAccountInfo()
        {
            PublicAccountInfo info = PublicFundAccess.QueryPublicAccountInfo(SysBLL.customerParam);
            if ("9999".Equals(info.recode))
            {
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFund"); }));
                return;
            }
            else if ((info.data == null) || ("false".Equals(info.success)))
            {
                if (info.msg != null)
                {
                    if (info.msg.Length > 0)
                    {
                        lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                    }
                    else lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                else
                {
                    lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFund"); }));
                return;
            }
            //输出info
            paramList.Add("info", info);
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFundAccountInfo", paramList); }));
        }

        void QueryLoanBalance()
        {
            LoanBalanceInfo info = PublicFundAccess.QueryLoanBalanceInfo(SysBLL.customerParam);
            if ("9999".Equals(info.recode))
            {
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            else if ((info.data == null) || ("false".Equals(info.success)))
            {
                if (info.msg != null)
                {
                    if (info.msg.Length > 0)
                    {
                        lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                    }
                    else lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                else
                {
                    lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            //输出info
            paramList.Add("info", info);
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicLoanBalanceInfo", paramList); }));
        }

        PublicDetailedParam dparam = new PublicDetailedParam();
        void QueryDetailedInfo()
        {

            dparam.Id = SysBLL.customerParam.Id;
            dparam.conName = SysBLL.customerParam.conName;
            dparam.pwd = SysBLL.customerParam.pwd;
            dparam.percode = SysBLL.customerParam.selcnt;
            PublicFundDetailedInfo info = PublicFundAccess.QueryPublicFundDetailedInfo(dparam);
            if ("9999".Equals(info.recode))
            {
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFund"); }));
                return;
            }
            else if ((info.data == null) || ("false".Equals(info.success)))
            {
                if (info.msg != null)
                {
                    if (info.msg.Length > 0)
                    {
                        lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                    }
                    else lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                else
                {
                    lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            //输出info
            paramList.Add("info", info);
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPublicFundDetailedInfo", paramList); }));
        }

        void QueryLoanDetail()
        {
            LoanDetailedInfo info = PublicFundAccess.QueryLoanDetailedInfo(SysBLL.customerParam);
            if ("9999".Equals(info.recode))
            {
                lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            else if ((info.data == null) || ("false".Equals(info.success)))
            {
                if (info.msg != null)
                {
                    if (info.msg.Length > 0)
                    {
                        lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, info.msg);
                    }
                    else lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                else
                {
                    lblIsChargingTip.Dispatcher.Invoke(new labelTextDelegate(setLabelText), lblIsChargingTip, "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
                return;
            }
            //输出info
            paramList.Add("info", info);
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormpublicLoanDetailedInfo", paramList); }));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                isCloseForm = true;
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;

                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch (Exception ex)
            {
                log.Write("公积金查询页结束线程时异常：" + ex.Message);
            }
        }
    }
}
