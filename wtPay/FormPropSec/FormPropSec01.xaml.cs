using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using wtPayModel;
using wtPayModel.ConfigModel;
using wtPayModel.PaymentModel;

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGas.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSec01 : UserControl
    {
        public FormPropSec01()
        {
            InitializeComponent();
        }
        

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPropStep");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EnableButton.xiaoquWater)
            {
                GcManage.gcType = "5_2";
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2TanAnNum))
                {
                    //泰安
                    //物业2水
                    Payment.propSecPayParam.CardType = "01";
                }
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2JinQiNum))
                {
                    //锦旗
                    //物业2水
                    Payment.propSecPayParam.CardType = "01";
                }
                //Util.JumpUtil.jumpCommonPage("FormPropSec01_2");
                Util.JumpUtil.jumpCommonPage("FormPropSecStep02");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (EnableButton.xiaoquElec)
            {
                //物业2
                //电
                GcManage.gcType = "5_2";
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2TanAnNum))
                {
                    //泰安
                    //物业2电
                    Payment.propSecPayParam.CardType = "02";
                }
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2JinQiNum))
                {
                    //锦旗
                    //物业2电
                    Payment.propSecPayParam.CardType = "02";
                }
                //Util.JumpUtil.jumpCommonPage("FormPropSec01_2");
                Util.JumpUtil.jumpCommonPage("FormPropSecStep02");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (EnableButton.xiaoquWater)
            {
                GcManage.gcType = "5_2";
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2TanAnNum))
                {
                    //泰安
                    //物业2水
                    Payment.propSecPayParam.CardType = "01";
                }
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2JinQiNum))
                {
                    //锦旗
                    //物业2水
                    Payment.propSecPayParam.CardType = "05";
                }
                //Util.JumpUtil.jumpCommonPage("FormPropSec01_2");
                Util.JumpUtil.jumpCommonPage("FormPropSecStep02");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (EnableButton.xiaoquElec)
            {
                //物业2
                //电
                GcManage.gcType = "5_2";
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2TanAnNum))
                {
                    //泰安
                    //物业2电
                    Payment.propSecPayParam.CardType = "02";
                }
                if (ConfigPropParam.Prop2ManufacturerNum.Equals(DictParam.Prop2JinQiNum))
                {
                    //锦旗
                    //物业2电
                    Payment.propSecPayParam.CardType = "06";
                }
                //Util.JumpUtil.jumpCommonPage("FormPropSec01_2");
                Util.JumpUtil.jumpCommonPage("FormPropSecStep02");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Payment.propPayTempParam = null;
            Payment.propPayTempParam = new PropPayTempParam();
            Payment.propSecPayParam = null;
            Payment.propSecPayParam = new PropSecPayParam();
            try
            {
                if ("W001".Equals(PropReadCard()))
                {
                    //泰安
                    Payment.propSecPayParam.ManufacturerNum = DictParam.Prop2TanAnNum;
                    //当前物业读卡器厂商编号
                    ConfigPropParam.Prop2ManufacturerNum = DictParam.Prop2TanAnNum;
                    log.Write("厂商编号:物业泰安");
                }
                if ("W002".Equals(PropReadCard()))
                {
                    //锦旗
                    Payment.propSecPayParam.ManufacturerNum = DictParam.Prop2JinQiNum;
                    //当前物业读卡器厂商编号
                    ConfigPropParam.Prop2ManufacturerNum = DictParam.Prop2JinQiNum;
                    log.Write("厂商编号:物业锦旗");
                }
                //获取小区编号
                ConfigSysParam.ResidentialNo = xiaoqubianhao();
            }
            catch (Exception ex) { log.Write("error:1232131" + ex.Message + ex.InnerException); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
        public static string PropReadCard()
        {
            try
            {
                string filePath = "C://wtPayConfig//PropCardType.txt";
                if (!System.IO.File.Exists(filePath)) return null;
                return System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (IOException ioe) { return null; }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// 小区编号
        /// </summary>
        /// <returns></returns>
        public static string xiaoqubianhao()
        {
            try
            {
                string filePath = "C://wtPayConfig//ResidentialNo.txt";
                if (!System.IO.File.Exists(filePath)) return null;
                return System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (IOException ioe) { return null; }
            catch (Exception ex) { return null; }
        }
    }
}
