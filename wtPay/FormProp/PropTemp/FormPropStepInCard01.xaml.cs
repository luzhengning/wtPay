using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPay.Properties;
using wtPayBLL;
using wtPayCommon;
using wtPayModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormProp
{
    /// <summary>
    /// FormGasGoldenCardStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStepInCard01 : UserControl
    {
        Thread readQuery = null;

        private delegate void setTextBlockTextDelegate(TextBlock textBlock, string value);

        private delegate void readCardDelegate(WebBrowser wb, string value);




        private AxELECTREADER01Lib.AxElectReader01 axElectReader011;

        bool isRead = true;
        public FormPropStepInCard01()
        {
            InitializeComponent();
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void ReadCard(WebBrowser wb, string value)
        {
            wb.Navigate(new Uri(value));
            bool ret = wb.IsLoaded;

        }
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isRead == false) return;
                readQuery = new Thread(delegate () { read(); });
                readQuery.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormPropStepInCard01:FormPropStepInCard01:" + ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //F001("5");
                SysBLL.Player("请插入物业卡.wav");
            }
            catch (Exception ex)
            {
                log.Write("error:FormPropStepInCard01:UserControl_Loaded:" + ex.Message);
            }
        }
        private void read()
        {
            try
            {
                isRead = false;
                PrintInfo("读取中，请稍后...");
                SysBLL.Player("读取中.wav");
                //测试
                //string cardNo = BeiAnGGCardBLL.readCard();
                string cardNo = "10007100";
                if ((cardNo.Trim().Length == 0)||("000000".Equals(cardNo.Trim())))
                {
                    throw new Exception("读取失败...");
                }
                Payment.propPayTempParam.AccountNo = cardNo;
                Util.JumpUtil.jumpCommonPage("FormPropStepTemp02");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:金卡读卡异常：" + ae.Message); }
            catch (WtException e)
            {
                PrintInfo("本卡暂不支持此小区");
            }
            catch (Exception e)
            {
                String msg = WtException.formatMsg(WtExceptionCode.Card.UNION_READ_CARD, e.Message);
                PrintInfo("本卡暂不支持此小区");
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
            lblInsertGasCardTip.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), lblInsertGasCardTip, msg);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                readQuery.Abort();
                readQuery.DisableComObjectEagerCleanup();
                readQuery = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02：UserControl_Unloaded：" + ex.Message);
            }
        }
        public string F001(string portNum)
        {
            axElectReader011 = new AxELECTREADER01Lib.AxElectReader01();
            WindowsFormsHost host = new WindowsFormsHost();
            ((System.ComponentModel.ISupportInitialize)(this.axElectReader011)).BeginInit();
            host.Child = axElectReader011;
            this.axElectReader011.Enabled = true;
            //this.axElectReader011.Name = "axElectReader011";
            //this.axElectReader011.Size = new System.Drawing.Size(100, 50);
            //this.axElectReader011.TabIndex = 2;
            back.Children.Add(host);
            ((System.ComponentModel.ISupportInitialize)(this.axElectReader011)).EndInit();

            string ret = axElectReader011.OpenDevice("COM" + portNum, "CardRW.dll").ToString();
            if (!"0".Equals(ret.Trim()))
            {
                string a = axElectReader011.ReadAll(1).ToString();
                string aa = "响应\n" + axElectReader011.GetCardData(1, 1) + "\n" + axElectReader011.GetCardData(2, 1) + "\n" + axElectReader011.GetCardData(3, 1);
                string cardNoSource = axElectReader011.GetCardData(1, 1).ToString();

                string cardNoStr = cardNoSource.Substring(10, 6);
                string cardNoPart1 = cardNoStr.Substring(0, 2);
                cardNoPart1 = cardNoPart1.PadLeft(4, '0');

                string cardNoPart2 = cardNoStr.Substring(2);
                cardNoPart2 = cardNoPart2.PadLeft(8, '0');
                // MessageBox.Show("卡号：" + cardNoPart1 + cardNoPart2 + ";次数" + cardNoSource.Substring(30, 4));
                axElectReader011.CloseDevice();
                return cardNoStr;
            }
            else
            {
                throw new Exception("读取物业卡号出错");

            }
        }
        private bool ExtendFrameControl_oncontextmenu()
        {
            throw new NotImplementedException();
        }

        private void wb_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
