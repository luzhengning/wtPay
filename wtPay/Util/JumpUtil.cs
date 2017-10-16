using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using wtPay.FormCitizen;
using wtPayBLL;
using wtPayModel.PayParamModel;
using wtPayModel.SystemModel;

namespace wtPay.Util
{
    class JumpUtil
    {
        private static ResourceManager rm = ResourceManager.getInstance();  //持有资源管家引用
        private static Dictionary<string, Object> paramsMap = new Dictionary<string, Object>();

        private delegate void AdvertisePageContentDelegate(AdvertisePage advertisePage, UserControl userControl);

        private delegate void setAdvertisePageDelegate(AdvertisePage advertisePage);

        public static Dictionary<string, object> ParamsMap
        {
            get
            {
                return paramsMap;
            }
        }

        /// <summary>
        /// 跳转回首页
        /// </summary>
        public static void jumpMainPage()
        {
            //MainPage mp = (MainPage)rm.getResource("mainPage");
            MainWindow.getMainFrame().Dispatcher.Invoke(new mainPageToFramDelegate(set00001), MainWindow.getMainFrame(), (MainPage)rm.getResource("mainPage"));
        }

        private delegate void mainPageToFramDelegate(Frame f, MainPage m);

        private static void set00001(Frame f, MainPage m)
        {
            f.Content = m;
        }

        /// <summary>
        /// 跳转二级页面
        /// </summary>
        /// <param name="nextUserControl">代表要跳转的页面</param>
        public static void jumpCommonPage(string nextUserControl)
        {
            try
            {
                paramsMap.Clear();
                AdvertisePage advertisePage = (AdvertisePage)rm.getResource("advertisePage");
                UserControl userControl = (UserControl)rm.getResource(nextUserControl);
                //advertisePage.inputFrame.Content = userControl;
                advertisePage.Dispatcher.Invoke(new AdvertisePageContentDelegate(setAdvertisePageContent), advertisePage, userControl);
                //MainWindow.getMainFrame().Content = advertisePage;
                advertisePage.Dispatcher.Invoke(new setAdvertisePageDelegate(setAdvertisePage), advertisePage);
            }catch(Exception ex) { log.Write("error:jumpCommonPage" +ex.Message); }
        }

        public static void jumpCommonPage(string nextUserControl, Dictionary<string, Object> condition)
        {
            paramsMap.Clear();
            paramsMap = paramsMap.Concat(condition).ToDictionary(k => k.Key, v => v.Value);
            AdvertisePage advertisePage = (AdvertisePage)rm.getResource("advertisePage");
            UserControl userControl = (UserControl)rm.getResource(nextUserControl);
            //advertisePage.inputFrame.Content = userControl;
            advertisePage.Dispatcher.Invoke(new AdvertisePageContentDelegate(setAdvertisePageContent), advertisePage, userControl);
            //MainWindow.getMainFrame().Content = advertisePage;
            advertisePage.Dispatcher.Invoke(new setAdvertisePageDelegate(setAdvertisePage), advertisePage);
        }

        private static void setAdvertisePageContent(AdvertisePage advertisePage, UserControl userControl) {
            advertisePage.inputFrame.Content = userControl;
        }

        private static void setAdvertisePage(AdvertisePage advertisePage)
        {
            MainWindow.getMainFrame().Content = advertisePage;
        }
        /// <summary>
        /// 根据支付结果跳转
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool PayResultJump(PayParam p,Dictionary<string,string> map)
        {
            string resultCode = PayBLL.PayResult(p, map);
            if (resultCode==null) return true;
            if ("55".Equals(resultCode))
            {
                Util.JumpUtil.jumpCommonPage("FormInputPassword");
                return false;
            }
            else if ("51".Equals(resultCode))
            {
                log.Write("余额不足");
                exit("余额不足");
                return false;
            }
            else
            {
                log.Write("支付失败：返回码：" + map["recode"]);
                exit("充值失败，请重新缴费，或退卡");
                return false;
            }
        }
        /// <summary>
        /// 获取冲正报文
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string CorrectJump(PayParam p)
        {
            return PayBLL.Correct(ref p);
        }

            
        private static void exit(string info)
        {
            log.Write("--------------------交易结束--------------------");
            //new FormJump().openForm(senderLoad, eLoad, this, new FormMobileStep06_fail(info));
            FormTip.FormFailShowinfo = info;
            Util.JumpUtil.jumpCommonPage("FormFail");
        }
    }
}
