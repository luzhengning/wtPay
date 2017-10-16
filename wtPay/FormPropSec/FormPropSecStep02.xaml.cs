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
using wtPayModel;
using wtPayDAL;
using wtPayModel.PropSecModel;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Security;
using wtPayModel.ConfigModel;
using System.Windows.Controls.Primitives;
using wtPayModel.PaymentModel;

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGasGoldenCardStep02.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSecStep02 : UserControl
    {
        private GifImage waterImage=null;

        Thread readQuery = null;
        //读卡器访问类
        private JinCardBLL jinCard = new JinCardBLL();

        private delegate void setTextBlockTextDelegate(TextBlock textBlock,string value);

        bool isRead = true;
        public FormPropSecStep02()
        {
            InitializeComponent();
           
            
        }
        private void setTextBlockText(TextBlock textBlock, string value)
        {
            textBlock.Text = value;
        }
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isRead == false) return;
                readQuery = new Thread(delegate () { read(); });
                readQuery.Start();
            }catch(Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02:FormGasGoldenCardStep02:" + ex.Message);
            }
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormPropStep");
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                waterImage = null;
                switch (ConfigSysParam.gifBusiness)
                {
                    case GifBusiness.prop2Water_IC:
                        waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\waterGIF\\IC\\IC.gif");
                        showInfo.Text = "请将水卡插入指定读卡器内";
                        break;
                    case GifBusiness.prop2Water_RFID:
                        waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\waterGIF\\RFID\\RFID.gif");
                        showInfo.Text = "请将水卡放在身份证磁感区内";
                        break;
                    case GifBusiness.prop2Elec_IC:
                        waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\elecGIF\\IC\\IC.gif");
                        showInfo.Text = "请将电卡插入指定读卡器内";
                        break;
                    case GifBusiness.prop2Elec_RFID:
                        waterImage = new GifImage(System.AppDomain.CurrentDomain.BaseDirectory + "\\sysImage\\GIF\\elecGIF\\RFID\\RFID.gif");
                        showInfo.Text = "请将电卡卡放在身份证磁感区内";
                        break;
                }
                this.waterImage.Height = 500;
                showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, Payment.propSecPayParam.cardInfoMsg);
                Popup dd = new Popup();
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                imgGrid.Children.Add(waterImage);
                this.waterImage.StartAnimate();
                okBtn.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { log.Write("error:" + ex.Message); }
            
        }
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private void read()
        {
            try
            {
                isRead = false;
                try
                {
                    SysBLL.Player("读取中.wav");
                    showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, "正在读卡，请稍后...");
                    PropSecAccess access = new PropSecAccess();
                    PropSecQueryParam param = new PropSecQueryParam();
                    param.SC10009 = Payment.propSecPayParam.ManufacturerNum;
                    param.SC10010 = Payment.propSecPayParam.CardType;
                    param.SC10011 = "01";
                    param.ResidentialNo = ConfigSysParam.ResidentialNo;
                    PropSecQueryInfo info = access.query(param);
                    if (!"0000".Equals(info.msgrsp.retcode))
                    {
                        PrintInfo(info.msgrsp.retshow);
                        return;
                    }
                    StringBuilder result1 = new StringBuilder(2048);
                    StringBuilder result2 = new StringBuilder(2048);
                    log.Write("物业2读卡：业务类型：08，卡片种类："+ Payment.propSecPayParam.CardType+",业务步骤："+ info.msgrsp.SC10011+",表具厂商编号："+ Payment.propSecPayParam.ManufacturerNum+",输入信息："+ info.msgrsp.SC20003+",端口："+ SysConfigHelper.readerNode("PropSwwyName"));
                    IntPtr status = PropSwwy.WF002(
                        new StringBuilder("08"),//业务类型
                        new StringBuilder(Payment.propSecPayParam.CardType),//卡片种类
                        new StringBuilder("01"),//卡片版本
                        new StringBuilder(info.msgrsp.SC10011),//，业务步骤
                        new StringBuilder(""),//卡片唯一识别号
                        new StringBuilder(""),//物业公司编号
                        new StringBuilder(""),//小区编号
                        new StringBuilder(Payment.propSecPayParam.ManufacturerNum),//表具产商编号
                        new StringBuilder(SysConfigHelper.readerNode("PropSwwyName")),//端口号
                        result1,//返回说明
                        new StringBuilder(info.msgrsp.SC20003),//业务输入信息
                        result2//业务返回信息
                        );
                    string result = Marshal.PtrToStringAnsi(status);
                    log.Write("物业2读卡返回："+result);
                    if (!"0".Equals(result))
                    {
                        PrintInfo("读取失败，请重试...");
                        isRead = true;
                        return;
                    }
                    PropSecCardJson card = new PropSec().JsonToModel(result2.ToString());
                    log.Write("result2.ToString():"+ result2.ToString());
                    Payment.propSecPayParam.propMeterInfo = access.queryMeter(ConfigSysParam.ResidentialNo, card.G_1802);
                    log.Write("获取表具列表返回码："+ Payment.propSecPayParam.propMeterInfo.msgrsp.retcode);
                    if (!"0000".Equals(Payment.propSecPayParam.propMeterInfo.msgrsp.retcode))
                    {
                        if (Payment.propSecPayParam.propMeterInfo.msgrsp.retshow != null)
                        {
                            if (Payment.propSecPayParam.propMeterInfo.msgrsp.retshow.Length > 0)
                            {
                                showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, Payment.propSecPayParam.propMeterInfo.msgrsp.retshow);
                            }
                            else
                            {
                                showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, "读卡失败，请稍后再试...");
                            }
                        }
                        else
                        {
                            showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, "读卡失败，请稍后再试...");
                        }
                        SysBLL.Player("读物业卡2失败.wav");
                        return;
                    }
                    if (DictParam.Prop2TanAnNum.Equals(Payment.propSecPayParam.ManufacturerNum))
                    {
                        log.Write("G_2015:" + card.G_2015);
                        if (card.G_2015 != null)
                        {
                            if (!"0".Equals(card.G_2015))
                            {
                                showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, "请将上次的气量插入到表内再进行充值！");
                                return;
                            }
                        }
                        else
                        {
                            showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, "读卡失败，请稍后再试....");
                            SysBLL.Player("读物业卡2失败.wav");
                            return;
                        }
                    }
                    Payment.propSecPayParam.SC10007 = card.G_0806;
                    Payment.propSecPayParam.SC10008 = card.G_1802;
                    Payment.propSecPayParam.merchantNo = info.msgrsp.merchantNo;
                    Util.JumpUtil.jumpCommonPage("FormPropSec01_2");
                    return;
                }
                catch (ThreadAbortException ae) { log.Write("error:物业2读卡异常：" + ae.Message); }
                catch (Exception ex)
                {
                    PrintInfo("读取失败，请重试...");
                    SysBLL.Player("读物业卡2失败.wav");
                    log.Write("error:FormGasGoldenCardStep02:UserControl_Loaded:" + ex.Message);
                    return;
                }

            }
            catch (AccessViolationException ave)
            {
                PrintInfo("读取失败，请重试....");
                //SysBLL.Player("读物业卡2失败.wav");
                log.Write("error1:"+ave.Message);
            }
            catch (ThreadAbortException ae) { log.Write("error:物业2读卡异常："+ae.Message); }
            catch (WtException e)
            {
                PrintInfo("读取失败，请重试....");
                //SysBLL.Player("读物业卡2失败.wav");
                log.Write("error:2" + e.Message);
            }
            catch (Exception e)
            {
                PrintInfo("读取失败，请重试....");
                SysBLL.Player("读物业卡2失败.wav");
                log.Write("error:3" + e.Message);
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
            showInfo.Dispatcher.Invoke(new setTextBlockTextDelegate(setTextBlockText), showInfo, msg);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imgGrid.Children.Contains(waterImage)) imgGrid.Children.Remove(waterImage);
                this.waterImage.StopAnimate();
                readQuery.Abort();
                readQuery.DisableComObjectEagerCleanup();
                readQuery = null;
            }
            catch (Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep02：UserControl_Unloaded："+ex.Message);
            }
        }

        private void lblInsertGasCardTip_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SysBLL.Player("请将卡片放在区域内.wav");
                showInfo.Text = Payment.propSecPayParam.cardInfoMsg;
                if (Payment.propSecPayParam.ManufacturerNum == null)
                {
                    showInfo.Text = "此小区暂不支持该业务";
                    okBtn.Visibility = Visibility.Hidden;
                }
            }catch(Exception ex) { }
        }
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
    }
}
