using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using wtPayBLL;
using wtPayDAL;



namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormMechineState.xaml 的交互逻辑
    /// </summary>
    public partial class FormMechineState : UserControl
    {
        //硬件测试线程
        Thread check = null;

        Thread sendStatu = null;
        //委托
        private delegate void setTextBlockTextDelegate(TextBlock textBlock, string value);

        Thread crt310Thread = null;
        bool isCrt310 = true;

        Thread crt603Thread = null;
        bool isCrt603 = true;

        Thread cj201Thread = null;
        bool isCj201 = true;

        Thread zt598Thread = null;
        bool isZt598 = true;

        Thread printThread = null;
        bool isPrint = true;
        public FormMechineState()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                isPrint = true;
                isCj201 = true;
                isCrt310 = true;
                isCrt603 = true;
                isZt598 = true;
                isSendStatu = true;
                crt310lbl.Text = "";
                printlbl.Text = "";
                cj201lbl.Text = "";
                crt603lbl.Text = "";
                zt598lbl.Text = "";
                lblshow.Text = "正在检测，请稍后...";
                check = new Thread(delegate () { checkState(); });
                check.Start();
            }catch(Exception ex)
            {
                log.Write("error:维护人员硬件检测线程异常："+ex.Message);
                return;
            }
        }
        private void checkState()
        {
            try
            {
                string str = DeviceState.SendCRT310State();
                if (str == null) setTextBlockText(crt310lbl, "正常"); else setTextBlockText(crt310lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));

                str = DeviceState.SendZT598State();
                if (str == null) setTextBlockText(zt598lbl, "正常"); else setTextBlockText(zt598lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));

                str = DeviceState.SendPrintState();
                if (str == null) setTextBlockText(printlbl, "正常"); else setTextBlockText(printlbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));

                str = DeviceState.SendCJ201State();
                if (str == null) setTextBlockText(cj201lbl, "正常"); else setTextBlockText(cj201lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));

                str = DeviceState.SendCRT603State();
                if (str == null) setTextBlockText(crt603lbl, "正常"); else setTextBlockText(crt603lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));

                showInfo("设备状态");
            }
            catch (Exception ex)
            {
                log.Write("error：设备检测异常：" + ex.Message);
                showInfo("设备检测异常");
            }
        }
        private string ExceptionInfo(string value)
        {
            for (int i = 0; i < SysBLL.maintainSignParam.MaintainSignInfo.data.Count; i++)
            {
                if (value.Equals(SysBLL.maintainSignParam.MaintainSignInfo.data[i].code_no))
                {
                    return SysBLL.maintainSignParam.MaintainSignInfo.data[i].code_des;
                }
            }
            return "";
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (isCrt310 == false) return;
                if (crt310Thread != null)
                {
                    crt310Thread.Abort();
                    crt310Thread.DisableComObjectEagerCleanup();
                    crt310Thread = null;
                }
                crt310Thread = new Thread(delegate() {
                    isCrt310 = false;
                    showInfo("正在检测,请稍后...");
                    //银联读卡器
                    Thread.Sleep(1000);
                    string str = DeviceState.SendCRT310State();
                    if (str == null) setTextBlockText(crt310lbl, "正常"); else setTextBlockText(crt310lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));
                    showInfo("设备状态");
                        isCrt310 = true;
                });
                crt310Thread.Start();
            }
            catch(ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch(Exception ex)
            {
                log.Write("error:"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isCrt603 == false) return;
                if (crt603Thread != null)
                {
                    crt603Thread.Abort();
                    crt603Thread.DisableComObjectEagerCleanup();
                    crt603Thread = null;
                }
                crt603Thread = new Thread(delegate ()
                {
                    isCrt603 = false;
                    showInfo("正在检测,请稍后...");
                    //公交读卡器
                    Thread.Sleep(1000);
                    string str = DeviceState.SendCRT603State();
                    if (str == null) setTextBlockText(crt603lbl, "正常"); else setTextBlockText(crt603lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));
                    showInfo("设备状态");
                    isCrt603 = true;
                });
                crt603Thread.Start();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try {
                if (isCj201 == false) return;
                if (cj201Thread != null)
                {
                    cj201Thread.Abort();
                    cj201Thread.DisableComObjectEagerCleanup();
                    cj201Thread = null;
                }
                cj201Thread = new Thread(delegate() {
                    isCj201 = false;
                    showInfo("正在检测,请稍后...");
                    //燃气读卡器
                    Thread.Sleep(1000);
                    string str = DeviceState.SendCJ201State();
                    if (str == null) setTextBlockText(cj201lbl, "正常"); else setTextBlockText(cj201lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));
                    showInfo("设备状态");
                    isCj201 = true;
                });
                cj201Thread.Start();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try {
                if (isZt598 == false) return;
                if (zt598Thread != null)
                {
                    zt598Thread.Abort();
                    zt598Thread.DisableComObjectEagerCleanup();
                    zt598Thread = null;
                }
                zt598Thread = new Thread(delegate()
                {
                    showInfo("正在检测,请稍后...");
                    //密码键盘
                    Thread.Sleep(1000);
                    string str = DeviceState.SendZT598State();
                    if (str == null) setTextBlockText(zt598lbl, "正常"); else setTextBlockText(zt598lbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));
                    showInfo("设备状态");
                });
                zt598Thread.Start();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try {
                if (isPrint == false) return;
                if (printThread != null)
                {
                    printThread.Abort();
                    printThread.DisableComObjectEagerCleanup();
                    printThread = null;
                }
                printThread = new Thread(delegate() { 
                showInfo("正在检测,请稍后...");
                //打印机
                Thread.Sleep(1000);
                string str = DeviceState.SendPrintState();
                if (str == null) setTextBlockText(printlbl, "正常"); else setTextBlockText(printlbl, "异常：" + str.Remove(0, 2) + " " + ExceptionInfo(str));
                showInfo("设备状态");
                });
                printThread.Start();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }
        bool isSendStatu = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isSendStatu == false) return;
            if ("正常".Equals(crt310lbl.Text) && "正常".Equals(zt598lbl.Text) && "正常".Equals(printlbl.Text) && "正常".Equals(cj201lbl.Text) && "正常".Equals(crt603lbl.Text)) {
                sendStatu = new Thread(delegate () {
                    try
                    {
                        isSendStatu = false;
                        DeviceState.SendStatu();
                        showInfo("监控平台状态：" + MaintainSignAccess.upException(SysBLL.maintainSignParam.Account));

                        isSendStatu = true;
                    }
                    catch (Exception ex) { log.Write("error：维护人员提交修复异常：" + ex.Message); showInfo("监控平台状态：" + "提交失败"); isSendStatu = true; }
                });
                sendStatu.Start();
            }
            else
            {
                showInfo("监控平台状态：" + "异常未处理");
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try {
                sendStatu.Abort();
                sendStatu.DisableComObjectEagerCleanup();
                sendStatu = null;
                check.Abort();
                check.DisableComObjectEagerCleanup();
            }catch(Exception ex) { log.Write("error:维护人员硬件检测关闭窗体时异常:"+ex.Message); }
        }
        private void showInfo(string value)
        {
            lblshow.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblshow, value);
        }

        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText2),textBlock,value);
        }
        private void setTextBlockText2(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        [DllImport("user32.dll ", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,int X, int Y, int cx, int cy, int uFlags);

        
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormMechineTemp");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                //如果有卡则退卡
                MachCardBLL.backCard();
                //禁止用户插卡
                MachCardBLL.CancelWaitCard();
            }catch(Exception ex)
            {
                log.Write("error:维护人员签到页面退卡异常："+ex.Message);
            }
        }
    }
}
