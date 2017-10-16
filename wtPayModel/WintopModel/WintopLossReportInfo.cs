using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡挂失返回结果
    /// </summary>
    public class WintopLossReportInfo
    {
        public WintopLossReportInfoMsg msgrsp { get; set; }
        public Msghead msghead { get; set; }
    }
    public class WintopLossReportInfoMsg
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
    }
}
