using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.PropSecModel;

namespace wtPayDAL
{
    public class PropSecAccess
    {
        PropSecInterface access = new PropSecInterface();
        /// <summary>
        /// 物业2登录认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string login()
        {
            PropSecLoginInfo info = null;
            PropSecLoginParam loginParam = new PropSecLoginParam();
            loginParam.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            loginParam.servicename = "DL001";
            loginParam.reqsn = SysBLL.getSerialNum();
            loginParam.loginId = SysBLL.getCpuNo();
            SysBLL.Authcode = access.login(loginParam).msgrsp.authcode;
            return access.login(loginParam).msgrsp.authcode;
        }
        /// <summary>
        /// 物业2读卡
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropSecQueryInfo query(PropSecQueryParam param)
        {
            param.trandateTime=SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename= "SC001";
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            param.authcode = login();
            //param.SC10009;
            //param.SC10010;
            //param.SC10011;
            return access.query(param);
            
        }
        /// <summary>
        /// 物业2提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PropSecOrderInfo order(PropSecOrderParam param)
        {
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename= "SC002";
             param.reqsn = SysBLL.getSerialNum();
            param.loginId=SysBLL.getCpuNo();
            param.authcode= login();
            //param.shopType
            //param.AMOUNT
            //param.paymentAmout
            //param.SC10009
            //param.SC10010
            //param.SC10007
            //param.SC10008
            return access.order(param);
            
        }
        public PropSecPayResInfo payRes(PropSecPayResParam param)
        {
            PropSecPayResInfo info = null;
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("orderno", param.orderno);
            parameters.Add("realAmout", param.realAmout);
            parameters.Add("payCode", param.payCode);
            parameters.Add("trandeNo", param.trandeNo);

            string jsonText = HttpHelper.getHttp("http://10.88.250.27:8083/propSec/payResService", parameters, null);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            info = JsonConvert.DeserializeObject<PropSecPayResInfo>(jsonText);
            return info;
        }
        /// <summary>
        /// 查询表具列表
        /// </summary>
        /// <param name="SC10007"></param>
        /// <param name="SC10008"></param>
        /// <returns></returns>
        public PropMeterInfo queryMeter(string SC10007, string SC10008)
        {
            PropSecOrderParam param = new PropSecOrderParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "SC003";
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            param.authcode = login();
            return access.queryMeter(param,SC10007, SC10008);
        }
    }
}
