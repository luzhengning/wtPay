using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BroadCas
{
    /// <summary>
    /// 支付接口通知返回
    /// </summary>
    public class BroadCasPayresInfo
    {
        public BroadCasPayresMsghead msghead { get; set; }
        public BroadCasPayresMsgrsp msgrsp { get; set; }
    }
    public class BroadCasPayresMsghead
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
    public class BroadCasPayresMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }

    }
}
