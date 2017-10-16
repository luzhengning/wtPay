using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡密码修改请求参数
    /// </summary>
    public class WintopUpdateWtPwdParam
    {
        private string trandateTime;
        private string loginId;
        private string reqsn;
        private string servicename;
        private string authcode;
        private string wtcardid;
        private string newpassword;
        private string password;
        private string type;

        public string TrandateTime
        {
            get
            {
                return trandateTime;
            }

            set
            {
                trandateTime = value;
            }
        }

        public string LoginId
        {
            get
            {
                return loginId;
            }

            set
            {
                loginId = value;
            }
        }

        public string Reqsn
        {
            get
            {
                return reqsn;
            }

            set
            {
                reqsn = value;
            }
        }

        public string Servicename
        {
            get
            {
                return servicename;
            }

            set
            {
                servicename = value;
            }
        }

        public string Authcode
        {
            get
            {
                return authcode;
            }

            set
            {
                authcode = value;
            }
        }

        public string Wtcardid
        {
            get
            {
                return wtcardid;
            }

            set
            {
                wtcardid = value;
            }
        }

        public string Newpassword
        {
            get
            {
                return newpassword;
            }

            set
            {
                newpassword = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
