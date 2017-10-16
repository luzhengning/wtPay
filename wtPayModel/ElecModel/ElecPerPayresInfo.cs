using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    public class ElecPerPayresInfo
    {
        public ElecPerPayresMsgrsp msgrsp { get; set; }
        public ElecPerPayresMsghead msghead { get; set; }
    }
    public class ElecPerPayresMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
    }
    public class ElecPerPayresMsghead
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
