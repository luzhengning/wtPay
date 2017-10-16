using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using wtPayModel;

namespace wtPay.FormMaintainSign
{
    /// <summary>
    /// FormMechineState.xaml 的交互逻辑
    /// </summary>
    public partial class FormMechineTemp : UserControl
    {
        private delegate void isOnlineDelegate(TextBlock text, string value);
        public FormMechineTemp()
        {
            InitializeComponent();
        }
        public void isOnline(TextBlock text,string value)
        {
            text.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_HIDE);
            //父页面监听此变量
            SysBLL.portNum = 11;
            Util.JumpUtil.jumpCommonPage("FormMechineState");
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            volumeTxt.Text = GcManage.readYinliangValue();
            lklTxt.Text = SysStateParam.lklSignInfo;
            wtTxt.Text = SysStateParam.wtSignInfo;
            onlineTxt.Text = "正在查询...";
            new Thread(delegate() { onlineTxt.Dispatcher.Invoke(new isOnlineDelegate(isOnline), onlineTxt, new TerminalAccess().isOnline()); }).Start();
        }
     
        bool isSendStatu = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            ComputerBLL.Restart();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }
        
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(log.getLogPath()))
                {
                    Process.Start(log.getLogPath());
                }
            }
            catch (IOException ioex) { }
            catch(Exception ex) { }
        }
        /// <summary>
        /// 退出缴费程序按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Write(@"D:\appliaction\IsRestart.txt", "1");
            Application.Current.MainWindow.Topmost = false;
            //显示Windows任务栏
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_RESTORE);
            SysBLL.portNum = 99;
            Environment.Exit(0);
        }
        public void Write(string path, string content)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        //开始写入
                        sw.Write(content);
                        //清空缓冲区
                        sw.Flush();
                        //关闭流
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            catch (IOException io) { }
            catch (Exception ex) { }

        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe");
            foreach (System.Windows.Forms.InputLanguage inputLanguage in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
            {
                //MessageBox.Show(inputLanguage.LayoutName);
                if (inputLanguage.LayoutName.Contains("谷歌"))
                {
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = inputLanguage;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //GcManage.WriteYinliangValue(volumeTxt.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int volume = Convert.ToInt32(volumeTxt.Text);
            if (volume <= 0)
            {
                volumeTxt.Text = "0";
                return;
            }
            volume--;
            volumeTxt.Text = volume.ToString();
            GcManage.WriteYinliangValue(volumeTxt.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int volume = Convert.ToInt32(volumeTxt.Text);
            if (volume >=50)
            {
                volumeTxt.Text = "50";
                return;
            }
            volume++;
            volumeTxt.Text = volume.ToString();
            GcManage.WriteYinliangValue(volumeTxt.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                //父页面监听此变量
                SysBLL.portNum = 10;
                ComputerBLL.KillApplication("SndVol");
                ComputerBLL.PopupController();
                SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_RESTORE);
            }
            catch(Exception ex)
            {
                log.Write("error:系统音量："+ex.Message);
            }
        }
    }
}
