using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AnalysisBase
{
  public  class _3DES
    {
        static string IV = "0000000000000000";
        //3DES 加密           
        public static  string getDes3EncryptedText(string key,  string sourceText)         {
            var des3 = new TripleDESCryptoServiceProvider();
            des3.IV = HexStringToByteArray(IV);
            des3.Key = HexStringToByteArray(key);
            des3.Mode = CipherMode.ECB;         //ECB模式        
            des3.Padding = PaddingMode.Zeros;   //0x00填充       
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des3.CreateEncryptor(), CryptoStreamMode.Write);
            var data = HexStringToByteArray(sourceText);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            var result = ms.ToArray();
            cs.Close();
            ms.Close();
            return ByteArrayToHexString(result);
        }
        //3DES 解密        
        public static string getDes3DescryptText(string key, string sourceText)
        {
            var des3 = new TripleDESCryptoServiceProvider();
            des3.IV = HexStringToByteArray(IV);
            des3.Key = HexStringToByteArray(key);
            des3.Mode = CipherMode.ECB;         //ECB模式        
            des3.Padding = PaddingMode.Zeros;   //0x00填充       
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des3.CreateDecryptor(), CryptoStreamMode.Write);
            var data = HexStringToByteArray(sourceText);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            var result = ms.ToArray();
            cs.Close();
            ms.Close();
            return ByteArrayToHexString(result);
        }
        //字节数组转换成16进制字符串     
        public static string ByteArrayToHexString(byte[] ba)         {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "").Trim();
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }          
        /// <summary>         
        /// 字节数组转16进制字符串
        /// </summary>         
        /// <param name="bytes"></param>         
        /// <returns></returns>        
         public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }             }
            return returnStr;
        }    
    }
}
