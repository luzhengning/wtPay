using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.WintopModel;

namespace wtPayModel.PaymentModel
{
    public class WintopReChargeParam:PaymentBase
    {
        /// <summary>
        /// 钱包类型
        /// </summary>
        public WintopQueryResult WintopQueryResult { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public WintopOrderInfo WintopOrderInfo { get; set; }
        /// <summary>
        /// md5加密密码
        /// </summary>
        public string Md5Pwd { get; set; }
        /// <summary>
        /// 万通卡号
        /// </summary>
        public string WtCardNo { get; set; }
        /// <summary>
        /// tradeNo编号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 支付编码
        /// </summary>
        public string PayCode { get; set; }
        public string TerminalNo { get; set; }

        public string Terminalno { get; set; }

        public string ShopType { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int ExcuteType { get; set; }
        /// <summary>
        /// 挂失参数
        /// </summary>
        public WintopLossReportParam WintopLossReportParam { get; set; }

        public WintopUpdateWtPwdParam WintopUpdateWtPwdParam { get; set; }

        public int QueryType { get; set; }

        public WintopDiscountInfoMsgrspList WintopDiscountInfoMsgrspList { get; set; }

        
    }
}
