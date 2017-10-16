using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.BusModel;

namespace wtPayModel.PaymentModel
{
    /// <summary>
    /// 公交充值参数
    /// </summary>
    public class BusPayParam:PaymentBase
    {
        public BusQueryThatOutput Output{ get;set; }
        public string BusNo { get; set; }
        public BusCpuCardInfo BusCpuCardInfo { get; set; }
        /// <summary>
        /// 公交商户号
        /// </summary>
        public static string busShopNo { get; set; }
        /// <summary>
        /// 公交终端号
        /// </summary>
        public static string busClient { get; set; }

    }
}
