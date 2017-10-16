using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using wtPay.FormCitizen;
using wtPayBLL;
using wtPayDAL.GCResource;

namespace wtPay
{
    /// <summary>
    /// AdvertiseFrame.xaml 的交互逻辑
    /// </summary>GcManage.leftImgPath="//"
    public partial class AdvertisePage : UserControl
    {
        private delegate void setlistBoxPicDelegate(Image img, BitmapImage bimg);
        List<string> paths = new List<string>();
        DispatcherTimer timer;
        Image tempImage = null;
        public AdvertisePage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(4000);
            timer.Tick += new EventHandler(timer_Tick);
            //Thread loadThread = new Thread(delegate () { isUpdate(); }); loadThread.Start();
        }
        private void setlistBoxPic(Image img, BitmapImage bimg)
        {
            try
            {
                img.Source = bimg;
            }catch(Exception ex)
            {
                log.Write("error:PictureChangeUserControl:"+ex.Message+ex.InnerException);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load();
            try
            {
                count = 1;
                if (File.Exists("D://payMedia//leftImg//" + GcManage.gcType + "//" + GcManage.getImgMaxPath("D://payMedia//leftImg//" + GcManage.gcType) + "//" + count + ".png"))
                    this.listBoxPic.Source = new BitmapImage(new Uri("D://payMedia//leftImg//" + GcManage.gcType + "//" + GcManage.getImgMaxPath("D://payMedia//leftImg//" + GcManage.gcType) + "//" + count + ".png", UriKind.Absolute));
                tempImage = listBoxPic;
                timer.Start();
            }catch(Exception ex)
            {
                log.Write("error:PictureChangeUserControl"+ex.Message+ex.InnerException);
            }
        }
        

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
        int count = 1;
        BitmapImage bitmapImage = new BitmapImage();
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if(File.Exists("D://payMedia//leftImg//" + GcManage.gcType + "//"+ GcManage.getImgMaxPath("D://payMedia//leftImg//" + GcManage.gcType)+"//" + count + ".png"))
                    this.listBoxPic.Source = new BitmapImage(new Uri("D://payMedia//leftImg//" + GcManage.gcType + "//" + GcManage.getImgMaxPath("D://payMedia//leftImg//" + GcManage.gcType) + "//" + count + ".png", UriKind.Absolute));
                GC.Collect();
                count++;
                if (count >= 5)
                {
                    count = 1;
                }
            }
            catch (IOException ie) {  }
            catch (Exception ex)
            {
                log.Write("error:AdvertisePage:timer_Tick:" + ex.Message + ex.InnerException);
                this.listBoxPic.Dispatcher.Invoke(new setlistBoxPicDelegate(setlistBoxPic), listBoxPic, new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/temp/temp.png", UriKind.Absolute)));
            }

        }
    }
}
