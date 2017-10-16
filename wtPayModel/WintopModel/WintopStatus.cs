using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 一卡通状态
    /// </summary>
    public class WintopStatusInfo
    {
        public WintopStatusInfoMsgrsp msgrsp { get; set; }

        public WintopStatusInfoMsghead msghead { get; set; }
    }
    public class WintopStatusInfoMsgrsp
    {
        public string retcode { get; set; }
        public string WTSTATE { get; set; }
        public string retshow { get; set; }
        public string STATENAME { get; set; }
        public string merchantNo { get; set; }
    }
    public class WintopStatusInfoMsghead
    {

    }

}
