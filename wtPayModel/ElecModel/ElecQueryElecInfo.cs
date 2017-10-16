using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ElecModel
{
    /// <summary>
    /// 电网查询电费实体类
    /// </summary>
    public class ElecQueryElecInfo
    {
        public ElecQueryMsgrsp msgrsp { get; set; }
        public ElecQueryMsghead msghead { get; set; }
    }
    public class ElecQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string dfbs { get; set; }
        public string dzpc { get; set; }
        public string hsdw { get; set; }
        public string yhbh { get; set; }
        public string yddz { get; set; }
        public string ysje { get; set; }
        public string dfze { get; set; }
        public string ylzd { get; set; }
        public string yhmc { get; set; }

        public string qfje { get; set; }
        public List<ElecQueryDianFeiDetail> dianFeiDetail { get; set; }

    }
    public class ElecQueryDianFeiDetail
    {
        public string yhbh { get; set; }
        public string ysbz { get; set; }
        public string dfny { get; set; }
        public string bbyjje { get; set; }
        public string dfje { get; set; }
        public string wyjje { get; set; }
        public string sctw { get; set; }
        public string bctw { get; set; }
        public string yszt { get; set; }
        public string ylzd { get; set; }
        public string xzsx { get; set; }

    }
    public class ElecQueryMsghead
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
