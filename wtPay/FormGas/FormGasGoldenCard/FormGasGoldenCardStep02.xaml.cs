using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayCommon;
using wtPayModel;
using wtPayDAL;
using System.Windows.Controls.Primitives;
using wtPayModel.ConfigModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormGas.FormGasGoldenCard
{
    /// <summary>
    /// FormGasGoldenCardStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormGasGoldenCardStep02 : UserControl
    {
        private GifImage waterImage = null;

        Thread readQuery = null;
        //读卡器访问类
        private JinCardBLL jinCard = new JinCardBLL();

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);

        bool isRead = true;
        public FormGasGoldenCardStep02()
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
                if (isRead == false) return;
                readQuery = new Thread(delegate () { read(); });
                readQuery.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02:FormGasGoldenCardStep02:" + ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormGas");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                SysBLL.Player("插入燃气卡.wav");
            }catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02:UserControl_Loaded:"+ex.Message);
            }
            try
            {
                waterImage = null;
                waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\gasGIF\\jinkaGIF\\IC.gif");
                this.waterImage.Height = 500;
                Popup dd = new Popup();
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                imgGrid.Children.Add(waterImage);
                this.waterImage.StartAnimate();
            }
            catch (Exception ex) { log.Write("error:金卡初始化：" + ex.Message + ex.InnerException); }
        }
        private void read()
        {
            try
            {
               

                isRead = false;
                PrintInfo("读取中，请稍后...");
                SysBLL.Player("读取中.wav");
                GasCard card = (GasCard)(jinCard.getCardInfo(null));
                string cardNo = card.CardNo;
                card.cardType = 1;
                Payment.GasPayParam.GasCard = card;
                GasInterface gasInterface = new GasInterface();
                SysBLL.Authcode=gasInterface.GasLogin();
                //int rechageNum = 10;
                //if (!jinCard.Recharge(card.CardNo, rechageNum))
                //{
                //    throw new Exception();
                //}
                //执行下一步
                //new FormJump().openForm(senderLoad, eLoad, this, new FormGasGoldenCardStep3(card));
                ConfigSysParam.gifBusiness = GifBusiness.gas_IC;
                Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep03");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:金卡读卡异常："+ae.Message); }
            catch (WtException e)
            {
                PrintInfo("读取失败，请重试...");
            }
            catch (Exception e)
            {
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                this.waterImage.StopAnimate();

                readQuery.Abort();
                readQuery.DisableComObjectEagerCleanup();
                readQuery = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02：UserControl_Unloaded："+ex.Message);
            }
        }

        private void lblInsertGasCardTip_Loaded(object sender, RoutedEventArgs e)
        {
            lblInsertGasCardTip.Text = "请插入燃气卡";
           
        }
    }
}
