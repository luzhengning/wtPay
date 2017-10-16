using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayCommon;
using wtPayDAL;
using wtPayModel;
using wtPayModel.GasModel;

namespace wtPayDAL
{
    public class GasAccess
    { 
    
        /// <summary>
        /// 燃气查询
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static GasQueryInfo query(string cardNo,string amount)
        {
            //燃气查询
            GasInterface access = new GasInterface();
            GasQueryParam param = new GasQueryParam();
            param.authcode = SysBLL.Authcode;
            param.servicename = "RQ001";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.resqn = SysBLL.getSerialNum();
            param.paymentno = cardNo; //缴费户号 not null, ### 键盘输入
            param.chargeAmount = amount;    //购气量  not null 
            param.loginId = SysBLL.getCpuNo();  //设备ID
            GasQueryInfo info = access.GasQuery(param);
            return info;
        }

        /// <summary>
        /// 燃气充值
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static GasOrderInfo gasOrder(GasOrderParam param)
        {
            GasInterface access = new GasInterface();
            return access.GasOrder(param);
        }
        public static GasPayresInfo Payres(GasPayresParam param)
        {
            GasInterface access = new GasInterface();
            param.authcode = SysBLL.Authcode;
            param.servicename = "DD004";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.reqsn = SysBLL.getSerialNum();
            //param.trandeNo = SysBLL.getYYYYMMDDHHMMSSTime();
            param.loginId = SysBLL.getCpuNo();
            GasPayresInfo info = access.Payres(param);
            return info;
        }
    }
}
