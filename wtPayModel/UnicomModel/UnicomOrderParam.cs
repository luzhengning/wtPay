using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomOrderParam
    {
        public string authcode;// 认证码 not null
        public string servicename;// = LT002 交易号 not null
        public string trandateTime;// 交易时间  格式：YYYYMMDDHHMMSS
        public string reqsn;// 请求流水号 not null
        public string loginId;//交互终端的设备编号（用于自助终端） 
        public string phoneNo;// 电话号码  not null[对应文档中的PHONE_NO]
        public string paymentAmout;// 缴费金额  not null  [LT001接口返回的PAYMENT_AMOUNT]
        public string account_no;
        public string shopType;
    }
}
