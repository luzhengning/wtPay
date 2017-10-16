using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    public class WintopQueryInfo
    {
        /// <summary>
        /// 万通卡查询实体类
        /// </summary>
        public WintopQueryMsgrsp msgrsp { get; set; }
        public WintopQueryMsghead msghead { get; set; }
    }
    public class WintopQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<WintopQueryResult> wTCardInfoList { get; set; }

        public string code { get; set; }
        public string msg { get; set; }
    }
    public class WintopQueryResult
    {
        public string AMOUNT { get; set; }
        public string LASTTIME { get; set; }
        public string STATE { get; set; }
        public string TYPE { get; set; }
        public string USERID { get; set; }
        public string WTCARDID { get; set; }
    }
    public class WintopQueryMsghead
    {
        public string trandatetime { get; set; }
        public string ressn { get; set; }
        public string tranchannel { get; set; }
        public string devno { get; set; }
        public string resqn { get; set; }
        public string servicename { get; set; }
        public string version { get; set; }
    }
}
