using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtPayModel
{
    public class GasCard : Card
    {
        /// <summary>
        /// 卡内剩余气量
        /// </summary>
        public int GasValue { get; set; }

        /// <summary>
        /// 总用气量
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 充值气量
        /// </summary>
        public string rechargeNum { get; set; }

        /// <summary>
        /// 应缴金额
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 燃气卡类型，1：金卡，2：先锋卡
        /// </summary>
        public int cardType { get; set; }

        public string wtCardNo { get; set; }
    }
}
