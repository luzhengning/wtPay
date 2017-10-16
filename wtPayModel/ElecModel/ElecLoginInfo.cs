using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    /// <summary>
    /// 电力认证返回的实体类
    /// </summary>
    public class ElecLoginInfo
    {
        public ElecLoginMsgrsp msgrsp { get; set; }
        public ElecLoginMsghead msghead { get; set; }
    }
    public class ElecLoginMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
        public string retshow { get; set; }

    }
    public class ElecLoginMsghead
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
