using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    
        /// <summary>
        /// 公交卡
        /// </summary>
        public class BusQueryThatInfo
        {
            public BusQueryThatMsgrsp msgrsp { get; set; }
            public BusQueryThatCpumsg cpumsg { get; set; }
            public BusQueryThatMsghead msghead { get; set; }
        }
        public class BusQueryThatMsgrsp
    {
            public string retcode { get; set; }
            public string retshow { get; set; }

        }
    
    public class BusQueryThatCpumsg
{
        public string BTYPE { get; set; }
        public BusQueryThatOutput OUTPUT { get; set; }
        public string RESMSG { get; set; }
        public string RESULT { get; set; }
        public string TRADENO { get; set; }
    }
    public class BusQueryThatOutput
{
        public string STEP { get; set; }
        public string APPSID { get; set; }
        public string CITYCODE { get; set; }
        public string DEPOSIT { get; set; }
        public string CMTYPE { get; set; }
        public string CSTYPE { get; set; }
        public string CRDATE { get; set; }
        public string CUF { get; set; }
        public string CSDATE { get; set; }
        public string CVDATE { get; set; }
        public string WMONEY { get; set; }
        public string YCDATE { get; set; }
        public string ONAME { get; set; }
        public string OIDNO { get; set; }
    }
    public class BusQueryThatOutApdu
{
        public List<BusQueryThatApdu> APDU { get; set; }
        public string LASTAPDU { get; set; }
    }
    public class BusQueryThatApdu
{
        public string APDUDATA { get; set; }
        public string DATAFLAG { get; set; }
        public string ENCFLAG { get; set; }
        public string RETSW { get; set; }
        public string APDUSW { get; set; }
    }

    public class BusQueryThatMsghead
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

