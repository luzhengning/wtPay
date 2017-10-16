using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace wtPayCommon
{
    public class TTCurrency
    {
        /// <summary>
        ///打开端口
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="OpenParm"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_OpenDevice(StringBuilder Port, StringBuilder OpenParm, StringBuilder szMsg);
        
        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_CloseDevice(string szMsg);

        /// <summary>
        /// 取设备状态
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_GetDeviceStatus(StringBuilder szMsg);

        /// <summary>
        /// 设置可接收金额范围
        /// </summary>
        /// <param name="CashType"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_SetBillType(int CashType, StringBuilder szMsg);

        /// <summary>
        /// 允许投币
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_EnableCash(int timeout, StringBuilder szMsg);

        /// <summary>
        /// 禁止投币
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_DisableCash(StringBuilder szMsg);

        /// <summary>
        /// 取得接收金额
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_GetMoney(StringBuilder szMsg);

        /// <summary>
        /// 设置现金模块日志记录参数
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency")]
        public static extern int TT_SetLogParm(StringBuilder parm,StringBuilder szMsg);
    }
}
