using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PromptModel
{
    /// <summary>
    /// 缴费页面提示信息，后台查询
    /// </summary>
    public class PayPromptInfo
    {
        public string code { get; set; }
        public string msg { get; set; }
        public List<PayPromptInfoData> data {get;set;}

    }
    public class PayPromptInfoData
    {
        public string service_type { get; set; }
        public string hint { get; set; }
    }
}
