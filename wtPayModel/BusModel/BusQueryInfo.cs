using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    /// <summary>
    /// 公交卡
    /// </summary>
    public class BusQueryInfo
    {
        public BusQueryMsgrsp msgrsp { get; set; }
        public BusQueryCpumsg cpumsg { get; set; }
        public BusQueryMsghead msghead { get; set; }
    }
    public class BusQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        
}
    }
// busQueryInfo.cpumsg.OUTPUT.OUTAPDU.LASTAPDU;
public class BusQueryCpumsg
    {
        public string BTYPE { get; set; }
        public BusQueryOutput OUTPUT { get; set; }
        public string RESMSG { get; set; }
        public string RESULT { get; set; }
        public string TRADENO { get; set; }
    }
    public class BusQueryOutput
    {
        public string STEP { get; set; }
        public BusQueryOutApdu OUTAPDU { get; set; }
    }
    public class BusQueryOutApdu
    {
        public List<BusQueryApdu> APDU { get; set; }
        public string LASTAPDU { get; set; }
    }
    public class BusQueryApdu
    {
        public string APDUDATA { get; set; }
        public string DATAFLAG { get; set; }
        public string ENCFLAG { get; set; }
        public string RETSW { get; set; }
        public string APDUSW { get; set; }
    }

    public class BusQueryMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string reqsn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }


