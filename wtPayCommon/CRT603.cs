using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace wtPayCommon
{
    public class CRT603
    {
        /// <summary>
        /// 指定波特率打开串口
        /// </summary>
        /// <param name="Port">串口</param>
        /// <param name="Baudrate">波特率</param>
        /// <returns>0 成功，非0失败</returns>
       [DllImport("CRT_603_Vx_Drv.dll")]
        public static extern int CRT603Vx_OpenConnection(int Port, int Baudrate);

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>0 成功，非0失败</returns>
        [DllImport("CRT_603_Vx_Drv.dll")]
        public static extern int CRT603Vx_CloseConnection();

        /// <summary>
        /// RF卡复位上电(包括Type A和Type B)
        /// </summary>
        /// <param name="iOutAtrLen">[out]iOutAtrLen  上电ATR数据的长度</param>
        /// <param name="byOutAtrData">[out]byOutAtrData 上电ATR数据</param>
        /// <returns>0 成功，非0失败</returns>
        [DllImport("CRT_603_Vx_Drv.dll")]
        public static extern int CRT603Vx_RF_chipPower(ref int iOutAtrLen, byte[] byOutAtrData);

        /// <summary>
        /// RF卡发送APDU(包括Type A和Type B)
        /// </summary>
        /// <param name="iSendApduLen">发送APDU数据的长度 比如 acs码 10， bcd码 5</param>
        /// <param name="bySendApduData">发送APDU数据      比如 acs码 "0084000008", bcd码 "\x00\x84\x00\x00\x08"</param>
        /// <param name="iRecvApduLen"> APDU返回的数据长度</param>
        /// <param name="byRecvApduData"> APDU返回的数据</param>
        /// <returns>0 成功，非0失败</returns>
        [DllImport("CRT_603_Vx_Drv.dll")]
        public static extern int CRT603Vx_RF_SendApdu(int iSendApduLen, byte[] bySendApduData, int[] iRecvApduLen, byte[] byRecvApduData);

    }
}
