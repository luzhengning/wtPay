using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡发送验证码返回结果
    /// </summary>
    public class WintopSendValidateCodeInfo
    {
        public WintopSendValidateCodeInfoMsg msgrsp { get; set; }
        public Msghead msghead { get; set; }
    }
    public class WintopSendValidateCodeInfoMsg
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
    }
}
