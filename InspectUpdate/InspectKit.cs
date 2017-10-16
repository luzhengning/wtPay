using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using wtPayBLL;

namespace InspectUpdate
{
    public class InspectKit
    {
        /// <summary>
        /// 解压ZIP文件
        /// </summary>
        /// <param name = "strZipPath" > 待解压的ZIP文件 </ param >
        /// < param name="strUnZipPath">解压的目录</param>
        /// <param name = "overWrite" > 是否覆盖 </ param >
        /// < returns > 成功：true/失败：false</returns>
        public static List<Dictionary<string, string>> Decompression(string strZipPath)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            try
            {
                if (!File.Exists(strZipPath))
                {
                    Console.WriteLine("Cannot find file '{0}'", strZipPath);
                    return result;
                }

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(strZipPath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (fileName != String.Empty)
                        {
                            using (HashAlgorithm hash = HashAlgorithm.Create())
                            {
                                int size = 2048;
                                byte[] data = new byte[theEntry.Size];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        //哈希算法根据文本得到哈希码的字节数组 
                                        byte[] hashByte = hash.ComputeHash(data, 0, size);
                                        Dictionary<string, string> dic = new Dictionary<string, string>();
                                        dic.Add("name", theEntry.Name);
                                        //将字节数组装换为字符串 
                                        dic.Add("hash", BitConverter.ToString(hashByte));
                                        result.Add(dic);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("更新程序解压失败:" + ex.Message);
            }
            return result;
        }

        public static string GetFileHash(string filePath)
        {
            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    //哈希算法根据文本得到哈希码的字节数组 
                    byte[] hashByte = hash.ComputeHash(file);
                    //将字节数组装换为字符串  
                    return BitConverter.ToString(hashByte);
                }
            }
        }
        /// <summary>
        /// 进行验证
        /// </summary>
        /// <param name="url">请求路劲</param>
        /// <returns></returns>
        public static bool Inspect(string url)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            IDictionary<string, string> param = new Dictionary<string, string>();
             string[] dirName = path.Split('\\');
            param.Add("version", dirName[dirName.Length-2]);
            Result source = JsonConvert.DeserializeObject<Result>(HttpHelper.getHttp(url, param, Encoding.UTF8));
            if (source.data == null)//当数据为null时说明无需验证直接通过
            {
                return true;
            }
            List<Dictionary<string, string>> result = GetDifferentData(source, path);
            if (result.Count == 0)//验证结束后未通过个数为0时验证成功
            {
                return true;
            }
            else//验证失败时，报告后台并且将验证失败的文件名发送至后台
            {
                string msg = "验证失败文件名：";
                foreach (Dictionary<string, string> dic in result)
                {
                    msg += dic["name"] + "、";
                }
                DeviceState.SendState("100000", msg);
            }
            return false;
        }
        private static List<Dictionary<string, string>> GetDifferentData(Result result, string path)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> that = ListFiles(new DirectoryInfo(path), path);
            foreach (Dictionary<string, string> dic in result.data)
            {
                foreach (Dictionary<string, string> ndic in that)
                {
                    if (dic["name"].Equals(ndic["name"]))
                    {
                        if (dic["hash"].Equals(ndic["hash"]))
                        {
                            break;
                        }
                        else
                        {
                            data.Add(ndic);
                        }
                    }
                }
            }
            return data;
        }
        public static List<Dictionary<string, string>> ListFiles(FileSystemInfo info, string path)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            if (!info.Exists) return result;
            DirectoryInfo dir = info as DirectoryInfo;
            //不是目录   
            if (dir == null) return result;
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件   
                if (file != null)
                {
                    using (HashAlgorithm hash = HashAlgorithm.Create())
                    {

                        try
                        {
                            using (FileStream fileStream = file.Open(FileMode.Open))
                            {
                                byte[] hashByte = hash.ComputeHash(fileStream);
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic.Add("name", file.FullName.Replace(path, "").Replace("\\", "/"));
                                //将字节数组装换为字符串 
                                dic.Add("hash", BitConverter.ToString(hashByte));
                                result.Add(dic);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message+"======="+file.Name);
                        }

                    }
                }
                //对于子目录，进行递归调用   
                else
                    result.AddRange(ListFiles(files[i], path));
            }
            return result;
        }
    }
}
