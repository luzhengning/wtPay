using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
//using wyzh;
//using ELECTREADER01Lib;
using AxWMPLib;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace wtPayBLL
{
    
    /// <summary>
    /// 北岸公馆读卡器
    /// </summary>
    public static class BeiAnGGCardBLL
    {
        static string mainPath = System.AppDomain.CurrentDomain.BaseDirectory;
        static string baggUrl = mainPath + "/beianGG.xml";
        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        public static string readCard()
        {
            SysBLL.RunCmd(mainPath + "\\Rd.exe " + SysConfigHelper.readerNode("baggRdPort") + " read");
            Thread.Sleep(2000);
            string cardNo = readerBAGGNode("beianCard");
            writeBAGGNode("");
            return cardNo;
        }

        public static void killRd()
        {
            //System.Diagnostics.Process[] proList = System.Diagnostics.Process.GetProcesses(".");//获得本机的进程
            Process[] process = Process.GetProcesses();
            foreach (Process prc in process)
            {
                if (prc.ProcessName.Equals("Rd"))
                    prc.Kill();
            }
        }
        /// <summary>
        /// 写卡
        /// </summary>
        /// <param name="payType"></param>
        /// <param name="amount"></param>
        public static void writeCard(string payType,string amount)
        {
            if (payType.Equals("01")) payType = "00";
            if (payType.Equals("02")) payType = "01";
            SysBLL.RunCmd(mainPath + "\\BAGG\\Rd.exe "+ SysConfigHelper.readerNode("baggRdPort") + " write "+payType+" "+amount+"");
            Thread.Sleep(2000);
            writeBAGGNode("");
        }

        public static void writeBAGGNode(string value)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(baggUrl);//加载xml文件，文件
            XmlNode xns = xmlDoc.SelectSingleNode("app");//查找要修改的节点
            XmlNodeList xnl = xns.ChildNodes;//取出所有的子节点
            foreach (XmlNode node in xnl)
            {
                if (node.Name.Equals("beianCard"))
                {
                    node.InnerText = value;
                }
            }
            xmlDoc.Save(baggUrl);
        }
        public static string readerBAGGNode(string nodeName)
        {
            string result = string.Empty;
            using (XmlTextReader reader = new XmlTextReader(baggUrl))
            {
                while (reader.Read())
                {
                    if (nodeName.Equals(reader.Name))
                    {
                        result = reader.ReadElementString().Trim();
                        break;
                    }
                }
            }
            return result;
        }
    }
}
