using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel
{
   public class PaySign
    {
        public string mechine_no;// 设备自编号
        public string terminal_no;// 交易终端号41域
        public string shop_no;// 商户号
        public string sign_type;// 签到类型：0拉卡拉，1万通
        public string sign_result;// 签到39域应答码

    }
}
