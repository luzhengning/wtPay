using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.WintopModel;

namespace wtPayModel.PayParamModel
{
    public class PayParam
    {
        /// <summary>
        /// 缴费类型
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// 万通拉卡拉支付流水
        /// </summary>
        public string WtLklorderNo { get; set; }
        /// <summary>
        /// 云平台交易流水
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string payType { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// 最终交易信息(发送记录和打印日志)
        /// </summary>
        public string  reconcStr { get; set; }
        /// <summary>
        /// 交易批次号(支付时)
        /// </summary>
        public string  batchNo { get; set; }
        /// <summary>
        /// 用户输入金额
        /// </summary>
        public string  userInputAmount { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string rechageAmount { get; set; }
        /// <summary>
        /// 支付交易流水号_支付通知前获取（trandNo = map["cloudOrderNo"]）
        /// </summary>
        public string  trandNo { get; set; }
        /// <summary>
        /// 商户号|终端号（发送记录）
        /// </summary>
        public string  ClientNo { get; set; }
        /// <summary>
        /// 流水号|终端号|商户号（发送记录）
        /// </summary>
        public string  shopNo  { get; set; }
        /// <summary>
        /// 支付编码
        /// </summary>
        public string payCode { get; set; }
        /// <summary>
        /// 支付终端号(支付通知用)
        /// </summary>
        public string terminalNo { get; set; }

        /// <summary>
        ///分商户号
        /// </summary>
        public string MERCHANTNO_shopNo { get; set; }
        /// <summary>
        /// 分终端号
        /// </summary>
        public string TERMINALNO_clientNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cloudNo { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception ex { get; set; }
        
        public Dictionary<string, string> icParams { get; set; }
        /// <summary>
        /// 万通卡号
        /// </summary>
        public string WtNo { get; set; }

        /// <summary>
        /// 燃气用
        /// </summary>
        public string rqFlushesCode { get; set; }

        public PayResultInfo payResultInfo { get; set; }

        /// <summary>
        /// 物业二次，支付通知返回
        /// </summary>
        public string propSecSC20003 { get; set;}

    }
}
