using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PropModel;
using wtPayBLL;
using System.Configuration;
using Newtonsoft.Json;
using wtPayModel.ElecModel;

namespace wtPayDAL
{
    public class PropInterface
    {

        /// <summary>
        /// /物业登录签到
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Login(ElecLoginParam param)
        {
            ElecLoginInfo elecLoginInfo = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("trandateTime", param.trandateTime);
            parameters.Add("servicename", param.servicename);
            parameters.Add("reqsn", param.reqsn);
            parameters.Add("loginId", param.loginId);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("PropLogin"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            elecLoginInfo = JsonConvert.DeserializeObject<ElecLoginInfo>(jsonText);
            //同步系统时间
            SysBLL.SetSystemTime(elecLoginInfo.msghead.trandatetime);
            return elecLoginInfo.msgrsp.authcode;
        }

        /// <summary>
        /// 获取房屋信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public HouseQueryInfo HouseQuery(HouseQueryParam param)
        {
            HouseQueryInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);// 交互终端的设备编号（用于自助终端）
            parameters.Add("mobile", param.mobile);// not null
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("PropQuery"), parameters, null);
            log.Write("物业房屋查询json："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<HouseQueryInfo>(jsonText);
            return info;
        }

        /// <summary>
        /// 车位查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ParkingLotQueryInfo ParkingLotQuery(ParkingLotQueryParam param) {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);// 交互终端的设备编号（用于自助终端）
            parameters.Add("mobile", param.mobile);//
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("PropParkingLotQuery"), parameters, null);
            log.Write("物业车位json："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            ParkingLotQueryInfo info = JsonConvert.DeserializeObject<ParkingLotQueryInfo>(jsonText);
            return info;
        }

        /// <summary>
        /// 物业费用查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public　PropCostsQueryInfo CostQuery(PropCostsQueryParam param)
        {
            PropCostsQueryInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);// 交互终端的设备编号（用于自助终端）
            parameters.Add("queryid", param.queryid);// 房屋编码【houseid】或者车位编码【parkingid】	String 否
            parameters.Add("communityid", param.communityid);// 小区编码【communityid】	String 否
            parameters.Add("isPark", param.isPark);// 费用编号  String 房屋费用 1，车位费用 0
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("PropCostQuery"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropCostsQueryInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 小区物业缴费信息查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropFeeQueryInfo PropFeeQuery(PropFeeQueryParam param)
        {
            PropFeeQueryInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);// 交互终端的设备编号（用于自助终端）
            parameters.Add("paymentno", param.paymentno);// 房屋编码【houseid】或者车位编码【parkingid】	String 否
            parameters.Add("AMOUNT", param.AMOUNT);// 小区编码【communityid】	String 否
            parameters.Add("TYPE", param.TYPE);// 费用编号  String 房屋费用 1，车位费用 0
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("propFeeQueryName"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropFeeQueryInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 小区物业获取订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropOrderInfo getPropOrder(PropOrderParam param)
        {
            PropOrderInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);//交互终端的设备编号（用于自助终端） 

            parameters.Add("paymentno", param.paymentno);// 费用编码【物业费用查询(WY004)时的queryid】	String 否   由WY002接口返回
            parameters.Add("merchantNo", param.merchantNo);// 缴费金额 String 否   由WY004接口返回【可单笔缴费，对应每一笔的money，可整体缴费，所有缴费金额和】
            parameters.Add("AMOUNT", param.AMOUNT);
            parameters.Add("HOUSEID", param.HOUSEID);
            parameters.Add("shopType", param.shopType);
            parameters.Add("paymentAmout", param.paymentAmout);
            parameters.Add("TYPE", param.TYPE);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderProp"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropOrderInfo>(jsonText);
            return info;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropOrderInfo Order(PropOrderParam param)
        {
            PropOrderInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
            parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);//交互终端的设备编号（用于自助终端） 
            parameters.Add("queryid", param.queryid);// 费用编码【物业费用查询(WY004)时的queryid】	String 否   由WY002接口返回
            parameters.Add("paymentAmout", param.paymentAmout);// 缴费金额 String 否   由WY004接口返回【可单笔缴费，对应每一笔的money，可整体缴费，所有缴费金额和】
            parameters.Add("shopType", param.shopType);
            parameters.Add("isOld", "0");
            parameters.Add("isPark", param.isPark);
            parameters.Add("merchantNo", param.merchantNo);
            parameters.Add("terminalNo", ConfigurationManager.AppSettings["MechineNo"]);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("getOrderProp"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropOrderInfo>(jsonText);
            return info;
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropPayResInfo payRes(PropPayResParam param)
        {
            PropPayResInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("authcode", param.authcode);// 认证码 not null
	        parameters.Add("servicename", param.servicename);// 交易号 not null
            parameters.Add("trandateTime", param.trandateTime);// 交易时间  格式：YYYYMMDDHHMMSS
            parameters.Add("reqsn", param.reqsn);// 请求流水号 not null
            parameters.Add("loginId", param.loginId);//交互终端的设备编号（用于自助终端）
	        parameters.Add("orderno", param.orderno);// 订单编号 not null
            parameters.Add("mobile", param.mobile);// 缴费电话号码  Not null  
            parameters.Add("chargeseids", param.chargeseids);//     费用编码	String	否 WY004返回的chargeid。多个使用“-”分隔 【单笔缴费传单笔缴费金额对应的chargeid 多笔合计缴费传多笔对应的chargeid,'-'连接】
            parameters.Add("trandeNo", param.trandeNo);// 支付渠道交易流水号 not null【银行支付流水号】
            parameters.Add("realAmout", param.realAmout);// 实际支付金额 not null
            parameters.Add("payCode", param.payCode);// 支付渠道编码 not null 
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("PropPayres"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropPayResInfo>(jsonText);
            return info;
        }
    }
}
