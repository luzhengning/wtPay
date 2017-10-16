using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    /// <summary>
    /// 提交燃气订单参数
    /// </summary>
    public class GasOrderParam
    {
        public string shopType { get; set; }//  商户类型 String  否	1万通 0拉卡拉
        public string paymentno { get; set; }// 用户名 String 否   用户编号
        public string paymentAmout { get; set; }// 账单金额 String  否
        public string chargeAmount { get; set; }// 购气量 String  否

    }
}
