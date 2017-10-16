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
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurityWait.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationWait : UserControl
    {
        //页面是否关闭
        bool isCloseForm = false;
        //查询线程
        Thread queryThread = null;

        DispatcherTimer timerLoad;

        private delegate void setTextBlockDelegate(TextBlock tBlock,string value);
        private delegate void setShowLabelDelegate(Label l,bool isShow);

        string msg = "";
        string recode = "";


        public FormRegistrationWait()
        {
            InitializeComponent();
        }
        private void setShowLabel(Label label,bool isShow){
            if(isShow) label.Visibility = Visibility.Visible;
            else label.Visibility = Visibility.Hidden;
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistration");
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
                log.Write("error:预约挂号异常："+ex.Message);
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
               
                switch (RegistrationClass.RegistrationType)
                {
                    //预约挂号
                    case 0:
                        showInfo("正在预约，请稍后...");
                        PropLoginNameParam propLoginNameParam = new PropLoginNameParam();
                        propLoginNameParam.mobile = RegistrationClass.registrationParam.tel;
                        propLoginNameParam.password = RegistrationClass.registrationParam.patient_name;
                        PropLoginNameInfo propLoginNameInfo = RegistrationAccess.getName(propLoginNameParam);
                        if (!"0000".Equals(propLoginNameInfo.code))
                        {
                            if ("9955".Equals(propLoginNameInfo.msgCode)) { showInfo("账户密码不匹配，请重试..."); return; }
                            if ("9960".Equals(propLoginNameInfo.msgCode)) { showInfo("用户编号不存在，请先注册后预约..."); return; }
                            if ("9999".Equals(propLoginNameInfo.msgCode)) { showInfo("登录失败，请稍后再试..."); return; }
                            showInfo("登录失败，请稍后再试..."); return;
                        }
                        RegistrationClass.registrationParam.patient_name = propLoginNameInfo.data.userName;
                        if (propLoginNameInfo.appImg != null)
                        {
                            if (propLoginNameInfo.appImg.Length > 0)
                            {
                                RegistrationClass.appImg = propLoginNameInfo.appImg;
                            }
                        }
                        loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, true);
                        RegistrationClass.registrationInfo = RegistrationAccess.Registration(RegistrationClass.registrationParam);
                        if ("0000".Equals(RegistrationClass.registrationInfo.Result_Code))
                        {
                            Util.JumpUtil.jumpCommonPage("FormRegistration_success");
                            return;
                        }
                        showInfo(RegistrationClass.registrationInfo.Error_Msg);
                        loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel),loadlbl,false);
                        timerLoad.Stop();
                        break;
                        //预约撤销
                    case 1:
                        showInfo("正在取消，请稍后");
                        UndoRegistration info = RegistrationAccess.undoRegistration(RegistrationClass.undoRegistrationParam);
                        if ("0000".Equals(info.Result_Code))
                        {
                            showInfo("取消成功");
                            loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, false);
                            timerLoad.Stop();
                            return;
                        }
                        loadlbl.Dispatcher.Invoke(new setShowLabelDelegate(setShowLabel), loadlbl, false);
                        timerLoad.Stop();
                        showInfo(info.Error_Msg);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Write("预约挂号异常：" + ex.Message);
                showInfo( "查询失败，请稍后再试...");
            }
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
                log.Write("error：预约挂号："+ex.Message);
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
