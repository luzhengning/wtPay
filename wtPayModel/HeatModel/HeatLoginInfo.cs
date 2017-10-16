using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.HeatModel
{
    public class HeatLoginInfo
    {
         public HeatLoginMsgrsp msgrsp
            { get; set; }
            public HeatLoginMsghead msghead
            { get; set; }
    }
        public class HeatLoginMsgrsp
        {
            public string retcode
            { get; set; }
            public string authcode
            { get; set; }
            public string retshow
            { get; set; }

        }
        public class HeatLoginMsghead
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
