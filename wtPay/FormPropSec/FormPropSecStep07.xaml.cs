using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using wtPay.pay;
using wtPayBLL;
using WtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayDAL.Pay;
using wtPayModel;
using wtPayModel.GasModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;
using wtPayModel.PropSecModel;
using wtPayModel.SystemModel;
using wtPayModel.WintopModel;

namespace wtPay.FormPropSec
{
    /// <summary>
    /// FormGasGoldenCardStep07.xaml 的交互逻辑
    /// </summary>
    public partial class FormPropSecStep07 : UserControl
    {
        BitmapImage jinImage = new BitmapImage(new Uri("/cut-2/金卡.png", UriKind.Relative));
        BitmapImage xianfengImage = new BitmapImage(new Uri("/cut-2/先锋.png", UriKind.Relative));

        PasswordBLL pwdBLL = new PasswordBLL();

        //甘肃一卡通卡读卡器
        WantongBLL wt = new WantongBLL();
        

        Dictionary<string, string> icParams = null;
        //支付线程
        Thread payThread = null;
        public FormPropSecStep07()
        {
            InitializeComponent();
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
                SysBLL.Player("交易处理中，请稍后.wav");
                icParams = Payment.propSecPayParam.IcParams;
                //if (card.cardType == 1)
                //{
                //    //金卡
                //    img1.Source = jinImage;
                //}
                //else
                //{
                //    //先锋卡
                //    img1.Source = xianfengImage;
                //}

                payThread = new Thread(delegate () { pay(); });
                payThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormGasGoldenCardStep07:load():" + ex.Message);
                exit("充值失败，请稍后再试...");
            }
        }
        public void pay()
        {
            log.Write("--------------------交易开始--------------------");
            log.Write("----------缴费类型：物业二次");
            PayAccess payAccess = new PayAccess();
            PayParam p = Payment.propSecPayParam.p;
            Pay pay = new Pay();
            //订单结果
            PropSecOrderInfo orderinfo = Payment.propSecPayParam.orderInfo;
            //初始化参数
            payAccess.InitPayParam(ref p);
            PayBLL.payCode_terminalNo(ref p);
            try
            {
                //获取订单
                //orderInfo(ref orderinfo, p);
                //获取订单结果处理
                if (pay.orderInfoResult(orderinfo.msgrsp.retcode, orderinfo.msgrsp.retshow) == false) return;
                //支付
                if (pay.payResult(ref p))
                {
                    bool isCarsWrite = false;
                    //写卡状态
                    WriteCardParam writeCardParam = new WriteCardParam();

                    StringBuilder result1 = new StringBuilder(2048);
                    StringBuilder result2 = new StringBuilder(2048);
                    log.Write("物业2读卡：业务类型：08，卡片种类：" + Payment.propSecPayParam.CardType + ",表具厂商编号：" + Payment.propSecPayParam.ManufacturerNum + ",业务输入信息：" + p.propSecSC20003);
                    IntPtr status = PropSwwy.WF002(
                        new StringBuilder("02"),//业务类型
                        new StringBuilder(Payment.propSecPayParam.CardType),//卡片种类
                        new StringBuilder("01"),//卡片版本
                        new StringBuilder(""),//，业务步骤
                        new StringBuilder(""),//卡片唯一识别号
                        new StringBuilder(""),//物业公司编号
                        new StringBuilder(""),//小区编号
                        new StringBuilder(Payment.propSecPayParam.ManufacturerNum),//表具产商编号
                        new StringBuilder(SysConfigHelper.readerNode("PropSwwyName")),//端口号
                        result1,//返回说明
                        new StringBuilder(p.propSecSC20003),//业务输入信息
                       result2//业务返回信息
                        );
                    string result = Marshal.PtrToStringAnsi(status);

                    if ("0".Equals(result))
                    {
                        //写卡成功
                        writeCardParam.write_card_status = "1";
                        isCarsWrite = true;
                    }
                    else
                    {
                        //写卡失败
                        writeCardParam.write_card_status = "3";
                        PayResultInfo payresultInfo = p.payResultInfo;
                        p.rqFlushesCode = "SC005";
                        //pay.refund(ref payresultInfo, p);
                        isCarsWrite = false;
                    }
                    //PropSecCardJson card = new PropSec().JsonToModel(result2.ToString());
                    writeCardParam.serviceType = p.serviceType;
                    writeCardParam.rqFlushesCode = isCardTypeRq();
                    writeCardParam.shop_type = PayAccess.isWtLkl(p.icParams);
                    writeCardParam.cloud_no = orderinfo.msgrsp.orderNo;
                    writeCardParam.data = pay.refundStr(p.payResultInfo, p);
                    PayResultInfo resultInfo = payAccess.WriteCardAcc(writeCardParam);
                    if (isCarsWrite)
                    {
                        log.Write("--------------------交易结束--------------------");
                        Util.JumpUtil.jumpCommonPage("FormPropSecStep08_success");
                        return;
                    }
                    else
                    {
                        log.Write("error:物业卡写卡失败");
                        FormTip.FormFailRefundShowinfo = resultInfo.msg;
                        Util.JumpUtil.jumpCommonPage("FormFailRefund");
                        return;
                    }
                }
                return;
            }
            catch (AccessViolationException ave) { log.Write("error:" + ave.Message); }
            catch (Exception ex) { log.Write("error:" + ex.Message); }
            exit("缴费失败，请稍后再试...");
            return;
        }
        private void exit(string info)
        {
            FormTip.FormFailShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFail");
        }
        private void exitRefund(string info)
        {
            FormTip.FormFailRefundShowinfo = info;
            log.Write("--------------------交易结束--------------------");
            Util.JumpUtil.jumpCommonPage("FormFailRefund");
        }
        private string isCardTypeRq()
        {
            return "SC005";
        }

        
    }
}
