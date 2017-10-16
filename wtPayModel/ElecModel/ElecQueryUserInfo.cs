using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    /// <summary>
    /// 获取电网用户资料 实体类
    /// </summary>
    public class ElecQueryUserInfo
    {
        public ElecQueryUserMsgrsp msgrsp { get; set; }
        public ElecQueryUserMsghead msghead { get; set; }

    }
    public class ElecQueryUserMsgrsp
    {
        public string rtnMsg { get; set; }
        public string totalCount { get; set; }
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<ElecQueryUserInfoList> userInfoList { get; set; }
    }
    public class ElecQueryUserInfoList
    {
        public string yhbh { get; set; }
        public string yhmc { get; set; }
        public string yddz { get; set; }
        public string hsdwbh { get; set; }
        public string jffs { get; set; }
        public string khyh { get; set; }
        public string zhmc { get; set; }
        public string yhzh { get; set; }
        public string qfje { get; set; }
        public string ysje { get; set; }

    }
    public class ElecQueryUserMsghead
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
