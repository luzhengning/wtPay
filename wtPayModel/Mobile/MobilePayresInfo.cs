using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.Mobile
{
    public class MobilePayresInfo
    {
        public MobilePayresMsgrsp msgrsp { get; set; }
        public MobilePayresMsgHead msghead { get; set; }
    }
    public class MobilePayresMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
    }
    public class MobilePayresMsgHead
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
