using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.SystemModel
{
    /// <summary>
    /// 系统命令
    /// </summary>
    public class SystemOrder
    {
        public SystemOrderData data { get; set; }
        public string code { get; set; }
    }
    public class SystemOrderData
    {
        public string id { get; set; }
        public string cmdOrder { get; set; }
        public string ext_time { get; set; }
        public string update_time { get; set; }
        public string cmd_type { get; set; }
        public string cmd_additional { get; set; }
        public string cmd_no { get; set; }
    }
}
