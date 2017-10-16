using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    class WintopDiscount
    {
    }
    /// <summary>
    /// 充值优惠信息查询参数
    /// </summary>
    public class WintopDiscountParam
    {
        public string trandateTime { get; set; }
        public string reqsn { get; set; }
        public string loginId { get; set; }
        public string servicename { get; set; }
        public string authcode { get; set; }
        public string wtcardid { get; set; }

    }

    /// <summary>
    /// 充值优惠信息查询结果
    /// </summary>
    public class WintopDiscountInfo
    {
        public WintopDiscountInfoMsgrsp msgrsp { get; set; }
        public WintopDiscountInfoMsghead msghead { get; set;}
    }
    public class WintopDiscountInfoMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<WintopDiscountInfoMsgrspList> RECHARGEINFO { get; set; }
    }
    public class WintopDiscountInfoMsgrspList
    {
        /// <summary>
        /// 优惠信息ID
        /// </summary>
        public string CZ00016 { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string CZ00017 { get; set; }
        /// <summary>
        /// 实际付款金额
        /// </summary>
        public string CZ00030 { get; set; }

    }
    public class WintopDiscountInfoMsghead
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
