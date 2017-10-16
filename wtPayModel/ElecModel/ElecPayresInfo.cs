using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    //支付结果通知实体类
    public class ElecPayresInfo
    {
        public ElecPayresMsgrsp msgrsp { get; set; }
        public ElecPayresMsghead msghead { get; set; }
    }
    public class ElecPayresMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
    }
    public class ElecPayresMsghead
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
