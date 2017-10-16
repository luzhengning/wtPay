using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    /// <summary>
    /// 电网获取订单接口 参数
    /// </summary>
    public class ElecOrderParam
    {
        public string authcode { get; set; }
        public string servicename { get; set; }
        public string trandateTime { get; set; }
        public string reqsn { get; set; }
        public string paymentno { get; set; }
        public string paymentamout { get; set; }
        public string shopType { get; set; }
        public string payCode { get; set; }
    }
}
