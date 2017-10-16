using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.BroadCas;

namespace wtPayModel.PaymentModel
{
    public class BroadCasPayParam:PaymentBase
    {
        public string Account { get; set; }
        public BoadCasQueryOrderlist List { get; set; }
        public BroadCasOrderInfo BroadCasOrderInfo { get; set; }

    }
}
