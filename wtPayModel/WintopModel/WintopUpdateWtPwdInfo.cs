using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡密码修改响应结果
    /// </summary>
    public class WintopUpdateWtPwdInfo
    {
        public WintopUpdateWtPwdInfoMsg msgrsp { get; set; }
        public Msghead msghead { get; set; }
    }
    public class WintopUpdateWtPwdInfoMsg
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string merchantNo { get; set; }
    }
}
