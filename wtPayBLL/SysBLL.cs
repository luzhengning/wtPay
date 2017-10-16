using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Configuration;
using System.Xml;
using WMPLib;
using AnalysisBase;
using wtPayCommon;
using System.Threading;
using Microsoft.Win32;  //写入注册表时要用到
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using wtPayModel;
using wtPayModel.Mobile;
using wtPayModel.UnicomModel;
using wtPayModel.PublicFundModel;
using wtPayModel.ElecModel;
using wtPayModel.SocialSecurityModel;
using wtPayModel.WintopModel;
using wtPayModel.WaterModel;
using wtPayModel.GasModel;
using System.Security.Cryptography;
using wtPayModel.BroadCas;
using wtPayModel.HeatModel;
using wtPayModel.BusModel;
using wtPayModel.PayParamModel;
using wtPayModel.PropModel;
using wtPayModel.NewsModel;
using wtPayModel.PropSecModel;
using wtPayModel.PromptModel;

namespace wtPayBLL
{
    public class SysBLL
    {
        /// <summary>
        /// 测试Or正式环境（参数“正式”Or“测试”）
        /// </summary>
        public static string IsTest = "";
        /// <summary>
        /// 是否停用缴费程序
        /// </summary>
        public static bool isDisableApp = false;
        /// <summary>
        /// 电话缴费类型
        /// 1:联通
        /// 2:移动
        /// </summary>
        public static int MobilePayType = 1;
        

        /// <summary>
        /// 时政新闻查询参数
        /// </summary>
        public static NewsListInfoData newsListInfoData;


        public static string PasswordErrorInfo = "重要提示";

        public static string payTypeName = "";



        /// <summary>
        /// 公积金查询参数
        /// </summary>
        public static CustomerParam customerParam = null;
        /// <summary>
        /// 社保查询参数
        /// </summary>
        public static SocialSecurityParam socialSecurityParam = null;

        /// <summary>
        /// 维护人员登录参数
        /// </summary>
        public static MaintainSignParam maintainSignParam = new MaintainSignParam();

        /// <summary>
        /// 缴费类型
        /// 1.移动缴费
        /// 2.联通缴费
        /// </summary>
        public static int payCostType = 0;

        //天气属性
        public static string temp = "";
        public static string fengxiang = "";
        public static string AQI = "";
        public static string quality = "";
        public static string WeatherState = "";
        public static int WeatherQuerystate = 4;


        //后台发送设备状态服务
        public static BackgroundWorker SendDeviceWorker = null;

        //首页是否在显示
        public static bool IsOpenIndexForm = true;

        //燃气写卡状态
        public static string GasReadState = "";

        /// <summary>
        /// 支付卡类型
        /// </summary>
        public static int payCardType = -1;
        ///
        public static string payCardNo = "";

        /// <summary>
        /// 主页面监听码，用于子页面和父页面通信
        /// </summary>
        public static int portNum = -1;

        /// <summary>
        /// 银行卡信息
        /// </summary>
        public static Dictionary<string, string> IcBankDictionary = null;

        //密码输入超时
        public static int pwdTimer = 30;

        //捕获密码键盘密码时间间隔
        public static int pwdWhile = 20;

        //顶部广告是否打开
        public static bool isOpenGCMp4 = false;

        /// <summary>
        /// 缴费页面提示，后台获取
        /// </summary>
        public static PayPromptInfo payPromptInfo = null;

        //签到时间
        public static DateTime signTime;


        public static bool wangtonSignResult = false;
        public static bool lakalaSignResult = false;

        
        
        //设置签到
        public static void setSignTime()
        {
            try
            {
                //int hour = (DateTime.Now - SysBLL.signTime).Hours;
                if (DateTime.Now.Hour == 1)
                {
                    if (SysBLL.signTime.Day != DateTime.Now.Day)
                    {
                        //PasswordBLL pwdBLL = new PasswordBLL();
                        //StringBuilder info = new StringBuilder();
                        //ZT598.TT_DisableKeyboard(info);
                        log.Write("签到时间已到，开始签到，关闭键盘返回的状态：");
                        SysBLL.signTime = DateTime.Now;
                        LklWtSign();

                    }
                }
            }
            catch (Exception e) { log.Write("签到异常：" + e.Message); }
        }

        public static bool isGasCRT = false;
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static bool isGcWindow = false;
        /// <summary>
        /// 开启广告屏
        /// </summary>
        public static void setGCMp4()
        {

            int monitorCount = System.Windows.Forms.Screen.AllScreens.Count();
            if (monitorCount > 1)
            {
                if (SysBLL.isOpenGCMp4 == false)
                {
                    SysBLL.isOpenGCMp4 = true;
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "GCMp4.exe";
                    p.Start();
                    isGcWindow = true;//
                }
            }
        }


        public static int SW_HIDE = 0;  //隐藏任务栏

        public static string md5(string value)
        {
            byte[] result = Encoding.Default.GetBytes(value);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }



        public static Dictionary<string, string> MechineSign(string mechineNo)
        {
            Dictionary<String, String> signResult = new Dictionary<String, String>();
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("tId", mechineNo);
                string url = SysConfigHelper.readerNode("mechineSign");
                string jsonText = HttpHelper.getHttp(url, parameters, null);
                log.Write("设备签到返回："+jsonText);
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
                if ("1".Equals(jsonObj["status"].ToString()))
                {

                    string clientPayNos = jsonObj["clientPayNo"].ToString();
                    signResult.Add("wtClientPayNo", clientPayNos.Split('|')[0]);
                    signResult.Add("lklClientPayNo", clientPayNos.Split('|')[1]);
                    signResult.Add("status", "1");

                    SysConfigHelper helper = new SysConfigHelper();

                    SysConfigHelper.writerNode("LklClientNo", signResult["lklClientPayNo"]);//设置拉卡拉终端号
                    SysConfigHelper.writerNode("ClientNo", signResult["wtClientPayNo"]);//设置万通卡终端号
                                                                                        //SetAppConfig("LklClientNo", signResult["lklClientPayNo"]);
                                                                                        //SetAppConfig("ClientNo", signResult["wtClientPayNo"]);
                                                                                        //设置加载
                    SysFormLoad.loadBar("设备签到成功");
                }
                else
                {
                    signResult.Add("status", "0");

                }
            }
            catch (Exception e)
            {
                log.Write("设备签到异常：" + e.Message);
            }
            return signResult;
        }

        /// <summary>
        /// 打开窗体线程
        /// </summary>
        public static Thread openFormThread = null;

        public static Boolean IsShowCursor = false;

        public static int SW_RESTORE = 9;//显示任务栏

        /// <summary>
        /// 密码错误次数
        /// </summary>
        public static int PasswordError = 0;


        public static int WeatherHour = 0;


        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        public static DateTime SendDeviceStatuTime;

        public static string isSwallowCard;

        public static Application application;


        PasswordBLL pwdBLL = new PasswordBLL();

        /// <summary>
        /// 签到码
        /// </summary>
        public static string Authcode;

        /// <summary>
        /// byte[]数组转字符
        /// </summary>
        /// <param name="strByte"></param>
        /// <returns></returns>
        public static string byteToString(byte[] strByte)
        {
            System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
            String str = ASCII.GetString(strByte);
            return str;
        }
        static WindowsMediaPlayer player = null;
        public static void Player(string url)
        {
            try
            {
                if (player != null) player.controls.stop();
                player = new WindowsMediaPlayer();
                player.URL = System.AppDomain.CurrentDomain.BaseDirectory + "\\sysMp3\\" + url;
                player.controls.play();
            }
            catch(Exception ex) { }
        }
        /// <summary>
        /// 注册COM组件
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public static string RunCmd(string strCmd)
        {
            string rInfo;
            try
            {
                Process myProcess = new Process();
                ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("cmd.exe");
                myProcessStartInfo.UseShellExecute = false;
                myProcessStartInfo.CreateNoWindow = true;
                myProcessStartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo = myProcessStartInfo;
                myProcessStartInfo.Arguments = "/c " + strCmd;
                myProcess.Start();
                StreamReader myStreamReader = myProcess.StandardOutput;
                rInfo = myStreamReader.ReadToEnd();
                myProcess.Close();
                rInfo = strCmd + "\r\n" + rInfo;
                return rInfo;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private static XmlDocument xDoc = new XmlDocument();
        public static void SetAppConfig(string appKey, string appValue)
        {
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", appValue);
            else
            {
                var xNewElem = xDoc.CreateElement("add");
                xNewElem.SetAttribute("key", appKey);
                xNewElem.SetAttribute("value", appValue);
                xNode.AppendChild(xNewElem);
            }
            xDoc.Save(AppConfig());
        }
        public static string AppConfig()
        {
            int intPos = Application.StartupPath.Trim().IndexOf("bin") - 1;

            string strDirectoryPath = System.IO.Path.Combine(Application.StartupPath.Substring(0, intPos), "App.config");

            //string strDirectoryPath = "App.config";
            return strDirectoryPath;
        }
        /// <summary>
        /// 清除空格 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearBlank(string str)
        {
            //去掉两端空格
            str = str.Trim();
            //以空格切割
            string[] strArray = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //以空格连接
            string newStr = string.Join(" ", strArray);
            return newStr;

        }
        /// <summary>
        /// 更新连接字符串
        /// </summary>
        /// <param name="newName"> 连接字符串名称 </param>
        /// <param name="newConString"> 连接字符串内容 </param>
        /// <param name="newProviderName"> 数据提供程序名称 </param>
        private static void UpdateConnectionStringsConfig(string newName,
            string newConString,
            string newProviderName)
        {
            bool isModified = false;    // 记录该连接串是否已经存在
            // 如果要更改的连接串已经存在
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            // 新建一个连接字符串实例
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            // 打开可执行的配置文件*.exe.config
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        /// <summary>
        /// 获取当前系统时间
        /// </summary>
        /// <returns></returns>
        public static string getYYYYMMDDHHMMSSTime()
        {
            string year = DateTime.Now.Year.ToString();
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return year + month + day + hour + minute + second;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public static string getTimeFormat()
        {
            string year = DateTime.Now.Year.ToString();
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return year + "-" + month + "-" + day + "  " + hour + ":" + minute + ":" + second;
        }

        public static string getHHMMSSITime10()
        {
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            string time = hour + minute + second + DateTime.Now.Millisecond;
            if (time.Length < 10)
            {
                while (true)
                {
                    time = time + "0";
                    if (time.Length == 10)
                    {
                        break;
                    }
                }
            }
            return time;
        }
        public static string getHHMMSSITime8()
        {
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            string time = hour + minute + second;
            if (time.Length < 8)
            {
                while (true)
                {
                    time = time + "0";
                    if (time.Length == 8)
                    {
                        break;
                    }
                }
            }
            return time;
        }

        private static string TimeStrLenZero(string str)
        {
            int length = str.Length;
            if (length != 2)
            {
                str = "0" + str;
            }
            return str;
        }

        /// <summary>
        /// 获取CPU_ID
        /// </summary>
        /// <returns></returns>
        public static string getCpuNo()
        {
            //string cpuInfo = "";
            //ManagementClass mc = new ManagementClass("Win32_Processor");
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
            //    //break;
            //}
            //moc = null;
            //mc = null;
            //return cpuInfo;
            return getMac();
            //return SysConfigHelper.readerNode("LklClientNo"); ;
        }
        static Random ra = new Random();
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        public static string getSerialNum()
        {

            int rodam = ra.Next(10, 99);
            return getYYYYMMDDHHMMSSTime() + TimeStrLenZero(DateTime.Now.Millisecond.ToString()) + rodam;
        }
        /// <summary>
        /// 获取Mac     
        /// </summary>
        /// <returns></returns>
        public static string getMac()
        {
            string MacAddress;
            //设置MAC地址
            string MAC = "";
            ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection MOC = MC.GetInstances();
            foreach (ManagementObject moc in MOC)
            {
                if (moc["IPEnabled"].ToString() == "True")
                {
                    MAC = moc["MacAddress"].ToString();
                }
            }
            MacAddress = MAC;
            MacAddress = MacAddress.Replace(":", "");
            return MacAddress;
        }

        public static string padLeft(string str, int length)
        {
            int len = length - str.Length;
            string pad = "";
            for (int i = 0; i < len; i++)
            {
                pad += "0";
            }
            return pad + str;
        }



        public static Boolean WantongSign()
        {
            try
            {
                //签到
                Dictionary<string, ResultData> signResult = null;
                if (signResult == null) return false;
                //键盘安装工作秘钥
                ResultData _62 = signResult["62"];


                PaySign paySign = new PaySign();
                paySign.mechine_no = ConfigurationManager.AppSettings["MechineNo"];
                paySign.terminal_no = SysConfigHelper.readerNode("ClientNo");
                paySign.shop_no = SysConfigHelper.readerNode("ShopNo");
                paySign.sign_type = "1";


                if (signResult["39"].value.Trim().Equals("00"))
                {
                    //获取44域中的密文密钥
                    string mackeyExpress = _62.value.Substring(_62.value.Length - 40, 40);

                    string pinKeyExpress = _62.value.Substring(0, 40);

                    bool ret = Sign(1, mackeyExpress, pinKeyExpress);

                    if (ret)
                    {
                        paySign.sign_result = "00";

                    }
                    else
                    {
                        paySign.sign_result = "222222";

                    }
                    TradeBLL.SendSignRecord(paySign);
                    return ret;
                }
                else
                {
                    paySign.sign_result = signResult["39"].value;
                    TradeBLL.SendSignRecord(paySign);
                }


                return false;
            }
            catch { return false; }
        }
        //签到
        
        public static Boolean LakalaSign()
        {
            //try
            //{
            //    AnalysisBaseLKL lk = new AnalysisBaseLKLConsu();
            //    byte[] signMsg = LKLProcedure.sign();
            //    Dictionary<string, ResultData> rd = lk.analysis(signMsg);
            //    if (rd == null) return false;

            //    PaySign paySign = new PaySign();
            //    paySign.mechine_no = ConfigurationManager.AppSettings["MechineNo"];
            //    paySign.terminal_no = SysConfigHelper.readerNode("LklClientNo");
            //    paySign.shop_no = SysConfigHelper.readerNode("ShopNo");
            //    paySign.sign_type = "0";


            //    if (rd["39"].value.Trim().Equals("00"))
            //    {
            //        ResultData _44 = rd["44"];
            //        //获取44域中的密文密钥
            //        string mackeyExpress = _44.value.Substring(_44.value.Length - 16, 16);
            //        string pinKeyExpress = _44.value.Substring(0, 32);

            //        LKLProcedure.RebuildBatchNo();//批次号+1

            //        bool ret = Sign(0, mackeyExpress, pinKeyExpress);
            //        if (ret)
            //        {
            //            paySign.sign_result = "00";
            //        }
            //        else
            //        {
            //            paySign.sign_result = "111111";

            //        }
            //        TradeBLL.SendSignRecord(paySign);
            //        return ret;
            //    }
            //    else
            //    {
            //        paySign.sign_result = rd["39"].value;
            //        TradeBLL.SendSignRecord(paySign);
            //    }
            //    return false;
            //}
            //catch { return false; }
            return false;
        }
        /// <summary>
        /// bat批处理
        /// </summary>
        /// <param name="batPath"></param>
        public static void RunBat(string batPath)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = batPath;
            startInfo.Verb = "runas";
            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch 
            {
                return;
            }
            //Process pro = new Process();
            //FileInfo file = new FileInfo(batPath);
            //pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            //pro.StartInfo.FileName = batPath;
            //pro.StartInfo.CreateNoWindow = false;
            //pro.Start();
            //pro.WaitForExit();
        }

        public static Boolean Sign(int type, string mackeyExpress, string pinKeyExpress)
        {

            PasswordBLL pwdBLL = new PasswordBLL();

            //bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
            //if (!ret)
            //{
            //    return false;
            //}
            try
            {
                bool setMode = true;
                if (type == 0)
                {
                    setMode = pwdBLL.SetCryptMode(0);
                    if (!setMode)
                    {
                        return false;
                    }
                    //激活万通主密钥
                    /* if (!pwdBLL.ActivWorkKey(0, 0))
                     {
                         return false;
                     }*/
                    bool pingSuccess = pwdBLL.DownloadWorkKey(0, 0, pinKeyExpress);
                    if (!pingSuccess)
                    {
                        return false;

                    }
                    bool macSuccess = pwdBLL.DownloadWorkKey(0, 1, mackeyExpress);
                    if (!macSuccess)
                    {
                        return false;
                    }
                    SysBLL.RebuildBatchNo();

                }
                else if (type == 1)
                {
                    setMode = pwdBLL.SetCryptMode(1);
                    if (!setMode)
                    {
                        return false;
                    }
                    //激活万通主密钥
                    /*if(!pwdBLL.ActivWorkKey(1, 0))
                    {
                        return false;
                    }*/
                    bool pingSuccess = pwdBLL.DownloadWorkKey(1, 2, pinKeyExpress);
                    if (!pingSuccess)
                    {
                        return false;

                    }
                    bool macSuccess = pwdBLL.DownloadWorkKey(1, 3, mackeyExpress);
                    if (!macSuccess)
                    {
                        return false;
                    }

                }
                pwdBLL.closeKey();
                return true;
            }
            catch { return false; }
        }

        public static string getOrderNo()
        {
            Random ra = new Random();
            int rodam = ra.Next(10, 99);
            return rodam.ToString();
        }
        public static string GetRnd(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {

            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return hour + minute + second;
            //byte[] b = new byte[4];
            //new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            //Random r = new Random(BitConverter.ToInt32(b, 0));
            //string s = null, str = custom;
            //if (useNum == true) { str += "0123456789"; }
            //if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            //if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            //if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            //for (int i = 0; i < length; i++)
            //{
            //    s += str.Substring(r.Next(0, str.Length - 1), 1);
            //}
            //return s;
        }

        public static string MonitorCom()
        {
            //终端号
            string ClientNo = SysConfigHelper.readerNode("ClientNo");

            string strCom = "";
            //电动读卡器
            string port = SysConfigHelper.readerNode("CRT310Port");
            StringBuilder info = new StringBuilder(2048);
            int handle = CRT310.CommOpen("COM" + port);
            if (handle > 0) strCom = strCom + "1";
            else strCom = strCom + "0";
            CRT310.CommClose(handle);
            //燃气
            port = SysConfigHelper.readerNode("CJ201");
            CJ201.handle = CJ201.Open_Com(Int32.Parse(port), 9600, 8, 0, 0);
            if (CJ201.handle > 0) strCom = strCom + "1";
            else strCom = strCom + "0";
            CJ201.Close_Com(CJ201.handle);
            ////公交读卡器
            //port = SysConfigHelper.readerNode("CRT603Port");
            //int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);
            //if (openRet == 0) strCom = strCom + "1";
            //else strCom = strCom + "0";
            //CRT603.CRT603Vx_CloseConnection();
            //打印机
            handle = Print.TT_OpenDevice(new StringBuilder("COM" + SysConfigHelper.readerNode("PrintPort")), new StringBuilder("38400"), info);
            if (handle == 0) strCom = strCom + "1";
            else strCom = strCom + "0";
            Print.TT_CloseDevice(info);
            //密码键盘
            PasswordBLL pwdBLL = new PasswordBLL();
            bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
            if (ret) strCom = strCom + "1";
            else strCom = strCom + "0";

            return strCom;
        }

        /// <summary>
        /// 启动一个程序
        /// </summary>
        /// <param name="path"></param>
        public static void StartApplication(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = path;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory01()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo p = null;
                System.Diagnostics.Process Proc;
                string path = System.IO.Directory.GetCurrentDirectory();


                if (!File.Exists(path + "\\empty.exe"))
                {
                    //Message8Box.Show(System.IO.Directory.GetCurrentDirectory() + "\\empty.exe不存在");
                }
                p = new ProcessStartInfo(path + "\\empty.exe", "Hisign.ACS.Main.exe");
                p.WorkingDirectory = path;//设置此外部程序所在windows目录
                p.WindowStyle = ProcessWindowStyle.Hidden;//在调用外部exe程序的时候，控制台窗口不弹出
                                                          //如果想获得当前路径为

                Proc = System.Diagnostics.Process.Start(p);//调用外部程序
                System.Threading.Thread.Sleep(1000);

                GC.Collect();
                //GC.WaitForPendingFinalizers();
                //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                //{
                //    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                //}
            }
            catch { }
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //Process[] processes = Process.GetProcesses();
            //foreach (Process process in processes)
            //{
            //    //以下系统进程没有权限，所以跳过，防止出错影响效率。
            //    if ((process.ProcessName == "System") && (process.ProcessName == "Idle"))
            //        continue;
            //    try
            //    {
            //        EmptyWorkingSet(process.Handle);
            //    }
            //    catch
            //    {
            //    }
            //}

        }
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void DispoMemorys()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                //以下系统进程没有权限，所以跳过，防止出错影响效率。
                if ((process.ProcessName == "System") && (process.ProcessName == "Idle"))
                    continue;
                try
                {
                    EmptyWorkingSet(process.Handle);
                }
                catch
                {
                }
            }
        }
        public static string getHour()
        {
            string minute = DateTime.Now.Minute.ToString();
            if (minute.Length == 1) minute = "0" + minute;
            return DateTime.Now.Hour + ":" + minute;
        }
        public static string getDate()
        {
            string week = "";
            switch (DateTime.Now.DayOfWeek.ToString("D"))
            {
                case "0":
                    week = "星期日 ";
                    break;
                case "1":
                    week = "星期一 ";
                    break;
                case "2":
                    week = "星期二 ";
                    break;
                case "3":
                    week = "星期三 ";
                    break;
                case "4":
                    week = "星期四 ";
                    break;
                case "5":
                    week = "星期五 ";
                    break;
                case "6":
                    week = "星期六 ";
                    break;
            }
            return DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日 " + week;
        }

        private static string batchNo = null;
        private static string orderNo = null;
        private static Random rd = new Random();

        /// <summary>
        /// 银联交易批次号，对应一次签到下批次号都应该一样
        /// </summary>
        /// <returns></returns>
        public static string GetBatchNo()
        {

            if (batchNo == null)
            {
                batchNo = GetNum(rd).ToString() + "0";
            }
            return batchNo;
        }

        /// <summary>
        /// 重新签到时需要重新设置批次号
        /// </summary>
        public static void RebuildBatchNo()
        {

            batchNo = null;
        }


        /// <summary>
        /// 银联交易流水号：每次加1
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNo()
        {

            if (orderNo == null)
            {
                orderNo = GetNum(rd).ToString() + "0";
            }
            else
            {
                orderNo = (Convert.ToInt32(orderNo) + 1).ToString();
            }

            return orderNo;
        }


        /// <summary>
        /// 生成不重复的6位随机数
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        public static int GetNum(Random rd)
        {
            int num = rd.Next(10000, 99999); //产生1~9的随机数
            return num;
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
         int uAction,
         int uParam,
         string lpvParam,
         int fuWinIni
         );

        public static void setImagePage()
        {
            //设置墙纸显示方式
            //RegistryKey myRegKey = Registry.CurrentUser.OpenSubKey("Control Panel/desktop", true);
            //myRegKey.SetValue("TileWallpaper", "1");
            //myRegKey.SetValue("WallpaperStyle", "0");
            //myRegKey.Close();

            //设置墙纸
            string strSavePath = Application.StartupPath + "/1.jpg";
            SystemParametersInfo(20, 1, strSavePath, 1);
        }

        private const int SWP_HIDEWINDOW = 0x80;
        private const int SWP_SHOWWINDOW = 0x40;
        private const int GW_CHILD = 5;
        private const int SW_HIDEE = 0;
        private const int SW_SHOWNORMAL = 1;
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(
        int hWnd,                           //   handle   to   window     
        int hWndInsertAfter,     //   placement-order   handle     
        short X,                                   //   horizontal   position     
        short Y,                                   //   vertical   position     
        short cx,                                 //   width     
        short cy,                                 //   height     
        uint uFlags                         //   window-positioning   options     
        );
        [DllImport("user32.dll")]
        public static extern int FindWindows(
        string lpClassName,     //   class   name     
        string lpWindowName     //   window   name     
        );
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(
        int hWnd,           //   handle   to   window     
        short nCmdShow       //   show   state     
        );
        [DllImport("user32.dll")]
        public static extern short ShowCursor(
        bool bShow       //   cursor   visibility     
        );

        public static void LklWtSign()
        {
            PasswordBLL pwdBLL = new PasswordBLL();
            bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
            if (ret)
            {
                if (!SysBLL.WantongSign())
                {
                    SysBLL.wangtonSignResult = false;
                    log.Write("警告：万通签到失败");
                }
                else
                {
                    SysBLL.wangtonSignResult = true;
                    log.Write("万通签到成功");
                }
                if (!SysBLL.LakalaSign())
                {
                    SysBLL.lakalaSignResult = false;
                    log.Write("警告：拉卡拉签到失败");
                }
                else
                {
                    SysBLL.lakalaSignResult = true;
                    log.Write("拉卡拉签到成功");
                }
            }
            else
            {
                log.Write("警告：键盘打开失败");
            }
        }

        public static void refund(refundParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("servicename", "TK001");// TK001 not null
            parameters.Add("loginId", param.loginId);//交互终端的设备编号（用于自助终端）not null
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("reqsn", param.reqsn);//请求流水号 not null
            parameters.Add("trandateTime", param.trandateTime);//  交易时间 格式：YYYYMMDDHHMMSS not null
            parameters.Add("orderno", param.orderno);//订单编号 not null
            parameters.Add("transType", param.transType);//线上线下 not null
            parameters.Add("conName", param.conName);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("refund"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            //gasQueryInfo = JsonConvert.DeserializeObject<GasQueryInfo>(jsonText);

            //return gasQueryInfo;
        }
        public static string getMMDDHHMMSSTime()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return getMMDD() + getHHMMSS();
        }
        /// <summary>
        /// 获取当前月日
        /// </summary>
        /// <returns></returns>
        static string getMMDD()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            return month + day;
        }
        /// <summary>
        /// 获取当前时分秒
        /// </summary>
        /// <returns></returns>
        public static string getHHMMSS()
        {

            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return hour + minute + second;
        }

        public static void SetSystemTime(string dateString)
        {
            try
            {
                log.Write("开始同步时间.");
                DateTime dt = DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);


                SystemTime MySystemTime = new SystemTime();

                SetSystemDateTime.GetLocalTime(MySystemTime);

                MySystemTime.vYear = (ushort)dt.Year;

                MySystemTime.vMonth = (ushort)dt.Month;

                MySystemTime.vDay = (ushort)dt.Day;

                MySystemTime.vHour = (ushort)dt.Hour;

                MySystemTime.vMinute = (ushort)dt.Minute;

                MySystemTime.vSecond = (ushort)dt.Second;

                SetSystemDateTime.SetLocalTime(MySystemTime);
            }
            catch(Exception ex) { log.Write("error:SetSystemTime:" + ex.Message); }
        }
        /// <summary>
        /// 删除指定文件下的文件
        /// </summary>
        /// <param name="path"></param>
        public static void deleteDirFile(string path)
        {
            string mainPath1 = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            while (true) {
                try
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].Exists) files[i].Delete();
                    }
                    return;
                }
                catch (IOException) { continue; }
                catch (Exception ex) { continue; }
            }
                
        }
    }
    public class SetSystemDateTime
    {

        [DllImportAttribute("Kernel32.dll")]

        public static extern void GetLocalTime(SystemTime st);

        [DllImportAttribute("Kernel32.dll")]

        public static extern void SetLocalTime(SystemTime st);
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class SystemTime

    {
        public ushort vYear;

        public ushort vMonth;

        public ushort vDayOfWeek;

        public ushort vDay;

        public ushort vHour;

        public ushort vMinute;

        public ushort vSecond;
    }

   

}
