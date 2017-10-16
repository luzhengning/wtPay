using System;
using System.Collections.Generic;
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
using wtPayBLL;
using wtPayCommon;
using wtPayModel.PaymentModel;

namespace wtPay.FormBus
{
    /// <summary>
    /// FormBusStep01.xaml 的交互逻辑
    /// </summary>
    public partial class FormBusStep01 : UserControl
    {
        Thread readThread = null;

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);
        public FormBusStep01()
        {
            InitializeComponent();
        }
        private void setTextBlockText(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormBusStep03");
        }
        //Load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                lblBankCard.Text = "请将公交卡放在相应的磁感区";
                Payment.BusPayParam = new BusPayParam();
                readThread = new Thread(readCard);
                readThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormBusStep01:load():"+ex.Message);
            }
        }
        /// <summary>
        /// 读公交卡
        /// </summary>
        private void readCard()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(200);
                    BusCardBLL service = new BusCardBLL();
                    string busNo = service.GetCardNo();
                    if (busNo.Length == 0) continue;
                    Payment.BusPayParam.BusNo = busNo;
                    Util.JumpUtil.jumpCommonPage("FormBusStep03");
                    return;
                }
                catch (ThreadAbortException ae) { return; }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (readThread.IsAlive)
                {
                    readThread.Abort();
                    readThread.DisableComObjectEagerCleanup();
                    readThread = null;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormBusStep01:Unloaded:" + ex.Message);
            }
        }
    }
}
