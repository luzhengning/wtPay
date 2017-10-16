using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WaterModel
{
    public class WaterPayresParam
    {
        private string authcode;
        private string servicename;
        private string trandateTime;
        private string reqsn;
        private string loginId;
        private string orderno;
        private string realAmout;
        private string payCode;
        private string trandeNo;
        private string billDate;
        private string terminalNo;

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

        public string Orderno
        {
            get
            {
                return orderno;
            }

            set
            {
                orderno = value;
            }
        }

        public string RealAmout
        {
            get
            {
                return realAmout;
            }

            set
            {
                realAmout = value;
            }
        }

        public string PayCode
        {
            get
            {
                return payCode;
            }

            set
            {
                payCode = value;
            }
        }

        public string TrandeNo
        {
            get
            {
                return trandeNo;
            }

            set
            {
                trandeNo = value;
            }
        }

        public string BillDate
        {
            get
            {
                return billDate;
            }

            set
            {
                billDate = value;
            }
        }

        public string TerminalNo
        {
            get
            {
                return terminalNo;
            }

            set
            {
                terminalNo = value;
            }
        }
    }
}
