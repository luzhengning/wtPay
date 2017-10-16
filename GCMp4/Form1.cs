using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GCMp4
{
    public partial class Form1 : Form
    {
        Thread playstopThread = null;
        private delegate void setMp4Delegate(AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1,int i);

        private void setMp4(AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1, int i)
        {
            if (i == 1)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                
            }
            else if (i == 2)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        public Form1()
        {
            InitializeComponent();
            

        }
        public static string ReadGCStateID()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//Volume.txt";
            return File.ReadAllText(filePath);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            showOnMonitor(1);
            this.WindowState = FormWindowState.Maximized;
            InitVedio();

            InitVedioUrl();
            InitEvent();
            try
            {
                new Thread(delegate ()
                {
                    Thread.Sleep(2000);
                    axWindowsMediaPlayer1.uiMode = "None";
                    //axWindowsMediaPlayer1.fullScreen = true;

                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }).Start();

                //axWindowsMediaPlayer1.MouseDownEvent+= new AxWMPLib._WMPOCXEvents_MouseDownEventHandler(Form1_MouseDown);
                axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(axWindowsMediaPlayer1_PlayStateChange);
                axWindowsMediaPlayer1.MouseDownEvent += new AxWMPLib._WMPOCXEvents_MouseDownEventHandler(_WMPOCXEvents_MouseDownEvent);
            }
            catch { }
            playstopThread=new Thread(delegate() {
                try
                {
                    while (true)
                    {
                        if (isClose) return;
                        Thread.Sleep(1000 * 30);
                        if (isClose) return;
                        isStop = false;
                        if (isClose) return;
                        if ("1".Equals(SqlLiteHelper.query("isStopGCMp4")[0].FormalValue))
                        {
                            if (isClose) return;
                            if (Convert.ToInt32(axWindowsMediaPlayer1.playState) == 3)
                            {
                                isStop = true;
                                if (isClose) return;
                                axWindowsMediaPlayer1.Invoke(new setMp4Delegate(setMp4), axWindowsMediaPlayer1, 1);
                                if (isClose) return;
                                int monitorCount = System.Windows.Forms.Screen.AllScreens.Count();
                                if (monitorCount == 1)
                                {
                                    Application.Exit();
                                    return;
                                }
                            }
                        }
                        else if ("2".Equals(SqlLiteHelper.query("isStopGCMp4")[0].FormalValue))
                        {
                            if (isClose) return;
                            axWindowsMediaPlayer1.Invoke(new setMp4Delegate(setMp4), axWindowsMediaPlayer1, 2);
                        }
                    }
                }catch(Exception ex)
                {
                    //MessageBox.Show("error:"+ex.Message+","+ex.InnerException);
                }
            });
            playstopThread.Start();
        }
        Boolean isStop = false;
        private void showOnMonitor(int showOnMonitor)
        {

            Screen[] sc;
            sc = Screen.AllScreens;
            if (showOnMonitor >= sc.Length)
            {
                showOnMonitor = 0;
            }
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(sc[showOnMonitor].Bounds.Left, sc[showOnMonitor].Bounds.Top);
            // If you intend the form to be maximized, change it to normal then maximized.  
            //this.WindowState = FormWindowState.Normal;
        }

        //初始化播放控件
        private void InitVedio()
        {
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(this.Width, this.Height);
            this.axWindowsMediaPlayer1.TabIndex = 2;
            this.Controls.Add(this.axWindowsMediaPlayer1);
            Thread.Sleep(1000);
        }
        //初始化播放控件的视频文件地址
        protected void InitVedioUrl()
        {
            //if (System.IO.File.Exists("gcmp4.mp4"))
            //{
            //    this.axWindowsMediaPlayer1.URL = @"gcmp4.mp4";
            //}else
            //{
            //    this.axWindowsMediaPlayer1.URL = @"gcmp4.mp4";

            //}
            //建立播放列表，名字为aa
            axWindowsMediaPlayer1.currentPlaylist = axWindowsMediaPlayer1.newPlaylist("aa", "");

            //string path = SysConfigHelper.readerNode("gcMp4path");
            string path = "D:/payMedia/mp4/";
            //遍历打开的集合
            for (int i = 1; i <= 5; i++)
            {
                if (System.IO.File.Exists(path + i + ".mp4"))
                {
                    //添加播放列表
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(path + i + ".mp4"));
                }
            }
        }


        protected void InitEvent()
        {
            axWindowsMediaPlayer1.StatusChange += new EventHandler(axWindowsMediaPlayer1_StatusChange);
        }

        //通过控件的状态改变，来实现视频循1环播放
        protected void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            if (ReadGCStateID().Length == 0)
            {
                axWindowsMediaPlayer1.settings.volume = 10;
            }
            else
            {
                axWindowsMediaPlayer1.settings.volume = Convert.ToInt32(ReadGCStateID()) * 2;
            }
            /*  0 Undefined Windows Media Player is in an undefined state.(未定义)
                1 Stopped Playback of the current media item is stopped.(停止)
                2 Paused Playback of the current media item is paused. When a media item is paused, resuming playback begins from the same location.(停留)
                3 Playing The current media item is playing.(播放)
                4 ScanForward The current media item is fast forwarding.
                5 ScanReverse The current media item is fast rewinding.
                6 Buffering The current media item is getting additional data from the server.(转换)
                7 Waiting Connection is established, but the server is not sending data. Waiting for session to begin.(暂停)
                8 MediaEnded Media item has completed playback. (播放结束)
                9 Transitioning Preparing new media item.
                10 Ready Ready to begin playing.(准备就绪)
                11 Reconnecting Reconnecting to stream.(重新连接)
            */
            //判断视频是否已停止播放
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {
                //停顿2秒钟再重新播放
                System.Threading.Thread.Sleep(100);
                //
                //重新播放
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            if (Convert.ToInt32(axWindowsMediaPlayer1.playState) == 3)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
        }
        public class SysConfigHelper
        {
            static string pathUrl = System.AppDomain.CurrentDomain.BaseDirectory + "SysConfig.xml";
            public static string readerNode(string nodeName)
            {
                string result = string.Empty;
                using (XmlTextReader reader = new XmlTextReader(pathUrl))
                {
                    while (reader.Read())
                    {
                        if (nodeName.Equals(reader.Name))
                        {
                            result = reader.ReadElementString().Trim();
                            break;
                        }
                    }
                }

                return result;
            }
        }

        private void Form1_MouseDown(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            MessageBox.Show("asfd");
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (isStop) {
                isStop = false;
                return;
            }
            if (e.newState == 2)
            {
                ConfigClass config = SqlLiteHelper.query("isMainClose")[0];
                config.FormalValue = "0";
                config.TestValue = "0";
                SqlLiteHelper.update(config);
                Application.Exit();
            }
            if (e.newState == 3)
            {
                //播放
            }
        }
        private void _WMPOCXEvents_MouseDownEvent(object sender, AxWMPLib._WMPOCXEvents_MouseDownEvent e)
        {
            if (playstopThread.IsAlive)
            {
                playstopThread.Abort();
                playstopThread.DisableComObjectEagerCleanup();
            }
             Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            if (playstopThread.IsAlive)
            {
                playstopThread.Abort();
                playstopThread.DisableComObjectEagerCleanup();
            }
            isClose = true;
            Application.Exit();
        }
        Boolean isClose = false;
    }
}
