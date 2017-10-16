using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    /// <summary>
    /// 获取表具列表
    /// </summary>
    public class PropMeterInfo
    {
        public PropMeterInfoMsgRsp msgrsp { get; set; }
        public PropMeterInfoMsghead msghead { get; set; }
    }
    public class PropMeterInfoMsgRsp
    {
        public string SC20003 { get; set; }
        public string retcode { get; set; }
        public List<PropMeterInfoMsgRspList> meterLists { get; set;}
        public string retshow { get; set;}
        public string merchantNo { get; set; }
    }
    public class PropMeterInfoMsgRspList
    {
        public string g0402 { get; set; }
        public string g0502 { get; set; }
        public string g0901 { get; set; }
        public string g1801 { get; set; }
        public string primaryKey { get; set; }
        public string g1912 { get; set; }

    }
    public class PropMeterInfoMsghead
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
