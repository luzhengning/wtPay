using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel;
using wtPayModel.PaymentModel;
using wtPayModel.PayParamModel;

namespace wtPayDAL.Pay
{
    /// <summary>
    /// 交易相关类
    /// </summary>
    public class PayAccess
    {
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SignResultInfo SignAcc(SignParam param)
        {
            SignResultInfo signResultInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("tr.message_type", "1");
            parameters.Add("tr.shop_type", param.shop_type);
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("tr.data", param.data);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("signName"), parameters, null);
            log.Write("error:签到返回："+jsonText);
            signResultInfo = JsonConvert.DeserializeObject<SignResultInfo>(jsonText);
            return signResultInfo;
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo PayResNewAcc(Dictionary<String, String> parameters)
        {
            PayResultInfo info = null;
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("authcode", SysBLL.Authcode);
            parameters.Add("tr.message_type", "2");
            //设备编号
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            string address = "";
            if(PayStaticParam.payType == -1)
            {
                //电子现金
                address=parameters["ipAddress"];
            }
            else
            {
                //识币器
                address = SysConfigHelper.readerNode("cashPayName");
            }
            string jsonText = HttpHelper.getHttp(address, parameters, null);
            info = JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
            log.Write("支付返回："+ jsonText);
            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject data = (JObject)JsonConvert.DeserializeObject(jsonObj["data"].ToString());
            if (data != null)
            {
                info.map = JsonConvert.DeserializeObject<Dictionary<string,string>>(data.ToString());
            }
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return info;
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo RefundAcc(RefundParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("serviceType", param.serviceType);
            parameters.Add("authcode", SysBLL.Authcode);
            parameters.Add("transType", "02");
            parameters.Add("rqFlushesCode", param.rqFlushesCode);
            parameters.Add("tr.message_type", "4");
            parameters.Add("tr.shop_type", param.shop_type);
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("tr.data", param.data);
            parameters.Add("tr.cloud_no", param.cloud_no);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("refundNewName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText); 
        }
        public PayResultInfo testRefundAcc(RefundParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("serviceType", param.serviceType);
            parameters.Add("authcode", SysBLL.Authcode);
            parameters.Add("transType", "02");
            parameters.Add("rqFlushesCode", param.rqFlushesCode);
            parameters.Add("tr.message_type", "4");
            parameters.Add("tr.shop_type", param.shop_type);
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("tr.data", param.data);
            parameters.Add("tr.cloud_no", param.cloud_no);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("refundEndLtest"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo CorrectAcc(CorrectParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("serviceType", param.serviceType);
            parameters.Add("tr.message_type", "3");
            parameters.Add("tr.shop_type", param.shop_type);
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("tr.data", param.data);
            parameters.Add("tr.cloud_no", param.cloud_no);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("correctName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }
        /// <summary>
        /// 写卡状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PayResultInfo WriteCardAcc(WriteCardParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("write_card_status", param.write_card_status);
            parameters.Add("loginId", SysBLL.getCpuNo());
            parameters.Add("reqsn", SysBLL.getSerialNum());
            parameters.Add("serviceType", param.serviceType);
            parameters.Add("authcode", SysBLL.Authcode);
            parameters.Add("transType", "02");
            parameters.Add("rqFlushesCode", param.rqFlushesCode);
            parameters.Add("tr.message_type", "5");
            parameters.Add("tr.shop_type", param.shop_type);
            parameters.Add("tr.t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("tr.data", param.data);
            parameters.Add("tr.cloud_no", param.cloud_no);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("writeCardName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<PayResultInfo>(jsonText);
        }

        /// <summary>
        /// 银行卡交易参数，包括卡号密码，交易金额等
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool InitPayParam(ref PayParam p)
        {
            switch (SysBLL.payCostType)
            {
                case 1:
                    //移动缴费
                    p.serviceType = "9";
                    p.icParams = Payment.mobilePayParam.IcParams;
                    p.WtNo = Payment.mobilePayParam.WtNo;
                    p.pwd = Payment.mobilePayParam.Pwd;
                    p.userInputAmount = Payment.mobilePayParam.UserInputMoney;
                    return true;
                case 2:
                    //联通缴费
                    p.serviceType = "3";
                    p.icParams = Payment.unicomPayParam.IcParams;
                    p.WtNo = Payment.unicomPayParam.WtNo;
                    p.pwd = Payment.unicomPayParam.Pwd;
                    p.userInputAmount = Payment.unicomPayParam.UserInputMoney;
                    return true;
                case 3:
                    //万通卡充值
                    //Payment.wintopReChargeParam.IcParams = icParams;
                    p.serviceType = "4";
                    p.icParams = Payment.wintopReChargeParam.IcParams;
                    p.WtNo = Payment.wintopReChargeParam.WtCardNo;
                    p.pwd = Payment.wintopReChargeParam.Pwd;
                    p.userInputAmount = Payment.wintopReChargeParam.UserInputMoney;
                    return true;
                case 4:
                    //电力缴费
                    //Payment.elecPayParam.WtNo = wtNo;
                    p.serviceType = "6";
                    p.icParams = Payment.elecPayParam.IcParams;
                    p.WtNo = Payment.elecPayParam.WtNo;
                    p.pwd = Payment.elecPayParam.Pwd;
                    p.userInputAmount = Payment.elecPayParam.UserInputMoney;
                    return true;
                case 5:
                    //燃气支付
                    p.serviceType = "1";
                    p.icParams = Payment.GasPayParam.IcParams;
                    p.WtNo = Payment.GasPayParam.WtNo;
                    p.pwd = Payment.GasPayParam.Pwd;
                    p.userInputAmount = Payment.GasPayParam.GasCard.price;
                    return true;
                case 6:
                    //广电缴费
                    p.serviceType = "7";
                    p.icParams = Payment.broadCasPayParam.IcParams;
                    p.WtNo = Payment.broadCasPayParam.WtNo;
                    p.pwd = Payment.broadCasPayParam.Pwd;
                    p.userInputAmount = Payment.broadCasPayParam.UserInputMoney;
                    return true;
                case 7:
                    //水务缴费
                    p.serviceType = "2";
                    p.icParams = Payment.waterPayParam.IcParams;
                    p.WtNo = Payment.waterPayParam.WtNo;
                    p.pwd = Payment.waterPayParam.Pwd;
                    p.userInputAmount = Payment.waterPayParam.WaterQueryInfo.msgrsp.orderlist[0].amout;
                    return true;
                case 8:
                    //热力缴费
                    //Payment.heatPayParam.WtNo = wtNo;
                    p.serviceType = "8";
                    p.icParams = Payment.heatPayParam.IcParams;
                    p.WtNo = Payment.heatPayParam.WtNo;
                    p.pwd = Payment.heatPayParam.Pwd;
                    p.userInputAmount = Payment.heatPayParam.HeatQueryOrderlist.amout;
                    return true;
                case 9:
                    //公交缴费
                    //Payment.BusPayParam.WtNo = wtNo;
                    return true;
                case 10:
                    //物业
                    p.serviceType = "5";
                    p.icParams = Payment.PropPayParam.IcParams;
                    p.WtNo = Payment.PropPayParam.WtNo;
                    p.pwd = Payment.PropPayParam.Pwd;
                    p.userInputAmount = Payment.PropPayParam.ChargeList.money;
                    return true;
                case 11:
                    //小区
                    p.serviceType = "5";
                    p.icParams = Payment.propPayTempParam.IcParams;
                    p.WtNo = Payment.propPayTempParam.WtNo;
                    p.pwd = Payment.propPayTempParam.Pwd;
                    p.userInputAmount = Payment.propPayTempParam.UserInputMoney;
                    return true;
                case 12:
                    //物业2
                    p.serviceType = "5_2";
                    p.icParams = Payment.propSecPayParam.IcParams;
                    p.WtNo = Payment.propSecPayParam.WtNo;
                    p.pwd = Payment.propSecPayParam.Pwd;
                    p.userInputAmount = Payment.propSecPayParam.UserInputMoney;
                    return true;
                default:
                    throw new Exception("缴费类型不匹配");
            }
        }
        public static Boolean LakalaSign()
        {
            try
            {
                SignParam param = LKLProcedure.sign();
                PayAccess access = new PayAccess();
                SignResultInfo signResultInfo = access.SignAcc(param);
                Dictionary<string, string> signResult = signResultInfo.data;
                if (signResult == null) return false;
                log.Write("拉卡拉签到返回码："+ signResult["39"]);
                SysStateParam.lklSignInfo = "返回码：" + signResult["39"];
                if (signResult["39"].Trim().Equals("00"))
                {
                    string _44 = signResult["44"];
                    //获取44域中的密文密钥
                    string mackeyExpress = _44.Substring(_44.Length - 16, 16);
                    string pinKeyExpress = _44.Substring(0, 32);
                    LKLProcedure.RebuildBatchNo();//批次号+1
                    bool ret = Sign(0, mackeyExpress, pinKeyExpress);
                    if (ret)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    log.Write("拉卡拉签到失败！");
                }
                return false;
            }
            catch { return false; }
        }
    
        /// <summary>
        /// 万通签到
        /// </summary>
        /// <returns></returns>
        public static Boolean WantongSign()
        {
            try
            {
                //签到
                SignParam param = WantongBLL.sign();
                PayAccess access = new PayAccess();
                SignResultInfo signResultInfo=access.SignAcc(param);
                Dictionary<string, string> signResult = signResultInfo.data;
                if (signResult == null) return false;
                log.Write("万通签到返回码："+ signResult["39"]);
                SysStateParam.wtSignInfo = "返回码：" + signResult["39"];
                //键盘安装工作秘钥
                string _62 = signResult["62"];
                

                if (signResult["39"].Trim().Equals("00"))
                {
                    //获取44域中的密文密钥
                    string mackeyExpress = _62.Substring(_62.Length - 40, 40);

                    string pinKeyExpress = _62.Substring(0, 40);

                    bool ret = Sign(1, mackeyExpress, pinKeyExpress);
                    return ret;
                }
                return false;
            }
            catch { return false; }
        }
        public static void ReadSign()
        {
            PasswordBLL pwdBLL = new PasswordBLL();
            bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
            if (ret)
            {
                if (SysBLL.wangtonSignResult == false)
                {
                    if (!WantongSign())
                    {
                        SysBLL.wangtonSignResult = false;
                        log.Write("警告：万通签到失败");
                    }
                    else
                    {
                        SysBLL.wangtonSignResult = true;
                        log.Write("万通签到成功");
                    }
                }
                if (SysBLL.lakalaSignResult == false)
                {
                    if (!LakalaSign())
                    {
                        SysBLL.lakalaSignResult = false;
                        log.Write("警告：拉卡拉签到失败");
                    }
                    else
                    {
                        SysBLL.lakalaSignResult = true;
                        log.Write("拉卡拉签到成功");
                    }
                }
            }
            else
            {
                log.Write("警告：键盘打开失败");
            }
        }
        public static void LklWtSign()
        {
            SysStateParam.lklSignInfo = "";
            SysStateParam.wtSignInfo = "";
            try
            {
                PasswordBLL pwdBLL = new PasswordBLL();
                bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
                if (ret)
                {
                    if (!WantongSign())
                    {
                        SysBLL.wangtonSignResult = false;
                        log.Write("警告：万通签到失败");
                        SysFormLoad.loadBar("error：万通签到失败");
                        SysStateParam.wtSignInfo = SysStateParam.wtSignInfo+" 签到失败";
                    }
                    else
                    {
                        SysBLL.wangtonSignResult = true;
                        log.Write("万通签到成功");
                        SysFormLoad.loadBar("万通签到成功");
                        SysStateParam.wtSignInfo = SysStateParam.wtSignInfo + " 签到成功";
                    }
                    if (!LakalaSign())//
                    {
                        SysBLL.lakalaSignResult = false;
                        log.Write("警告：拉卡拉签到失败");
                        SysFormLoad.loadBar("error：拉卡拉签到失败");
                        SysStateParam.lklSignInfo = SysStateParam.lklSignInfo + " 签到失败";
                    }
                    else
                    {
                        SysBLL.lakalaSignResult = true;
                        log.Write("拉卡拉签到成功");
                        SysFormLoad.loadBar("拉卡拉签到成功");
                        SysStateParam.lklSignInfo = SysStateParam.lklSignInfo + " 签到成功";
                    }
                }
                else
                {
                    log.Write("警告：键盘打开失败");
                    SysFormLoad.loadBar("error：密码键盘打开失败");
                }
            }catch(Exception ex) { log.Write("error：签到异常："+ex.Message); }
        }
        public static Boolean Sign(int type, string mackeyExpress, string pinKeyExpress)
        {

            PasswordBLL pwdBLL = new PasswordBLL();

            //bool ret = pwdBLL.OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
            //if (!ret)
            //{
            //    return false;
            //}
            try
            {
                bool setMode = true;
                if (type == 0)
                {
                    setMode = pwdBLL.SetCryptMode(0);
                    log.Write("设置加密模式返回："+setMode);
                    if (!setMode)
                    {
                        return false;
                    }
                    //激活万通主密钥
                    /* if (!pwdBLL.ActivWorkKey(0, 0))
                     {
                         return false;
                     }*/
                    log.Write("下载工作秘钥");
                    bool pingSuccess = pwdBLL.DownloadWorkKey(0, 0, pinKeyExpress);
                    if (!pingSuccess)
                    {
                        return false;

                    }
                    bool macSuccess = pwdBLL.DownloadWorkKey(0, 1, mackeyExpress);
                    if (!macSuccess)
                    {
                        return false;
                    }
                    SysBLL.RebuildBatchNo();

                }
                else if (type == 1)
                {
                    setMode = pwdBLL.SetCryptMode(1);
                    log.Write("设置加密模式返回：" + setMode);
                    if (!setMode)
                    {
                        return false;
                    }
                    //激活万通主密钥
                    /*if(!pwdBLL.ActivWorkKey(1, 0))
                    {
                        return false;
                    }*/
                    log.Write("开始下载工作秘钥" + ",pinKeyExpress:" + pinKeyExpress);
                    bool pingSuccess = pwdBLL.DownloadWorkKey(1, 2, pinKeyExpress);
                    log.Write("下载工作秘钥返回：：" + pingSuccess+ ",pinKeyExpress:"+ pinKeyExpress);
                    if (!pingSuccess)
                    {
                        return false;

                    }
                    log.Write("开始下载工作秘钥" + ",mackeyExpress:" + mackeyExpress);
                    bool macSuccess = pwdBLL.DownloadWorkKey(1, 3, mackeyExpress);
                    log.Write("下载工作秘钥返回：：" + macSuccess+ ",mackeyExpress:"+ mackeyExpress);
                    if (!macSuccess)
                    {
                        return false;
                    }

                }
                pwdBLL.closeKey();
                return true;
            }
            catch(Exception ex) { log.Write("error:Sign():"+ex.Message+ex.InnerException); return false; }
        }

        public static string isWtLkl(Dictionary<string, string> icParam)
        {
            if (PayStaticParam.payType == 0)  return "2";
            if (icParam == null)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
    }
}


