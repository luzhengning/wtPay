using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
   public class BCDUtil
    {
        public static byte[] str2Bcd(String asc)
        {
            int len = asc.Length;
            int mod = len % 2;
            if (mod != 0)
            {
                asc = "0" + asc;
                len = asc.Length;
            }
            byte[] abt = new byte[len];
            if (len >= 2)
            {
                len = len / 2;
            }
            byte[] bbt = new byte[len];
            // abt = asc.getBytes();
            abt = System.Text.Encoding.Default.GetBytes(asc);
            int j, k;
            for (int p = 0; p < asc.Length / 2; p++)
            {
                if ((abt[2 * p] >= '0') && (abt[2 * p] <= '9'))
                {
                    j = abt[2 * p] - '0';
                }
                else if ((abt[2 * p] >= 'a') && (abt[2 * p] <= 'z'))
                {
                    j = abt[2 * p] - 'a' + 0x0a;
                }
                else
                {
                    j = abt[2 * p] - 'A' + 0x0a;
                }
                if ((abt[2 * p + 1] >= '0') && (abt[2 * p + 1] <= '9'))
                {
                    k = abt[2 * p + 1] - '0';
                }
                else if ((abt[2 * p + 1] >= 'a') && (abt[2 * p + 1] <= 'z'))
                {
                    k = abt[2 * p + 1] - 'a' + 0x0a;
                }
                else
                {
                    k = abt[2 * p + 1] - 'A' + 0x0a;
                }
                int a = (j << 4) + k;
                byte b = (byte)a;
                bbt[p] = b;
            }
            return bbt;
        }


        /// <summary>
        /// 长度BCD，数据ASCII
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] sumLenBCDDataACSII(string str, int len) 
        {
            int length = (str.Length.ToString()).Length;
            int size = len - length;

            string strlen = "";
            string temp = "";
            for (int i = 0; i < size; i++)
            {
                temp += "0";
            }
            strlen = temp + str.Length;

            byte[] bcdlength = str2Bcd(strlen);
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] result = new byte[bcdlength.Length + data.Length];
            bcdlength.CopyTo(result, 0);
            data.CopyTo(result, bcdlength.Length);
            Console.WriteLine(BCDUtil.byteArrToString(result));
            return result;
        }

      
        /// <summary>
        /// 根据数据长度，右靠左补0
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string leftpad(string str ,int len)
        {
            int length = str.Length;
            string temp = "";
            for (int i = 0; i < len - length; i++)
            {
                temp += "0";
            }
            str = temp + str;
            return str;
        }

  
        /// <summary>
        /// 加长度字节BCD，数据BCD
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] sumLenBCDDataBCD(string str, int len) 
        {
            //
            int length = (str.Length.ToString()).Length;
            int size = len - length;

            string strlen = "";

            string temp = "";
            for (int i = 0; i < size; i++)
            {
                temp += "0";
            }
            strlen = temp+str.Length;
            byte[] bcdlength = str2Bcd(strlen);

            bool isNum = false;
            if ((str.Length % 2) != 0)  //奇数
            {
                isNum = true;
            }
            
            byte[] data = null;
            if (isNum)
            {
                data = str2Bcd(str+"0");
            }
            else
            {
                data = str2Bcd(str);
            }
            byte[] result = new byte[bcdlength.Length + data.Length];
            bcdlength.CopyTo(result, 0);
            data.CopyTo(result, bcdlength.Length);
            return result;
        }
        public static string byteArrToString(byte[] value)
        {
            string result = "";
             foreach(byte b in value)
            {
                String _ = Convert.ToString(Convert.ToInt64(b), 16);
                if(_.Length == 1)
                {
                    _ = "0" + _;
                }
                result += _;
            }
            return result;
        }

        public static bool[] getBinaryFromByte(byte[] b)
        {
            bool[] binary = new bool[b.Length * 8 + 1];
            String strsum = "";
            for (int i = 0; i < b.Length; i++)
            {
                strsum += getEigthBitsStringFromByte(b[i]);
            }
            for (int i = 0; i < strsum.Length; i++)
            {
                if (strsum.Substring(i, 1).Equals("1"))
                {
                    binary[i + 1] = true;
                }
                else
                {
                    binary[i + 1] = false;
                }
            }
            return binary;
        }

        public static String getEigthBitsStringFromByte(int b)
        {
            // if this is a positive number its bits number will be less
            // than 8
            // so we have to fill it to be a 8 digit binary string
            // b=b+100000000(2^8=256) then only get the lower 8 digit
            b |= 256; // mark the 9th digit as 1 to make sure the string
                      // has at
                      // least 8 digits
            String str = ToBinaryString(b);
            int len = str.Length;
            return str.Substring(len - 8, 8);
        }
        public static String ToBinaryString(Int64 plNumber)
        {
            string strBinary = "";
            long iTmp;
            do
            {
                iTmp = plNumber % 2;
                strBinary = iTmp.ToString() + strBinary;
                plNumber = plNumber / 2;
            } while (plNumber != 0);
            return strBinary;
        }

      

        /// <summary>
        /// 将指定进制的字符串转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromBase"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string value, int fromBase = 16)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            switch (fromBase)
            {
                case 2:
                    return BinaryStringToByteArray(value);
                case 16:
                    return HexStrToByteArray(value);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 字节数组节转换成二进制字符串
        /// </summary>
        /// <param name="b">要转换的字节数组</param>
        /// <returns></returns>
        private static string ByteArrayToBinaryString(byte[] byteArray)
        {
            int capacity = byteArray.Length * 8;
            StringBuilder sb = new StringBuilder(capacity);
            for (int i = 0; i < byteArray.Length; i++)
            {
                sb.Append(byte2BinString(byteArray[i]));
            }
            return sb.ToString();
        }

        private static string byte2BinString(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        /// <summary>
        /// 二进制字符串转换成字节数组
        /// </summary>
        /// <param name="binaryString">要转换的字符，如"00000000 11111111"</param>
        /// <returns></returns>
        public  static byte[] BinaryStringToByteArray(string binaryString)
        {
            //binaryString = binaryString.Replace(" ", "");
            int strLen = System.Text.Encoding.ASCII.GetByteCount(binaryString);
            if ((strLen % 8) != 0)
                throw new ArgumentException("二进制字符串长度不对");

            byte[] buffer = new byte[binaryString.Length / 8];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(binaryString.Substring(i * 8, 8).Trim(), 2);
            }
            return buffer;

        }
        //9FC8CF92383807FD
        //6C5901571220DB6A
        /// <summary>
        /// 字节数组转换成十六进制字符串
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns></returns>
        private static string ByteArrayToHexStr(byte[] byteArray)
        {
            int capacity = byteArray.Length * 2;
            StringBuilder sb = new StringBuilder(capacity);

            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    sb.Append(byteArray[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十六进制字符串转换成字节数组 
        /// </summary>
        /// <param name="hexString">要转换的字符串</param>
        /// <returns></returns>
        public  static byte[] HexStrToByteArray(string hexString)
        {
            hexString = hexString.Trim();
            if ((hexString.Length % 2) != 0)
                throw new ArgumentException("十六进制字符串长度不对");
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 0x10);
            }
            return buffer;
        }
      public  static string HexString2BinString(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4} ", v2);
            }
            return result;
        }
        public static void printData(string dataName, string data)
        {
            Console.WriteLine("========================="+ dataName + "========================");
            Console.WriteLine((data));
            Console.WriteLine("=========================" + dataName + "========================");
        }

        public static void printData(string dataName, byte[] data)
        {
            Console.WriteLine("=========================" + dataName + "========================");
            Console.WriteLine(BCDUtil.byteArrToString(data));
            Console.WriteLine("=========================" + dataName + "========================");
        }
    }
}
