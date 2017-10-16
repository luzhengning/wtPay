using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomQueryInfo
    {
        public UnicomQueryMsgRsp msgrsp { get; set; }
        public UnicomQueryMsgHead msghead { get; set; }
    }
    public class UnicomQueryMsgRsp
    {
        public string retcode { get; set; }
        public string PAYABLE_AMOUNT { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string retshow { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string PUBLIC_AMOUNT { get; set; }
        public string PAYMENT_AMOUNT { get; set; }
        public string PRIVATE_AMOUNT { get; set; }
        public string PRESENT_AMOUNT { get; set; }
        public string REALTIME_BALANCE { get; set; }
    }
    public class UnicomQueryMsgHead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
        public string username { get; set; }
        public string loginId { get; set; }
        public string clientIp { get; set; }
        public string password { get; set; }

    }
}
