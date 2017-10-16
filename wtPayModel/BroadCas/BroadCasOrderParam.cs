using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BroadCas { 

    /// <summary>
    /// 提交订单参数
    /// </summary>
    public class BroadCasOrderParam
    {
        public string authcode { get; set; }
        public string servicename { get; set; }
        public string trandateTime { get; set; }
        public string reqsn { get; set; }
        public string loginId { get; set; }
        public string paymentno { get; set; }
        public string paymentAmout { get; set; }
        public string balenceNO { get; set; }
        public string shopType { get; set; }
    }
}
