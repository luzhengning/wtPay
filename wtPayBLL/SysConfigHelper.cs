using SqlLiteHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace wtPayBLL
{
    public class SysConfigHelper
    {
        public static string readerNode(string nodeName)
        {
            try
            {
                if ("LklOrderNo".Equals(nodeName.Trim()))
                {
                    log.Write("拉卡拉流水号：");
                    return getWtLklNoNum("0", "0", "0", "0");
                }
                if ("LklBatchNo".Equals(nodeName.Trim()))
                {
                    log.Write("拉卡拉批次号：");
                    return getWtLklNoNum("0", "0", "1", "0");
                }
                if ("WtOrderNo".Equals(nodeName.Trim()))
                {
                    log.Write("万通流水号：");
                    return getWtLklNoNum("0", "1", "0", "0");
                }
                if ("WtBatchNo".Equals(nodeName.Trim()))
                {
                    log.Write("万通批次号：");
                    return getWtLklNoNum("0", "1", "1", "0");
                }
                try
                {
                    if ("正式".Equals(SysBLL.IsTest))
                        return SqlLiteHelper.SqlLiteHelper.query(nodeName)[0].FormalValue;
                    if ("测试".Equals(SysBLL.IsTest))
                        return SqlLiteHelper.SqlLiteHelper.query(nodeName)[0].TestValue;
                    return "";
                }
                catch (Exception ex) { log.Write("error:readerNode:" + ex.Message); return ""; }
            }
            catch (Exception ex) { log.Write("error:readerNode:" + ex.Message); return ""; }
        }
        public static void writerNode(string nodeName, string nodeValue)
        {
            try
            {
                if ("LklOrderNo".Equals(nodeName.Trim()))
                {
                    getWtLklNoNum("1", "0", "0", nodeValue.Trim());
                    return;
                }
                if ("LklBatchNo".Equals(nodeName.Trim()))
                {
                    getWtLklNoNum("1", "0", "1", nodeValue.Trim());
                    return;
                }
                if ("WtOrderNo".Equals(nodeName.Trim()))
                {
                    getWtLklNoNum("1", "1", "0", nodeValue.Trim());
                    return;
                }
                if ("WtBatchNo".Equals(nodeName.Trim()))
                {
                    getWtLklNoNum("1", "1", "1", nodeValue.Trim());
                    return;
                }
                try
                {
                    ConfigClass config = new ConfigClass();
                    config.Name = nodeName;
                    config.FormalValue = nodeValue;
                    SqlLiteHelper.SqlLiteHelper.update(config);
                }
                catch (Exception ex) { log.Write("error:writerNode:" + ex.Message); }
            }
            catch (Exception ex) { log.Write("error:readerNode:" + ex.Message);}
        }
        public static string getWtLklNoNum(string ope_type, string bus_type, string field_type, string value)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("ope_type", ope_type);
            parameters.Add("bus_type", bus_type);
            parameters.Add("field_type", field_type);
            parameters.Add("t_id", ConfigurationManager.AppSettings["MechineNo"]);
            parameters.Add("t_value", value);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("operSePcNum"), parameters, null);
            log.Write(jsonText);
            return jsonText;
        }
        public static void createSysFolder()
        {
            try
            {
                string addressPathDown = readerNode("addressPathDown");
                string addressPathInstall = readerNode("addressPathInstall");
                if (!Directory.Exists(addressPathDown))
                {
                    Directory.CreateDirectory(addressPathDown);
                }
                if (!Directory.Exists(addressPathInstall))
                {
                    Directory.CreateDirectory(addressPathInstall);
                }
            }
            catch (Exception)
            {
                createSysFolder();
            }
        }
       
    }
}
