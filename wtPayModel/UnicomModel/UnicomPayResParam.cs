using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomPayResParam
    {
        public string authcode;// 认证码 not null
	    public string servicename;// 交易号 not null
        public string trandateTime;//交易时间 格式：YYYYMMDDHHMMSS
        public string reqsn;//请求流水号 not null
        public string loginId;//交互终端的设备编号（用于自助终端）
        public string orderno;//订单编号 not null
        public string phoneNo;//手机号码 Not null 【由LT001接口返回 文档中对应PHONE_NO 】
        public string accountNo;//账户号not null 【由LT001接口返回 文档中对应ACCOUNT_NO】
        public string trandeNo;//支付渠道交易流水号 not null【银行支付流水号】
        public string realAmout;//实际支付金额 not null
        public string payCode;//支付渠道编码 not null  
        public string terminalNo;
    }
}
