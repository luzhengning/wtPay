using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayCommon
{
    public class WtException : Exception
    {
        private string Msg { get; set; }
        public WtException() : base() {

        }
        public int getCode(){
            return this.Code;
        }
        public WtException(int code)
            : base()
        {
            this.Code = code;
            this.Msg = string.Empty;
        }

        public WtException(int code, string msg) : base(msg)
        {
            this.Code = code;
            this.Msg = msg;
        }
        public WtException(int code, string msg,Exception innerException) : base(msg,innerException)
        {
            this.Code = code;
            this.Msg = msg;
        }
        private  int Code { get; set; }

        public string getMsg()
        {
            return string.Format(" {0}！[{1}] ", WtExceptionType.Instance().getType(this.Code), this.Code);
        }

        public static string formatMsg(int code)
        {
            WtException e = new WtException(code,"");
            return e.getMsg();
        }
        public static string formatMsg(int code,string msg)
        {
            WtException e = new WtException(code,msg);
            return e.getMsg();
        }

    }
    public static class WtExceptionCode
    {
        //未知异常
        public static readonly int DEFAULT = 0000;
        public static class Sys
        {
            public static readonly int NETWORK = 1001;
            //银联读卡器打开
            public static readonly int UNION_READ = 1002;
            //密码键盘
            public static readonly int KEYBOARD = 1003;
            //万通卡签到
            public static readonly int WT_SIGN = 1004;
            //拉卡拉签到
            public static readonly int LKL_SIGN = 1005;
            //拉卡拉签退
            public static readonly int LKL_SIGN_OUT = 1006;
            //万通卡签退
            public static readonly int WT_SIGN_OUT = 1007;
        }

        public static class Bus
        {
            //在线业务查询失败
            public static readonly int BUS_QUERY = 2001;
            //在线业务缴费失败
            public static readonly int BUS_PAY = 2002;
            //银行卡支付失败
            public static readonly int LKL_PAY = 2003;
            //万通卡支付失败
            public static readonly int WT_PAY = 2004;
             //拉卡拉冲正失败
            public static readonly int LKL_CORRECT = 2005;
            //万通卡冲正失败
            public static readonly int WT_CORRECT = 2006;

        }

        public static class Card
        {
            //银联读卡器读取卡
            public static readonly int UNION_READ_CARD = 3001;
            //燃气卡读卡器读取卡失败
            public static readonly int GAS_READ_CARD = 3002;
            //燃气卡读卡器写入卡失败
            public static readonly int GAS_WRITE_CARD = 3003;
            //公交读卡器读取卡失败
            public static readonly int BUS_READ_CARD = 3004;
            //公交读卡器写入卡失败
            public static readonly int BUS_WRITE_CARD = 3005;
            //公交读卡器读取惠民卡失败
            public static readonly int WT_BUS_READ_CARD = 3006;
            //公交读卡器写入惠民卡失败
            public static readonly int WT_BUS_WRITE_CARD = 3007;
            //万通卡读取卡
            public static readonly int WT_READ_CARD = 3008;
        }
    }
    public class WtExceptionType{

        

        private Dictionary<int, string> type = new Dictionary<int, string>();
        private static WtExceptionType instance;

  


        private WtExceptionType() {
            type.Add(WtExceptionCode.DEFAULT, "系统故障");
            ///系统故障
            type.Add(WtExceptionCode.Sys.NETWORK, "网络故障，请稍后重试");
            type.Add(WtExceptionCode.Sys.UNION_READ, "系统故障,请稍后重试");
            type.Add(WtExceptionCode.Sys.KEYBOARD, "系统故障,请稍后重试");
            type.Add(WtExceptionCode.Sys.WT_SIGN, "系统故障");
            type.Add(WtExceptionCode.Sys.LKL_SIGN, "系统故障");
            type.Add(WtExceptionCode.Sys.LKL_SIGN_OUT, "系统故障");
            type.Add(WtExceptionCode.Sys.WT_SIGN_OUT, "系统故障");

            ///业务故障
            type.Add(WtExceptionCode.Bus.BUS_QUERY, "查询失败");
            type.Add(WtExceptionCode.Bus.BUS_PAY, "缴费失败");
            type.Add(WtExceptionCode.Bus.LKL_PAY, "支付失败");
            type.Add(WtExceptionCode.Bus.WT_PAY, "支付失败");
            type.Add(WtExceptionCode.Bus.LKL_CORRECT, "冲正失败");
            type.Add(WtExceptionCode.Bus.WT_CORRECT, "冲正失败");


            //读卡器或卡故障
            type.Add(WtExceptionCode.Card.UNION_READ_CARD, "读取失败,请重试");//,请确认是否插入卡
            type.Add(WtExceptionCode.Card.GAS_READ_CARD, "读取燃气卡失败");
            type.Add(WtExceptionCode.Card.GAS_WRITE_CARD, "写入燃气卡失败");
            type.Add(WtExceptionCode.Card.BUS_READ_CARD, "读取公交卡失败");
            type.Add(WtExceptionCode.Card.BUS_WRITE_CARD, "写入公交卡失败");
            type.Add(WtExceptionCode.Card.WT_BUS_READ_CARD, "读取失败,请重试"); 
            type.Add(WtExceptionCode.Card.WT_BUS_WRITE_CARD, "写入惠民卡失败");
            type.Add(WtExceptionCode.Card.WT_READ_CARD, "读取失败,请重试");
        }

        public static WtExceptionType Instance()
        {
            if(instance == null)
            {
                instance = new WtExceptionType();
            }
            return instance;
        }
       public string getType(int code)
        {
            return type[code];
        }
    }
}
