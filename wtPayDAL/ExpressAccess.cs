using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.ExpressModel;

namespace wtPayDAL
{
    /// <summary>
    /// 快递查询类
    /// </summary>
    public class ExpressAccess
    {
        /// <summary>
        /// 快递查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExpressQuery<T>(ExpressQueryParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("billcode", param.billcode);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryExpress"), parameters, null);
            //jsonText = "{\"data\":{\"detail\":[{\"time\":\"2016 - 11 - 16 05:15:03\",\"scantype\":\"离开\",\"Waybill_No\":\"9890266089353\",\"memo\":\"离开【西安】，下一站【电子发投】\"},{\"time\":\"2016 - 11 - 16 06:30:17\",\"scantype\":\"到达\",\"Waybill_No\":\"9890266089353\",\"memo\":\"到达【电子城发投】\"},{\"time\":\"2016 - 11 - 16 10:51:28\",\"scantype\":\"【电子城发投】正在投递,投递员：吕江 63625492\",\"Waybill_No\":\"9890266089353\",\"memo\":\"【电子城发投】正在投递,投递员：吕江 63625492\"},{\"time\":\"2016 - 11 - 16 20:10:05\",\"scantype\":\"【电子城发投】已妥投,投递员：吕江 63625492\",\"Waybill_No\":\"9890266089353\",\"memo\":\"已签收,扬州便利店 代收【电子城发投】\"}],\"billcode\":\"9890266089353\"},\"dateTime\":\"2017 - 03 - 27 15:41:33\",\"success\":true,\"msg\":\"\",\"u\":\"193c54ecef8c6dc07a48b9cd28b93b35\"}";
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}
