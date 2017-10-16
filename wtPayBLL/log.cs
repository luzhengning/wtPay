using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wtPayBLL
{
    public class log
    {
        /**//// <summary>
            /// 写入日志文件
            /// </summary>
            /// <param name="input"></param>
        public static void Write(string input)
        {
            string year = DateTime.Now.Year.ToString();
            string mouth = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string fileName = year + mouth + day;
            try {
                /**/
                ///指定日志文件的目录
                //string fname = Directory.GetCurrentDirectory() + "\\LogFile.txt";          fname= "D:\\wtPay_log\\http\\" + fileName + ".txt";
                string fname = "D://pay_log//" + fileName + ".txt";


                System.IO.Directory.CreateDirectory("D://pay_log");

                //DirectoryInfo dir = new DirectoryInfo("D:\\wtPay_log\\http");
                //dir.Create();//自行判断一下是否存在。

                /// 如果是创建子文件夹
                //DirectoryInfo dir = new DirectoryInfo("D:\\");
                //dir.CreateSubdirectory(string subFolderName);

                FileInfo finfo = new FileInfo(fname);

                if (!finfo.Exists)
                {
                    FileStream fs;
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }

                /**/
                ///判断文件是否存在以及是否大于2K
                if (finfo.Length > 1024 * 1024 * 10)
                {
                    /**/
                    ///文件超过10MB则重命名
                    File.Move(Directory.GetCurrentDirectory() + "\\LogFile.txt", Directory.GetCurrentDirectory() + DateTime.Now.TimeOfDay + "\\LogFile.txt");
                    /**/
                    ///删除该文件
                    //finfo.Delete();
                }
                //finfo.AppendText();
                /**/
                ///创建只写文件流

                using (FileStream fs = finfo.OpenWrite())
                {
                    /**/
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter w = new StreamWriter(fs);

                    /**/
                    ///设置写数据流的起始位置为文件流的末尾
                    w.BaseStream.Seek(0, SeekOrigin.End);

                    /**/
                    ///写入“Log Entry : ”
                    w.Write("\n\rLog: ");

                    /**/
                    ///写入当前系统时间并换行
                    w.Write("{0} \n\r", DateTime.Now.ToLongTimeString());

                    /**/
                    ///写入日志内容并换行
                    w.Write(input + "\n\r");

                    /**/
                    ///写入------------------------------------“并换行
                    //w.Write("\n\r");

                    /**/
                    ///清空缓冲区内容，并把缓冲区内容写入基础流
                    w.Flush();

                    /**/
                    ///关闭写数据流
                    w.Close();
                }
            }catch(IOException ioe) { Console.WriteLine(ioe.Message); return; }
            catch(Exception ex) { Console.WriteLine(ex.Message); return; }
        }

        public static string getLogPath()
        {
            string year = DateTime.Now.Year.ToString();
            string mouth = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string fileName = year + mouth + day;
            string fname = "D://pay_log//" + fileName + ".txt";
            return fname;
        }
    }
}
