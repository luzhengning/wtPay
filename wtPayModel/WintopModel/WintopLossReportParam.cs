﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡挂失参数
    /// </summary>
    public class WintopLossReportParam
    {
        private string trandateTime;
        private string loginId;
        private string reqsn;
        private string servicename;
        private string authcode;
        private string wtcardid;
        private string validatecode;

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

        public string Validatecode
        {
            get
            {
                return validatecode;
            }

            set
            {
                validatecode = value;
            }
        }
    }
}
