using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.Mobile
{
    public class MobileOrderParam
    {
        public string shopType { get; set; }//  商户类型 String  否	1万通 0拉卡拉
        public string servicename { get; set; }// 交易编码 String  否 交易编号LT002
        public string loginId { get; set; }//用户登录名   String 否   交互终端的设备编号（用于自助终端）
        public string reqsn { get; set; }//交易发起方流水号    String 否
        public string authcode { get; set; }//认证码 String 否   认证码，由DL001接口返回
        public string trandateTime { get; set; }//   交易时间 String  否 YYYYMMDDHHmmss



        public string paymentAmout { get; set; }//缴费金额    String 否
        public string mobile { get; set; }//手机号 String 否
        public string homeRegion { get; set; }// 归属地区    String 否   由YD001接口返回
        public string homeOffice { get; set; }//归属局向 String  否 由YD001接口返回
        public string contractNo { get; set; }// 合同号码    String 否   由YD001接口返回
        public string overdueMoney { get; set; }//合同号码欠费总金额 String  否 由YD001接口返回
        public string prepaidBalance { get; set; }// 预付费原有余额 String 否   由YD001接口返回
        public string changeBalance { get; set; }//零头费用 String  否 由YD001接口返回
        public string channelNo { get; set; }//渠道标识    String 否	002：自助机交费


    }
}
