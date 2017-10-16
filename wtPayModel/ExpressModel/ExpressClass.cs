using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.ExpressModel
{
    /// <summary>
    /// 快递参数类
    /// </summary>
    public class ExpressClass
    {
        /// <summary>
        /// 邮政接口编码
        /// </summary>
        public static string youzhengAppId = "EXP002";

        /// <summary>
        /// 圆通接口编码
        /// </summary>
        public static string yuantonAppId = "EXP005";

        /// <summary>
        /// 申通接口编码
        /// </summary>
        public static string shentonAppId = "EXP003";

        /// <summary>
        /// 汇升接口编码
        /// </summary>
        public static string huishengAppId = "EXP004";

        /// <summary>
        /// 宅急送接口编码
        /// </summary>
        public static string zhaijisongAppId = "EXP001";

        /// <summary>
        /// 中铁快运接口编码
        /// </summary>
        public static string zhongtieAppId = "EXP007";
    }

    /// <summary>
    /// 快递查询参数
    /// </summary>
    public class ExpressQueryParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string billcode { get; set; }
    }
}
