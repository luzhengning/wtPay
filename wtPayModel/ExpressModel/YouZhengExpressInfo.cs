using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ExpressModel
{
    /// <summary>
    /// 邮政快递查询结果
    /// </summary>
    public class YouZhengExpressInfo
    {
        public YouZhengExpressInfoData data { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
    }
    public class YouZhengExpressInfoData
    {
        public List<YouZhengExpressInfoDataDetail> detail { get; set; }
        public string billcode { get; set; }
    }
    public class YouZhengExpressInfoDataDetail
    {
        public string time { get; set; }
        public string memo { get; set; }
    }
}
