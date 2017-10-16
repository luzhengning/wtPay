using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.UnicomModel;

namespace wtPayModel.PaymentModel
{
    /// <summary>
    /// 联通支付参数
    /// </summary>
    public class UnicomPayParam:PaymentBase
    {
        public string PhoneOn { get; set; }
        public UnicomQueryMsgRsp Msgrsp { get; set; }
        public string Resqn { get; set; }
        public string PasswordInfo { get; set; }
        public UnicomOrderInfo Orderinfo { get; set; }
        public string AccountNo { get; set; }


    }
}
