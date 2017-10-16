using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    public class PropSecOrderInfo
    {
        public PropSecOrderInfoRsp msgrsp { get; set; }
        public PropSecOrderInfoHead msghead { get; set; }
    }
    public class PropSecOrderInfoRsp
    {
        public string realAmout { get; set; }
        public string MERCHANTNO { get; set; }
        public string retcode { get; set; }
        public string EXTENDCARDTYPE { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
        public string TERMINALNO { get; set; }
        public string paymentNo { get; set; }
    }
    public class PropSecOrderInfoHead
    {
        public string trandatetime { get; set; }
    }
    /// <summary>
    /// 物业2订单查询参数
    /// </summary>
    public class PropSecOrderParam
    {
        public string trandateTime { get; set; }
        public string reqsn { get; set; }

        public string loginId { get; set; }
        public string servicename { get; set; }
        public string authcode { get; set; }
        public string shopType { get; set; }
        public string AMOUNT { get; set; }
        public string paymentAmout { get; set; }
        public string SC10009 { get; set; }
        public string SC10010 { get; set; }
        public string SC10007 { get; set; }
        public string SC10008 { get; set; }
        public string SC10014 { get; set; }
        public string merchantNo { get; set; }

    }


}
