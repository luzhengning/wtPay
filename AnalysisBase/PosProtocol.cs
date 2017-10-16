using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisBase
{
    public class PosProtocol
    {
        public static string encode(byte[] input)
        {
            return null;
        }

        public static byte[] decode(byte[] input)
        {
            int in0 = 2;
            Console.WriteLine("报文头：" + bcd2Str(subbyte(input, 0, in0)));
           
            int in1 = 5;
            Console.WriteLine("TPDU:" + bcd2Str(subbyte(input, in0, in1)));
           
            int in2 = 2;

            Console.WriteLine("MSGTYPE：" + bcd2Str(subbyte(input, in1 + in0, in2)));
           
                      //bit map 
            int in3 = 8;
            string str = bytesToHexString(subbyte(input, in1 + in0 + in2, in3));
            Console.WriteLine("bit map :" + str);
            Int64 bitmap = Convert.ToInt64(str, 16);
            string _bitmap = leftpad(ToBinaryString(bitmap));
            for (int i = 0; i < _bitmap.Length; i++)
            {
                //TODO 
            }

            Console.WriteLine();

            

            return null;
        }

        public static string leftpad(string s)
        {
            if(s.Length != 64)
            {
                string temp = "";
                for(int i = 0; i < (64 - s.Length); i++)
                {
                    temp += "0";
                }
                s = temp + s;
            }
            return s;
        }

        /// <summary>
        /// Int64 Convert To Binary String
        /// </summary>
        /// <param name="plNumber"></param>
        /// <returns></returns>
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

        public static byte[] subbyte(byte[] src, int begin,int count)
        {
            byte[] bn1 = new byte[count];
            
                for (int i = begin; i < (begin + count); i++)
                {
                    bn1[i - begin] = src[i];
                }
            return bn1;
        }
        /// <summary>
        /// 转换为16进制字符串
        /// </summary>
        /// <param name="bArray"></param>
        /// <returns></returns>
        public static string bytesToHexString(byte[] bArray)
        {
            StringBuilder sb = new StringBuilder(bArray.Length);
            string sTemp;
            for (int i = 0; i < bArray.Length; i++)
            {
                sTemp = String.Format("{0:X}", 0xFF & bArray[i]);
                if (sTemp.Length < 2)
                {
                    sb.Append(0);
                }
                sb.Append(sTemp.ToUpper());
            }
            return sb.ToString();
        }
        /// <summary>
        /// bcd to string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String bcd2Str(byte[] bytes)
        {
            StringBuilder temp = new StringBuilder(bytes.Length * 2);
            for(int i = 0; i < bytes.Length; i++)
            {
               temp.Append((byte)(((uint)(bytes[i] & 0xf0)) >> 4));
               temp.Append((byte)(bytes[i] & 0x0f));
            }
            return temp.ToString();
        }

        /// <summary>
        /// bcd to string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] bcd2Byte(byte[] bytes)
        {
            byte[] result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i+=2)
            {
                result[i] = (byte)(((uint)(bytes[i] & 0xf0)) >> 4);
                result[i + 1] = (byte)(bytes[i] & 0x0f);
            }
            return result;
        }
        public static String toAsciiStr(byte[] bytes)
        {
          
            return System.Text.Encoding.Default.GetString(bytes);
        }
     
        public static int bcdlength(String bcdlen)
        {
            int len = Int32.Parse(bcdlen);
            int _len = ((len ^ 1) == 0) ? len : (len + 1);
            return _len / 2;
        }

        public static int bcdlength(int bcdlen)
        {
            int _len = ((bcdlen ^ 1) == 0) ? bcdlen : (bcdlen + 1);
            return _len / 2;
        }

        public static void rightBcdComplete(ResultData rd,int len)
        {
            string sub = string.Empty;
            for(int i = 0; i < len- rd.value.IndexOf(','); i++)
            {
                sub += "0";
            }
            rd.value = sub+rd.value;
        }


    }
}
