using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.GasModel;

namespace wtPayModel.PaymentModel
{
    /// <summary>
    /// 燃气缴费参数
    /// </summary>
    public class GasPayParam:PaymentBase
    {
        public GasCard GasCard { get; set; }
        public GasOrderInfo GasOrderInfo { get; set; }
        public static string Showinfo { get; set; }
    }
}
