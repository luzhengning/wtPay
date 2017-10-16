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
using wtPayModel.SocialSecurityModel;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json;
using WtPayBLL;
using Newtonsoft.Json.Linq;

using wtPayBLL;
using wtPayDAL;
using System.Windows.Threading;

namespace wtPay.FormSocialSecurity
{
    /// <summary>
    /// FormSocialSecurityWait.xaml 的交互逻辑
    /// </summary>
    public partial class FormSocialSecurityWait : UserControl
    {
        //输出查询结果参数
        Dictionary<string, Object> param = new Dictionary<string, object>();
        //页面是否关闭
        bool isCloseForm = false;
        //查询线程
        Thread queryThread = null;

        DispatcherTimer timerLoad;

        private delegate void setTextBlockDelegate(TextBlock tBlock,string value);

        PensionGrantInfo pensionGrantInfo = null;
        SocialSecurityInfo socialSecurityInfo = null;
        MedicalAccountInfo medicalAccountInfo =null;
        PensionAccountInfo pensionAccountInfo = null;
        MedicalAccountConsumeInfo medicalAccountConsumeInfo = null;

        string msg = "";
        string recode = "";


        public FormSocialSecurityWait()
        {
            InitializeComponent();
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {

        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                msg = "";
                recode = "";
                param.Clear();
                SysBLL.Player("正在查询.wav");
                lblIsChargingTip.Text = "正在查询，请稍后...";
                isCloseForm = false;
                queryThread = new Thread(delegate() { query(); });
                queryThread.Start();

                timerLoad = new DispatcherTimer();
                timerLoad.Interval = TimeSpan.FromMilliseconds(400);
                timerLoad.Tick += new EventHandler(timer_Tick);
                timerLoad.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:社保查询异常："+ex.Message);
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (loadlbl.Content.ToString().Length >= 6)
            {
                loadlbl.Content = "";
            }
            else
            {
                loadlbl.Content += ".";
            }
        }
        public void query()
        {
            try
            {
                switch (SysBLL.socialSecurityParam.type)
                {
                    case 1:
                        //养老发放信息查询
                        QueryPensionGrantInfo();
                        break;
                    case 2:
                        // 个人参保信息查询
                        QuerySocialSecurityInfo();
                        break;
                    case 3:
                        // 医疗账户消费信息查询
                        QueryMedicalAccountInfo();
                        break;
                    case 4:
                        // 养老月账户信息查询
                        QueryPensionAccountInfo();
                        break;
                    case 5:
                        //医疗账户消费信息查询
                        QueryMedicalAccountConsumeInfo();
                        break;
                    default:
                        throw new Exception();

                }
            }
            catch (Exception ex)
            {
                if (isCloseForm) return;
                log.Write("社保查询异常：" + ex.Message);
                showInfo( "查询失败，请稍后再试...");
                Thread.Sleep(1000 * 3);
                if (isCloseForm) return;
                 Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
            }
        }
       
        /// <summary>
        /// 养老发放信息查询
        /// </summary>
        void QueryPensionGrantInfo()
        {
            pensionGrantInfo = null;
            pensionGrantInfo = (PensionGrantInfo)SocialSecurityAccess.QueryPensionGrantInfo<PensionGrantInfo>(SysBLL.socialSecurityParam, ref recode, ref msg);
            if ("9999".Equals(recode))
            {
                showInfo( msg);
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            else if ((pensionGrantInfo.data == null) || ("false".Equals(pensionGrantInfo.success)))
            {
                if (pensionGrantInfo.msg != null)
                {
                    if (pensionGrantInfo.msg.Length >= 4)
                    {
                        showInfo(pensionGrantInfo.msg);
                    }
                    else showInfo( "未查到相关信息，请稍后再试...");
                }
                else
                {
                    showInfo( "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            //输出info
            param.Add("info", pensionGrantInfo);
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityPensionGrant", param);
        }

        /// <summary>
        /// 个人参保信息查询
        /// </summary>
        void QuerySocialSecurityInfo()
        {
            socialSecurityInfo = null;
            socialSecurityInfo = (SocialSecurityInfo)SocialSecurityAccess.QueryPensionGrantInfo<SocialSecurityInfo>(SysBLL.socialSecurityParam, ref recode, ref msg);
            if ("9999".Equals(recode))
            {
                showInfo( msg);
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            else if ((socialSecurityInfo.data == null) || ("false".Equals(socialSecurityInfo.success)))
            {
                if (socialSecurityInfo.msg != null)
                {
                    if (socialSecurityInfo.msg.Length >= 4)
                    {
                        showInfo(socialSecurityInfo.msg);
                    }
                    else showInfo( "未查到相关信息，请稍后再试...");
                }
                else
                {
                    showInfo( "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            //输出info
            param.Add("info", socialSecurityInfo);
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityInfo", param);
        }

        /// <summary>
        /// 医保账户信息查询
        /// </summary>
        void QueryMedicalAccountInfo()
        {
            medicalAccountInfo = null;
            medicalAccountInfo = (MedicalAccountInfo)SocialSecurityAccess.QueryPensionGrantInfo<MedicalAccountInfo>(SysBLL.socialSecurityParam, ref recode, ref msg);
            if ("9999".Equals(recode))
            {
                showInfo( msg);
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            else if ((medicalAccountInfo.data == null) || ("false".Equals(medicalAccountInfo.success)))
            {
                if (medicalAccountInfo.msg != null)
                {
                    if (medicalAccountInfo.msg.Length >= 4)
                    {
                        showInfo(medicalAccountInfo.msg);
                    }
                    else showInfo( "未查到相关信息，请稍后再试...");
                }
                else
                {
                    showInfo( "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            //输出info
            param.Add("info", medicalAccountInfo);
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityMedicalAccount", param);
        }

        /// <summary>
        /// 养老月账户信息查询
        /// </summary>
        void QueryPensionAccountInfo()
        {
            pensionAccountInfo = null;
            pensionAccountInfo = (PensionAccountInfo)SocialSecurityAccess.QueryPensionGrantInfo<PensionAccountInfo>(SysBLL.socialSecurityParam, ref recode, ref msg);
            if ("9999".Equals(recode))
            {
                showInfo( msg);
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            else if ((pensionAccountInfo.data == null) || ("false".Equals(pensionAccountInfo.success)))
            {
                if (pensionAccountInfo.msg != null)
                {
                    if (pensionAccountInfo.msg.Length >= 4)
                    {
                        showInfo(pensionAccountInfo.msg);
                    }
                    else showInfo( "未查到相关信息，请稍后再试...");
                }
                else
                {
                    showInfo( "未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            //输出info
            param.Add("info", pensionAccountInfo);
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityPensionAccount", param);
        }

        /// <summary>
        /// 医疗账户消费信息查询
        /// </summary>
        void QueryMedicalAccountConsumeInfo()
        {
            medicalAccountConsumeInfo = null;
            medicalAccountConsumeInfo = (MedicalAccountConsumeInfo)SocialSecurityAccess.QueryPensionGrantInfo<MedicalAccountConsumeInfo>(SysBLL.socialSecurityParam, ref recode, ref msg);
            if ("9999".Equals(recode))
            {
                showInfo(msg);
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            else if ((medicalAccountConsumeInfo.data == null) || ("false".Equals(medicalAccountConsumeInfo.success)))
            {
                if (medicalAccountConsumeInfo.msg != null)
                {
                    if (medicalAccountConsumeInfo.msg.Length >= 4)
                    {
                        showInfo(medicalAccountConsumeInfo.msg);
                    }
                    else showInfo("未查到相关信息，请稍后再试...");
                }
                else
                {
                    showInfo("未查到相关信息，请稍后再试...");
                }
                Thread.Sleep(1000 * 3);
                Util.JumpUtil.jumpCommonPage("FormSocialSecurity");
                return;
            }
            //输出info
            param.Add("info", medicalAccountConsumeInfo);
            Util.JumpUtil.jumpCommonPage("FormSocialSecurityMedicalAccountConsume", param);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                isCloseForm = true;
                queryThread.Abort();
                queryThread.DisableComObjectEagerCleanup();
                queryThread = null;

                timerLoad.Stop();
                timerLoad.Tick += null;
                timerLoad = null;
            }
            catch (Exception ex)
            {
                log.Write("error：社保查询页面结束线程异常："+ex.Message);
            }
        }
        private void showInfo(string value)
        {
            lblIsChargingTip.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlockText), lblIsChargingTip, value);
        }
        private void setTextBlockText(TextBlock tBlock,string value)
        {
            tBlock.Text = value;
        }
    }
}
