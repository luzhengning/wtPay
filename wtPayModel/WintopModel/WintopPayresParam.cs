using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通支付结果通知参数
    /// </summary>
    public class WintopPayresParam
    {
        public string servicename;
	    public string trandateTime;
	    public string reqsn;     
	    public string loginId;
	    public string authcode;  
	
	 
	    public string orderno;
	    public string realAmout;
	    public string payCode;
	    public string trandeNo;
	 
	    public string wtcardid;
	    public string wtuserid;
	    public string type;
	    public string terminalno;
        public string terminalNo;
        public string operators;
        public string deptno;
    }
}
