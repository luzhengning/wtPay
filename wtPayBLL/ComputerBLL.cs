using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using wtPayModel.SystemModel;
using CoreAudioApi;
using System.IO;
using System.Diagnostics;

namespace wtPayBLL
{
    public class ComputerBLL
    {
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        [DllImport("user32")]
        public static extern void LockWorkStation();
        [DllImport("user32")]
        public static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
        public enum MonitorState
        {
            MonitorStateOn = -1,
            MonitorStateOff = 2,
            MonitorStateStandBy = 1
        }

        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);
        /// <summary>
        /// 关机
        /// </summary>
        public static void ShutDown()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo("shutdown.exe", "-s -t 00");
                System.Diagnostics.Process.Start(startinfo);
            }
            catch { }
        }
        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo("shutdown.exe", "-r -t 00");
                System.Diagnostics.Process.Start(startinfo);
            }
            catch { }
        }

        public static void Restart(int s)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo("shutdown.exe", "-r -t ");
                System.Diagnostics.Process.Start(startinfo);
            }
            catch { }
        }
        public static void LogOff()
        {
            try
            {
                ExitWindowsEx(0, 0);
            }
            catch { }
        }
        /// <summary>
        /// 锁定电脑
        /// </summary>
        public static void LockPC()
        {
            try
            {
                LockWorkStation();
            }
            catch { }
        }
        /// <summary>
        /// 显示
        /// </summary>
        public static void Turnoffmonitor()
        {
            SetMonitorInState(MonitorState.MonitorStateOff);
        }
        private static void SetMonitorInState(MonitorState state)
        {
            SendMessage(0xFFFF, 0x112, 0xF170, (int)state);
        }
        /// <summary>
        /// 关闭指定程序
        /// </summary>
        public static void KillApplication(string appName)
        {
            try
            {
                System.Diagnostics.Process[] proList = System.Diagnostics.Process.GetProcesses(".");//获得本机的进程
                foreach (System.Diagnostics.Process pro in proList)
                {
                    if (appName.Contains(pro.ProcessName))
                    {
                        pro.Kill();
                    }
                }
            }catch(Exception ex) { log.Write("error:killapp:" + ex.Message); }
        }
        /// <summary>
        /// 启动指定的程序
        /// </summary>
        /// <param name="appPath"></param>
        public static void StartApp(string appPath)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = appPath;
            p.Start();
        }
        /// <summary>
        /// 音量上
        /// </summary>
        public static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 音量下
        /// </summary>
        public static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 静音
        /// </summary>
        public static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void SetVol(int arg)
        {
            for (int i = 1; i <= 50; i++)
            {
                //减小音量
                VolumeDown();
            }
            for (int i = 1; i <= arg; i++)
            {
                //增加音量
                VolumeUp();
            }
        }
        /*
         * 弹出系统音量控制器
         * */
        public static void PopupController()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.FileName = "SndVol";
            Process.Start(Info);
        }
        //private static MMDevice device;
        //private static MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
        ///<summary>  
        ///直接删除指定目录下的所有文件及文件夹(保留目录)
        ///</summary>  
        ///<param name="strPath">文件夹路径</param> 
        ///<returns>执行结果</returns> 
        public static void DeleteDir(string strPath)
        {
            try
            {
                // 清除空格  
                strPath = @strPath.Trim().ToString();
                // 判断文件夹是否存在  
                if (System.IO.Directory.Exists(strPath))
                {
                    // 获得文件夹数组 
                    string[] strDirs = System.IO.Directory.GetDirectories(strPath);
                    // 获得文件数组  
                    string[] strFiles = System.IO.Directory.GetFiles(strPath);
                    // 遍历所有子文件夹  
                    foreach (string strFile in strFiles)         
                    { 
                        // 删除文件夹  
                        System.IO.File.Delete(strFile);
                    } // 遍历所有文件 
                    foreach (string strdir in strDirs)              
                    {
                        // 删除文件  
                        System.IO.Directory.Delete(strdir, true);      
                    }
                }
            }  catch (Exception Exp) // 异常处理  
            { // 异常信息 
                System.Diagnostics.Debug.Write(Exp.Message.ToString()); 
            }
        }
        public static bool executeOrder(SystemOrder order)
        {
            try {
                switch (order.data.cmd_no)
                {
                    case
                        "1011"://重启电脑
                        log.Write("重启电脑");
                        Restart();
                        break;
                    case
                        "2011"://缴费系统停用
                        SysBLL.isDisableApp = true;
                        break;
                    case
                        "2021"://吞卡
                        break;
                    case
                        "2022"://退卡
                        break;
                    case
                        "2031"://检查所有设备硬件
                        break;
                    case
                        "2041"://关闭广告程序
                        KillApplication("GCMp4");
                        break;
                    case
                        "2042"://关闭监控程序
                        KillApplication("1413241343241234123");
                        break;
                    case
                        "2043"://关闭更新程序
                        KillApplication("1324123413241324132");
                        break;
                    case
                        "2051"://重启缴费程序
                        ShutDown();
                        break;
                    case
                        "2052"://重启广告播放程序
                        KillApplication("GCMp4");
                        StartApp(System.AppDomain.CurrentDomain.BaseDirectory + "GCMp4.exe");
                        break;
                    case
                        "2053"://重启更新程序
                        StartApp("");
                        break;
                    case
                        "2061"://上传日志
                        if (order.data.cmd_additional != null)
                        {
                            SysConfigHelper.writerNode("Gcmp4PlayIntervalName", (Convert.ToInt32(order.data.cmd_additional) * (1000*60)).ToString());
                        }
                        break;
                    case
                        "2071"://静音
                        Mute();
                        break;
                    case
                        "2072"://音量
                        GcManage.WriteYinliangValue(Convert.ToInt32(order.data.cmd_additional).ToString());
                        KillApplication("GCMp4");
                        StartApp(System.AppDomain.CurrentDomain.BaseDirectory + "GCMp4.exe");
                        break;
                    case
                        "2081"://删除指定文件
                        System.IO.File.Delete(order.data.cmd_additional);
                        break;
                    case
                        "2091"://暂停视频广告
                        SysConfigHelper.writerNode("isStopGCMp4","1");
                        break;
                    case
                        "2092"://播放视频广告
                        SysConfigHelper.writerNode("isStopGCMp4","2");
                        break;
                }
                return true;
            }catch(Exception ex)
            {
                log.Write("error:命令执行异常："+ex.Message+"，命令编号："+order.data.cmd_no);
                return false;
            }
        }
        public static string ReadOrderID()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//OrderID.txt";
            return File.ReadAllText(filePath);
        }
        public static void WriteOrderID(string value)
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//OrderID.txt";
            File.WriteAllText(filePath, value);
        }
        /// <summary>
        /// 服务器命令ID文件是否存在
        /// </summary>
        public static void OrderIDFileIsExits()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//OrderID.txt";
            if (System.IO.File.Exists(filePath)) return;

            File.Create(filePath).Dispose();
            File.WriteAllText(filePath, "0");
        }
    }
}