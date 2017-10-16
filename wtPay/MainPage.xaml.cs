using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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
using wtPay.Util;
using wtPayBLL;
using wtPayCommon;
using wtPayModel;
using wtPayModel.ConfigModel;

namespace wtPay
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : UserControl
    {
        Thread loadThread = null;

        Thread gcWindowThread = null;
        public MainPage()
        {
            InitializeComponent();
        }
        //甘肃一卡通
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "4";
            if (EnableButton.WantonEnable)
                JumpUtil.jumpCommonPage("formCitizenStep01");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void PictureChangeUserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void test(object sender, RoutedEventArgs e)
        {
            //Messag6eBox.Show("测试开始");
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "26";
            if (EnableButton.mobileEnable)
                JumpUtil.jumpCommonPage("FormMobileStep");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
       

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            SysBLL.IsOpenIndexForm = false;
            Write(@"D:\appliaction\IsRestart.txt", "0");
            Console.WriteLine(Read(@"D:\appliaction\IsRestart.txt"));
        }
        //电力缴费按钮
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "6";
            if (EnableButton.elecEnable)
                Util.JumpUtil.jumpCommonPage("FormElectricStep01");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "12";
            if (EnableButton.gongjijinEnable)
                Util.JumpUtil.jumpCommonPage("FormPublicFundInput");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load() { 
         try {
                ConfigSysParam.gifBusiness = GifBusiness.no;
                ComputerBLL.KillApplication("TabTip");
                //支付方式默认为电子现金
                PayStaticParam.payType = -1;
                //设置页面终端号 
                this.lblMechineNo.Text = "NO：" + ConfigurationManager.AppSettings["MechineNo"];
                SysBLL.PasswordErrorInfo = "重要提示";
                //软件版本号
                this.lblVersion.Text = SysConfigHelper.readerNode("currentVersion");
                loadThread = new Thread(delegate ()
                {
                //如果有卡则退卡
                MachCardBLL.backCard();
                //禁止用户插卡
                MachCardBLL.CancelWaitCard();
                });
                loadThread.Start();
                SysBLL.IsOpenIndexForm = true;
                //初始化公积金查询参数
                SysBLL.customerParam = null;
                SysBLL.customerParam = new wtPayModel.PublicFundModel.CustomerParam();
                SysBLL.socialSecurityParam = null;
                SysBLL.socialSecurityParam = new wtPayModel.SocialSecurityModel.SocialSecurityParam();
                BeiAnGGCardBLL.killRd();
                Write(@"D:\appliaction\IsRestart.txt", "1");
                Console.WriteLine(Read(@"D:\appliaction\IsRestart.txt"));

                if (SysBLL.isGcWindow == false)
                {
                    gcWindowThread = new Thread(delegate ()
                    {
                        try
                        {
                            while (true)
                            {
                                try
                                {
                                    int sleepNum = Convert.ToInt32(SysConfigHelper.readerNode("Gcmp4PlayIntervalName"));
                                    Thread.Sleep(sleepNum);
                               
                                    if (!System.IO.File.Exists("D:/payMedia/mp4/1.mp4"))
                                    {
                                        ComputerBLL.KillApplication("GCMp4");
                                        continue;
                                    }
                                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                                    if (("0".Equals(SysConfigHelper.readerNode("isStopGCMp4"))) || ("2".Equals(SysConfigHelper.readerNode("isStopGCMp4"))))
                                    {
                                        p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "GCMp4.exe";
                                        p.Start();
                                        SysConfigHelper.writerNode("isMainClose", "1");
                                    }
                                    while (true)
                                    {
                                        System.Diagnostics.Process[] proList = System.Diagnostics.Process.GetProcesses(".");//获得本机的进程
                                        bool iss = false;

                                        foreach (System.Diagnostics.Process pro in proList)
                                        {
                                            if ("GCMp4".Contains(pro.ProcessName))
                                            {
                                                iss = true;
                                                break;
                                            }
                                        }
                                        if ("1".Equals(SysConfigHelper.readerNode("isStopGCMp4")))
                                        {
                                            ComputerBLL.KillApplication("GCMp4");
                                        }
                                        if (iss == false) break;
                                        Thread.Sleep(1000 * 5);
                                    }
                                }
                                catch (ThreadAbortException ae) { log.Write("error:1loadGCMp4:" + ae.Message); return; }
                                catch (Exception ex)
                                {
                                    log.Write("error:视屏循环播放：" + ex.Message + ex.InnerException);
                                    continue;
                                }
                            }
                        }
                        catch (ThreadAbortException ae) { log.Write("error:2loadGCMp4:" + ae.Message); return; }
                        catch (Exception ex) { log.Write("error：2loadGCMp4：" + ex.Message); }
                    });
                    gcWindowThread.Start();
                }
                //隐藏Windows任务栏
                SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_HIDE);
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex) { log.Write("error：首页load事件异常："+ex.Message); }
    }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SysBLL.isGcWindow == false)
                {
                    ComputerBLL.KillApplication("GCMp4");
                    gcWindowThread.Abort();
                    gcWindowThread.DisableComObjectEagerCleanup();
                    gcWindowThread = null;
                }

                loadThread.Abort();
                loadThread.DisableComObjectEagerCleanup();
                loadThread = null;
            }
            catch (Exception ex) { log.Write("error:首页结束线程异常："+ex.Message); }
        }

        private void 社保_Click(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "11";
            if (EnableButton.shebaoEnable)
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityInput");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "25";
            Util.JumpUtil.jumpCommonPage("FormTemp");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "20";
            if (EnableButton.busEnable)
            Util.JumpUtil.jumpCommonPage("FormBusStep01");
            else
            Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "1";
            if (EnableButton.gasEnable)
            Util.JumpUtil.jumpCommonPage("FormGas");
            else
            Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "5";
            if (EnableButton.propEnable)
                Util.JumpUtil.jumpCommonPage("FormPropStep");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "15";
            //政务信息
            if (EnableButton.zhengwuEnable)
                Util.JumpUtil.jumpCommonPage("FormNewsList");
            else
                Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "7";
            if (EnableButton.broadEnable)
            Util.JumpUtil.jumpCommonPage("FormBroadCasStep01");
            else
                JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "8";
            if (EnableButton.realEnable)
            Util.JumpUtil.jumpCommonPage("FormHeatStep01");
            else
            Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {

            GcManage.gcType = "2";
            if (EnableButton.waterEnable)
                Util.JumpUtil.jumpCommonPage("FormWaterStep01");
            else
                Util.JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "16";
            //预约挂号
            if (EnableButton.guahaoEnable)
            Util.JumpUtil.jumpCommonPage("FormRegistration");
            else
            JumpUtil.jumpCommonPage("FormNot");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            GcManage.gcType = "13";
            Util.JumpUtil.jumpCommonPage("FormExpressType");
            //Util.JumpUtil.jumpCommonPage("FormNot");
        }
        public String Read(string path)
        {
            try
            {
                String line = String.Empty;
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    line = sr.ReadLine();
                }
                return line;
            }
            catch (IOException io) { return ""; }
            catch(Exception ex) { return ""; }

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
            catch (IOException io) {  }
            catch (Exception ex) {  }

        }

        private void PictureChangeUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show(bgimg.Height + "  " + bgimg.Width);                                                                                                                                                                                                                                         
        }
    }
}
