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
using wtPayDAL;
using wtPayModel.PaymentModel;
using wtPayModel.WintopModel;

namespace wtPay.FormCitizen
{
    /// <summary>
    /// FormCitizenStep08_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormCitizenStepLoad : UserControl
    {
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));

        private delegate void setImageImgDelegate(Image image,BitmapImage img);
        private delegate void setTextBlockDelegate(TextBlock textBlock,string value);

        Thread excuteThread = null;
        public FormCitizenStepLoad()
        {
            InitializeComponent();
        }
        private void setImageImg(Image image, BitmapImage img)
        {
            image.Source = img;
        }
        private void setTextBlock(TextBlock textBlock,string value)
        {
            textBlock.Text = value;
        }
        private void showInfo(string value)
        {
            lblAccountInfoTip.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock),lblAccountInfoTip,value);
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            if (Payment.wintopReChargeParam.ExcuteType == 1)
            {
                Util.JumpUtil.jumpMainPage();
                return;
            }
            Util.JumpUtil.jumpCommonPage("FormCitizenStep");
            return;
        }
        private void load()
        {
            try
            {
                //img1.Source = null;
                showInfo("正在操作，请稍后...");
                excuteThread = new Thread(delegate() { excute(); });
                excuteThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormCitizenStepValidatecodeLoad:load():" + ex.Message);
            }
        }
        private void excute() {
            try {
                switch (Payment.wintopReChargeParam.ExcuteType)
                {
                    case 1:
                        //挂失
                        lossReport();
                        break;
                    case 2:
                        //修改密码
                        updatePwd();
                        break;
                }
            }
            catch (ThreadAbortException ae)
            {
                log.Write("error:FormCitizenStepLoad:" + ae.Message);
            }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepLoad:excute():" + ex.Message);
                //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, errorImage);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        /// <summary>
        /// 挂失
        /// </summary>
        private void lossReport()
        {
            try {
                step2.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock),step2,"挂失");
                step3.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step3, "验证");
                WintopLossReportInfo info = WintopAccess.LossReport(Payment.wintopReChargeParam.WintopLossReportParam);
                if ("0000".Equals(info.msgrsp.retcode))
                {
                    showInfo("挂失成功");
                    //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg),img1,succcesImage);
                }
                else
                {
                    if (info.msgrsp.retshow.Length != 0)
                    {
                        showInfo(info.msgrsp.retshow);
                    }
                    else
                    {
                        showInfo("万通卡挂失失败");
                    }
                    //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, errorImage);
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:FormCitizenStepLoad:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepLoad:lossReport():"+ex.Message);
                showInfo("万通卡挂失失败");
                //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, errorImage);
            }
        }
        //修改密码
        private void updatePwd()
        {
            try
            {
                step2.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step2, "修改密码");
                step3.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), step3, "核对密码");
                WintopUpdateWtPwdInfo info = WintopAccess.updateWtPwd(Payment.wintopReChargeParam.WintopUpdateWtPwdParam);
                if ("0000".Equals(info.msgrsp.retcode))
                {
                    showInfo(info.msgrsp.retshow);
                    //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, succcesImage);
                }
                else
                {
                    if (info.msgrsp.retshow.Length != 0)
                    {
                        showInfo(info.msgrsp.retshow);
                    }
                    else
                    {
                        showInfo("修改失败，请稍后再试...");
                    }
                    //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, errorImage);
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:FormCitizenStepLoad:" + ae.Message); }
            catch (Exception ex)
            {
                log.Write("error:FormCitizenStepLoad:updatePwd():"+ex.Message);
                showInfo("修改失败，请稍后再试...");
                //img1.Dispatcher.Invoke(new setImageImgDelegate(setImageImg), img1, errorImage);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                excuteThread.Abort();
                excuteThread.DisableComObjectEagerCleanup();
                excuteThread = null;
            }catch(Exception ex)
            {
                log.Write("error:FormCitizenStepLoad:UserControl_Unloaded:"+ex.Message);
            }
        }
    }
}
