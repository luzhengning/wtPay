using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.WeatherModel;

namespace wtPayDAL
{
    public class WeatherAccess
    {
        /// <summary>
        /// 查询天气
        /// </summary>
        /// <returns></returns>
        public static WeatherInfo QueryWeather()
        {
            try {
                WeatherParam param = new WeatherParam();
                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("appId", param.appId);
                parameters.Add("conName", param.conName);
                parameters.Add("cityweatercode", param.cityweatercode);
                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("WeatherForecast"), parameters, null);

                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                return JsonConvert.DeserializeObject<WeatherInfo>(jsonText);
            }catch(Exception ex)
            {
                log.Write("查询天气异常："+ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询PM2.5
        /// </summary>
        /// <returns></returns>
        public static PmInfo QueryPm()
        {
            try {
                PmParam param = new PmParam();

                Dictionary<String, String> parameters = new Dictionary<String, String>();
                parameters.Add("appId", param.appId);
                parameters.Add("conName", param.conName);

                string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryPM25"), parameters, null);

                //反序列化JSON字符串,将JSON字符串转换成LIST列表  
                return JsonConvert.DeserializeObject<PmInfo>(jsonText);
            }catch(Exception ex)
            {
                log.Write("查询PM2.5异常："+ex.Message);
                return null;
            }
        }
    }
}
