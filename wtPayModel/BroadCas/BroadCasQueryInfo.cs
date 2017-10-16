using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BroadCas
{
    /// <summary>
    /// 广电查询返回
    /// </summary>
    public class BroadCasQueryInfo
    {
        public BroadCasQueryMsghead msghead { get; set; }
        public BoadCasQueryrMsgrsp msgrsp { get; set; }
    }
    public class BroadCasQueryMsghead
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
    public class BoadCasQueryrMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string merchantNo { get; set; }
        public BoadCasQueryOrderlist guangDianData { get; set; }

    }
    public class BoadCasQueryOrderlist
    {
        public string BANLANCE { get; set; }
        public string CUSTNAME { get; set; }
        public string CUSTNO { get; set; }
        public string PAYMENTAMOUNT { get; set; }


    }
}
