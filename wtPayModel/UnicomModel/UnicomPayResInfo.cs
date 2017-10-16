using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomPayResInfo
    {
        public UnicomPayResMsgrsp msgrsp { get; set; }
        public UnicomPayResHead msghead { get; set; }
    }
    public class UnicomPayResMsgrsp
    {
        public string retcode { get; set; }

        public string retshow { get; set; }
        public string PAY_AMOUNT { get; set; }
        public string REALTIME_BALANCE { get; set; }
    }
    public class UnicomPayResHead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }
}
