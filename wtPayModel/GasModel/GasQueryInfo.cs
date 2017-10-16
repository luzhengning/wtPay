using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    /// <summary>
    /// 燃气查询结果
    /// </summary>
    public class GasQueryInfo
    {
        public GasQueryMsgrsp msgrsp
        { get; set; }
        public GasQueryMsghead msghead
        { get; set; }
    }
    public class GasQueryMsgrsp
    {
        public string retcode
        { get; set; }
        public string retshow
        { get; set; }
        public List<GasQueryOrderlist> orderlist
        { get; set; }
        public string resMsg
        { get; set; }
    }

    public class GasQueryOrderlist
    {
        public string accountBalance
        { get; set; }
        public string arrearsBalance
        { get; set; }
        public string billingcategory
        { get; set; }
        public string billingtype
        { get; set; }
        public string curBalance
        { get; set; }
        public string custName
        { get; set; }
        public string custNo
        { get; set; }
        public string lateFines
        { get; set; }
        public string maximum
        { get; set; }
        public string merchantNo
        { get; set; }
        public string payableAmount
        { get; set; }
        public string paymentAmout
        { get; set; }
        public string perMouthAmount
        { get; set; }
        public string preBalance
        { get; set; }
        public string price
        { get; set; }
        public string rechargeTime
        { get; set; }
    }
    public class GasQueryMsghead
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
