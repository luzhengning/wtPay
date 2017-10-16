using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wtPayBLL
{
    public class GcManage
    {
        public static string mp4Path = "D://payMedia//mp4";
        public static string mainImgPath = "D://payMedia//index";
        public static string leftImgPath = "D://payMedia//leftImg";

        public static string[] paths = { "5", "11", "12", "13", "15", "16", "1", "20", "4"
                ,"7","3","8","2","9","6","25","26"};

        public static string gcType = "";

        public static string ReadGCID()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//GCID.txt";
            return File.ReadAllText(filePath).Trim();
        }
        public static void WriteGCID(string value)
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//GCID.txt";
            File.WriteAllText(filePath, value);
        }
        /// <summary>
        /// 广告ID文件是否存在
        /// </summary>
        public static void GCIdFileIsExits()
        {
            string filePath = "D://payMedia";
            if (System.IO.Directory.Exists(filePath)) return;
            System.IO.Directory.CreateDirectory(filePath);
            filePath = filePath + "//GCID.txt";
            File.Create(filePath).Dispose();
        }
        /// <summary>
        /// 广告是否更新成功，0表示广告未更新，1表示广告已经更新
        /// </summary>
        /// <returns></returns>
        public static string ReadGCStateID()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//GCState.txt";
            return File.ReadAllText(filePath);
        }
        /// <summary>
        /// 广告是否更新成功，程序启动时必须写0，0表示广告未更新，1表示广告已经更新
        /// </summary>
        /// <param name="value"></param>
        public static void WriteGCStateID(string value)
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//GCState.txt";
            File.WriteAllText(filePath, value);
        }
        /// <summary>
        /// 广告是否更新成功的文件，是否存在
        /// </summary>
        public static void GCStateIdFileIsExits()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//GCState.txt";
            if (System.IO.File.Exists(filePath))
            {
                GcManage.WriteGCStateID("0");
                return;
            }
            File.Create(filePath).Dispose();
        }
        public static void WriteYinliangValue(string value)
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//Volume.txt";
            File.WriteAllText(filePath, value);
        }
        public static string readYinliangValue()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//Volume.txt";
            return File.ReadAllText(filePath);
        }
        /// <summary>
        /// 音量文件是否存在
        /// </summary>
        public static void YinliangFileIsExits()
        {
            string filePath = "D://payMedia";
            filePath = filePath + "//Volume.txt";
            if (System.IO.File.Exists(filePath)) return;

            File.Create(filePath).Dispose();
        }
        /// <summary>
        /// 视频广告路径是否存在
        /// </summary>
        public static void GcMp4FileIsExists()
        {
            string filePath = mp4Path;
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
                SysBLL.deleteDirFile(filePath);
                for (int i = 1; i <= 6; i++)
                {
                    if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4/" + i + ".mp4"))
                        System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4/" + i + ".mp4", mp4Path + "//" + i + ".mp4");
                }
                SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4");
            }
            else
            {
                //if ("1".Equals(ReadGCStateID()))
                //{
                if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4/1.mp4"))
                {
                    SysBLL.deleteDirFile(filePath);
                    for (int i = 1; i <= 6; i++)
                    {
                        if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4/" + i + ".mp4"))
                            System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4/" + i + ".mp4", mp4Path + "//" + i + ".mp4");
                    }
                    SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/mp4");
                }
                //}
            }
        }
        /// <summary>
        /// 首页轮播图是否存在
        /// </summary>
        public static void GcMainImgFileIsExists()
        {
            string filePath = mainImgPath;
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            for (int i = 1; i <= 6; i++)
            {
                if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/index/" + i + ".png"))
                {
                    if (getImgMaxPath(mainImgPath) == 0)
                    {
                        int count = getImgMaxPath(mainImgPath); count++;
                        System.IO.Directory.CreateDirectory(mainImgPath + "//" + count);
                    }
                    if (System.IO.File.Exists(mainImgPath + "//" + getImgMaxPath(mainImgPath) + "//" + i + ".png")) continue;
                    System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/index/" + i + ".png", mainImgPath + "//" + getImgMaxPath(mainImgPath) + "//" + i + ".png");
                }
            }

        }
        /// <summary>
        /// 左侧轮播图路径是否存在
        /// </summary>
        public static void GcLeftImgFileIsExists()
        {
            //string filePath = leftImgPath;
            //if (!System.IO.Directory.Exists(filePath))
            //{
            //    System.IO.Directory.CreateDirectory(filePath);

            //    for (int i = 1; i <= 6; i++)
            //    {
            //        if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + i + ".png"))
            //            System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + i + ".png", leftImgPath + "//" + i + ".png");
            //    }
            //}
            for (int i = 0; i < paths.Length; i++)
            {
                if (!System.IO.Directory.Exists("D://payMedia//leftImg//" + paths[i]))
                {
                    System.IO.Directory.CreateDirectory("D://payMedia//leftImg//" + paths[i]);
                }
            }
            for (int i = 0; i < paths.Length; i++)
            {
                if (getImgMaxPath("D://payMedia//leftImg//" + paths[i] + "//")==0)
                {
                    int count = getImgMaxPath("D://payMedia//leftImg//" + paths[i] + "//");
                    count++;
                    System.IO.Directory.CreateDirectory("D://payMedia//leftImg//" + paths[i]+"//"+ count);
                }
            }
            //if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/1.png"))
            //{
            //    SysBLL.deleteDirFile(leftImgPath);
            //    for (int i = 1; i <= 6; i++)
            //    {
            //        if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + i + ".png"))
            //            System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + i + ".png", leftImgPath + "//" + i + ".png");
            //    }
            //    SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg");
            //}
            for (int i = 0; i < paths.Length; i++)
            {
                copyImg(paths[i],paths[i]);
            }
        }
        private static void copyImg(string mainPath,string copyPath)
        {
            if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + mainPath + "/1.png"))
            {
                SysBLL.deleteDirFile("D://payMedia//leftImg//" + copyPath+"//"+getImgMaxPath("D://payMedia//leftImg//" + copyPath));
                for (int i = 1; i <= 6; i++)
                {
                    if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + mainPath + "/" + i + ".png"))
                        System.IO.File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + mainPath + "/" + i + ".png", "D://payMedia//leftImg//"+copyPath + "//"+"//"+getImgMaxPath("D://payMedia//leftImg//" + copyPath + "//") +"//" + i + ".png");
                }
                SysBLL.deleteDirFile(System.AppDomain.CurrentDomain.BaseDirectory + "/image/GCImage/leftImg/" + mainPath);
            }
        }
        ///// <summary>
        ///// 缴费程序状态文件(正在缴费/未缴费)，更新程序来判断此文件是否更新
        ///// </summary>
        //public static void UpdateAppIsExits()
        //{
        //    string filePath = "D://payMedia";
        //    filePath = filePath + "//UpdateAppState.txt";
        //    if (System.IO.File.Exists(filePath)) return;
        //    System.IO.File.Create(filePath).Dispose();
            
        //    File.Create(filePath).Dispose();
        //    WriteUpdateAppID("0");
        //}
        //public static string ReadUpdateAppID()
        //{
        //    string filePath = "D://payMedia";
        //    filePath = filePath + "//UpdateAppState.txt";
        //    return File.ReadAllText(filePath);
        //}
        //public static void WriteUpdateAppID(string value)
        //{
        //    string filePath = "D://payMedia";
        //    filePath = filePath + "//UpdateAppState.txt";
        //    File.WriteAllText(filePath, value);
        //}
        public static int getImgMaxPath(string path)
        {
            List<int> list = new List<int>();
            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (DirectoryInfo f in dii)
            {
                list.Add(Convert.ToInt32(f.Name));//添加文件的路径到列表
            }
            if (list.Count == 0) return 0;
            list.Sort();
            int count = list.Count;
            count--;
            return list[count];
        }
    }
}
