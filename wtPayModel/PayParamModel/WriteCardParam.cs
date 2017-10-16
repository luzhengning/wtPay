using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PayParamModel
{
    public class WriteCardParam
    {
        public string write_card_status { get; set; }// 写卡状态    String 否	1成功 3失败
        public string serviceType { get; set; }// 交易编码类型 String  否 交易编码类型:1燃气
        public string rqFlushesCode { get; set; }//  冲正字段 String  是
        public string shop_type { get; set; }//商户类型    String 否   必须，商户类型：1万通 0拉卡拉
        public string data { get; set; }//实际数据信息      否
        public string cloud_no { get; set; }//云平台订单号  String 否
        
    }
}
