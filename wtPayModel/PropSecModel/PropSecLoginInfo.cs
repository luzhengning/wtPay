using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    /// <summary>
    /// 物业2登录认证
    /// </summary>
    public class PropSecLoginInfo
    {
        public PropSecLoginInfoRsp msgrsp { get; set; }
        public PropSecLoginInfoHead msghead { get; set; }
    }
    
    public class PropSecLoginInfoRsp
    {
        public string retcode { get; set; }
        public string authcode { get; set; }
        public string retshow { get; set; }
    }
    public class PropSecLoginInfoHead
    {
        public string trandatetime { get; set; }
    }
    public class PropSecLoginParam
    {
        public string trandateTime { get; set; }
        public string reqsn { get; set; }
        public string loginId { get; set; }
        public string servicename { get; set; }

    }
}
