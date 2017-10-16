using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    /// <summary>
    /// 公交卡登录认证
    /// </summary>
    public class BusLoginInfo
    {
        public BusLoginMsgrsp msgrsp
        { get; set; }
        public BusLoginMsghead msghead
        { get; set; }
    }
    public class BusLoginMsgrsp
    {
        public string retcode
        { get; set; }
        public string authcode
        { get; set; }
        public string retshow
        { get; set; }
    }
    public class BusLoginMsghead
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
