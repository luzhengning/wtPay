using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PayParamModel
{
    public class SignResultInfo
    {
        public string code { get; set; }
        public string msgCode { get; set; }
        public string msg { get; set; }
        public Dictionary<string, string> data { get; set; }
    }
    public class SignParam
    {
        public string message_type { get; set; }//报文类型    String 否   必须，报文类型：1、签到2、消费3、冲正4、消费撤销
        public string shop_type { get; set; }//   商户类型 String  否 必须，商户类型：1万通 0拉卡拉
        public string data { get; set; }//实际数据信息      否
        public string cloud_no { get; set; }//云平台订单号  String 否

    }
}
