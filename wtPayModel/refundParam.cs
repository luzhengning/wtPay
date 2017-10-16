using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel
{
    public class refundParam
    {
        public string servicename;// TK001 not null
	    public string  loginId;//交互终端的设备编号（用于自助终端）not null
        public string  authcode;// 认证码 not null
        public string  reqsn;//请求流水号 not null
        public string  trandateTime;//  交易时间 格式：YYYYMMDDHHMMSS not null
        public string  orderno;//订单编号 not null
        public string  transType;//线上线下 not null
        public string  conName;//线下某某某退款，比如：线下联通退款、线下万通卡退款等 not null
    }
}
