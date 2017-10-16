using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            start();
        }
        private static void Process_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("触发Exited事件");
            start();
        }
        static void start()
        {
            System.Diagnostics.Process[] proList = System.Diagnostics.Process.GetProcesses(".");//获得本机的进程
            foreach (System.Diagnostics.Process pro in proList)
            {
                if ("main".Equals(pro.ProcessName))
                {
                    pro.Kill();
                }
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\main.exe";
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(Process_Exited);
            p.Start();
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
