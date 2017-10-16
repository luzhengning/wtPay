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
using System.Threading;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel.WintopModel;
using System.Windows.Controls.Primitives;
using wtPayModel.ConfigModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStep01 : UserControl
    {
        private GifImage waterImage = null;

        Thread readCardThread = null;

        bool isCloseForm = false;
        

        //603读万通卡
        WantongBLL wt = new WantongBLL();

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);

        public FormCitizenStep01()
        {
            InitializeComponent();
           

        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //load事件
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                waterImage = null;
                waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\wintopGIF\\RFID.gif");
                this.waterImage.Height = 500;

                ConfigSysParam.gifBusiness = GifBusiness.wintop_RFID;
            }
            catch (Exception ex) { log.Write("error:一卡通读卡初始化：" + ex.Message + ex.InnerException); }
            try {
                Popup dd = new Popup();
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                imgGrid.Children.Add(waterImage);
                this.waterImage.StartAnimate();

                isCloseForm = false;
                PrintInfo("请将甘肃一卡通放置在公交卡或非接触磁感区内");
                SysBLL.Player("放置甘肃一卡通.wav");
                Payment.wintopReChargeParam = null;
                Payment.wintopReChargeParam = new WintopReChargeParam();

                readCardThread = new Thread(delegate () { read(); });
                readCardThread.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStep01:load():"+ex.Message);
            }
        }
        string wtCard = "";
        void read()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(300);
                    if (isCloseForm) return;
                    wtCard = wt.GetCardNoByBusReader();
                    if (wtCard == null) continue;
                    if (wtCard.Length == 16)
                    {
                        Payment.wintopReChargeParam.WtCardNo = wtCard;
                        if (isCloseForm) return;
                        PrintInfo("正在读取，请稍后...");
                        WintopStatusInfo statusInfo = new WintopAccess().queryCardStatus(wtCard);
                        if ((statusInfo.msgrsp.WTSTATE == null) || (statusInfo.msgrsp.WTSTATE.Equals("")))
                        {
                            PrintInfo("业务正忙，请稍后再试");
                            return;
                        }
                        if (!"01".Equals(statusInfo.msgrsp.WTSTATE))
                        {
                            PrintInfo("该卡已挂失，无法正常使用");
                            return;//
                        }//
                        Util.JumpUtil.jumpCommonPage("FormCitizenStep");
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); }
                catch(Exception ex)
                {
                    log.Write("error:读取万通卡异常："+ex.Message);
                    continue;
                }
            }
        }
        private void PrintInfo(string info)
        {
            lblBankCard.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblBankCard,info);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                this.waterImage.StopAnimate();

                isCloseForm = true;
                readCardThread.Abort();
                readCardThread.DisableComObjectEagerCleanup();
            }catch(Exception ex)
            {
                log.Write("error：读取万通卡线程结束异常："+ex.Message);
                return;
            }
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormCitizenInputNo");
        }
    }
}
