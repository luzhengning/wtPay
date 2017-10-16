using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
    /// <summary>
    /// 水务认证 实体类
    /// </summary>
    public class WaterLoginInfo
    {
        public WaterLoginMsgrsp msgrsp { get; set; }
        public WaterLoginMsghead msghead { get; set; }
    }
    public class WaterLoginMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
        public string retshow { get; set; }

    }

    public class WaterLoginMsghead
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
