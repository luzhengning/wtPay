using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BroadCas
{
    public class BroadCasOrderInfo
    {
        public BroadCasOrderMsghead msghead { get; set; }
        public BroadCasOrderMsgrsp msgrsp { get; set; }
    }
    public class BroadCasOrderMsghead
    {
        public string loginId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string authcode { get; set; }
        public string trandateTime { get; set; }
        public string tranchannel { get; set; }
        public string servicename { get; set; }
        public string reqsn { get; set; }
        public string clientIp { get; set; }
    }

    public class BroadCasOrderMsgrsp
    {
        public string paymentNo { get; set; }
        public string orderNo { get; set; }
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string retshow { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }

    }
}
