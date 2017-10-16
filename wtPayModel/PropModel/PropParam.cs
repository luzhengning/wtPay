using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropModel
{
    /// <summary>
    /// 查询车位参数
    /// </summary>
    public class ParkingLotQueryParam
    {
        public string authcode;// 认证码 not null
	    public string  servicename;// 交易号 not null
        public string  trandateTime;//交易时间 格式：YYYYMMDDHHMMSS
        public string  reqsn;//请求流水号 not null
        public string  loginId;//交互终端的设备编号（用于自助终端）
        public string  mobile;//not null
    }

    /// <summary>
    /// 房屋查询
    /// </summary>
    public class HouseQueryParam
    {
        public string authcode;// 认证码 not null
	    public string  servicename;//交易号 not null
        public string  trandateTime;//交易时间 格式：YYYYMMDDHHMMSS
        public string  reqsn;//请求流水号 not null
        public string  loginId;//交互终端的设备编号（用于自助终端）
        public string  mobile;//not null
    }
    /// <summary>
    /// 车位查询
    /// </summary>
    public class PropCostsQueryParam
    {
        public string authcode;// 认证码 not null
	    public string  servicename;//交易号 not null
        public string  trandateTime;//交易时间 格式：YYYYMMDDHHMMSS
        public string  reqsn;//请求流水号 not null
        public string  loginId;//交互终端的设备编号（用于自助终端）
        public string  queryid;//房屋编码【houseid】或者车位编码【parkingid】	String 否
        public string  communityid;//小区编码【communityid】	String 否	
        public string  isPark;//费用编号 String    房屋费用 1，车位费用 0 
    }
    /// <summary>
    /// 物业缴费信息查询
    /// </summary>
    public class PropFeeQueryParam
    {
        public string servicename { get; set; }// 交易编码    String 否   交易编号WY5101
        public string loginId { get; set; }//用户登录名 String  否 交互终端的设备编号（用于自助终端）
        public string reqsn { get; set; }//交易发起方流水号    String 否
        public string authcode { get; set; }//认证码 String 否   认证码，由DL001接口返回
        public string trandateTime { get; set; }//   交易时间 String  否 YYYYMMDDHHmmss
        public string paymentno { get; set; }//缴费户号    String 否
        public string AMOUNT { get; set; }// 购买量 String 否   必须为整数
        public string TYPE { get; set; }//  缴费类型 String  否	 01——水
    }
    /// <summary>
    /// 物业缴费信息
    /// </summary>
    public class PropFeeQueryInfo
    {
        public PropMsghead msghead { get; set; }
        public PropFeeQueryMsgrsp msgrsp { get; set; }
    }

    public class PropFeeQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string resMsg { get; set; }
        public PropFeeQueryList result { get; set; }
        public string merchantNo { get; set; }

    }
    public class PropFeeQueryList
    {
       public string USERNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string MONEY { get; set; }
        public string AMOUNT { get; set; }
        public string TYPE { get; set; }
        public string HOUSEID { get; set; }

    }
    /// <summary>
    /// 提价订单参数
    /// </summary>
    public  class PropOrderParam
    {
        public string authcode { get; set; }// 认证码 not null
        public string shopType { get; set; }
        public string  servicename{ get; set; }//交易号 not null
        public string  trandateTime{ get; set; }//交易时间  格式：YYYYMMDDHHMMSS
        public string  reqsn{ get; set; }//请求流水号 not null
        public string  loginId{ get; set; }//交互终端的设备编号（用于自助终端） 
        public string  queryid{ get; set; }//费用编码【物业费用查询(WY004)时的queryid】	String	否	由WY002接口返回 
        public string  paymentAmout{ get; set; }//缴费金额	String	否	由WY004接口返回【可单笔缴费，对应每一笔的money，可整体缴费，所有缴费金额和】
        public string paymentno { get; set; }
        public string merchantNo { get; set; }
        public string AMOUNT { get; set; }
        public string HOUSEID { get; set; }        public string TYPE { get; set; }

        public string isPark { get; set; }


    }

    public class PropPayResParam
    {
        public string  authcode;// 认证码 not null
	    public string  servicename;//交易号 not null
        public string  trandateTime;//me 交易时间 格式：YYYYMMDDHHMMSS
        public string  reqsn;//请求流水号 not null
        public string  loginId;//交互终端的设备编号（用于自助终端）
        public string  orderno;//订单编号 not null
        public string  mobile;//缴费电话号码 Not null  
        public string  chargeseids;//费用编码 String  否 WY004返回的chargeid。多个使用“-”分隔 【单笔缴费传单笔缴费金额对应的chargeid 多笔合计缴费传多笔对应的chargeid,'-'连接】
        public string  trandeNo;//支付渠道交易流水号 not null【银行支付流水号】
        public string  realAmout;//实际支付金额 not null
        public string  payCode;//支付渠道编码 not null 
    }

    

    public class PropMsghead
    {
        public string trandatetime
        { get; set; }
        public string ressn
        { get; set; }
        public string tranchannel
        { get; set; }
        public string devno
        { get; set; }
        public string resqn
        { get; set; }
        public string servicename
        { get; set; }
        public string version
        { get; set; }
    }

    /// <summary>
    /// 查询的房屋信息
    /// </summary>
    public class HouseQueryInfo
    {
        public HouseQueryMsgrsp msgrsp { get; set; }
        public PropMsghead msghead { get; set; }
    }

    public class HouseQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<wyDataHouseList> wyDataHouseList { set; get; }
    }

    public class wyDataHouseList
    {
        public string housemsg { get; set; }
        public string houseid { get; set; }
        public string communityid { get; set; }
        public string is_def { get; set; }
    }

    /// <summary>
    /// 查询的车位
    /// </summary>
    public class ParkingLotQueryInfo
    {
        public ParkingLotQueryMsgrsp msgrsp { get; set; }
        public PropMsghead msghead { get; set; }
    }
    public class ParkingLotQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<wyDataParkList> wyDataParkList { set; get; }
    }
    public class wyDataParkList
    {
        public string parkingid { get; set; }
        public string parknum { get; set; }
        public string communityid { get; set; }
        public string parkplace { get; set; }
        public string parkingmsg { get; set; }
    }

    /// <summary>
    /// 查询的房屋信息
    /// </summary>
    public class PropCostsQueryInfo
    {
        public PropCostsQueryMsgrsp msgrsp { get; set; }
        public PropMsghead msghead { get; set; }
    }

    public class PropCostsQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string housemsg { get; set; }
        public string personname { get; set; }
        public string deptname { get; set; }
        public string stime { get; set; }
        public string searchmonth { get; set; }
        public List<wyDataChargeList> wyDataChargeList { set; get; }
        public string merchantNo { get; set; }
    }
    public class wyDataChargeList
    {
        public string chargeid { get; set; }
        public string standardname { get; set; }
        public string money { get; set; }
        public string late_fees { get; set; }
        public string stime { get; set; }
        public string etime { get; set; }
        public string chargepid { get; set; }
        public string chargetype { get; set; }
        public string chargetypename { get; set; }
        public string adjustMoney { get; set; }
    }


    /// <summary>
    /// 获取提交订单
    /// </summary>
    public class PropOrderInfo
    {
        public PropOrderMsgrsp msgrsp { get; set; }
        public PropMsghead msghead { get; set; }
    }
    public class PropOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
        public string paymentNo { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
    }

    /// <summary>
    /// 支付通知
    /// </summary>
    public class PropPayResInfo
    {
        public PropPayResMsgreq msgrsp { get; set; }
        public PropMsghead msghead { get; set; }
    }
    public class PropPayResMsgreq
    {
        public string retcode { get; set; }
        public string retshow { get; set; }

    }

}
