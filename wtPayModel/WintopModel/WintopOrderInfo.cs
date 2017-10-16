using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    public class WintopOrderInfo
    {
        /// <summary>
        /// 万通卡提交订单 实体类
        /// </summary>
        public WintopOrderMsgrsp msgrsp { get; set; }
        public WintopOrderMsghead msghead { get; set; }
    }
    public class WintopOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }
        public string wtcardid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
    }
    public class WintopOrderMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string resqn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }

    }
}
