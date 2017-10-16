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
using wtPayBLL;

namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormPropStep06.xaml 的交互逻辑
    /// </summary>
    public partial class FormNetTest : UserControl
    {
        Thread netTestThread = null;

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        public FormNetTest()
        {
            InitializeComponent();
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.FontSize = 20;
            textBlock.Text = value.Replace("10.88.240.2", "服务器");
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormTemp");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblShowInfo.FontSize = 30;
                lblShowInfo.Text = "网络状态：正在检测...";
                netTestThread = new Thread(delegate() { netTest(); });
                netTestThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:网络测试异常：load():"+ex.Message);
            }
        }
        private void netTest()
        {
            try
            {
                lblShowInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.lblShowInfo, SysBLL.RunCmd("ping 10.88.240.2"));
            }
            catch(ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch (Exception ex) { log.Write("error：网络测试异常：" + ex.Message); }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                netTestThread.Abort();
                netTestThread.DisableComObjectEagerCleanup();
                netTestThread = null;
            }catch(Exception ex)
            {
                log.Write("error:网络测试异常：UserControl_Unloaded：" + ex.Message);
            }
        }
    }
}
