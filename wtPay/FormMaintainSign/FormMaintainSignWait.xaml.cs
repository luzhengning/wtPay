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
using wtPayModel;

namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormMaintainSignWait.xaml 的交互逻辑
    /// </summary>
    public partial class FormMaintainSignWait : UserControl
    {
        DispatcherTimer timerLoad;
        //查询线程
        Thread queryThread = null;
        //签到结果
        MaintainSignInfo maintainSignInfo = null;
        //委托
        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);

        private bool isCloseForm = false;
        public FormMaintainSignWait()
        {
            InitializeComponent();
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
         }
        private void load()
        {
            try {
                isCloseForm = false;
                lblIsChargingTip.Text = "正在签到，请稍后...";
                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();

                queryThread = new Thread(delegate () { query(); });
                queryThread.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormMaintainSignWait:load():"+ex.Message);
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
        private void query()
        {
            try
            {
                maintainSignInfo = MaintainSignAccess.MaintainSign(SysBLL.maintainSignParam.Account.ToLower(), SysBLL.maintainSignParam.Passsword.ToLower());
                SysBLL.maintainSignParam.MaintainSignInfo = maintainSignInfo;
                if ("0".Equals(maintainSignInfo.code))
                {
                    Util.JumpUtil.jumpCommonPage("FormMechineState");
                    return;
                }
                throw new Exception("维护人员签到失败");
            }
            catch (ThreadAbortException ae)
            {
                shoeInfo("查询失败，请稍后再试...");
                log.Write("error:维护人员签到等待页面："+ae.Message);
                Thread.Sleep(1000 * 4);
                if (isCloseForm) return;
                Util.JumpUtil.jumpCommonPage("FormTemp");
                return;
            }
            catch (Exception ex)
            {
                log.Write("error:维护人员签到异常：" + ex.Message);
                if (maintainSignInfo != null)
                {
                    if (maintainSignInfo.message != null)
                    {
                        shoeInfo(maintainSignInfo.message);
                    }else
                    {
                        shoeInfo("查询失败，请稍后再试...");
                    }
                }else
                {
                    shoeInfo("查询失败，请稍后再试...");
                }
                Thread.Sleep(1000 * 4);
                if (isCloseForm) return;
                Util.JumpUtil.jumpCommonPage("FormTemp");
            }
        }

        private void shoeInfo(string value)
        {
            lblIsChargingTip.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),lblIsChargingTip,value);
        }

        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                isCloseForm = true;
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;
            }
            catch (Exception ex)
            {
                log.Write("error:维护人员登录等待页面关闭页面时异常："+ex.Message);
                return;
            }
        }
    }
}
