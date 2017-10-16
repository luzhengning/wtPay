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

namespace wtPay.GeneralForm
{
    /// <summary>
    /// FormMobileStep5.xaml 的交互逻辑
    /// </summary>
    public partial class FormInputPassword : UserControl
    {
        //页面状态
        bool isCloseForm = false;

        Thread keyThread = null;

        PasswordBLL pwdBLL = new PasswordBLL();

        //设置label的值
        private delegate void showInfoDelegate(TextBlock label, string value);

        private delegate string getPasswordDelegate();

        //密码文本框显示字符
        private delegate void passwordDelegate(string value);

        private delegate void deletePwdDelegate(TextBox t);
        public FormInputPassword()
        {
            InitializeComponent();
        }
        private void deletePwd(TextBox t)
        {
            t.Text = t.Text.Remove(t.Text.Length - 1, 1);
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Cancle();
        }
        //窗体加载事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                lblAmount.Visibility = Visibility.Hidden;
                if (SysBLL.payCostType == 5)
                {
                    lblAmount.Visibility = Visibility.Visible;
                    lblAmount.Content = "应缴金额：" + Payment.GasPayParam.GasCard.price + " 元"; ;
                }
                if (SysBLL.payCostType == 11)
                {
                    lblAmount.Visibility = Visibility.Visible;
                    lblAmount.Content = "应缴金额：" + Payment.propPayTempParam.RechageAmount + " 元"; ;
                }
                if (SysBLL.payCostType == 12)
                {
                    lblAmount.Visibility = Visibility.Visible;
                    lblAmount.Content = "应缴金额：" + Payment.propSecPayParam.RechageAmount + " 元"; ;
                }
                txtPassword.Text = "";
                if (SysBLL.PasswordErrorInfo.Contains("错误")) SysBLL.Player("密码错误.wav");
                else SysBLL.Player("输入密码.wav");
                showinfoLbl.Text = SysBLL.PasswordErrorInfo;
                isCloseForm = false;
                if (SysBLL.payCardType == 1)
                {
                    //万通支付
                    //pwdBLL.SetCryptMode(1);//万通卡加密方式
                    pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", SysBLL.payCardNo);
                }
                else
                {
                    //pwdBLL.SetCryptMode(0);//银行卡加密方式
                    pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", SysBLL.payCardNo);
                }
                //扫描键盘
                keyThread = new Thread(delegate ()
                {
                    key();
                });
                keyThread.Start();
                new Thread(delegate () { SysBLL.Player("输入密码.wav"); }).Start();
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (WtException we) { log.Write("error:输入密码页面异常"); }
            catch (Exception ex) {
                log.Write("error:输入密码页面异常：" + ex.Message);
            }

        }
        private void key()
        {
            try
            {
                string key = "";
                while (true)
                {
                    if (isCloseForm) return;
                    key = pwdBLL.isPassword();
                    if (key != null)
                    {
                        if ("0D".Equals(key))
                        {
                            //确定
                            if (this.txtPassword.Dispatcher.Invoke(new getPasswordDelegate(getPassword)).ToString().Length >= 6)
                            {
                                OK();
                                return;
                            }
                        }
                        if ("08".Equals(key))
                        {
                            //删除
                            if (this.txtPassword.Dispatcher.Invoke(new getPasswordDelegate(getPassword)).ToString().Length > 0)
                            {
                                this.txtPassword.Dispatcher.Invoke(new deletePwdDelegate(deletePwd), this.txtPassword);
                            }
                        }
                        if ("1B".Equals(key))
                        {
                            //取消
                            Cancle();
                            return;
                        }
                        if ("*".Equals(key))
                        {
                            this.txtPassword.Dispatcher.Invoke(new passwordDelegate(setPasswordText), this.txtPassword.Dispatcher.Invoke(new getPasswordDelegate(getPassword)).ToString() + "*");
                        }
                        Thread.Sleep(SysBLL.pwdWhile);
                    }
                }
            }
            catch (ThreadAbortException ae) { log.Write("error:扫描密码键盘异常："+ae.Message); return; }
            catch(WtException we) { log.Write("error:扫描密码键盘异常"); return; }
            catch (Exception e) {
                log.Write("error:扫描密码键盘异常："+e.Message);
                return;
            }
        }
        //设置密码框
        private void setPasswordText(string value)
        {
            this.txtPassword.Text = value;
        }
        private void Cancle()
        {
            pwdBLL.closeKey();
            Util.JumpUtil.jumpMainPage();
            this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpMainPage(); }));
            return;
        }
        //显示提示信息
        private void PrintInfo(string msg)
        {
            showinfoLbl.Dispatcher.Invoke(new showInfoDelegate(showInfo), showinfoLbl, msg);
        }
        //设置label的值
        private void showInfo(TextBlock lable, string msg)
        {
            lable.Text = msg;
        }
        private string getPassword()
        {
            return txtPassword.Text;
        }
        void OK()
        {
            try
            {
                if (this.txtPassword.Dispatcher.Invoke(new getPasswordDelegate(getPassword)).ToString().Length == 0) return;
                setPassword(pwdBLL.GetPasswordFromZT598());
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); return; }
            catch (WtException e)
            {
                PrintInfo(e.Message);
            }
            catch (Exception e)
            {
                PrintInfo(e.Message);
            }

        }
        private void setPassword(string pwd)
        {
            switch (SysBLL.payCostType)
            {
                case 1:
                    //移动缴费
                    Payment.mobilePayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormMobileStep06"); }));
                    break;
                case 2:
                    //联通缴费
                    Payment.unicomPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormUnicomStep06"); }));
                    break;
                case 3:
                    //万通卡充值
                    Payment.wintopReChargeParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormCitizenStep07"); }));
                    break;
                case 4:
                    //电力缴费
                    Payment.elecPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormElectricStep06"); }));
                    break;
                case 5:
                    //燃气缴费
                    Payment.GasPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormGasGoldenCardStep07"); }));
                    break;
                case 6:
                    //广电缴费
                    Payment.broadCasPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormBroadCasStep06"); }));
                    break;
                case 7:
                    //水务缴费
                    Payment.waterPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormWaterStep06"); }));
                    break;
                case 8:
                    //热力缴费
                    Payment.heatPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormHeatStep06"); }));
                    break;
                case 9:
                    //公交缴费
                    Payment.BusPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormBusStep07"); }));
                    break;
                case 10:
                    //物业缴费
                    Payment.PropPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPropStep06"); }));
                    break;
                case 11:
                    //小区物业缴费
                    Payment.propPayTempParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPropStepTemp06"); }));
                    break;
                case 12:
                    //物业二次专供缴费
                    Payment.propSecPayParam.Pwd = pwd;
                    this.Dispatcher.Invoke(new Action(() => { Util.JumpUtil.jumpCommonPage("FormPropSecStep07"); }));
                    break;
                default:
                    throw new Exception("缴费类型不匹配");
            }
        }

        //关闭窗体时触发
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                isCloseForm = true;
                keyThread.Abort();
                keyThread.DisableComObjectEagerCleanup();
                keyThread = null;
            }
            catch(Exception ex)
            {
                log.Write("Error：移动密码输入密码页面关闭时："+ex.Message);
            }
        }
    }
}
