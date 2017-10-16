using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalysisBase
{
    public class EncryptWT
    {

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="src"></param>
        /// <param name="macByte"></param>
        /// <returns></returns>
        public static Dictionary<int, byte[]> encryptLKL(Dictionary<int, string> src, byte[] macByte)
        {
            Dictionary<int, byte[]> data = new Dictionary<int, byte[]>();
            foreach (KeyValuePair<int, string> item in src)
            {
                switch (item.Key)
                {
                    case 1:
                        data.Add(1, null);
                        break;
                    case 2:
                        data.Add(2, BCDUtil.sumLenBCDDataBCD(item.Value, 2));
                        break;
                    case 3:
                        data.Add(3, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 4:
                        data.Add(4, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 5:
                        data.Add(5, null);
                        break;
                    case 6:
                        data.Add(6, null);
                        break;
                    case 7:
                        data.Add(7, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 8:
                        data.Add(8, null);
                        break;
                    case 9:
                        data.Add(9, null);
                        break;
                    case 10:
                        data.Add(10, null);
                        break;
                    case 11:
                        data.Add(11, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 12:
                        data.Add(12, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 13:
                        data.Add(13, null);
                        break;
                    case 14:
                        data.Add(14, null);
                        break;
                    case 15:
                        data.Add(15, null);
                        break;
                    case 16:
                        data.Add(16, null);
                        break;
                    case 17:
                        data.Add(17, null);
                        break;
                    case 18:
                        data.Add(18, null);
                        break;
                    case 19:
                        data.Add(19, null);
                        break;
                    case 20:
                        data.Add(20, null);
                        break;
                    case 21:
                        data.Add(21, null);
                        break;
                    case 22:
                        data.Add(22, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 23:
                        data.Add(23, null);
                        break;
                    case 24:
                        data.Add(24, null);
                        break;
                    case 25:
                        data.Add(25, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 26:
                        data.Add(26, null);
                        break;
                    case 27:
                        data.Add(27, null);
                        break;
                    case 28:
                        data.Add(28, null);
                        break;
                    case 29:
                        data.Add(29, null);
                        break;
                    case 30:
                        data.Add(30, null);
                        break;
                    case 31:
                        data.Add(31, null);
                        break;
                    case 32:
                        data.Add(32, null);
                        break;
                    case 33:
                        data.Add(33, null);
                        break;
                    case 34:
                        data.Add(34, null);
                        break;
                    case 35:
                        data.Add(35, BCDUtil.sumLenBCDDataBCD(item.Value, 2));
                        break;
                    case 36:
                        data.Add(36, BCDUtil.sumLenBCDDataBCD(item.Value, 3));
                        break;
                    case 37:
                        data.Add(37, null);
                        break;
                    case 38:
                        data.Add(38, System.Text.Encoding.Default.GetBytes(item.Value));
                        break;
                    case 39:
                        data.Add(39, System.Text.Encoding.Default.GetBytes(item.Value));
                        break;
                    case 40:
                        data.Add(40, null);
                        break;
                    case 41:
                        data.Add(41, System.Text.Encoding.Default.GetBytes(item.Value));
                        break;
                    case 42:
                        data.Add(42, System.Text.Encoding.Default.GetBytes(item.Value));
                        break;
                    case 43:
                        data.Add(43, null);
                        break;
                    case 44:
                        data.Add(44, null);
                        break;
                    case 45:
                        data.Add(45, null);
                        break;
                    case 46:
                        data.Add(46, null);
                        break;
                    case 47:
                        data.Add(47, null);
                        break;
                    case 48:
                        data.Add(48, BCDUtil.sumLenBCDDataACSII(item.Value, 3));
                        break;
                    case 49:
                        data.Add(49, System.Text.Encoding.Default.GetBytes(item.Value));
                        break;
                    case 50:
                        data.Add(50, null);
                        break;
                    case 51:
                        data.Add(51, null);
                        break;
                    case 52:
                        data.Add(52, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 53:
                        data.Add(53, BCDUtil.str2Bcd(item.Value));
                        break;
                    case 54:
                        data.Add(54, null);
                        break;
                    case 55:
                        data.Add(55, null);
                        break;
                    case 56:
                        data.Add(56, null);
                        break;
                    case 57:
                        data.Add(57, BCDUtil.sumLenBCDDataACSII(item.Value, 3));
                        break;
                    case 58:
                        data.Add(58, null);
                        break;
                    case 59:
                        data.Add(59, BCDUtil.sumLenBCDDataACSII(item.Value, 3));
                        break;
                    case 60:
                        data.Add(60, BCDUtil.sumLenBCDDataBCD(item.Value, 3));
                        break;
                    case 61:
                        data.Add(61, BCDUtil.sumLenBCDDataACSII(item.Value, 3));
                        break;
                    case 62:
                        string str = item.Value;
                        int len = 3;


                        string strlen = "";
                        string temp = "";
                        byte[] dd = BCDUtil.ToByteArray((str));
                        int length = (dd.Length.ToString()).Length;
                        int size = len - length;
                        for (int i = 0; i < size; i++)
                        {
                            temp += "0";
                        }
                        strlen = temp + dd.Length;

                        byte[] bcdlength = BCDUtil.str2Bcd(strlen);
                        byte[] result = new byte[bcdlength.Length + dd.Length];
                        bcdlength.CopyTo(result, 0);
                        dd.CopyTo(result, bcdlength.Length);
                        Console.WriteLine(BCDUtil.byteArrToString(result));
                        data.Add(62, result);
                        break;
                    case 63:
                        data.Add(63, BCDUtil.sumLenBCDDataACSII(item.Value, 3));
                        break;
                        //case 64:
                        //    data.Add(64, BCDUtil.HexStrToByteArray(item.Value));
                        //    break;
                }

            }
            //添加64域校验码
            if (macByte != null)
            {
                data.Add(64, macByte);
            }
            return data;
        }
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="qiandaoNo"></param>
        /// <returns></returns>
        public static byte[] EncryptData(string msgType, string tpduStr, Dictionary<int, byte[]> data)
        {
            int signLength = 0;


            byte[] bcdMsgType = BCDUtil.str2Bcd(msgType);
            signLength += bcdMsgType.Length;


            string bin = "";
            for (int i = 1; i <= 64; i++)
            {
                if (data.ContainsKey(i))
                {
                    bin += "1";
                    signLength += data[i].Length;
                }
                else
                {
                    bin += "0";
                }
            }


            byte[] bitMap = BCDUtil.str2Bcd(BCDUtil.leftpad(Convert.ToString(Convert.ToInt64(bin, 2), 16), 8));

            signLength = signLength + bitMap.Length;
            signLength = signLength + 7;
            byte[] result = new byte[signLength];
            int copytopos = 0;

            byte[] messageLen = BCDUtil.str2Bcd(BCDUtil.leftpad(Convert.ToString((signLength - 2), 16), 4));
            byte[] tpdu = BCDUtil.str2Bcd(tpduStr);
            messageLen.CopyTo(result, copytopos);
            copytopos += 2;
            tpdu.CopyTo(result, copytopos);
            copytopos += 5;

            bcdMsgType.CopyTo(result, copytopos);
            copytopos += bcdMsgType.Length;
            bitMap.CopyTo(result, copytopos);
            copytopos += bitMap.Length;
            foreach (byte[] b in data.Values)
            {
                b.CopyTo(result, copytopos);
                copytopos += b.Length;
            }
            return result;
        }
        /// <summary>
        /// 计算MAC的加密数据
        /// </summary>
        /// <param name="qiandaoNo"></param>
        /// <returns></returns>
        public static byte[] countMacData(string msgType, Dictionary<int, byte[]> data)
        {
            int signLength = 0;


            byte[] bcdMsgType = BCDUtil.str2Bcd(msgType);
            signLength += bcdMsgType.Length;


            string bin = "";
            for (int i = 1; i <= 63; i++)
            {
                if (data.ContainsKey(i))
                {
                    bin += "1";
                    signLength += data[i].Length;
                }
                else
                {
                    bin += "0";
                }
            }
            bin += "1";
            byte[] bitMap = BCDUtil.str2Bcd(BCDUtil.leftpad(Convert.ToString(Convert.ToInt64(bin, 2), 16), 8));

            signLength = signLength + bitMap.Length;
            byte[] result = new byte[signLength];
            int copytopos = 0;


            bcdMsgType.CopyTo(result, copytopos);
            copytopos += bcdMsgType.Length;
            bitMap.CopyTo(result, copytopos);
            copytopos += bitMap.Length;
            foreach (byte[] b in data.Values)
            {
                b.CopyTo(result, copytopos);
                copytopos += b.Length;
            }
            return result;
        }


    }
}
