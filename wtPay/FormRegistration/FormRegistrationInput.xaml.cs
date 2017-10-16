using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurityInput.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationInput : UserControl
    {

        // 安装钩子
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        // 卸载钩子
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(int idHook);
        // 继续下一个钩子
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
        //声明定义
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        BitmapImage errorImage = new BitmapImage(new Uri("/cut-2/error.png", UriKind.Relative));
        BitmapImage succcesImage = new BitmapImage(new Uri("/cut-2/success.png", UriKind.Relative));
        BitmapImage weixuanzhong= new BitmapImage(new Uri("/cut-2/textBlockBack.png", UriKind.Relative));
        BitmapImage xuanzhong = new BitmapImage(new Uri("/cut-2/xuanzhong.png", UriKind.Relative));
        public FormRegistrationInput()
        {
            InitializeComponent();
            
        }
        
        /// <summary>
        /// 文本框验证
        /// </summary>
        /// <returns></returns>
        bool checkTextIsNull()
        {
            if (passwordTxt.Text.Length == 0)
            {
                img2.Source = errorImage;
                return false;
            }
            if (phoneTxt.Text.Length == 0)
            {
                img1.Source = errorImage;
                return false;
            }
            if (cardTxt.Text.Length == 0)
            {
                img3.Source = errorImage;
                return false;
            }
            if (phoneTxt.Text.Length !=11)
            {
                //电话号码不足11位
                img1.Source = errorImage;
                return false;
            }
            return true;
        }
      
        private void load()
        {
            try {
                passwordBox2.Visibility = Visibility.Hidden;
                passwordTxt.Text = "";
                phoneTxt.Text = "";
                cardTxt.Text = "";
                phoneTxtGotFocus();
                SysBLL.Player("输入三维之家账号.wav");
            }
            catch(Exception ex)
            {
                log.Write("error:FormRegistrationInput:load():" + ex.Message);
            }
        }

        private void nameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Hook_Clear();
            phoneTxtGotFocus();
            //Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe");
            Process.Start("osk.exe");
            Thread.Sleep(500);
            Hook_Start();
            foreach (System.Windows.Forms.InputLanguage inputLanguage in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
            {
                if (inputLanguage.LayoutName.Contains("搜狗"))
                {
                    System.Windows.Forms.InputLanguage.CurrentInputLanguage = inputLanguage;
                    break;
                }
            }

        }

        private void phoneTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordTxtGotFocus();
            
        }

        private void cardTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            cardTxtGotFocus();
        }

        private void passwordTxtGotFocus()
        {
            keyboard38.textBox = passwordBox2;
            img2.Source = xuanzhong;
            img1.Source = null;
            img3.Source = null;
        }
        private void phoneTxtGotFocus()
        {
            keyboard38.textBox = phoneTxt;
            img1.Source = xuanzhong;
            img2.Source = null;
            img3.Source = null;
        }
        private void cardTxtGotFocus()
        {
            keyboard38.textBox = cardTxt;
            img3.Source = xuanzhong;
            img1.Source = null;
            img2.Source = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        string sexstr = "1";
        private void imgsex1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            imgsex1.Source = xuanzhong;
            imgsex2.Source = weixuanzhong;
            imgsex2.Width = 38;
            imgsex2.Height = 33;
            imgsex1.Width = 45;
            imgsex1.Height = 40;
            sexstr = "1";
        }

        private void imgsex2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            imgsex2.Source = xuanzhong;
            imgsex1.Source = weixuanzhong;
            imgsex1.Width = 38;
            imgsex1.Height = 33;
            imgsex2.Width = 45;
            imgsex2.Height = 40;
            sexstr = "2";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkTextIsNull() == false) return;
                RegistrationClass.registrationParam.appId = RegistrationClass.appId;
                RegistrationClass.registrationParam.conName = "预约挂号";
                RegistrationClass.registrationParam.hospital_code = RegistrationClass.hospitalInfo.hospital_code;
                RegistrationClass.registrationParam.tel = phoneTxt.Text;
                RegistrationClass.registrationParam.operate_type = "0";
                RegistrationClass.registrationParam.hb_date = RegistrationClass.doctorInfo.HB_DATE;
                RegistrationClass.registrationParam.card_type = "Idcard";
                RegistrationClass.registrationParam.sex = sexstr;
                RegistrationClass.registrationParam.hid = RegistrationClass.doctorInfo.HID;
                RegistrationClass.registrationParam.patient_name = passwordBox2.Text;
                RegistrationClass.registrationParam.id_no = cardTxt.Text;
                RegistrationClass.RegistrationType = 0;
                RegistrationClass.registrationAddress.phone = phoneTxt.Text;
                Util.JumpUtil.jumpCommonPage("FormRegistrationWait");
            }
            catch(Exception ex)
            {
                log.Write("error:"+ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistrationDoctor_3");
        }

        private void phoneTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordTxt.Text.Length >= 30)
            {
                passwordTxt.Text = passwordTxt.Text.Remove(passwordTxt.Text.Length - 1, 1);
                return;
            }
        }

        private void nameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (phoneTxt.Text.Length >= 12)
            {
                phoneTxt.Text = phoneTxt.Text.Remove(phoneTxt.Text.Length - 1, 1);
                return;
            }
        }

        private void cardTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            cardTxt.Text = cardTxt.Text.ToUpper();
            if (cardTxt.Text.Length >= 19)
            {
                cardTxt.Text = cardTxt.Text.Remove(cardTxt.Text.Length - 1, 1); 
                return;
            }
        }
        static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;
        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        HookProc KeyBoardHookProcedure;
        //键盘Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        #region DllImport 

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
        public void Hook_Start()
        {
            // 安装键盘钩子 
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                //hHook = SetWindowsHookEx(2, 
                //            KeyBoardHookProcedure, 
                //          GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), GetCurrentThreadId()); 
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                          KeyBoardHookProcedure,
                        GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    Hook_Clear();
                    //throw new Exception("设置Hook失败!"); 
                }
            }
        }
        //取消钩子事件 
        public void Hook_Clear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
            //如果去掉钩子失败. 
            if (!retKeyboard) { };
        }
        //这里可以添加自己想要的信息处理 
        public static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                if (kbh.vkCode == 91)  // 截获左win(开始菜单键) 
                {
                    return 1;
                }
                if (kbh.vkCode == 92)// 截获右win 
                {
                    return 1;
                }
                if (kbh.vkCode == (int)System.Windows.Forms.Keys.Escape && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control) //截获Ctrl+Esc 
                {
                    return 1;
                }
                if (kbh.vkCode == (int)System.Windows.Forms.Keys.F4 && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)  //截获alt+f4 
                {
                    return 1;
                }
                if (kbh.vkCode == (int)System.Windows.Forms.Keys.Tab && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt) //截获alt+tab 
                {
                    return 1;
                }
                if (kbh.vkCode == (int)System.Windows.Forms.Keys.Escape && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Shift) //截获Ctrl+Shift+Esc 
                {
                    return 1;
                }
                if ((int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt + (int)System.Windows.Forms.Keys.Delete)      //截获Ctrl+Alt+Delete 
                {
                    return 1;
                }
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ComputerBLL.KillApplication("osk");
        }

        private void nameTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            ComputerBLL.KillApplication("osk");
        }

        private void passwordBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passwordTxt.Text = "";
            for (int i= 1; i <= passwordBox2.Text.Length; i++)
            {
                passwordTxt.Text += "*";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistrationSw");
        }
    }
}
