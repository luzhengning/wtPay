using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.Mobile;

namespace wtPayModel.PaymentModel
{
    public class MobilePayParam:PaymentBase
    {
        public string PhoneOn { get; set; }
        public string Resqn { get; set; }
        public MobileQueryInfo QueryInfo { get; set; }
        public MobileOrderInfo OrderInfo { get; set; }
        public string PasswordInfo = "重要提示";
       
        

    }
}
