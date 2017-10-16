using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using wtPayCommon;

namespace wtPayBLL
{
    public class PrintBLL
    {
        public static void print(PrintParam param)
        {
            StringBuilder outMsg = new StringBuilder();
            Print.TT_OpenDevice(new StringBuilder("COM" + SysConfigHelper.readerNode("PrintPort")), new StringBuilder("38400"), outMsg);
            Print.TT_GetDeviceStatus(outMsg);
           // Print.TT_PrintText(new StringBuilder("三维终端 快捷支付 便利生活\n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_PrintText(new StringBuilder("  兰州三维便民服务终端交易凭条  \n"), outMsg);
            Print.TT_PrintText(new StringBuilder("--------------------------------"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_PrintText(new StringBuilder("交易类型:" + SysBLL.payTypeName + "\n"),outMsg);
            Print.TT_PrintText(new StringBuilder("缴费类型:"+param.payType+"缴费\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("缴费账号:" + param.account + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("交易卡号:" + hideCardNo(param.cardNo) + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("交易终端:" + ConfigurationManager.AppSettings["MechineNo"] + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("交易时间:" + SysBLL.getTimeFormat() + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("支付金额：￥" + param.amout + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("订单编号：" + param.orderno + "\n"), outMsg);
           // Print.TT_PrintText(new StringBuilder("流水号：" + param.resqn + "\n"), outMsg);
            Print.TT_PrintText(new StringBuilder("备注：\n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg); 
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);

            Print.TT_PrintText(new StringBuilder("-----------持卡人存根--------\n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
            Print.TT_CutPaper(1, outMsg);
            Print.TT_CloseDevice(outMsg);
        }

        public static string hideCardNo(string value)
        {
            try {
                char[] values = value.ToCharArray();
                int length = value.Length - 8;
                values[length] = '*';
                values[length + 1] = '*';
                values[length + 2] = '*';
                values[length + 3] = '*';
                return new string(values);
            }
            catch { return null; }
        }
    }
    public class PrintParam
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string tradingType;
        /// <summary>
        /// 缴费类型
        /// </summary>
        public string payType;
        /// <summary>
        /// 缴费账号
        /// </summary>
        public string account;
        /// <summary>
        /// 交易价格
        /// </summary>
        public string amout;
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderno;
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string resqn;
        /// <summary>
        /// 商户类型
        /// </summary>
        public string shopType;
        /// <summary>
        /// 批次号
        /// </summary>
        public string batchNo;
        /// <summary>
        /// 流水号
        /// </summary>
        public string orderNo;
        /// <summary>
        /// 交易卡号
        /// </summary>
        public string cardNo;
    }
}
