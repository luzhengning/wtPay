using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PropSecModel
{
    public class PropSecPayResInfo
    {
        public PropSecPayResInfoRsp msgrsp { get; set; }
    }
    public class PropSecPayResInfoRsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string SC20003 { get; set; }
    }
    public class PropSecPayResParam
    {
        public string orderno { get; set; }
public string realAmout { get; set; }
        public string payCode { get; set; }
        public string trandeNo { get; set; }

    }
}
