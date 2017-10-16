using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace wtPayCommon
{
    public class CashRMB
    {
        //打开端口
        [DllImport("TTCurrency.dll")]//打开端口
        public static extern int TT_OpenDevice(StringBuilder Port, StringBuilder OpenParm, StringBuilder szMsg);

        //关闭端口
        [DllImport("TTCurrency.dll")]//
        public static extern int TT_CloseDevice(StringBuilder szMsg);
        
        /// <summary>
        /// 0：设备正常，准备就绪
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency.dll")]//打开端口
        public static extern int TT_GetDeviceStatus(StringBuilder szMsg);

        /// <summary>
        /// 设置可接收金额范围，0:成功，其它值失败
        /// </summary>
        /// <param name="CashType"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency.dll")]
        public static extern int TT_SetBillType(int CashType, StringBuilder szMsg);

        /// <summary>
        /// 允许投币,0成功，其它值失败
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency.dll")]//
        public static extern int TT_EnableCash(int timeout, StringBuilder szMsg);
        /// <summary>
        /// 禁止投币,0成功，其它值失败
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency.dll")]//
        public static extern int TT_DisableCash(StringBuilder szMsg);

        /// <summary>
        /// 取得接收金额
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTCurrency.dll")]//
        public static extern int TT_GetMoney(StringBuilder szMsg);
















    }
}
