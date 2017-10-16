using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.HeatModel;

namespace wtPayModel.PaymentModel
{
   public  class HeatPayParam:PaymentBase
    {
        public string AccountNo { get; set; }
        public HeatQueryOrderlist HeatQueryOrderlist { get; set; }
        public HeatOrderInfo HeatOrderInfo { get; set; }


    }
}
