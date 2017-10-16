using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WeatherModel
{
    /// <summary>
    /// 查询天气请求参数
    /// </summary>
    public class WeatherParam
    {
        public string appId= "WEA002";
        public string conName= "城市信息查询";
        public string cityweatercode = "101160101";
    }

    /// <summary>
    /// 天气返回结果
    /// </summary>
    public class WeatherInfo
    {
        public WeatherInfoData data { get; set; }
        public string u { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
    }

    public class WeatherInfoData
    {
        public WeatherInfoDataRetData retData { get; set; }
        public string errNum { get; set; }
        public string errMsg { get; set; }

    }

    public class WeatherInfoDataRetData
    {
        public string city { get; set; }
        public string cityid { get; set;}
        public WeatherInfoDataRetDataToday today { get; set; }
        public List<WeatherInfoDataRetDataForecast> forecast { get; set; }
        public List<WeatherInfoDataRetDataHistory> history { get; set; }
    }
    public class WeatherInfoDataRetDataToday
    {
        public string date { get; set; }
        public string week { get; set; }
        public string curTemp { get; set; }
        public string aqi { get; set; }
        public string fengxiang { get; set; }
        public string fengli { get; set; }
        public string hightemp { get; set; }
        public string lowtemp { get; set; }
        public string type { get; set; }
        public List<WeatherInfoDataRetDataTodayIndex> index { get; set; }
    }

    public class WeatherInfoDataRetDataTodayIndex
    {
        public string name { get; set; }
        public string code { get; set; }
        public string index { get; set; }
        public string details { get; set; }
        public string otherName { get; set; }
    }
    public class WeatherInfoDataRetDataForecast
    {
        public string date { get; set; }
        public string week { get; set; }
        public string fengxiang { get; set; }
        public string fengli { get; set; }
        public string hightemp { get; set; }
        public string lowtemp { get; set; }
        public string type { get; set; }
    }
    public class WeatherInfoDataRetDataHistory
    {
        public string date { get; set; }
        public string week { get; set; }
        public string aqi { get; set;}
        public string fengxiang { get; set; }
        public string fengli { get; set; }
        public string hightemp { get; set; }
        public string lowtemp { get; set; }
        public string type { get; set; }
    }




















}
