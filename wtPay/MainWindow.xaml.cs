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
using System.Windows.Threading;
using System.Reflection;
using Transitionals;
using System.Collections.ObjectModel;
using wtPayBLL;
using System.Threading;
using System.Configuration;
using System.ComponentModel;
using wtPayModel.WeatherModel;
using wtPayDAL;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using wtPayModel.SystemModel;
using wtPay.GeneralForm;
using wtPayDAL.Pay;
using System.IO;
using wtPayDAL.SysAccessDAL;

namespace wtPay
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool testFlag = false;

        private delegate Frame getMainFrameDelegate(MainWindow mainWindow);
        private delegate Window getApplicationDelegate(Application application);
        private delegate void setTextBlockTextDelegate(TextBlock textBlock, string value);
        private delegate void setImagePct(Image image, string url);
        private delegate void shutDownDelegate();
        private delegate void closeFromDelegate(FormLoad load);
        private delegate void setTopmostDelegate(MainWindow window, Boolean iss);

        private void setTopmost(MainWindow window, Boolean iss)
        {
            window.Topmost = iss;
        }

        /// <summary>
        /// 设置TextBlock控件显示的值
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="value"></param>
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void closeFrom(FormLoad load)
        {
            Thread.Sleep(1000 * 4);
            load.Close();
        }
        private void shutDown()
        {
            Application.Current.Shutdown();
        }

        //private ObservableCollection<Type> transitionTypes = new ObservableCollection<Type>();  //动画

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //初始化资源
                initResouces();
                //设置首页
                ResourceManager resourceManager = ResourceManager.getInstance();
                MainPage mp = (MainPage)resourceManager.getResource("mainPage");
                mainFrame.NavigationService.Content = mp;
            }catch(Exception ex) { log.Write("error:MainWindow" + ex.Message); }
            finally
            {
                new Thread(delegate()
                {
                    Thread.Sleep(3000 * 3);
                    ComputerBLL.KillApplication("FormPage");
                }).Start();
                

            }

            //测试指定页面
            //Util.JumpUtil.jumpCommonPage("FormRegistrationInput");
        }
        /// <summary>
        /// 初始化资源，所有要用的资源都在这里加载，防止内存泄露
        /// </summary>
        private void initResouces()
        {
            //加载页面
            new ResourceInitialise().LoadForm();
            try
            {
                //上送版本号
                WtPayAccess.insertVersion();
                //设置系统时间
                SysBLL.SetSystemTime(BroadCasAccess.getSystemTime());
                //设备签到
                string mechineNo = ConfigurationManager.AppSettings["MechineNo"];
                SysBLL.MechineSign(mechineNo);
                //万通拉卡拉签到
                PayAccess.LklWtSign();
                //燃气注册表
                SysBLL.RunBat(System.AppDomain.CurrentDomain.BaseDirectory + SysConfigHelper.readerNode("regist"));
            }
            catch (Exception ex)
            {
                log.Write("error:MainWindow:weatherWorker_DoWork:" + ex.Message);
            }
        }
        public static Frame getMainFrame()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.Dispatcher.Invoke(new getApplicationDelegate(getApplication), Application.Current);
            return (Frame)mainWindow.Dispatcher.Invoke(new getMainFrameDelegate(getMainWindow), mainWindow);
        }
        private static Frame getMainWindow(MainWindow mainWindow)
        {
            return mainWindow.mainFrame;
        }
        private static Window getApplication(Application application)
        {
            return application.MainWindow;
        }
        //Load事件
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                if ("正式".Equals(SysBLL.IsTest))
                {
                    //设置页面位置
                    fullScreen();
                }
                //初始化线程
                Thread loadThread = new Thread(delegate () {
                    Initialize();
                    return;
                });
                loadThread.Start();
                //设置顶部logo
                logoImg.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/logo/logo.png", UriKind.Absolute));
                //监听线程，设置页面显示在最前还是最后
                Thread portThread= new Thread(delegate() {
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (SysBLL.portNum == 10)
                        {
                            this.Dispatcher.Invoke(new setTopmostDelegate(setTopmost), this, false);
                        }
                        if (SysBLL.portNum == 11)
                        {
                            this.Dispatcher.Invoke(new setTopmostDelegate(setTopmost), this, true);
                        }
                        if (SysBLL.portNum == 99)
                        {
                            return;
                        }
                    }
                });
                portThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error：首页load事件异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 主程序初始化
        /// </summary>
        private void Initialize()
        {
            //鼠标显示或隐藏
            SysBLL.ShowCursor(SysBLL.IsShowCursor);
            //设置页面滚动信息
            this.GClbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.GClbl, SysConfigHelper.readerNode("mainTopInfo"));
            //绑定天气更新事件
            this.weatherWorker.DoWork += new DoWorkEventHandler(this.weatherWorker_DoWork);
            this.weatherWorker.RunWorkerAsync();
            //绑定发送设备状态事件
            SysBLL.SendDeviceWorker = new BackgroundWorker();
            SysBLL.SendDeviceWorker.DoWork += new DoWorkEventHandler(this.SendDeviceWorker_DoWork);
            SysBLL.SendDeviceWorker.RunWorkerAsync();
            //获取命令
            systemOrderWorker.DoWork += new DoWorkEventHandler(this.systemOrderWorker_DoWork);
            systemOrderWorker.RunWorkerAsync();
            //如果有卡则退卡
            MachCardBLL.backCard();
            //禁止用户插卡
            MachCardBLL.CancelWaitCard();
        }
        SystemOrder systemOrder = null;
        private void systemOrderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (!SysBLL.IsOpenIndexForm) continue;
                    systemOrder = SystemOrderAccess.getSystemOrder();
                    if (!SysBLL.IsOpenIndexForm) continue;
                    if ("1".Equals(systemOrder.code)) continue;
                    log.Write("确认指令执行结果");
                    SystemOrderAccess.SaveResualt(systemOrder.data.cmd_no);
                    log.Write("开始执行");
                    ComputerBLL.executeOrder(systemOrder);
                }
                catch (Exception ex)
                {
                    log.Write("error：获取命令异常："+ex.Message);
                }
                Thread.Sleep((1000*60)*5);
            }
        }
        //更新天气服务
        private void weatherWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //开启广告屏
            SysBLL.setGCMp4();
            //永恒之蓝免疫工具
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "OnionWormImmune.exe";
            p.Start();
            //循环更新天气
            while(true)
            {
                try
                {
                    weather();
                    Thread.Sleep((1000 * 60)*1);
                    //SysBLL.ClearMemory();
                    //缴费程序是否停用
                    if (SysBLL.isDisableApp)
                    {
                        //维护页面
                        mainFrame.Margin = new Thickness(-10, -8, -12, -10);
                        FormStop stop = new FormStop();
                        stop.Margin = new Thickness(0, 0, 0, 0);
                        mainFrame.Content = stop;
                    }
                }
                catch (Exception ex) { log.Write("error：更新天气异常_weatherWorker_DoWork:" + ex.Message); continue; }
            }
        }
        //设置签到
        private void setSignTime()
        {
            try
            {
                if (DateTime.Now.Hour == 0)
                {
                    if (DateTime.Now.Minute > 5)
                    {
                        if (SysBLL.signTime.Day != DateTime.Now.Day)
                        {
                            log.Write("签到时间已到，开始签到。");
                            SysBLL.signTime = DateTime.Now;
                            //万通拉卡拉签到
                            PayAccess.LklWtSign();
                        }
                    }
                }
            }
            catch (Exception e) { log.Write("签到异常：" + e.Message); }
        }
        //发送设备状态事件
        private void SendDeviceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    //设置页面滚动信息
                    this.GClbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.GClbl, SysConfigHelper.readerNode("mainTopInfo"));
                    Thread.Sleep((1000 * 60)*5);
                    //获取缴费页面提示
                    SysBLL.payPromptInfo = PayPromptAccess.queryPayPrompt(null);
                    //签到
                    setSignTime();
                    //SysBLL.ClearMemory();
                    if (!SysBLL.IsOpenIndexForm) continue;
                    DeviceState.SendStatu();
                }
                catch (Exception ex) { log.Write("error：发送设备状态异常_SendDeviceWorker_DoWork:" + ex.Message); }
            }


        }
        /// <summary>
        /// 天气查询
        /// </summary>
        void weather()
        {
            try
            {
                if ((DateTime.Now.Hour % SysBLL.WeatherQuerystate) == 0)
                {
                    if (DateTime.Now.Hour != SysBLL.WeatherHour)
                    {
                        //上送版本
                        WtPayAccess.insertVersion();
                        SysBLL.WeatherHour = DateTime.Now.Hour;
                        SysBLL.WeatherQuerystate = 4;
                        //PM2.5
                        PmInfo pmInfo = WeatherAccess.QueryPm();
                        //天气预报
                        WeatherInfo weatherInfo = WeatherAccess.QueryWeather();
                        //温度
                        SysBLL.temp = weatherInfo.data.retData.today.lowtemp + "~" + weatherInfo.data.retData.today.hightemp;
                        this.tempLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), tempLbl, SysBLL.temp);
                        //风向
                        SysBLL.fengxiang = weatherInfo.data.retData.today.fengxiang + "(" + weatherInfo.data.retData.today.fengli + ")";
                        this.fengxiangLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), fengxiangLbl, SysBLL.fengxiang);
                        //AQI
                        SysBLL.AQI = "AQI " + pmInfo.data.aqi;
                        this.AQILbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), this.AQILbl, SysBLL.AQI);
                        //污染程度
                        SysBLL.quality = pmInfo.data.quality;
                        this.qualityLbl.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), qualityLbl, SysBLL.quality);
                        //天气状况
                        SysBLL.WeatherState = weatherInfo.data.retData.today.type;
                        //设置天气图片
                        this.weatherPct.Dispatcher.Invoke(new setImagePct(getWeatherImage), weatherPct, SysBLL.WeatherState);
                    }
                }
            }
            catch (Exception ex) { log.Write("error：天气查询异常：" + ex.Message); }
        }

        /// <summary>
        /// 设置页面位置
        /// </summary>
        private void fullScreen()
        {
            //隐藏Windows任务栏
            SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_HIDE);
            //显示Windows任务栏
            //SysBLL.ShowWindow(SysBLL.FindWindow("Shell_TrayWnd", null), SysBLL.SW_RESTORE);
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            this.Topmost = true;

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            ClsWin32.HideTask(true);
        }
        /// <summary>
        /// 动画初始化
        /// </summary>
        //public void LoadTransitions(Assembly assembly)
        //{

        //    foreach (Type type in assembly.GetTypes())
        //    {
        //        // Must not already exist
        //        if (transitionTypes.Contains(type)) { continue; }

        //        // Must not be abstract.
        //        if ((typeof(Transition).IsAssignableFrom(type)) && (!type.IsAbstract))
        //        {
        //            transitionTypes.Add(type);
        //        }
        //    }
        //}



        private void timerChange_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timerChange.Content = DateTime.Now.ToString("HH:mm:ss");
            lblDate.Content = SysBLL.getDate();
        }

        private void timerChange_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }



        private void quitTest(object sender, ExecutedRoutedEventArgs e)
        {
            this.testFlag = false;
        }



        public void getWeatherImage(Image image, string weatherState)
        {
            switch (weatherState)
            {
                case "晴间多云":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/qingjianduoyun.png", UriKind.Absolute));
                    break;
                case "多云":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/duoyun.png", UriKind.Absolute));
                    break;
                case "少云":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/duoyun.png", UriKind.Absolute));
                    break;
                case "阴":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/duoyun.png", UriKind.Absolute));
                    break;
                case "阵雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/zhenyu.png", UriKind.Absolute));
                    break;
                case "强阵雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/zhongyu.png", UriKind.Absolute));
                    break;
                case "雷阵雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/leiyu.png", UriKind.Absolute));
                    break;
                case "强雷阵雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/qiangleizhenyu.png", UriKind.Absolute));
                    break;
                case "晴":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/qing.png", UriKind.Absolute));
                    break;
                case "小雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/xiaoyu.png", UriKind.Absolute));
                    break;
                case "中雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/zhongyu.png", UriKind.Absolute));
                    break;
                case "大雨":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/dayu.png", UriKind.Absolute));
                    break;
                case "风暴":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/fengbao.png", UriKind.Absolute));
                    break;
                case "小雪":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/xiaoxue.png", UriKind.Absolute));
                    break;
                case "中雪":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/zhongxue.png", UriKind.Absolute));
                    break;
                case "大雪":
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/daxue.png", UriKind.Absolute));
                    break;
                default:
                    image.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/weather/qing.png", UriKind.Absolute));
                    return;
            }
        }
        //时间定时器
        DispatcherTimer timer;
        //天气
        BackgroundWorker weatherWorker = new BackgroundWorker();
        //Load加载事件
        BackgroundWorker loadWorker = new BackgroundWorker();
        //获取命令
        BackgroundWorker systemOrderWorker = new BackgroundWorker();

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void logoImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
