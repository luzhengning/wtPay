using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;

namespace Encryption
{
    public class EncryptionMD5
    {
        //str 需要加密的字符串  
        public static String MD5(Hashtable ht, String key)
        {
            ArrayList lst = new ArrayList(ht.Keys);
            lst.Sort();
            String str = string.Empty;
            foreach (string id in lst)
            {
                Console.WriteLine("key:" + id + "  vlaue:" + ht[id]);
                str += ht[id];
            }

            str = str + key;
            Console.WriteLine(str);
            byte[] result = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                // convert from hexa-decimal to character  
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }

    }
}
