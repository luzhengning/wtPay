using InspectUpdate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using wtPay.GeneralForm;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayDAL.GCResource;
using wtPayDAL.Pay;
using wtPayDAL.SysAccessDAL;
using wtPayModel;
using wtPayModel.PromptModel;
using wtPayModel.PropModel;
using wtPayModel.WintopModel;

namespace wtPay
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /*
         * 去阿里巴巴是不可能的，这辈子都不可能去的，小公司工资低，普通公司又没前途，
         * 只能上维拓智能这样子，维拓智能里面个个都是人才，说话又好听我超喜欢这里的。
         *  Author:luzhengning
         *  Date:2017-10-9
         */
        public static FormLoad frmload = null;//启动初始化页面
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger("------------------------------------------------AppStart!!!");
            //启动初始化页面
            ComputerBLL.StartApp(System.AppDomain.CurrentDomain.BaseDirectory + "FormPage.exe");
            //正式OR测试
            SysBLL.IsTest = SqlLiteHelper.SqlLiteHelper.query("isTest")[0].FormalValue;
            //显示任务栏
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_RESTORE);
            try
            {
                //设置全局异常
                exception();
                //外部配置文件，广告文件处理
                ExternalHandle();
                //驱动管理
                DriveHandle();
                //初始化缴费按钮
                InitialiseButton();
                //程序初始化
                Initialize();
                //输出程序日志
                PrintLog();
                //打开主页面
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                logger("error：程序入口函数异常：" + ex.Message + " " + ex.InnerException + ",请重启程序。");
            }
            
            
        }
        /// <summary>
        /// 输出程序日志
        /// </summary>
        private void PrintLog()
        {
            logger("缴费程序环境："+SysBLL.IsTest);
        }
        /// <summary>
        /// 外部配置文件广告文件处理
        /// </summary>
        private void ExternalHandle()
        {
            try
            {
                //广告ID文件是否存在
                GcManage.GCIdFileIsExits();
                //更新更新程序
                updateApp();
                //广告是否更新成功
                GcManage.GCStateIdFileIsExits();
                //视频广告是否存在，并复制到D盘
                GcManage.GcMp4FileIsExists();
                //首页轮播图是否存在，并复制到D盘
                GcManage.GcMainImgFileIsExists();
                //左侧轮播图路径是否存在，并复制到D盘
                GcManage.GcLeftImgFileIsExists();
                //音量文件是否存在
                GcManage.YinliangFileIsExits();
                //服务器命令ID文件是否存在
                ComputerBLL.OrderIDFileIsExits();
                //更新程序是否启动缴费程序
                resgitIsExits();
                //视频声音10%
                GcManage.WriteYinliangValue("5");
            }
            catch (Exception ex)
            {
                logger("error:文件初始化：" + ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// 驱动管理(DLL)
        /// </summary>
        private void DriveHandle()
        {
            //初始化燃气驱动
            InitGasCard();
            //测试公交读卡器
            testCRT603();
            //初始化识币器
            InitializeCashRMB();
            try
            {
                logger("开始复制乱七八糟的驱动！");
                //主程序所在路径
                string mainPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //更新程序所在路径
                string updatePath = System.Environment.CurrentDirectory + "\\";
                string CardReaderPath = updatePath + "CardReader.dll";
                string TTPortPath = updatePath + "TTPort.dll";
                string TTReaderCardPath = updatePath + "TTReaderCard.dll";
                string JsonPath = updatePath + "Newtonsoft.Json.dll";
                string TransitionalsPath = updatePath + "Transitionals.dll";
                string WMPLibPath = updatePath + "Interop.WMPLib.dll";

                if (!File.Exists(CardReaderPath))
                    System.IO.File.Copy(mainPath + "CardReader.dll", CardReaderPath);
                if (!File.Exists(TTPortPath))
                    System.IO.File.Copy(mainPath + "TTPort.dll", TTPortPath);
                if (!File.Exists(TTReaderCardPath))
                    System.IO.File.Copy(mainPath + "TTReaderCard.dll", TTReaderCardPath);
                if (!File.Exists(JsonPath))
                    System.IO.File.Copy(mainPath + "Newtonsoft.Json.dll", JsonPath);
                if (!File.Exists(TransitionalsPath))
                    System.IO.File.Copy(mainPath + "Transitionals.dll", TransitionalsPath);
                if (!File.Exists(WMPLibPath))
                    System.IO.File.Copy(mainPath + "Interop.WMPLib.dll", WMPLibPath);

                /////////////////物业读卡器文件//////////////////
                if (!File.Exists(updatePath + "wyzh.exp"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\wyzh.exp", updatePath + "wyzh.exp");

                if (!File.Exists(updatePath + "wyzh.lib"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\wyzh.lib", updatePath + "wyzh.lib");

                if (!File.Exists(updatePath + "wyzh.ocx"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\wyzh.ocx", updatePath + "wyzh.ocx");
                if (!File.Exists(updatePath + "Rd.exe"))
                {
                    System.IO.File.Copy(mainPath + "BAGG\\Rd.exe", updatePath + "Rd.exe");
                }
                if (!File.Exists(updatePath + "Rd.exe.config"))
                    System.IO.File.Copy(mainPath + "BAGG\\Rd.exe.config", updatePath + "Rd.exe.config");

                if (!File.Exists(updatePath + "AxInterop.ELECTREADER01Lib.dll"))
                    System.IO.File.Copy(mainPath + "BAGG\\AxInterop.ELECTREADER01Lib.dll", updatePath + "AxInterop.ELECTREADER01Lib.dll");
                if (!File.Exists(updatePath + "AxInterop.wyzh.dll"))
                    System.IO.File.Copy(mainPath + "BAGG\\AxInterop.wyzh.dll", updatePath + "AxInterop.wyzh.dll");
                if (!File.Exists(updatePath + "Interop.ELECTREADER01Lib.dll"))
                    System.IO.File.Copy(mainPath + "BAGG\\Interop.ELECTREADER01Lib.dll", updatePath + "Interop.ELECTREADER01Lib.dll");
                if (!File.Exists(updatePath + "Interop.wyzh.dll"))
                    System.IO.File.Copy(mainPath + "BAGG\\Interop.wyzh.dll", updatePath + "Interop.wyzh.dll");
                if (!File.Exists(updatePath + "beianGG.xml"))
                    System.IO.File.Copy(mainPath + "BAGG\\beianGG.xml", updatePath + "beianGG.xml");
                if (!File.Exists(updatePath + "0001.bat"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\0001.bat", updatePath + "0001.bat");
                if (!File.Exists(updatePath + "CardRW.dll"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\CardRW.dll", updatePath + "CardRW.dll");
                if (!File.Exists(updatePath + "ELECTR~1.oca"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\ELECTR~1.oca", updatePath + "ELECTR~1.oca");
                if (!File.Exists(updatePath + "ElectReader01.ocx"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\ElectReader01.ocx", updatePath + "ElectReader01.ocx");
                if (!File.Exists(updatePath + "MCS_SR.dll"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\MCS_SR.dll", updatePath + "MCS_SR.dll");
                if (!File.Exists(updatePath + "Mwic_32.dll"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\Mwic_32.dll", updatePath + "Mwic_32.dll");
                if (!File.Exists(updatePath + "wyzh.bat"))
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\wyzh.bat", updatePath + "wyzh.bat");

                if (!File.Exists(updatePath + "00010001"))
                {
                    System.IO.Directory.CreateDirectory(updatePath + "00010001");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\0001.bat", updatePath + "00010001\\0001.bat");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\CardRW.dll", updatePath + "00010001\\CardRW.dll");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\ELECTR~1.oca", updatePath + "00010001\\ELECTR~1.oca");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\ElectReader01.ocx", updatePath + "00010001\\ElectReader01.ocx");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\MCS_SR.dll", updatePath + "00010001\\MCS_SR.dll");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\Mwic_32.dll", updatePath + "00010001\\Mwic_32.dll");
                    System.IO.File.Copy(mainPath + "BeiAnGGOcx\\00010001\\wyzh.bat", updatePath + "00010001\\wyzh.bat");
                }
                logger("乱七八糟的驱动复制完毕！");
            }catch(Exception ex)
            {
                logger("乱七八糟的驱动复制异常："+ex.Message);
            }
        }
        /// <summary>
        /// 初始化纸币器驱动
        /// </summary>
        private void InitializeCashRMB()
        {
            StringBuilder info = new StringBuilder(1024);
            int resultCode = CashRMB.TT_OpenDevice(new StringBuilder(SysConfigHelper.readerNode("CashRMBPort")), new StringBuilder("9600"), info);
            if (resultCode == 0)
            {
                PayStaticParam.isHaveRMB = true;
                PayStaticParam.RmbIsOpen = true;
                Thread.Sleep(5000);
                DeviceState.sendRmdStatu();
                logger("识币器识别成功：" + resultCode);
            }
            else
            {
                PayStaticParam.isHaveRMB = false;
                PayStaticParam.RmbIsOpen = false;
                logger("识币器识别失败：" + resultCode);
            }
        }
        /// <summary>
        /// 程序初始化
        /// </summary>
        private  void Initialize()
        {
            try
            {

                //if (!InspectKit.Inspect(SysConfigHelper.readerNode("Inspect")))//对比失败
                //{
                //    //打开主页面
                //    MainWindow mainWindow = new MainWindow();
                //    mainWindow.Show();
                //    MainWindow.Margin = new Thickness(-10, -8, -12, -10);
                //    FormStop stop = new FormStop();
                //    stop.Margin = new Thickness(0, 0, 0, 0);
                //    MainWindow.Content = stop;
                //    return;
                //}
                
                //北岸公馆读卡器注册表
                SysBLL.RunBat(System.AppDomain.CurrentDomain.BaseDirectory + "\\BeiAnGGOcx\\00010001\\wyzh.bat");
                SysBLL.RunBat(System.AppDomain.CurrentDomain.BaseDirectory + "\\BeiAnGGOcx\\00010001\\0001.bat");
                //物业2
                SysBLL.RunBat(System.AppDomain.CurrentDomain.BaseDirectory + "\\perp2\\prop2.bat");
                //SysBLL.RunBat(System.AppDomain.CurrentDomain.BaseDirectory + "\\BeiAnGGOcx\\wyzh.bat");
                //是否需要启动其他程序
                bool startUpOtherProject = Convert.ToBoolean(SysConfigHelper.readerNode("startUpOtherApp"));
                if (startUpOtherProject)
                {
                    StartUpApp(SysConfigHelper.readerNode("startUpOtherAppPath"));
                }
                //关闭广告屏
                killGcMp4();
                //获取缴费页面提示
                SysBLL.payPromptInfo = PayPromptAccess.queryPayPrompt(null);
                SysBLL.ShowCursor(SysBLL.IsShowCursor);
                //设置桌面背景
                SysBLL.setImagePage();
                //天气查询状态
                SysBLL.WeatherQuerystate = 1;
                //禁止用户插卡
                MachCardBLL.CancelWaitCard();
                //初始化发送设备状态时间
                SysBLL.SendDeviceStatuTime = DateTime.Now;
                if (SysBLL.IsTest.Equals("正式"))
                {
                    //广告ID
                    GCResourceAccess.adv_id = GcManage.ReadGCID();
                    //指令id
                    SystemOrderAccess.id = ComputerBLL.ReadOrderID();
                }else
                {
                    //广告ID
                    GCResourceAccess.adv_id = SysConfigHelper.readerNode("GcAdv_id");
                    //指令id
                    SystemOrderAccess.id = SysConfigHelper.readerNode("sysOrderId");
                }
                //电动读卡器如果有卡则吞卡
                //MachCardBLL.swallowCard();

            }
            catch(Exception ex) { logger("error:程序初始化异常：" + ex.Message); }
        }

        /// <summary>
        /// 初始化燃气读卡器类型
        /// </summary>
        public static void InitGasCard()
        {
            string mainPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string Path = mainPath + "CJ201api.dll";
            string registXFPath = mainPath + "xianfengCOM\\CJ201api.dll";
            logger("开始初始化燃气读卡器驱动");
            string result = GasBLL.GasJudge();
            try
            {
                if ("06".Equals(result))
                {
                    //CRT
                    if (File.Exists(Path)) File.Delete(Path);
                    System.IO.File.Copy(mainPath + "GasDll\\CRTDLL\\CJ201api.dll", Path);
                    if (File.Exists(registXFPath)) File.Delete(registXFPath);
                    System.IO.File.Copy(mainPath + "GasDll\\CRTDLL\\CJ201api.dll", registXFPath);
                    SysBLL.isGasCRT = true;
                    logger("燃气读卡器：CRT");
                }
                else
                {
                    //GG
                    if (File.Exists(Path)) File.Delete(Path);
                    System.IO.File.Copy(mainPath + "GasDll\\GGDLL\\CJ201api.dll", Path);
                    if (File.Exists(registXFPath)) File.Delete(registXFPath);
                    System.IO.File.Copy(mainPath + "GasDll\\GGDLL\\CJ201api.dll", registXFPath);
                    SysBLL.isGasCRT = false;
                    logger("燃气读卡器：GG");
                }
            }
            catch (TimeoutException tx)
            {
                logger("燃气读卡器数据读取超时，默认设置国光燃气读卡器驱动");
                //超时
                //GG
                if (File.Exists(Path)) File.Delete(Path);
                System.IO.File.Copy(mainPath + "GasDll\\GGDLL\\CJ201api.dll", Path);
                if (File.Exists(registXFPath)) File.Delete(registXFPath);
                System.IO.File.Copy(mainPath + "GasDll\\GGDLL\\CJ201api.dll", registXFPath);
                SysBLL.isGasCRT = false;
            }
            catch (Exception ex)
            {
                logger("error:燃气读卡器数据读取异常："+ex.Message);
            }
            
        }

        /// <summary>
        /// 广告视频是否打开
        /// </summary>
        private void killGcMp4()
        {
            Process[] process = Process.GetProcesses();
            foreach (Process prc in process)
            {
                if (prc.ProcessName.Contains("GCMp4"))
                    prc.Kill();
            }
        }
        /// <summary>
        /// 测试公交读卡器
        /// </summary>
        /// <returns></returns>
        private bool testCRT603()
        {
            string port = SysConfigHelper.readerNode("CRT603Port");//
            int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);
            if (openRet != 0) { logger("error:公交读卡器打开失败！"); return false; }
            return true;
        }

        /// <summary>
        ///  绑定异常 
        /// </summary>
        private void exception()
        {
            //处理未捕获的异常   
            System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.ThrowException);
            //处理UI线程异常   
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// 启动监控程序
        /// </summary>
        /// <param name="appPath"></param>
        private static void StartUpApp(string appPath)
        {

            try
            {
                Process[] process = Process.GetProcesses();
                foreach (Process prc in process)
                {
                    if (prc.ProcessName == "InletWindow")
                        prc.Kill();
                    if (prc.ProcessName == "ProcessApp")
                        prc.Kill();
                }

                string win_64Path = @"C:\Program Files\UpdateProject";
                string win_86Path = @"C:\Program Files (x86)\UpdateProject";
                if (Directory.Exists(win_64Path))
                {
                    Directory.Delete(win_64Path, true);
                }
                else if (Directory.Exists(win_86Path))
                {
                    Directory.Delete(win_86Path, true);
                }
                if (File.Exists(appPath))
                {
                    Process.Start(appPath);
                    //Application.Exit();
                }
            }
            catch (Exception)
            {
            }
            //if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\ProcessApp.exe"))
            //{
            //    File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + @"\ProcessApp.exe");
            //}

        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                logger("Error:Application_ThreadException：" + ex.Message);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                logger("Error:CurrentDomain_UnhandledException:" + ex.Message + Environment.NewLine + ex.InnerException.ToString());
                ComputerBLL.Restart();
            }
        }
        /// <summary>
        /// 初始化缴费业务按钮
        /// </summary>
        private void InitialiseButton()
        {
            EnableButton.mobileEnable = checkBoxEnable("mobileEnable");
            EnableButton.WantonEnable = checkBoxEnable("WantonEnable");
            EnableButton.busEnable = checkBoxEnable("busEnable");
            EnableButton.gasEnable = checkBoxEnable("gasEnable");
            EnableButton.elecEnable = checkBoxEnable("elecEnable");
            EnableButton.waterEnable = checkBoxEnable("waterEnable");
            EnableButton.broadEnable = checkBoxEnable("broadEnable");
            EnableButton.shebaoEnable = checkBoxEnable("shebaoEnable");
            EnableButton.gongjijinEnable = checkBoxEnable("gongjijinEnable");
            EnableButton.guahaoEnable = checkBoxEnable("guahaoEnable");
            EnableButton.jiaotonEnable = checkBoxEnable("jiaotonEnable");
            EnableButton.zhengwuEnable = checkBoxEnable("zhengwuEnable");
            EnableButton.realEnable = checkBoxEnable("realEnable");
            EnableButton.propEnable = checkBoxEnable("propEnable");

            EnableButton.propHouse = checkBoxEnable("propHouse");
            EnableButton.propPark = checkBoxEnable("propPark");
            EnableButton.xiaoquWater = checkBoxEnable("xiaoquWater");
            EnableButton.xiaoquElec = checkBoxEnable("xiaoquElec");
    }
        private bool checkBoxEnable(string name)
        {
            if ("1".Equals(SqlLiteHelper.SqlLiteHelper.query(name)[0].FormalValue))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 缴费程序是否重启,该文件由更新程序判断，更新程序执行重启操作 1表示缴费程序未使用，0表示正在使用
        /// </summary>
        public void resgitIsExits()
        {
            if (!Directory.Exists("D:\\appliaction")) Directory.CreateDirectory("D:\\appliaction");
            string filePath = @"D:\\appliaction\\IsRestart.txt";
            if (System.IO.File.Exists(filePath)) return;
            File.Create(filePath).Dispose();
        }
        /// <summary>
        /// 更新更新程序
        /// </summary>
        public static void updateApp()
        {
            try
            {
                string updatePath = System.Environment.CurrentDirectory;
                logger("更新程序路径："+updatePath);
                //确定更新程序版本
                if (!updatePath.Contains("3.1"))
                {
                    logger("开始更新更新程序");
                    string configPath = @"D:\appliaction\UpdateProjectSharding\InletWindow.exe.config";
                    string dir = System.AppDomain.CurrentDomain.BaseDirectory + "UpdateInletWindow";
                    string path = @"D:\appliaction\UpdateProjectTV-G3.1\InletWindow.exe";
                    string newdir = @"D:\appliaction\UpdateProjectTV-G3.1";
                    if (System.IO.Directory.Exists(newdir))
                    {
                        System.IO.Directory.Delete(newdir);
                    }
                    System.IO.Directory.CreateDirectory(newdir);

                    CopyFolder(dir, newdir);

                    File.Copy(configPath, newdir + @"\InletWindow.exe.config");
                    string copy = System.AppDomain.CurrentDomain.BaseDirectory + @"InletWindow.exe";
                    
                    ComputerBLL.StartApp(path);
                    Thread.Sleep(1000 * 10);
                    ComputerBLL.Restart();
                }
            }
            catch (Exception ex)
            {
                logger("error:updateApp：" + ex.Message);
            }
        }
        /// <summary> 
        /// 复制文件夹中的所有文件夹与文件到另一个文件夹
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        public static void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        logger("error:CopyFolder：" + ex.Message + ex.InnerException);
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                try
                {
                    //获得源文件下所有文件
                    List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                    files.ForEach(c =>
                    {
                        string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                        File.Copy(c, destFile, true);//覆盖模式
                    });
                    //获得源文件下所有目录文件
                    List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                    folders.ForEach(c =>
                    {
                        string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                        //采用递归的方法实现
                        CopyFolder(c, destDir);
                    });
                }
                catch (Exception ex)
                {
                    logger("error:CopyFolder：" + ex.Message + ex.InnerException);
                    Thread.Sleep(2000);
                    CopyFolder(sourcePath, destPath);
                }
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }
        private static void logger(string value)
        {
            log.Write("Start => "+value);
        }

    }
}
