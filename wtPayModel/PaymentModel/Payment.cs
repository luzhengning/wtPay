using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PaymentModel
{
    //支付参数
    public class Payment
    {
        /// <summary>
        /// 移动支付参数
        /// </summary>
        public static MobilePayParam mobilePayParam { get; set; }
        /// <summary>
        /// 联通支付参数
        /// </summary>
        public static UnicomPayParam unicomPayParam { get; set; }
        /// <summary>
        /// 电力支付参数
        /// </summary>
        public static ElecPayParam elecPayParam { get; set; }
        /// <summary>
        /// 万通充值参数
        /// </summary>
        public static WintopReChargeParam wintopReChargeParam { get; set; }
        /// <summary>
        /// 水务支付参数
        /// </summary>
        public static WaterPayParam waterPayParam { get; set; }
        /// <summary>
        /// 燃气支付参数
        /// </summary>
        public static GasPayParam GasPayParam { get; set; }
        /// <summary>
        /// 广电支付参数
        /// </summary>
        public static BroadCasPayParam broadCasPayParam { get; set; }
        /// <summary>
        /// 热力支付参数
        /// </summary>
        public static HeatPayParam heatPayParam { get; set; }
        /// <summary>
        /// 公交支付参数
        /// </summary>
        public static BusPayParam BusPayParam { get; set; }
        /// <summary>
        /// 物业支付参数
        /// </summary>
        public static PropPayParam PropPayParam { get; set; }
        /// <summary>
        /// 小区物业支付参数
        /// </summary>
        public static PropPayTempParam propPayTempParam { get; set; }
        /// <summary>
        /// 物业二次专供
        /// </summary>
        public static PropSecPayParam propSecPayParam { get; set; }
    }
}
