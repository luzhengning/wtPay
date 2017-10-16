using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
    /// <summary>
    /// 查询水务缴费信息 实体类
    /// </summary>
    public class WaterQueryInfo
    {
        public WaterQueryMsgreq msgrsp { get; set; }
        public WaterQueryMsghead msghead { get; set; }
    }
    public class WaterQueryMsgreq
    {
        public List<WaterQueryOrderList> orderlist { get; set; }
        public string retcode { get; set; }
        public string retshow { get; set; }
    }
    public class WaterQueryOrderList
    {
        public string custNo { get; set; }
        public string custName { get; set; }
        public string billDate { get; set; }
        public string amout { get; set; }
        public string balance { get; set; }
    }
    public class WaterQueryMsghead
    {
        public string loginId { get; set; }
        public string trandateTime { get; set; }
        public string username { get; set; }
        public string tranchannel { get; set; }
        public string authcode { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string clientIp { get; set; }
        public string password { get; set; }

    }
}
