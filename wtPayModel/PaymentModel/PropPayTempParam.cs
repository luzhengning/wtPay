using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PropModel;

namespace wtPayModel.PaymentModel
{
    public class PropPayTempParam:PaymentBase
    {
        public string PropType { get; set; }
        public string AccountNo { get; set; }
        public string MerchantNo { get; set; }
        public string House { get; set; }
        public PropOrderInfo PropOrderInfo { get; set; }


    }
}
