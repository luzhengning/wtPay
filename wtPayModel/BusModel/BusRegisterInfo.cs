using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.BusModel
{
    /// <summary>
    /// 公交卡证书签到
    /// </summary>
    public class BusRegisterInfo
    {
        public BusRegisterMsgrsp msgrsp { get; set; }
        public BusRegisterMsghead msghead { get; set; }
        public BusRegisterCpuMsg cpumsg { get; set; }
    }

    public class BusRegisterMsgrsp
    {
       public string retcode { get; set; }
       public string authcode { get; set; }
       public string retshow { get; set; }
    }
    public class BusRegisterCpuMsg
    {
        
        public string BTYPE { get; set; }
        public BusRegisterCpuOutput OUTPUT { get; set; }
        public string RESMSG { get; set; }
        public string RESULT { get; set; }
        public string TRADENO { get; set; }


    }
    public class BusRegisterCpuOutput
    {
        public string OPNO { get; set; }
        public string RANDOM { get; set; }
        public string SCODE { get; set; }
        public string TKEY { get; set; }
    }
    public class BusRegisterMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string resqn
        { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }


}
