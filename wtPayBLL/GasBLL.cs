using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace wtPayBLL
{
    public class GasBLL
    {
        /// <summary>
        /// 燃气读卡器判断,06=CRT,未知=GG
        /// </summary>
        /// <returns></returns>
        public static string GasJudge()
        {
            SerialPort sPort = new SerialPort();
            try
            {
                sPort.PortName = "com" + SysConfigHelper.readerNode("CJ201");//串口的portname 
                sPort.BaudRate = 9600;//串口的波特率 
                sPort.Open();
                byte[] data = new byte[] { 0x02, 0x00, 0x02, 0x30, 0x30, 0x03, 0x03 };
                sPort.Write(data, 0, 7);

                Thread.Sleep(1000);
                byte[] data3 = new byte[1];
                sPort.ReadTimeout = 3000;
                sPort.Read(data3, 0, 1);

                 return SysBLL.byteToHexStr(data3);
            }catch(Exception ex)
            {
                return "";
            }
            finally
            {
                if (sPort.IsOpen)
                    sPort.Close();
            }
        }
    }
}
