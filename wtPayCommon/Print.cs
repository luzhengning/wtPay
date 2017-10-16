using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace wtPayCommon
{
   public class Print
    {
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="OpenParm"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReceiptPrinter.dll", EntryPoint = "TT_OpenDevice")]//打开端口
        public static extern int TT_OpenDevice(StringBuilder Port, StringBuilder OpenParm, StringBuilder szMsg);

        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReceiptPrinter.dll")]
        public static extern int TT_CloseDevice(StringBuilder szMsg);
        /// <summary>
        /// 打印数据
        /// </summary>
        /// <param name="PrintText"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReceiptPrinter.dll", EntryPoint = "TT_PrintText")]//打印数据
        public static extern int TT_PrintText(StringBuilder PrintText, StringBuilder szMsg);
        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReceiptPrinter.dll", EntryPoint = "TT_GetDeviceStatus")]//获取打印机状态
        public static extern int TT_GetDeviceStatus(StringBuilder szMsg);
        /// <summary>
        /// 切纸
        /// </summary>
        /// <param name="bHalfCut"></param>
        /// <param name="szMsg"></param>
        /// <returns></returns>
        [DllImport("TTReceiptPrinter.dll", EntryPoint = "TT_CutPaper")]//切纸
        public static extern int TT_CutPaper(int bHalfCut, StringBuilder szMsg);
    }
}
