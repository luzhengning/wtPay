using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
    /// <summary>
    /// 水务缴费 实体类
    /// </summary>
   public  class WaterOrderInfo
    {
        public WaterOrderMsgrsp msgrsp { get; set; }
        public WaterOrderMsghead msghead { get; set; }
    }
    public class WaterOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
        public string paymentNo { get; set; }
        public string billDate { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
    }


    public class WaterOrderMsghead
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
