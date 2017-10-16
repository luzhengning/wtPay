using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    /// <summary>
    /// 电网获取订单接口 实体类
    /// </summary>
    public  class ElecOrderInfo
    {
        public ElecOrderMsgrsp msgrsp { get; set; }
        public ElecOrderMsghead msghead { get; set; }
    }
    public class ElecOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
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
    public class ElecOrderMsghead
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
