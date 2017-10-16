using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using WtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel.SocialSecurityModel;

namespace wtPayDAL
{
    /// <summary>
    /// 社保查询
    /// </summary>
    public class SocialSecurityAccess
    {

        /// <summary>
        /// 养老发放信息查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Object QueryPensionGrantInfo<T>(SocialSecurityParam param, ref string recode, ref string msg)
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();

                parameters.Add("grcard", param.grcard);
                parameters.Add("idcard", param.idcard);
                parameters.Add("password", param.password);
                parameters.Add("qstime", "201001");
                parameters.Add("jztime", DateTime.Now.ToString("yyyyMM"));
                parameters.Add("appId", param.Id);
                parameters.Add("conName", param.conName);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("querySocialSecurity"), parameters, null);
                if (jsonText.IndexOf("密码错误") > 0)
                {
                    recode = "9999";
                    msg = "密码错误";
                }
                if (jsonText.IndexOf("个人信息不存在") > 0)
                {
                    recode = "9999";
                    msg = "个人信息不存在";
                }
                if (jsonText.IndexOf("身份证号不正确") > 0)
                {
                    recode = "9999";
                    msg = "身份证号不正确";
                }
                if (jsonText.IndexOf("未查到相关信息，请核对查询信息") > 0)
                {
                    recode = "9999";
                    msg = "未查到相关信息，请核对查询信息";
                }
                if (jsonText.IndexOf("社保卡号密码错误") > 0)
                {
                    recode = "9999";
                    msg = "社保卡号密码错误,请检查";
                }
                if (jsonText.IndexOf("查询信息不存在") > 0)
                {
                    recode = "9999";
                    msg = "查询信息不存在";
                }
                if (jsonText.IndexOf("未查询到个人养老月账户信息") > 0)
                {
                    recode = "9999";
                    msg = "未查询到个人养老月账户信息";
                }
                //反序列化JSON字符串,将JSON字符串转换成LIST列表 
                return JsonConvert.DeserializeObject<T>(jsonText);

            }
            catch (Exception ex) { log.Write("社保查询异常：" + ex.Message); return null; }
        }
    }
}
