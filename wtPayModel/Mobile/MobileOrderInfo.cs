using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.Mobile
{
    public class MobileOrderInfo
    {
        public MobileOrderMsgrsp msgrsp { get; set; }
        public MobileOrderMsgHead msghead { get; set; }
    }
    public class MobileOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
       // public string C_035_list_Str { get; set; }
        public string paymentNo { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
    }
    public class MobileOrderMsgHead
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
