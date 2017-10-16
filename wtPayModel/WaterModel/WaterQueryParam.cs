using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
    /// <summary>
    /// 查询水务缴费信息 参数
    /// </summary>
   public class WaterQueryParam
    {
        public string authcode;
        public string servicename;
        public string trandateTime;
        public string reqsn;
        public string paymentno;
        public string loginId;
    }
}
