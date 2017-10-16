using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.MediaResourceModel
{
    /// <summary>
    /// 是否更新广告
    /// </summary>
    public class FindIsUpdateAdvInfo
    {
        public string message { get; set; }
        public FindIsUpdateAdvInfoData data { get; set; }
        public string code { get; set; }
    }

    public class FindIsUpdateAdvInfoData
    {
        public string id { get; set; }
        public string advert_activate { get; set; }
        public string name { get; set; }
        public string create_time { get; set; }
        public string del_flag { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// 广告更新详情获取
    /// </summary>
    public class FindDownAdvInfo
    {
        public string message { get; set; }
        public FindDownAdvInfoData data { get; set; }
        public string code { get; set; }
    }
    public class FindDownAdvInfoData
    {
        public string id { get; set; }
        public string property_type { get; set; }
        public string layout_type { get; set; }
        public string data { get; set; }
        public string create_time { get; set; }
        public string m_name { get; set; }
        public string m_id { get; set; }
    }
}