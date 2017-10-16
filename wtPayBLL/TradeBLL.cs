using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel;

namespace wtPayBLL
{
    public class TradeBLL
    {
        
        /// <summary>
        /// 发送拉卡拉万通签到记录
        /// </summary>
        /// <param name="paySign"></param>
        public static void SendSignRecord(PaySign paySign)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("paySign.mechine_no", paySign.mechine_no);
                parameters.Add("paySign.terminal_no", paySign.terminal_no);
                parameters.Add("paySign.shop_no", paySign.shop_no);

                parameters.Add("paySign.sign_type", paySign.sign_type);
                parameters.Add("paySign.sign_result", paySign.sign_result);
                string url = SysConfigHelper.readerNode("lklwtSign");
                string jsonResult = HttpHelper.getHttp(url, parameters, null);
            }
            catch (Exception e)
            {
                log.Write("向后台发送拉卡拉万通签到结果时出错：" + e.Message);
            }
        }
        /// <summary>
        /// 发送订单记录
        /// </summary>
        /// <param name="tradeRecord"></param>
        /// <returns></returns>
        public static string SendOrderRecord(TradeRecord tradeRecord)
        {

            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("paySerial.cloud_no", tradeRecord.cloud_no);
                parameters.Add("paySerial.write_card_state", tradeRecord.write_card_stat);
                parameters.Add("paySerial.termail_no", tradeRecord.termail_no);
                parameters.Add("paySerial.order_no", tradeRecord.order_no);
                parameters.Add("paySerial.branch_shop_no", tradeRecord.branch_shop_no);
                parameters.Add("paySerial.branch_termail_no", tradeRecord.branch_termail_no);
                string url = SysConfigHelper.readerNode("savePaymentLog");
                string jsonResult = HttpHelper.getHttp(url, parameters, null);
                JObject jobject = JObject.Parse(jsonResult);

                tradeRecord.id = jobject["data"].ToString();

                return tradeRecord.id;

            }
            catch (Exception e)
            {
                log.Write("向后台发送订单记录时出错，订单号：" + tradeRecord.cloud_no + "，错误详情：" + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 发送订单支付记录
        /// </summary>
        /// <param name="tradeRecord"></param>
        /// <returns></returns>
        public static string SendOrderPayRecord(TradeRecord tradeRecord)
        {

            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("paySerial.id", tradeRecord.id);
                parameters.Add("data", tradeRecord.data_id);
                parameters.Add("paySerial.lkl_wt_state", tradeRecord.lkl_wt_state.ToString());
                parameters.Add("paySerial.termail_no", tradeRecord.termail_no);
                parameters.Add("paySerial.order_no", tradeRecord.order_no);
                parameters.Add("paySerial.batch_no", tradeRecord.batch_no);
                parameters.Add("paySerial.relation_order", tradeRecord.relation_order);
                parameters.Add("paySerial.order_type", tradeRecord.order_type.ToString());
                parameters.Add("paySerial.shop_type", tradeRecord.shop_type.ToString());
                parameters.Add("paySerial.reconc_str", tradeRecord.reconc_str);
                parameters.Add("paySerial.amount", tradeRecord.amount);
                string url = SysConfigHelper.readerNode("savePaymentLog");
                string jsonResult = HttpHelper.getHttp(url, parameters, null);
                JObject jobject = JObject.Parse(jsonResult);
                return jobject["data"].ToString();
            }
            catch (Exception e)
            {
                log.Write("向后台发送订单支付记录时出错，订单号：" + tradeRecord.cloud_no + "，错误详情：" + e.Message);
                return null;
            }
        }
        public static string SendOrderRefundRecord(TradeRecord tradeRecord)
        {

            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("paySerial.cloud_no", tradeRecord.cloud_no);
                parameters.Add("data", tradeRecord.data_id);
                parameters.Add("paySerial.lkl_wt_state", tradeRecord.lkl_wt_state.ToString());
                parameters.Add("paySerial.termail_no", tradeRecord.termail_no);
                parameters.Add("paySerial.order_no", tradeRecord.order_no);
                parameters.Add("paySerial.batch_no", tradeRecord.batch_no);
                parameters.Add("paySerial.relation_order", tradeRecord.relation_order);
                parameters.Add("paySerial.order_type", tradeRecord.order_type.ToString());
                parameters.Add("paySerial.shop_type", tradeRecord.shop_type.ToString());
                parameters.Add("paySerial.reconc_str", tradeRecord.reconc_str);
                parameters.Add("paySerial.amount", tradeRecord.amount);
                parameters.Add("paySerial.lkl_wt_shop_no", tradeRecord.lkl_wt_shop_no);
                string url = SysConfigHelper.readerNode("savePaymentLog");
                string jsonResult = HttpHelper.getHttp(url, parameters, null);
                JObject jobject = JObject.Parse(jsonResult);
                //return jobject["data"].ToString();
                return "";
            }
            catch (Exception e)
            {
                log.Write("向后台发送退款记录时出错，订单号：" + tradeRecord.cloud_no + "，错误详情：" + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 发送云缴费通知记录
        /// </summary>
        /// <param name="tradeRecord"></param>
        /// <returns></returns>
        public static string SendCloudNoticeRecord(TradeRecord tradeRecord)
        {

            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("paySerial.id", tradeRecord.id);
                parameters.Add("paySerial.cloud_state", tradeRecord.cloud_state.ToString());
                parameters.Add("paySerial.order_state", tradeRecord.order_state);
                parameters.Add("paySerial.write_card_state", tradeRecord.write_card_stat);
                parameters.Add("paySerial.reconc_str", tradeRecord.reconc_str);
                string url = SysConfigHelper.readerNode("savePaymentLog");
                string jsonResult = HttpHelper.getHttp(url, parameters, null);
                JObject jobject = JObject.Parse(jsonResult);
                return jobject["data"].ToString();
            }
            catch (Exception e)
            {
                log.Write("向后台发送云平台结果时出错，订单号：" + tradeRecord.cloud_no + "，错误详情：" + e.Message);
                return null;
            }
        }




        


    }

}
