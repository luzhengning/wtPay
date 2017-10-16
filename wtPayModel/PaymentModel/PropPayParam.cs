using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PropModel;

namespace wtPayModel.PaymentModel
{
    public class PropPayParam:PaymentBase
    {
        public int PropType { get; set; }
        public string Mobile { get; set; }
        public wyDataHouseList HouseInfo { get; set; }
        public wyDataParkList ParkInfo { get; set; }
        public wyDataChargeList ChargeList { get; set; }
        public PropOrderInfo PropOrderInfo { get; set; }
        public string MerchantNo { get; set; }
    }
}
