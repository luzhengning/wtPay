using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.ElecModel;

namespace wtPayModel.PaymentModel
{
    public class ElecPayParam:PaymentBase
    {
        /// <summary>
        /// 电力账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 电力查询结果
        /// </summary>
        public ElecPayresParam Param { get; set; }
        public Boolean IsArrearage = false;
        /// <summary>
        /// 订单信息
        /// </summary>
        public ElecOrderInfo OrderInfo { get; set; }

        public string ShopType { get; set; }

        public  ElecQueryDianFeiDetail ElecQueryDianFeiDetail { get; set; }

    }
}
