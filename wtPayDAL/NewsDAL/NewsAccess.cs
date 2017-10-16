using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.NewsModel;

namespace wtPayDAL.NewsDAL
{
    /// <summary>
    /// 政务信息
    /// </summary>
    public class NewsAccess
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static NewsListInfo QueryNewsLists(QueryNewsParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", News.queryPoliticalNewsAppId);
            parameters.Add("conName", param.conName);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryPoliticalNewsName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<NewsListInfo>(jsonText);
        }
        
        /// <summary>
        /// 新闻详情        
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static NewsDetailsInfo QueryNewsDetailsInfo(QueryNewsDetailsParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", News.newsInfoAppId);
            parameters.Add("articleId", param.articleId);
            parameters.Add("articlePath", param.articlePath);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("queryPoliticalNewsName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<NewsDetailsInfo>(jsonText);
        }
    }
}
