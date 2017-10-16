using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡登录认证返回的实体类
    /// </summary>
    public class WintopLoginInfo
    {
        public WintopLoginMsgrsp msgrsp { get; set; }
        public WintopLoginMsghead msghead { get; set; }
    }
    public class WintopLoginMsgrsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
        public string retshow { get; set; }
    }
    public class WintopLoginMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string resqn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }
}
