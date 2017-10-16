using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.WaterModel;

namespace wtPayModel.PaymentModel
{
    /// <summary>
    /// 水务支付参数
    /// </summary>
    public class WaterPayParam:PaymentBase
    {
        public string Account { get; set; }
        public WaterQueryInfo WaterQueryInfo { get; set; }
        public WaterOrderInfo OrderInfo { get; set; }


    }
}
