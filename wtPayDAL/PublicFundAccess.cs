using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PublicFundModel;
using wtPayBLL;
using WtPayBLL;
using wtPayCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wtPayDAL
{
    /// <summary>
    /// 公积金查询
    /// </summary>
    public class PublicFundAccess
    {
        /// <summary>
        /// 个人客户信息查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static CustomerInfo QueryCustomerInfo(CustomerParam param)
        {
            CustomerInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();

            parameters.Add("selcnt", param.selcnt);
            parameters.Add("seltype", param.seltype);
            parameters.Add("pwd", param.pwd);
            parameters.Add("appId", param.Id);
            parameters.Add("conName", param.conName);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("accumulationFund"), parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<CustomerInfo>(jsonText);

            if (jsonText.IndexOf("密码错误") > 0)
            {
                info.recode = "9999";
                info.msg = "密码错误";
            }
            if (jsonText.IndexOf("个人信息不存在") > 0)
            {
                info.recode = "9999";
                info.msg = "个人信息不存在";
            }
            if (jsonText.IndexOf("身份证号不正确") > 0)
            {
                info.recode = "9999";
                info.msg = "身份证号不正确";
            }
            if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
            {
                info.recode = "9999";
                info.msg = "未查到相关信息，请核对查询信息";
            }
            if (jsonText.IndexOf("社保卡号密码错误") > 0)
            {
                info.recode = "9999";
                info.msg = "社保卡号密码错误,请检查";
            }
            if (jsonText.IndexOf("查询信息不存在") > 0)
            {
                info.recode = "9999";
                info.msg = "查询信息不存在";
            }
            return info;
        }


        /// <summary>
        /// 个人公积金账户查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PublicAccountInfo QueryPublicAccountInfo(CustomerParam param)
        {
            PublicAccountInfo info = new PublicAccountInfo();
            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                parameters.Add("selcnt", param.selcnt);
                parameters.Add("seltype", param.seltype);
                parameters.Add("pwd", param.pwd);
                parameters.Add("appId", param.Id);
                parameters.Add("conName", param.conName);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("accumulationFund"), parameters, null);
                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (json == null) return null;

                if (json["msg"] != null) { if (json["msg"].ToString().Length >= 4) info.msg = json["msg"].ToString(); else { info.msg = "查询失败..."; } }
                if (jsonText.IndexOf("密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "密码错误";
                }
                if (jsonText.IndexOf("个人信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "个人信息不存在";
                }
                if (jsonText.IndexOf("身份证号不正确") > 0)
                {
                    info.recode = "9999";
                    info.msg = "身份证号不正确";
                }
                if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
                {
                    info.recode = "9999";
                    info.msg = "未查到相关信息，请核对查询信息";
                }
                if (jsonText.IndexOf("社保卡号密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "社保卡号密码错误,请检查";
                }
                if (jsonText.IndexOf("查询信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "查询信息不存在";
                }
                PublicAccountInfoDatas list = null;
                if (json["data"].Count() >= 1)
                {
                    info.data = new List<PublicAccountInfoDatas>();
                    JObject data = (JObject)JsonConvert.DeserializeObject(json["data"].ToString());
                    for (int i = 1; i <= data.Count; i++)
                    {
                        list = new PublicAccountInfoDatas();
                        list.data1 = data["data" + i]["data1"].ToString();
                        list.data2 = data["data" + i]["data2"].ToString();
                        list.data3 = data["data" + i]["data3"].ToString();
                        list.data4 = data["data" + i]["data4"].ToString();
                        list.data5 = data["data" + i]["data5"].ToString();
                        list.data6 = data["data" + i]["data6"].ToString();
                        list.data7 = data["data" + i]["data7"].ToString();
                        list.data8 = data["data" + i]["data8"].ToString();
                        list.data9 = data["data" + i]["data9"].ToString();
                        info.data.Add(list);
                    }
                }
                info.dateTime = json["dateTime"].ToString();
                info.success = json["success"].ToString();

                info.u = json["u"].ToString();
            }catch(Exception ex) { }

            return info;
        }

        /// <summary>
        /// 个人公积金明细查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PublicFundDetailedInfo QueryPublicFundDetailedInfo(PublicDetailedParam param)
        {
            PublicFundDetailedInfo info = new PublicFundDetailedInfo();

            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                parameters.Add("percode", param.percode);
                parameters.Add("bdate", "20100101");
                parameters.Add("edate", DateTime.Now.ToString("yyyyMMdd"));
                parameters.Add("pwd", param.pwd);
                parameters.Add("appId", param.Id);
                parameters.Add("conName", param.conName);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("accumulationFund"), parameters, null);
                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (json == null) return null;

                if (json["msg"] != null) { if (json["msg"].ToString().Length >= 4) info.msg = json["msg"].ToString(); else { info.msg = "查询失败..."; } }
                if (jsonText.IndexOf("密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "密码错误";
                }
                if (jsonText.IndexOf("个人信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "个人信息不存在";
                }
                if (jsonText.IndexOf("身份证号不正确") > 0)
                {
                    info.recode = "9999";
                    info.msg = "身份证号不正确";
                }
                if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
                {
                    info.recode = "9999";
                    info.msg = "未查到相关信息，请核对查询信息";
                }
                if (jsonText.IndexOf("社保卡号密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "社保卡号密码错误,请检查";
                }
                if (jsonText.IndexOf("查询信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "查询信息不存在";
                }
                PublicFundDetailedInfoData list = null;
                if (json["data"].Count() >= 1)
                {
                    info.data = new List<PublicFundDetailedInfoData>();
                    JObject data = (JObject)JsonConvert.DeserializeObject(json["data"].ToString());
                    for (int i = 1; i <= data.Count; i++)
                    {
                        list = new PublicFundDetailedInfoData();
                        list.data1 = data["data" + i]["data1"].ToString();
                        list.data2 = data["data" + i]["data2"].ToString();
                        list.data3 = data["data" + i]["data3"].ToString();
                        list.data4 = data["data" + i]["data4"].ToString();
                        list.data5 = data["data" + i]["data5"].ToString();
                        list.data6 = data["data" + i]["data6"].ToString();
                        info.data.Add(list);
                    }
                }
                info.dateTime = json["dateTime"].ToString();
                info.success = json["success"].ToString();

                info.u = json["u"].ToString();
            }
            catch { }
            return info;
        }

        /// <summary>
        /// 个人公积金贷款余额查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static LoanBalanceInfo QueryLoanBalanceInfo(CustomerParam param)
        {
            LoanBalanceInfo info = new LoanBalanceInfo();
            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                parameters.Add("selcnt", param.selcnt);
                parameters.Add("seltype", param.seltype);
                parameters.Add("pwd", param.pwd);
                parameters.Add("appId", param.Id);
                parameters.Add("conName", param.conName);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("accumulationFund"), parameters, null);
                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (json == null) return null;

                if (json["msg"] != null) { if (json["msg"].ToString().Length >= 4) info.msg = json["msg"].ToString(); else { info.msg = "查询失败..."; } }
                if (jsonText.IndexOf("密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "密码错误";
                }
                if (jsonText.IndexOf("个人信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "个人信息不存在";
                }
                if (jsonText.IndexOf("身份证号不正确") > 0)
                {
                    info.recode = "9999";
                    info.msg = "身份证号不正确";
                }
                if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
                {
                    info.recode = "9999";
                    info.msg = "未查到相关信息，请核对查询信息";
                }
                if (jsonText.IndexOf("社保卡号密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "社保卡号密码错误,请检查";
                }
                if (jsonText.IndexOf("查询信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "查询信息不存在";
                }

                
                info.data = new LoanBalanceInfoData();
                info.data.pername = json["data"]["pername"].ToString();
                info.data.percode = json["data"]["percode"].ToString();
                info.data.cardtype = json["data"]["cardtype"].ToString();
                info.data.cardcode = json["data"]["cardcode"].ToString();
                info.data.agrcode = json["data"]["agrcode"].ToString();
                info.data.payacc = json["data"]["payacc"].ToString();
                info.data.loanmny = json["data"]["loanmny"].ToString();
                info.data.loanbal = json["data"]["loanbal"].ToString();
                info.data.payedmths = json["data"]["payedmths"].ToString();
                info.data.overmths = json["data"]["overmths"].ToString();
                info.data.lastpaydate = json["data"]["lastpaydate"].ToString();
                info.data.totoalmny = json["data"]["totoalmny"].ToString();
                info.data.corpus = json["data"]["corpus"].ToString();
                info.data.interests = json["data"]["interests"].ToString();
                info.data.overmny = json["data"]["overmny"].ToString();
                info.dateTime = json["dateTime"].ToString();
                info.success = json["success"].ToString();
                //
                info.u = json["u"].ToString();
            }
            catch { }
            return info;
        }

        /// <summary>
        /// 个人公积金贷款还款明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static LoanDetailedInfo QueryLoanDetailedInfo(CustomerParam param)
        {
            LoanDetailedInfo info = new LoanDetailedInfo();
            try {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                parameters.Add("selcnt", param.selcnt);
                parameters.Add("seltype", param.seltype);
                parameters.Add("pwd", param.pwd);
                parameters.Add("appId", param.Id);
                parameters.Add("conName", param.conName);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("accumulationFund"), parameters, null);
                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (json == null) return null;

                if (json["msg"] != null) { if (json["msg"].ToString().Length >= 4) info.msg = json["msg"].ToString(); else { info.msg = "查询失败..."; } }
                if (jsonText.IndexOf("密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "密码错误";
                }
                if (jsonText.IndexOf("个人信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "个人信息不存在";
                }
                if (jsonText.IndexOf("身份证号不正确") > 0)
                {
                    info.recode = "9999";
                    info.msg = "身份证号不正确";
                }
                if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
                {
                    info.recode = "9999";
                    info.msg = "未查到相关信息，请核对查询信息";
                }
                if (jsonText.IndexOf("社保卡号密码错误") > 0)
                {
                    info.recode = "9999";
                    info.msg = "社保卡号密码错误,请检查";
                }
                if (jsonText.IndexOf("查询信息不存在") > 0)
                {
                    info.recode = "9999";
                    info.msg = "查询信息不存在";
                }
                LoanDetailedInfoData list = null;
                if (json["data"].Count() >= 1)
                {
                    info.data = new List<LoanDetailedInfoData>();
                    JObject data = (JObject)JsonConvert.DeserializeObject(json["data"].ToString());
                    for (int i = 1; i <= data.Count; i++)
                    {
                        list = new LoanDetailedInfoData();
                        list.data1 = data["data" + i]["data1"].ToString();
                        list.data2 = data["data" + i]["data2"].ToString();
                        list.data3 = data["data" + i]["data3"].ToString();
                        list.data4 = data["data" + i]["data4"].ToString();
                        list.data5 = data["data" + i]["data5"].ToString();
                        list.data6 = data["data" + i]["data6"].ToString();
                        list.data7 = data["data" + i]["data7"].ToString();
                        list.data8 = data["data" + i]["data8"].ToString();
                        info.data.Add(list);
                    }
                }
                info.dateTime = json["dateTime"].ToString();
                info.success = json["success"].ToString();

                info.u = json["u"].ToString();
            }
            catch { }
            return info;
        }
        
    }

}
