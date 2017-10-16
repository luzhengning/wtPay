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
using wtPayBLL;
using wtPayDAL.GCResource;
using wtPayModel.MediaResourceModel;
using System.Net;
using System.Threading;
using System.IO;

namespace wtPay
{
    /// <summary>
    /// PictureChangeUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PictureChangeUserControl : UserControl
    {
        public PictureChangeUserControl()
        {
            InitializeComponent();
            IniPic();
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(4000);
                timer.Tick += new EventHandler(timer_Tick);
                listBoxPic.SelectedIndex = 0;
                timer.Start();
                Thread updateThread = new Thread(delegate () { update(); });
                updateThread.Start();
            }
            catch (Exception ex) { log.Write("error:PictureChangeUserControl:UserControl_Loaded:" + ex.Message); }
            //new Thread(delegate() {
            //    Thread.Sleep(1000*10);
            //    IniPic();
            //}).Start();
        }

        private delegate void setListbBoxSelectDelegate(ListView lv,int i);
        private delegate void setListbBoxNullDelegate(ListView lv, List<PicClass> pic);
        private void setListBoxSelect(ListView lv,int i)
        {
            lv.SelectedIndex = i;
        }
        private void setListbBoxNull(ListView lv, List<PicClass> pic)
        {
            try
            {
                lv.ItemsSource = pic;
            }catch(Exception ex)
            {
                log.Write("error:PictureChangeUserControl:"+ex.Message+ex.InnerException);
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
        private void update()
        {
            try
            {
                GCResourceAccess gcaccess = new GCResourceAccess();
                FindIsUpdateAdvInfo findIsUpdateAdvInfo = null;
                FindDownAdvInfo findDownAdvInfo = new FindDownAdvInfo();
                WebClient myclient = new WebClient();
                
                while (true)
                {
                    try
                    {
                        //Thread.Sleep(1);
                        Thread.Sleep((1000*60)*10);
                        // 是否更新广告
                        findIsUpdateAdvInfo = null;
                        //是否更新广告
                        findIsUpdateAdvInfo = gcaccess.findIsUpdateAdv();
                        if (!"0".Equals(findIsUpdateAdvInfo.code)) continue;
                        log.Write("----开始更新广告");
                        // 广告详情获取
                        GCResourceAccess.adv_id = findIsUpdateAdvInfo.data.id;
                        //将id记录到本地
                        if (SysBLL.IsTest.Equals("正式")) // GCResourceAccess.adv_id = SysConfigHelper.readerNode("GcAdv_id");
                            GcManage.WriteGCID(findIsUpdateAdvInfo.data.id);
                        else
                            SysConfigHelper.writerNode("GcAdv_id", findIsUpdateAdvInfo.data.id);
                        string m_id = "";
                        string m_name = "";
                        for (int i = 1; i <= 5; i++)
                        {
                            findDownAdvInfo = null;
                            if (i == 3)
                            {

                            }
                            if (i == 2)
                            {
                                for (int j = 0; j < GcManage.paths.Length; j++)
                                {
                                    findDownAdvInfo = gcaccess.findDownAdv(i.ToString(), GcManage.paths[j]);
                                    if (findDownAdvInfo == null) continue;
                                    if ("0".Equals(findDownAdvInfo.code))
                                    {
                                        m_id = findDownAdvInfo.data.m_id;
                                        m_name = findDownAdvInfo.data.m_name;
                                        //下载资源
                                        ResourceDownload(findDownAdvInfo.data.data, i, myclient,GcManage.paths[j]);
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                findDownAdvInfo = gcaccess.findDownAdv(i.ToString(),"0");
                                if (findDownAdvInfo == null) continue;
                                if ("0".Equals(findDownAdvInfo.code))
                                {
                                    m_id = findDownAdvInfo.data.m_id;
                                    m_name = findDownAdvInfo.data.m_name;
                                    //下载资源
                                    ResourceDownload(findDownAdvInfo.data.data, i, myclient, "");
                                }
                            }
                        }
                        // 更新结果通知参数
                        gcaccess.recordAdvUpdateLog(m_id, m_name);
                        while (true)
                        {
                            if (!SysBLL.IsOpenIndexForm) continue;
                            if ("1".Equals(GcManage.ReadGCStateID()))
                            {
                                log.Write("广告更新完毕，执行重启");
                                //ComputerBLL.Restart();
                                break;
                            }
                            break;
                        }
                    }
                    catch (ThreadAbortException e) { log.Write("error:PictureChangeUserControl:01：update:" + e.Message + e.InnerException);continue; }
                    catch (Exception e) { log.Write("error:PictureChangeUserControl:02：update:" + e.Message + e.InnerException); continue;  }
                }
            }
            catch (ThreadAbortException e) { log.Write("error:PictureChangeUserControl:03：update:" + e.Message + e.InnerException); }
            catch(Exception e)
            {
                log.Write("error:PictureChangeUserControl:04：update:"+e.Message+e.InnerException);
            }
        }
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="pathId"></param>
        /// <param name="type"></param>
        /// <param name="myclient"></param>
        public void ResourceDownload(string pathId,int type, WebClient myclient,string leftImgPath)
        {
            try
            {
                string mainPath = System.AppDomain.CurrentDomain.BaseDirectory;
                if (type == 1)
                {
                    //timer.Stop();

                    //List<PicClass> pic = new List<PicClass>();
                    //pic.Add(new PicClass() { Image = string.Format(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/temp/temp.png"), Site = 0.ToString() });
                    //listBoxPic.Dispatcher.Invoke(new setListbBoxNullDelegate(setListbBoxNull), listBoxPic, pic);
                    //listBoxPic.Dispatcher.Invoke(new setListbBoxSelectDelegate(setListBoxSelect), listBoxPic, 0);
                    Thread.Sleep(1000 * 3);
                    SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/index");

                    //1、缴费系统主界面轮播图
                    string[] imgId = pathId.Split(',');
                    log.Write("----更新首页轮播图："+pathId);
                    int count = GcManage.getImgMaxPath(GcManage.mainImgPath);
                    count++;
                    System.IO.Directory.CreateDirectory(GcManage.mainImgPath+"//"+count);
                    for (int j = 0; j < imgId.Length; j++)
                    {
                        string path = SysConfigHelper.readerNode("downAdvName") + "?id=" + imgId[j];
                        myclient.DownloadFile(path, GcManage.mainImgPath + "//" + count+"//" + (j + 1) + ".png");
                    }
                    IniPic();
                    GcManage.WriteGCStateID("1");
                    //IniPic();
                    //listBoxPic.Dispatcher.Invoke(new setListbBoxSelectDelegate(setListBoxSelect), listBoxPic, 0);
                    //timer.Start();
                }
                if (type == 2)
                {
                    SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + leftImgPath);
                    log.Write("----更新左侧轮播图："+pathId);
                    int count = GcManage.getImgMaxPath(GcManage.leftImgPath + "//" + leftImgPath);
                    count++;
                    System.IO.Directory.CreateDirectory(GcManage.leftImgPath + "//"+leftImgPath+"//" + count);
                    //2、缴费系统左侧轮播图
                    string[] imgId = pathId.Split(',');
                    for (int j = 0; j < imgId.Length; j++)
                    {
                        myclient.DownloadFile(SysConfigHelper.readerNode("downAdvName") + "?id=" + imgId[j], GcManage.leftImgPath + "//" + leftImgPath + "//" + count + "//" + (j + 1) + ".png");
                    }
                    GcManage.WriteGCStateID("1");
                }
                if (type == 3)
                {
                    SysConfigHelper.writerNode("mainTopInfo", pathId);
                }
                if (type == 4)
                {
                    log.Write("----更新视屏广告："+pathId);
                    ComputerBLL.KillApplication("GCMp4");
                    Thread.Sleep(1000);
                    SysBLL.deleteDirFile(GcManage.mp4Path);
                    //4.视频广告
                    string[] imgId = pathId.Split(',');
                    for (int j = 0; j < imgId.Length; j++)
                    {
                        myclient.DownloadFile(SysConfigHelper.readerNode("downAdvName") + "?id=" + imgId[j], System.AppDomain.CurrentDomain.BaseDirectory + "/mp4" + "//" + (j + 1) + ".mp4");
                    }
                    GcManage.WriteGCStateID("1");
                    ComputerBLL.KillApplication("GCMp4");
                    GcManage.GcMp4FileIsExists();
                    ComputerBLL.StartApp(System.AppDomain.CurrentDomain.BaseDirectory + "GCMp4.exe");
                }
            }
            catch(IOException e) { log.Write("error:PictureChangeUserControl:ResourceDownload:" + e.Message + e.InnerException); }
            catch (Exception e) { log.Write("error:PictureChangeUserControl:ResourceDownload:" + e.Message + e.InnerException); }
        }
        
        /// <summary>
        /// 初始化轮播图
        /// </summary>
        private void IniPic()
        {
            try
            {
                //this.listBoxPic.ItemsSource = null;
                List<PicClass> pic = new List<PicClass>();
                for (int i = 1; i <= 6; i++)
                {
                    if (System.IO.File.Exists(GcManage.mainImgPath + "//"+GcManage.getImgMaxPath(GcManage.mainImgPath) +"//" + i + ".png"))
                        pic.Add(new PicClass() { Image = string.Format(GcManage.mainImgPath + "//"+ GcManage.getImgMaxPath(GcManage.mainImgPath) + "//" + i + ".png"), Site = i.ToString() });
                }
                listBoxPic.Dispatcher.Invoke(new setListbBoxNullDelegate(setListbBoxNull), listBoxPic, pic);
            }catch(Exception ex) { log.Write("error:PictureChangeUserControl:IniPic：" + ex.Message);}
        }
        DispatcherTimer timer;

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (listBoxPic.Items.Count > 0)
                {

                    if (listBoxPic.SelectedIndex == listBoxPic.Items.Count - 1)
                    {
                        listBoxPic.SelectedIndex = 0;
                    }
                    else
                    {
                        listBoxPic.SelectedIndex += 1;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error:PictureChangeUserControl:"+ex.Message+ex.InnerException);
            }
           
        }
    }
}
