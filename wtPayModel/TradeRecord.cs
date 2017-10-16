using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel
{
    public class TradeRecord
    {
        public string id;

        public string data_id = "";
        public string lkl_wt_state = "";
        public string write_card_stat = "";
        public bool cloud_state = true;
        public string termail_no = "";
        public string order_no = "";
        public string batch_no = "";
        public string order_type = "";
        public string order_state = "";
        public string shop_type = "";
        public string reconc_str = "";
        public string relation_order = "";
        public string amount = "";
        public string branch_shop_no = "";
        public string branch_termail_no = "";

        public string cloud_no = "";//云平台订单号
        public string lkl_wt_shop_no = "";//拉卡拉万通商户号

    }
}
