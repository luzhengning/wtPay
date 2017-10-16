using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
   public class ByteUtil
    {
        public static String getHexStr(byte[] bs)
        {
            StringBuilder sb = new StringBuilder("");
            if (bs != null)
            {
                for (int i = 0; i < bs.Length; i++)
                {
                    sb.Append(getHexStr(bs[i]));
                }
            }
            return sb.ToString().ToUpper();
        }

        public static String getHexStr(byte bs)
        {
            String retStr = "";
            if (Convert.ToString((int)bs,16).Length > 1)
            {
                retStr += Convert.ToString((int)bs,16).Substring(
                        Convert.ToString((int)bs, 16).Length - 2);
            }
            else
            {
                retStr += "0"
                        + Convert.ToString((int)bs, 16).Substring(
                                Convert.ToString((int)bs, 16).Length - 1);
            }
            return retStr;
        }

        public static byte[] getHexByte(String byteStr)
        {
            if (byteStr.Length % 2 != 0)
            {
                byteStr = "0" + byteStr;
            }
            byte[] retByte = new byte[byteStr.Length / 2];

            for (int i = 0; i < byteStr.Length / 2; i++)
            {
                retByte[i] = int2byte(Convert.ToInt32(byteStr.Substring(2 * i,
                        2), 16))[3];
            }
            return retByte;
        }
        public static byte[] int2byte(int n)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(n >> 24);
            b[1] = (byte)(n >> 16);
            b[2] = (byte)(n >> 8);
            b[3] = (byte)n;
            return b;
        }

    }
}
