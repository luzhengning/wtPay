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

namespace wtPay.FormProp
{
    /// <summary>
    /// FormPropStep.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropStep : UserControl
    {
        Button[] btns = new Button[4];

        string waterIC = "物业IC水卡缴纳";
        string waterRFID = "物业射频水卡缴纳";
        string elecIC = "物业IC电卡缴纳";
        string elecRFID = "物业射频电卡缴纳";

        List<string> btnName = new List<string>();

        public FormPropStep()
        {
            InitializeComponent();
            btns[0] = btn00;
            btns[1] = btn01;
            btns[2] = btn02;
            btns[3] = btn03;

        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EnableButton.propHouse)
            {
                Payment.PropPayParam.PropType = 1;
                Util.JumpUtil.jumpCommonPage("FormPropStep01");
            }else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btnName.Clear();
            try
            {
                for (int i = 0; i < btns.Length; i++)
                {
                    btnGroup.Children.Remove(btns[i]);
                }
            }
            catch (Exception x) { }
            try
            {
                if ((waterRfidOrIC() != null))
                {
                    if (waterRfidOrIC().Length > 0)
                    {
                        if (waterRfidOrIC().Contains("IC"))
                        {
                            btnName.Add(waterIC);
                        }
                        if (waterRfidOrIC().Contains("RFID"))
                        {
                            btnName.Add(waterRFID);
                        }
                    }
                }
                if ((elecRfidOrIC() != null))
                {
                    if (elecRfidOrIC().Length > 0)
                    {
                        if (elecRfidOrIC().Contains("IC"))
                        {
                            btnName.Add(elecIC);
                        }
                        if (elecRfidOrIC().Contains("RFID"))
                        {
                            btnName.Add(elecRFID);
                        }
                    }
                }
               
                btnGroup.Children.Remove(btn00);
                btnGroup.Children.Remove(btn01);
                btnGroup.Children.Remove(btn02);
                btnGroup.Children.Remove(btn03);
                for (int i = 0; i < btnName.Count; i++)
                {
                    btns[i].Uid = btnName[i];
                    if (btnGroup.Children.Contains(btns[i])) btnGroup.Children.Remove(btns[i]);
                    btnGroup.Children.Add(btns[i]);
                }
            }
            catch (Exception ex) { }

            Payment.PropPayParam = null;
            Payment.PropPayParam = new PropPayParam();
            Payment.propPayTempParam = null;
            Payment.propPayTempParam = new PropPayTempParam();
            Payment.propSecPayParam = null;
            Payment.propSecPayParam = new PropSecPayParam();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (EnableButton.propPark)
            {
                Payment.PropPayParam.PropType = 2;
                Util.JumpUtil.jumpCommonPage("FormPropStep01");
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
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
        /// <summary>
        /// 水
        /// </summary>
        /// <returns></returns>
        public static string waterRfidOrIC()
        {
            try
            {
                string filePath = "C://wtPayConfig//propWater.txt";
                if (!System.IO.File.Exists(filePath)) return null;
                return System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (IOException ioe) { return null; }
            catch (Exception ex) { return null; }
        }
        /// <summary>
        /// 电
        /// </summary>
        /// <returns></returns>
        public static string elecRfidOrIC()
        {
            try
            {
                string filePath = "C://wtPayConfig//propElec.txt";
                if (!System.IO.File.Exists(filePath)) return null;
                return System.IO.File.ReadAllText(filePath).Trim();
            }
            catch (IOException ioe) { return null; }
            catch (Exception ex) { return null; }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (EnableButton.xiaoquElec)
            {
                Payment.propSecPayParam.cardInfoMsg = "请将电卡放入指定位置";
                //物业2
                //电
                GcManage.gcType = "5_2";
                if ("RFID".Equals(elecRfidOrIC()))
                {
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Elec_RFID;
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
                if ("IC".Equals(elecRfidOrIC())) {
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Elec_IC;
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
            }
            else
            {
                Util.JumpUtil.jumpCommonPage("FormNot");
            }
        }

        private void btn00_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            log.Write("物业按钮名称："+btn.Uid);
            if (btn.Uid.Equals(waterIC))
            {
                if (EnableButton.xiaoquWater)
                {
                    Payment.propSecPayParam.cardInfoMsg = "请将水卡放入指定位置";
                    GcManage.gcType = "5_2";
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Water_IC;
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
            else if (btn.Uid.Equals(waterRFID)) {
                if (EnableButton.xiaoquWater)
                {
                    Payment.propSecPayParam.cardInfoMsg = "请将水卡放入指定位置";
                    GcManage.gcType = "5_2";
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Water_RFID;
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
            else if (btn.Uid.Equals(elecIC)) {
                if (EnableButton.xiaoquElec)
                {
                    Payment.propSecPayParam.cardInfoMsg = "请将电卡放入指定位置";
                    //物业2
                    //电
                    GcManage.gcType = "5_2";
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Elec_IC;
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
            else if (btn.Uid.Equals(elecRFID)) {
                if (EnableButton.xiaoquElec)
                {
                    Payment.propSecPayParam.cardInfoMsg = "请将电卡放入指定位置";
                    //物业2
                    //电
                    GcManage.gcType = "5_2";
                    ConfigSysParam.gifBusiness = GifBusiness.prop2Elec_RFID;
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

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                for(int i = 0; i < btns.Length; i++)
                {
                    btnGroup.Children.Remove(btns[i]);
                }
            }catch(Exception x) { }
        }
    }
}
