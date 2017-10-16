using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PayParamModel;
using wtPayModel.PropSecModel;

namespace wtPayModel.PaymentModel
{
    public class PropSecPayParam:PaymentBase
    {
        public string Account { get; set; }
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 卡类型，用于识别GIF图
        /// </summary>
        public string PropType { get; set; }

        /// <summary>
        /// 卡片类型：01-水；02-电；03-燃气；04-暖气；
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 厂商编号
        /// </summary>
        public string ManufacturerNum { get; set; }

        public string SC10007 { get; set; }
        public string SC10008 { get; set; }
        public string merchantNo { get; set; }
        public PayParam p { get; set; }
        public PropSecOrderInfo orderInfo;
        public PropMeterInfo propMeterInfo { get; set; }
        /// <summary>
        /// 物业2读卡页面提示信息
        /// </summary>
        public string cardInfoMsg { get; set; }
    }
}
