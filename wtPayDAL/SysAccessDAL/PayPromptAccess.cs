using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.PromptModel;

namespace wtPayDAL.SysAccessDAL
{
    public class PayPromptAccess
    {
        /// <summary>
        /// 查询缴费页面提示
        /// </summary>
        /// <param name="hint_code"></param>
        /// <returns></returns>
        public static PayPromptInfo queryPayPrompt(string hint_code)
        {
            try
            {
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("hint_code", hint_code);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("findHintName"), parameters, null);
                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                return JsonConvert.DeserializeObject<PayPromptInfo>(jsonText);
            }catch(Exception ex)
            {
                log.Write("error:获取缴费页面提示错误："+ex.Message+ex.InnerException);
                return SysBLL.payPromptInfo;
            }

        }
        /// <summary>
        /// 根据当前的缴费页面类型获取提示语
        /// </summary>
        /// <param name="tcType"></param>
        /// <returns></returns>
        public static string getPrompt(string tcType)
        {
            try
            {
                if (SysBLL.payPromptInfo == null) return "";
                foreach (PayPromptInfoData data in SysBLL.payPromptInfo.data)
                {
                    if (data.service_type.Equals(tcType))
                    {
                        return data.hint;
                    }
                }
            }
            catch (Exception ex) { return ""; }
            return "";
        }
    }
}
