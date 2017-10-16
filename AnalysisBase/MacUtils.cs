using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AnalysisBase
{
   public class MacUtils
    {
        public static String DESMAC(String key, String data)
        {
            if (key.Length != 16)
            {
                new Exception("key's length must be 16!");
                return null;
            }

            StringBuilder sb = new StringBuilder(data);
            int mod = data.Length % 16;
            if (mod != 0)
            {
                for (int i = 0; i < 16 - mod; i++)
                {
                    sb.Append("0");
                }
            }

            String oper =sb.ToString();

            int count = oper.Length / 16;
            String[] blocks = new String[count];

            for (int i = 0; i < count; i++)
            {
                blocks[i] = oper.Substring(i * 16,  16);
            }
            String vector = blocks[0];
            //循环进行异或
            for (int i = 1; i < count; i++)
            {
                vector = xOrString(vector, blocks[i]);
            }
            Console.WriteLine("vector:" + vector);
            //DES加密
            String left = ByteUtil.getHexStr(Encoding.Default.GetBytes(vector.Substring(0, 8)));
            Console.WriteLine("left:" + left);
            String right = ByteUtil.getHexStr(Encoding.Default.GetBytes(vector.Substring(8)));
            Console.WriteLine("right:" + right);
            left = DESHelper.Encrypt_DES16(left, key);
            Console.WriteLine("加密left:" + left);
            right = xOrString(left, right);
            Console.WriteLine("加密后异或:" + right);
            right = DESHelper.Encrypt_DES16(right, key);
            Console.WriteLine("异或后加密:" + right);
            return right;
        }
        public static String xOrString(String pan, String pin)
        {
            if (pan.Length != pin.Length)
            {
                new Exception("异或因子长度不一致");
                return null;
            }

            byte[] bytepan = ByteUtil.getHexByte(pan);
            byte[] bytepin = ByteUtil.getHexByte(pin);

            byte[] result = new byte[bytepan.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(bytepan[i] ^ bytepin[i]);
            }

            return ByteUtil.getHexStr(result);
        }

        public static String CSMMAC(String key, String data)
        {
            if (key.Length != 32)
            {
                new Exception("key's length must be 16!");
                return null;
            }

            StringBuilder sb = new StringBuilder(data);
            int mod = data.Length % 32;
            if (mod != 0)
            {
                for (int i = 0; i < 32 - mod; i++)
                {
                    sb.Append("0");
                }
            }

            String oper = sb.ToString();

            int count = oper.Length / 32;
            String[] blocks = new String[count];

            for (int i = 0; i < count; i++)
            {
                blocks[i] = oper.Substring(i * 32, 32);
            }
            String vector = blocks[0];
            //循环进行异或
            for (int i = 1; i < count; i++)
            {
                vector = xOrString(vector, blocks[i]);
            }
            Console.WriteLine("vector:" + vector);
            //DES加密
            String left = ByteUtil.getHexStr(Encoding.Default.GetBytes(vector.Substring(0, 16)));
            Console.WriteLine("left:" + left);
            String right = ByteUtil.getHexStr(Encoding.Default.GetBytes(vector.Substring(16)));
            Console.WriteLine("right:" + right);
            left =BCDUtil.byteArrToString(CSm4.EncriptB(BCDUtil.HexStrToByteArray(key), BCDUtil.HexStrToByteArray(left)));
            Console.WriteLine("加密left:" + left);
            right = xOrString(left, right);
            Console.WriteLine("加密后异或:" + right);
            right = BCDUtil.byteArrToString(CSm4.EncriptB(BCDUtil.HexStrToByteArray(key), BCDUtil.HexStrToByteArray(right)));
            Console.WriteLine("异或后加密:" + right);
            return right;
        }
    }
}
