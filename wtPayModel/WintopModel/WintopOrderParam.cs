using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 提交订单 传递的参数
    /// </summary>
    public class WintopOrderParam
    {
        public string authcode { get; set; }
        public string servicename { get; set; }
        public string trandateTime { get; set; }
        public string resqn { get; set; }
        public string wtcardid { get; set; }
        public string wtuserid { get; set; }
        public string type { get; set; }
        public string money { get; set; }
        public string loginId { get; set; }
        public string shopType { get; set; }
        public string payCode { get; set; }
    }
}
