using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
        public class WaterPayresInfo
        {
            public WaterPayresMsgrsp msgrsp { get; set; }
            public WaterPayresMsghead msghead { get; set; }
        }
        public class WaterPayresMsgrsp
        {
            public string retcode { get; set; }
            public string authcode { get; set; }
        }
        public class WaterPayresMsghead
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
