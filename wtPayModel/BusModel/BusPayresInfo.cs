using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    public class BusPayresInfo
    {
        public BusPayresMsgrsp msgrsp { get; set; }
        public BusPayresMsghead msghead { get; set; }
    }
    public class BusPayresMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
    }
    public class BusPayresMsghead
    {
        public string trandatetime
        { get; set; }
        public string ressn
        { get; set; }
        public string tranchannel
        { get; set; }
        public string devno
        { get; set; }
        public string resqn
        { get; set; }
        public string servicename
        { get; set; }
        public string version
        { get; set; }
    }
}

