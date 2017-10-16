using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.NewsModel
{
    public class News
    {
        public static string queryPoliticalNewsAppId = "SZXW001";
        public static string newsInfoAppId = "SZXW002";
    }
    /// <summary>
    /// 新闻查询参数
    /// </summary>
    public class QueryNewsParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
    }
    /// <summary>
    /// 新闻列表
    /// </summary>
    public class NewsListInfo
    {
        public List<NewsListInfoData> data { get; set; }
        public string u { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
    }
    public class NewsListInfoData
    {
        public string pubTime { get; set; }
        public string title { get; set; }
        public string subscriber { get; set; }
        public string articlePath { get; set; }
        public string articleId { get; set; }
        public string articleUrl { get; set; }
    }
    /// <summary>
    /// 新闻详情查询
    /// </summary>
    public class QueryNewsDetailsParam
    {
        public string appId { get; set; }
        public string  articleId{get;set;}
        public string articlePath{get;set;}

    }
    /// <summary>
    /// 新闻详情
    /// </summary>
    public class NewsDetailsInfo
    {
        public NewsDetailsInfoData data { get; set; }
        public string u { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
    }
    public class NewsDetailsInfoData
    {
        public string pubTime { get; set; }
        public string content { get; set; }
        public string createTime { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string sourceName { get; set; }
        public string liability { get; set; }
        public string processor { get; set; }
        public string subscriber { get; set; }
        public string articleId { get; set; }
    }
}
