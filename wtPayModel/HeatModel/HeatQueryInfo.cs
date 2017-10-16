using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.HeatModel
{
    /// <summary>
    /// 热力查询返回的实体
    /// </summary>
    public class HeatQueryInfo
    {
        public HeatQueryMsgrsp msgrsp { get; set; }
        public HeatQueryMsghead msghead { get; set; }
    }
    public class HeatQueryMsgrsp
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public List<HeatQueryOrderlist> orderlist { get; set; }
        public string resMsg { get; set; }
    }
    public class HeatQueryOrderlist
    {
       public string amout{get;set;}
                public string baoWenNo{get;set;}
                public string billDate{get;set;}
                public string custName{get;set;}
                public string custNo{get;set;}
                public string fileNo{get;set;}
                public string filePath{get;set;}
                public string merchantNo{get;set;}
                public string rowNo { get; set; }
        public string price { get; set; }
        public string address { get; set; }
        public string area { get; set; }
    }
    public class HeatQueryMsghead
    {
        public string trandatetime
        { get; set; }
        public string ressn
        { get; set; }
        public string tranchannel
        { get; set; }
        public string devno
        { get; set; }
        public string resqn
        { get; set; }
        public string servicename
        { get; set; }
        public string version
        { get; set; }
    }

}
