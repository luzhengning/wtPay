using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using wtPayBLL;
using wtPayModel;

namespace RefundUtils
{
    public class TradeRecordUtils
    {
        private static string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "tradeRecord.xml";
        public static void SendTradeRecord()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("records");
                XmlNodeList list = root.ChildNodes;


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                foreach (XmlNode xn in list)
                {
                    string data_id = xn.ChildNodes[0].InnerText;
                    string lkl_wt_state = xn.ChildNodes[1].InnerText;
                    string write_card_state = xn.ChildNodes[2].InnerText;
                    string cloud_state = xn.ChildNodes[3].InnerText;
                    string termail_no = xn.ChildNodes[4].InnerText;
                    string order_no = xn.ChildNodes[5].InnerText;
                    string batch_no = xn.ChildNodes[6].InnerText;
                    string relation_order = xn.ChildNodes[7].InnerText;
                    string order_type = xn.ChildNodes[8].InnerText;
                    string shop_type = xn.ChildNodes[9].InnerText;
                    string reconc_str = xn.ChildNodes[10].InnerText;
                    string amount = xn.ChildNodes[11].InnerText;
                    string cloud_no = xn.ChildNodes[12].InnerText;
                    
                    parameters.Add("paySerial.data_id", data_id);
                    parameters.Add("paySerial.lkl_wt_state", lkl_wt_state.ToString());
                    parameters.Add("paySerial.write_card_state", write_card_state.ToString());
                    parameters.Add("paySerial.cloud_state", cloud_state.ToString());

                    parameters.Add("paySerial.termail_no", termail_no);
                    parameters.Add("paySerial.order_no", order_no);
                    parameters.Add("paySerial.batch_no", batch_no);
                    parameters.Add("paySerial.relation_order", relation_order);
                    parameters.Add("paySerial.order_type", order_type.ToString());
                    parameters.Add("paySerial.shop_type", shop_type.ToString());
                    parameters.Add("paySerial.reconc_str", reconc_str);
                    parameters.Add("paySerial.amount", amount);
                    parameters.Add("paySerial.cloud_no", cloud_no);
                    
                    string jsonResult = HttpHelper.getHttp(SysConfigHelper.readerNode("savePaymentLog"), parameters, null);
                    JObject jobject = JObject.Parse(jsonResult);
                    if ("1".Equals(jobject["state"]))
                    {//发送成功
                        root.RemoveChild(xn);
                        xmlDoc.Save(filePath);
                    }
                }
            }
            catch (Exception e)
            {

                log.Write("向后台发送交易纪录异常：" + e.Message);
            }
        }


        /// <summary>
        /// 向退款文件添加一条万通卡退款记录
        /// </summary>
        /// <param name="_2"></param>
        /// <param name="_4"></param>
        /// <param name="_59"></param>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <param name="orderNo"></param>
        /// <param name="transType"></param>
        /// <param name="conName"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool AddTradeRecord(TradeRecord tradeRecord)
        {
            try
            {
               
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode root = xmlDoc.SelectSingleNode("records");
                XmlElement record = xmlDoc.CreateElement("record");

                createXmlNode(record, "data_id",tradeRecord.data_id, xmlDoc);
                createXmlNode(record, "lkl_wt_state",tradeRecord.lkl_wt_state.ToString(), xmlDoc);//金额
                createXmlNode(record, "write_card_state", tradeRecord.write_card_stat.ToString(), xmlDoc);
                createXmlNode(record, "cloud_state", tradeRecord.cloud_state.ToString(), xmlDoc);
                createXmlNode(record, "termail_no", tradeRecord.termail_no, xmlDoc);
                createXmlNode(record, "order_no", tradeRecord.order_no, xmlDoc);
                createXmlNode(record, "batch_no", tradeRecord.batch_no, xmlDoc);
                createXmlNode(record, "relation_order", tradeRecord.relation_order, xmlDoc);
                createXmlNode(record, "order_type", tradeRecord.order_type, xmlDoc);
                createXmlNode(record, "shop_type", tradeRecord.shop_type, xmlDoc);
                createXmlNode(record, "reconc_str", tradeRecord.reconc_str, xmlDoc);
                createXmlNode(record, "amount", tradeRecord.amount, xmlDoc);
                createXmlNode(record, "cloud_no", tradeRecord.cloud_no, xmlDoc);
                
                root.AppendChild(record);
                xmlDoc.Save(filePath);
                log.Write("添加待发送交易纪录成功");
                return true;
            }
            catch (Exception e)
            {
                log.Write("添加待发送交易纪录异常：" + e.Message);
            }

            return false;
        }
        private static XmlElement createXmlNode(XmlElement fatherNode, string nodeName, string value, XmlDocument xmlDoc)
        {
            XmlElement sonNode = xmlDoc.CreateElement(nodeName);
            sonNode.InnerText = value;
            fatherNode.AppendChild(sonNode);
            return sonNode;
        }
    }
}
