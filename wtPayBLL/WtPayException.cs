using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayBLL
{
    public class WtPayException : Exception
    {
        //异常码：1001---银联支付异常，2001---在线业务查询异常，2002---在线业务缴费异常,3001---读卡器连接异常,3002---读卡器读取卡片异常
        public virtual int _errorCode { get; set; }

        public WtPayException(int ErrorCode)
        {
            this._errorCode = ErrorCode;
        }

        public WtPayException(int ErrorCode, string ErrorMessage) : base(ErrorMessage)
        {
            this._errorCode = ErrorCode;
        }

        public int ErrorCode
        {
            get
            {
                return this._errorCode;
            }
        }
    }


}
