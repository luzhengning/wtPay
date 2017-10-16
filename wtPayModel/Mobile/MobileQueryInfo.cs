using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.Mobile
{
    public class MobileQueryInfo
    {
       public MobileQueryMsgRsp msgrsp { get; set; }
       public MobileQueryMsgHead msghead { get; set; }
    }

    public class MobileQueryMsgRsp
    {
        public string publicMark { set; get; }
        public string startDate { set; get; }
        public string changeBalance { set; get; }
        public string retcode { set; get; }
        public string homeOffice { set; get; }
        public string endDate { set; get; }
        public string payOffice { set; get; }
        public string overdueMoney { set; get; }
        public string bankNo { set; get; }
        public string retshow { set; get; }
        public string lateFines { set; get; }
        public string payRegion { set; get; }
        public string contractNoCredit { set; get; }
        public string contractNo { set; get; }
        public string afterPrepaidBalance { set; get; }
        public string prepaidBalance { set; get; }
        public string homeRegion { set; get; }
        public string representNo { set; get; }
        public string payableAmout { set; get; }
        public string VIPBenefit { set; get; }
        public string mobile { set; get; }
        public string payMethod { set; get; }
    }
    public class MobileQueryMsgHead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
        public string username { get; set; }
        public string loginId { get; set; }
        public string clientIp { get; set; }
        public string password { get; set; }
    }
}
