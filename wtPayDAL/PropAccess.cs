using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayDAL;
using wtPayBLL;
using wtPayModel;
using wtPayModel.PropModel;
using System.Configuration;
using wtPayCommon;
using System.Threading;
using wtPayModel.ElecModel;

namespace wtPayDAL
{
    public class PropAccess
    {
        static PropInterface access = new PropInterface();

        /// <summary>
        /// 登录认证
        /// </summary>
        /// <returns></returns>
        public static string login()
        {
            ElecLoginParam param = new ElecLoginParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); //交易时间  格式：YYYYMMDDHHMMSS 
            param.servicename = "DL001";    //交易号 not null
            param.reqsn = SysBLL.getSerialNum();   //请求流水号 not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            return access.Login(param);
        }

        /// <summary>
        /// 房屋查询
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static HouseQueryInfo HouseQuery(string mobile)
        {
            HouseQueryParam param = new HouseQueryParam();
            SysBLL.Authcode = login();
            param.authcode= SysBLL.Authcode;// 认证码 not null
            param.servicename= "WY002";// 交易号 not null
            param.trandateTime= SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn= SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId= SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）
            param.mobile= mobile;// not null
            return access.HouseQuery(param);
        }
        /// <summary>
        /// 车位查询
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static ParkingLotQueryInfo ParkingLotQuery(string mobile)
        {
            ParkingLotQueryParam param = new ParkingLotQueryParam();
            SysBLL.Authcode = login();
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "WY003";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）
            param.mobile = mobile;// not null
            return access.ParkingLotQuery(param);
        }

        /// <summary>
        /// 物业费用查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropCostsQueryInfo CostQuery(PropCostsQueryParam param)
        {
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "WY004";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）
            //param.queryid", param.queryid);// 房屋编码【houseid】或者车位编码【parkingid】	String 否
            //param.communityid", param.communityid);// 小区编码【communityid】	String 否
            //param.isPark="";// 费用编号  String 房屋费用 1，车位费用 0
            return access.CostQuery(param);
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropOrderInfo Order(PropOrderParam param)
        {
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "WY008";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）
            //parameters.Add("queryid", param.queryid);// 费用编码【物业费用查询(WY004)时的queryid】	String 否   由WY002接口返回
            //parameters.Add("paymentAmout", param.paymentAmout);// 缴费金额 String 否   由WY004接口返回【可单笔缴费，对应每一笔的money，可整体缴费，所有缴费金额和】
           return access.Order(param);
        }

        /// <summary>
        /// 物业支付通知
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropPayResInfo payRes(PropPayResParam param)
        {
            param.authcode = SysBLL.Authcode;// 认证码 not null
            param.servicename = "DD004";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）
            //param.orderno", param.orderno);// 订单编号 not null
            //param.mobile", param.mobile);// 缴费电话号码  Not null  
            //param.chargeseids", param.chargeseids);//     费用编码	String	否 WY004返回的chargeid。多个使用“-”分隔 【单笔缴费传单笔缴费金额对应的chargeid 多笔合计缴费传多笔对应的chargeid,'-'连接】
            //param.trandeNo", param.trandeNo);// 支付渠道交易流水号 not null【银行支付流水号】
            //param.realAmout", param.realAmout);// 实际支付金额 not null
            //param.payCode", param.payCode);// 支付渠道编码 not null 
            return access.payRes(param);
        }

        /// <summary>
        /// 小区物业缴费信息查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropFeeQueryInfo PropFeeQuery(PropFeeQueryParam param)
        {
            param.authcode = login();// 认证码 not null
            param.servicename = "WY5101";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）

            return access.PropFeeQuery(param);
        }
        /// <summary>
        /// 小区物业获取订单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropOrderInfo getPropOrder(PropOrderParam param)
        {
            SysBLL.Authcode= login();
            param.authcode = SysBLL.Authcode; // 认证码 not null
            param.servicename = "WY5102 ";// 交易号 not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();// 交易时间  格式：YYYYMMDDHHMMSS
            param.reqsn = SysBLL.getSerialNum();// 请求流水号 not null
            param.loginId = SysBLL.getCpuNo();// 交互终端的设备编号（用于自助终端）

            return access.getPropOrder(param);
        }
    }
}
