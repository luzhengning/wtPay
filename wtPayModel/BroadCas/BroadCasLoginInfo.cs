using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BroadCas
{
    public class BroadCasLoginInfo
    {
        public BroadLoginMsgrsp msgrsp
        { get; set; }
        public BroadLoginMsghead msghead
        { get; set; }
    }
    public class BroadLoginMsgrsp
    {
        public string retcode
        { get; set; }
        public string authcode
        { get; set; }
        public string retshow
        { get; set; }
    }
    public class BroadLoginMsghead
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

