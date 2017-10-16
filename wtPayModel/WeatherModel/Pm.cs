using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WeatherModel
{
    /// <summary>
    /// PM2.5请求参数
    /// </summary>
    public class PmParam
    {
        public string appId="PM001";
        public string conName= "兰州PM2.5查询";
    }

    /// <summary>
    /// PM2.5返回结果
    /// </summary>
    public class PmInfo
    {
        public PmInfoData data { get; set; }
        public string u { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }

    }
    public class PmInfoData
    {
        public string pmid { get; set; }
        public string aqi { get; set; }
        public string area { get; set; }
        public string area_code { get; set; }
        public string co { get; set; }
        public string ct { get; set; }
        public string no2 { get; set; }
        public string o3 { get; set; }
        public string o3_8h { get; set; }
        public string pm10 { get; set; }
        public string pm2_5 { get; set; }
        public string primary_pollutant { get; set; }
        public string quality { get; set; }
        public string so2 { get; set; }
        public string remainder { get; set; }
        public string alalysis { get; set; }
        public string createtime { get; set; }

    }
}
