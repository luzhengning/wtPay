using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    /// <summary>
    /// 公交卡	CPU 卡售卡及钱包充值
    /// </summary>
    public class BusCpuCardInfo
    {
        public BusCpuCardMsghead msghead
        { get; set; }
        public BusCpuCardMsgrsp msgrsp
        { get; set; }
        public BusCpuCardCpumsg cpumsg
        { get; set; }
    }
    public class BusCpuCardMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }
    public class BusCpuCardMsgrsp
    {
        public string CMTYPE { get; set; }
        public string payCode { get; set; }
        public string paymentAmout { get; set; }
        public string billDate { get; set; }
        public string paymentno { get; set; }
        public string trandeNo { get; set; }
        public string orderno { get; set; }
        public string WMONEY { get; set; }
        public string merchantNo { get; set; }
        public string Tac { get; set; }
        public string retcode { get; set; }
        public string retshow { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MERCHANTNO { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TERMINALNO { get; set; }
    }
    public class BusCpuCardCpumsg
    {
        public string BTYPE { get; set; }
        public BusCpuCardCpumsgOutput OUTPUT { get; set; }
        public string RESULT { get; set; }
        public string RESMSG { get; set; }
        public string TRADENO { get; set; }

    }
    public class BusCpuCardCpumsgOutput
    {
        public BusCpuCardOUTAPDU OUTAPDU { get; set; }
        public string STEP { get; set; }
        public string SERNO { get; set; }

    }
    public class BusCpuCardOUTAPDU
    {
        public List<BusCpuCardAPDU> APDU { get; set; }
        public string LASTAPDU { get; set; }
    }
    public class BusCpuCardAPDU
    {
        public string RETDATA { get; set; }
        public string APDUDATA { get; set; }
        public string DATAFLAG { get; set; }
        public string ENCFLAG { get; set; }
        public string RETSW { get; set; }
    }

}
