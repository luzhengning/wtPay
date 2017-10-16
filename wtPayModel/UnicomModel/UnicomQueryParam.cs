using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomQueryParam
    {
        public string authcode;// 认证码 not null
        public string servicename;// = LT001 交易号 not null
        public string trandateTime;// 交易时间  格式：YYYYMMDDHHMMSS
        public string reqsn;// 请求流水号 not null
        public string loginId;// 交互终端的设备编号（用于自助终端）
        public string phoneNo;// not null 电话号码[对应文档中的PHONE_NO]
    }
}
