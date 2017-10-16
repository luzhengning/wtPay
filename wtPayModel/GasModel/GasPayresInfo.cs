using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    public class GasPayresInfo
    {
        public GasPayresMsgrsp msgrsp { get; set; }
        public GasPayresMsghead msghead { get; set; }
    }
    public class GasPayresMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
    }
    public class GasPayresMsghead
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
