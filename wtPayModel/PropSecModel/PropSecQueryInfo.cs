using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    /// <summary>
    /// 物业2读卡
    /// </summary>
    public class PropSecQueryInfo
    {
        public PropSecQueryInfoRsp msgrsp{get;set;}
        public PropSecQueryInfoHead msghead { get; set; }
    }
    public class PropSecQueryInfoRsp
    {
        public string SC20003 { get; set; }
        public string retcode { get; set; }
        public string retshow {get; set; }
        public string merchantNo { get; set; }
        public string SC10011 { get; set; }
    }
    public class PropSecQueryInfoHead
    {
        public string trandatetime { get; set; }
    }
    /// <summary>
    /// 物业2查询参数
    /// </summary>
    public class PropSecQueryParam
    {
        public string trandateTime { get; set; }
        public string reqsn { get; set; }
        public string loginId { get; set; }
        public string servicename { get; set; }
        public string authcode { get; set; }
        public string SC10009 { get; set; }
        public string SC10010 { get; set; }
        public string SC10011 { get; set; }
        public string ResidentialNo { get; set; }

    }
}
