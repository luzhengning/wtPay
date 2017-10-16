using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel
{
    public class MaintainSignInfo
    {
        public string message { get; set; }
        public List<MaintainSignInfoData> data { get; set; }
        public string code { get; set; }
    }

    public class MaintainSignInfoData
    {
        public string code_des { get; set; }
        public string code_no { get; set; }
    }

    /// <summary>
    /// 维护人员登录参数
    /// </summary>
    public class MaintainSignParam
    {
        private string account;
        private string passsword;
        private MaintainSignInfo maintainSignInfo;

        public string Passsword
        {
            get
            {
                return passsword;
            }

            set
            {
                passsword = value;
            }
        }

        public MaintainSignInfo MaintainSignInfo
        {
            get
            {
                return maintainSignInfo;
            }

            set
            {
                maintainSignInfo = value;
            }
        }

        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
            }
        }
    }
    public class OnLineInfo
    {
        public string code { get; set; }
        public OnLineInfoData data { get; set; }
        public string msg { get; set; }
        public string msgCode { get; set; }
    }
    public class OnLineInfoData
    {
        public string update_time { get; set; }
        public Boolean online { get; set; }
    }
}
