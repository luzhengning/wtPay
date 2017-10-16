using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.ConfigModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormGas.FromGasPioneerCard
{
    /// <summary>
    /// FormGasPioneerCardStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasPioneerCardStep02 : UserControl
    {

        private GifImage waterImage = null;
        Thread readThread = null;

        bool isRead = true;

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        public FormGasPioneerCardStep02()
        {
            InitializeComponent();
            
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isRead) return;
                readThread = new Thread(delegate() { read(); });
                readThread.Start();
            }catch(Exception ex)
            {
                log.Write("error：先锋卡读卡异常："+ex.Message);
            }
        }
        private void read()
        {
            try
            {
               

                isRead = false;
                PrintInfo("读取中，请稍后...");
                SysBLL.Player("读取中.wav");
                XianfengBLL xfCard = new XianfengBLL();
                GasCard card = (GasCard)(xfCard.getCardInfo(null));
                string cardNo = card.CardNo;
                card.cardType = 2;
                Payment.GasPayParam.GasCard = card;
                GasInterface gasInterface = new GasInterface();
                SysBLL.Authcode = gasInterface.GasLogin();
                //执行下一步
                //new FormJump().openForm(senderLoad, eLoad, this, new FormGasGoldenCardStep3(card));
                ConfigSysParam.gifBusiness = GifBusiness.gas_IC;
                Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep03");
                return;
            }
            catch (ThreadAbortException ae) { }
            catch (WtException e)
            {
                PrintInfo("读取失败，请重试...");
            }
            catch (Exception e)
            {
                String msg = WtException.formatMsg(WtExceptionCode.Card.UNION_READ_CARD, e.Message);
                PrintInfo("读取失败，请重试...");
            }
            finally
            {
                isRead = true;
            }
        }
        private void PrintInfo(WtException e)
        {
            PrintInfo(e.getMsg());
        }

        private void PrintInfo(String msg)
        {
            lblInsertGasCardTip.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText),lblInsertGasCardTip,msg);
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGas");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            SysBLL.Player("插入燃气卡.wav");
            lblInsertGasCardTip.Text = "请插入燃气卡";
            try
            {
                waterImage = null;
                waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\gasGIF\\xianfengGIF\\RFID.gif");
                this.waterImage.Height = 500;
                Popup dd = new Popup();
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                imgGrid.Children.Add(waterImage);
                this.waterImage.StartAnimate();
            }
            catch (Exception ex) { log.Write("error:先锋卡初始化：" + ex.Message + ex.InnerException); }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try {
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                this.waterImage.StopAnimate();

                readThread.Abort();
                readThread.DisableComObjectEagerCleanup();
                readThread = null;
            }catch(Exception ex)
            {
                log.Write("error:FormGasPioneerCardStep02:UserControl_Unloaded:"+ex.Message);
            }
        }

        private void lblInsertGasCardTip_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
