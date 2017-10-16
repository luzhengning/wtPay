using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel.PayParamModel;

namespace wtPayModel
{
    public class PayStaticParam
    {
        /// <summary>
        /// -1：电子现金，0：纸币
        /// </summary>
        public static int payType = -1;
        /// <summary>
        /// 识币器是否开启
        /// </summary>
        public static bool isHaveRMB = false;
        public static bool RmbIsOpen = false;
        /// <summary>
        /// 业务类型
        /// </summary>
        public static int businessType = -1;





        //银行卡退款，测试用
        public static List<RefundTest> refundTest = new List<RefundTest>();

    }
    public class RefundTest
    {
        public PayResultInfo refundPayResultInfo;
       public  PayParam refundPayParam;
    }
}
