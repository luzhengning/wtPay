using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AnalysisBase
{
    public class DESHelper
    {
        public static string Encrypt_DES16(string str_in_data, string str_DES_KEY) //数据为十六进制
        {
            var des3 = new TripleDESCryptoServiceProvider();
            des3.IV = HexStringToByteArray("0000000000000000");
            des3.Key = HexStringToByteArray(str_DES_KEY);
            des3.Mode = CipherMode.ECB;         //ECB模式  
            des3.Padding = PaddingMode.Zeros;   //0x00填充 
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des3.CreateEncryptor(), CryptoStreamMode.Write);
            var data = HexStringToByteArray(str_in_data);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            var result = ms.ToArray();
            cs.Close();
            ms.Close();
            return ByteArrayToHexString(result);
        }
        //字节数组转换成16进制字符串   
        public static string ByteArrayToHexString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
        private static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "").Trim();
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        /// <summary>      
        /// /// 字节数组转16进制字符串
        /// /// </summary>        
        /// /// <param name="bytes"></param>  
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    //DES解密
    public static string Decrypt_DES16(string str_in_data, string str_DES_KEY)//数据和密钥为十六进制
        {
            byte[] shuju = new byte[8];
            byte[] keys = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                shuju[i] = Convert.ToByte(str_in_data.Substring(i * 2, 2), 16);
                keys[i] = Convert.ToByte(str_DES_KEY.Substring(i * 2, 2), 16);
            }
            DES desDecrypt = new DESCryptoServiceProvider();
            desDecrypt.Mode = CipherMode.ECB;
            desDecrypt.Key = keys;
            desDecrypt.Padding = System.Security.Cryptography.PaddingMode.None;
            byte[] Buffer = shuju;
            ICryptoTransform transForm = desDecrypt.CreateDecryptor();
            byte[] R;
            R = transForm.TransformFinalBlock(Buffer, 0, Buffer.Length);
            string return_str = "";
            foreach (byte b in R)
            {
                return_str += b.ToString("X2");
            }
            return return_str;
        }
        //构造一个对称算法
        private static SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
        #region 加密解密函数
        /// <summary>
        /// 字符串的加密
        /// </summary>
        /// <param name="Value">要加密的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string Value, string sKey)
        {
            try
            {
                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;
                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.KeySize = 16;
                //指定加密的运算模式
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                //获取或设置加密算法的填充模式
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                byt = Encoding.UTF8.GetBytes(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ("Error in Encrypting " + ex.Message);
            }
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">加密后的字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>解密后的字符串</returns>
        public string DecryptString(string Value, string sKey, string sIV)
        {
            try
            {
                ICryptoTransform ct;//加密转换运算
                MemoryStream ms;//内存流
                CryptoStream cs;//数据流连接到数据加密转换的流
                byte[] byt;
                //将3DES的密钥转换成byte
                mCSP.Key = Convert.FromBase64String(sKey);
                //将3DES的向量转换成byte
                mCSP.IV = Convert.FromBase64String(sIV);
                mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                byt = Convert.FromBase64String(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {

                return ("Error in Decrypting " + ex.Message);
            }
        }
        #endregion
    }
}

