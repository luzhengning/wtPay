using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    /// <summary>
    /// 燃气登录认证传递参数
    /// </summary>
    public class GasLoginParam
    {
        /// <summary>
        /// 交易时间
        /// </summary>
        public string trandateTime;
        /// <summary>
        /// 交易号
        /// </summary>
        public string servicename;
        /// <summary>
        /// 请求流水号
        /// </summary>
        public string resqn;

        public string loginId;
    }
}
