using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.GasModel
{
    public class GasShowInfo{
        public static string rqhints="";
    }
    /// <summary>
    /// 燃气登录认证返回实体类
    /// </summary>
    public class GasLoginInfo
    {
        public GasLoginMsgrsp msgrsp
        { get; set; }
        public GasLoginMsghead msghead
        { get; set; }
        public GasLoginGsgtext msgtext { get; set; }
    }
    public class GasLoginGsgtext
    {
        public string rqhints { get; set; }
    }
    public class GasLoginMsgrsp
    {
        public string retcode
        { get; set; }
        public string authcode
        { get; set; }
        public string retshow
        { get; set; }

    }
    public class GasLoginMsghead
    {
        public string trandatetime
        { get; set; }
        public string ressn
        { get; set; }
        public string tranchannel
        { get; set; }
        public string devno
        { get; set; }
        public string resqn
        { get; set; }
        public string servicename
        { get; set; }
        public string version
        { get; set; }
    }
}
