using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    /// <summary>
    /// 提交订单返回实体类
    /// </summary>
    public class GasOrderInfo
    {
        public GasOrderMsgrsp msgrsp
        { get; set; }
        public GasOrderMsghead msghead
        { get; set; }
    }
    public class GasOrderMsgrsp
    {
        public string realAmout
        { get; set; }
        public string retcode
        { get; set; }
        public string orderNo
        { get; set; }
        public string retshow
        { get; set; }
        public string paymentNo
        { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
        /// <summary>
        /// 支付卡类型
        /// </summary>
        public string EXTENDCARDTYPE { get; set; }
    }

    public class GasOrderMsghead
    {
        public string trandatetime
        { get; set; }
        public string ressn
        { get; set; }
        public string tranchannel
        { get; set; }
        public string devno
        { get; set; }
        public string resqn
        { get; set; }
        public string servicename
        { get; set; }
        public string version
        { get; set; }
    }
}
