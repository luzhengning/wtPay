using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.UnicomModel
{
    public class UnicomOrderInfo
    {
        public UnicomOrderMsgrsp msgrsp { get; set; }
        public UnicomOrderMsghead msghead { get; set; }
    }
    public class UnicomOrderMsgrsp
    {
        public string realAmout { get; set; }
        public string retcode { get; set; }
        public string orderNo { get; set; }
        public string retshow { get; set; }

        ///public List<C_035_list_StrInfo> C_035_list_Str { get; set; }

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
    public class UnicomOrderMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }

    public class C_035_list_StrInfo
    {
        public string C_035 { get; set; }
        public string C_036 { get; set; }
    }

}
