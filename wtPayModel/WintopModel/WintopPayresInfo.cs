using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通支付通知返回
    /// </summary>
    public class WintopPayresInfo
    {
        public WintopPayresMsgrsp msgrsp { get; set; }
        public WintopPayresMsghead msghead { get; set; }
    }
    public class WintopPayresMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
    }
    public class WintopPayresMsghead
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
