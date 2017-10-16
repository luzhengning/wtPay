using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    public class WintopPayParam
    {
        //data.Add(2, "9300000000000600");//万通卡号
        //    data.Add(3, "000000");
        //    data.Add(4, "000000000001");//交易金额
        //    data.Add(11, SysBLL.padLeft(getOrder(),6)); //POS终端交易流水
        //    data.Add(25, "00");
        //    data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
        //    data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
        //    data.Add(48, "Y00000001");
        //    data.Add(49, "156");
        //    data.Add(62, getPin("111111", "9300000000000600"));//明文，卡号
        //    data.Add(63, "01");
        public string ExpressPwd;
        /// <summary>
        /// 万通卡号
        /// </summary>
        public string wintopNo;

        /// <summary>
        /// 交易金额
        /// </summary>
        public string price;

        /// <summary>
        /// 明文密码
        /// </summary>
        public string pwd;

        /// <summary>
        /// 交易流水号
        /// </summary>
        public string orderNo;
        
    }
}
