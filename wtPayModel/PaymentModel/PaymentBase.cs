using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PaymentModel
{
    /// <summary>
    /// 支付信息基础参数
    /// </summary>
    public abstract class PaymentBase
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 支付卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 万通卡号
        /// </summary>
        public string WtNo { get; set; }
        /// <summary>
        /// 支付信息
        /// </summary>
        public Dictionary<string, string> IcParams { get; set; }
        /// <summary>
        /// 用户输入金额
        /// </summary>
        public string UserInputMoney { get; set; }
        /// <summary>
        /// 实际支付金额(云平台订单接口返回)
        /// </summary>
        public string RechageAmount { get; set; }
        /// <summary>
        /// 订单编号(云平台订单接口返回)
        /// </summary>
        public string OrderNo { get; set; }
    }
}
