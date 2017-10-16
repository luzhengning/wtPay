using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayDAL;
using wtPayBLL;
using wtPayModel;
using wtPayModel.BusModel;
using System.Configuration;
using wtPayCommon;
using System.Threading;

namespace wtPayDAL
{
    public class BusAccess
    {
        /// <summary>
        /// 公交登陆认证
        /// </summary>
        /// <returns></returns>
        public static string BusLogin()
        {
            BusInterface access = new BusInterface();
            BusLoginParam param = new BusLoginParam();
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            param.servicename = "DL001";
            param.resqn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();  //设备ID
            BusLoginInfo info= access.BusLogin(param);
            //同步系统时间
            SysBLL.SetSystemTime(info.msghead.trandatetime);
            if (info != null)
            {
                if (info.msgrsp != null)
                {
                    if (info.msgrsp.authcode != null)
                    {
                        return info.msgrsp.authcode;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 公交签到
        /// </summary>
        /// <returns></returns>
        public static BusRegisterInfo BusRegister()
        {
            SysBLL.Authcode = BusLogin();
            string reqsn = SysBLL.getSerialNum();
            //公交卡签到
            BusInterface access = new BusInterface();
            BusRegisterParam param = new BusRegisterParam();
            
            param.authcode = SysBLL.Authcode;// 认证码 not nullA
            param.servicename = "DL002";//交易类型编号 not nullA
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易日期 YYYYMMDDHHMMSS not nullA
            param.resqn = reqsn;//请求流水号  not nullA
            param.btype = "0010";//业务类型 not nullA
            param.terno = SysBLL.getMac();//终端编号  not nullA
            param.tradeno = SysBLL.getHHMMSSITime10();//交易流水号 not nullA
            param.loginId = SysBLL.getCpuNo();  //设备ID
            BusRegisterInfo busRegisterInfo1 = access.BusRegister(param);

            param.btype = "0011";//业务类型 not nullA
            param.authcode = busRegisterInfo1.msgrsp.authcode;// 认证码 not nullA
            param.opno = busRegisterInfo1.cpumsg.OUTPUT.OPNO;//操作员卡号 null
            param.random = busRegisterInfo1.cpumsg.OUTPUT.RANDOM;
            BusRegisterInfo busRegisterInfo2 = access.BusRegister(param);
            busRegisterInfo2.msgrsp = new BusRegisterMsgrsp();
            busRegisterInfo2.msgrsp.authcode = busRegisterInfo1.msgrsp.authcode;
            busRegisterInfo2.cpumsg.OUTPUT.OPNO = busRegisterInfo1.cpumsg.OUTPUT.OPNO;
            return busRegisterInfo2;
        }
        /// <summary>
        /// 公交卡查询
        /// </summary>
        /// <returns></returns>
        public static BusQueryThatInfo QueryBus()
        {
            
            bool isSuccess = false;
            //签到
            BusRegisterInfo busRegisterInfo = BusAccess.BusRegister();
            if (busRegisterInfo.cpumsg != null)
            {
                isSuccess = true;

            }
            //公交卡签到未成功
            if (isSuccess == false)
            {
                throw new Exception("公交卡签到未成功");
            }
            isSuccess = false;
            BusQueryInfo busQueryInfo = new BusQueryInfo();

            //公交卡查询
            BusInterface access = new BusInterface();
            BusQueryParam param = new BusQueryParam();
            string inapdu = "";
            string trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();
            string reqsn = SysBLL.getSerialNum();
            string APDUDATA = "";
            string APDUSW = "";
            string RETDATA = "";
            int APDUSUM = 0;
            string step = "0";

            param.authcode = SysBLL.Authcode;
            param.servicename = "DS001";
            param.trandateTime = trandateTime;
            param.reqsn = reqsn;
            param.opno = busRegisterInfo.cpumsg.OUTPUT.OPNO;
            param.scode = busRegisterInfo.cpumsg.OUTPUT.SCODE;
            param.terno = SysBLL.getMac();
            param.tradeno = SysBLL.getHHMMSSITime10();
            param.btype = "1900";
            param.inapdu = inapdu;
            param.step = step;
            param.loginId = SysBLL.getCpuNo();  //设备ID
            busQueryInfo = access.BusQuery1(param);

                
            if (busQueryInfo.cpumsg.OUTPUT.OUTAPDU.APDU != null)
            {
                isSuccess = true;
            }
            //公交查询未成功
            if (isSuccess == false)
            {
                throw new Exception("公交查询未成功");
            }
            isSuccess = false;

            step = busQueryInfo.cpumsg.OUTPUT.STEP;
            string status = busQueryInfo.cpumsg.OUTPUT.OUTAPDU.LASTAPDU;
            BusCardBLL bus = new BusCardBLL();

            string port = SysConfigHelper.readerNode("CRT603Port");
            //int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);
            //上电
            int iOutAtrLen = 0;
            byte[] byOutAtrData = new byte[1024];
            int chipRet = CRT603.CRT603Vx_RF_chipPower(ref iOutAtrLen, byOutAtrData);
            string apduData = "";
            List<BusQueryApdu> apdu = busQueryInfo.cpumsg.OUTPUT.OUTAPDU.APDU;
            string result = "";
            string last = "";

            for (int i = 0; i < apdu.Count; i++)
            {
                APDUDATA = APDUDATA + apdu[i].APDUDATA + "|";

                result = bus.sendApdu(apdu[i].APDUDATA);
                    
                last = result.Substring(result.Length - 4);
                APDUSW = APDUSW + last + "|";
                result = result.Remove(result.Length - 4, 4);
                if (apduData != null)
                {
                    RETDATA = RETDATA + result + "|";
                }
                Thread.Sleep(20);
                APDUSUM++;
            }
            //CRT603.CRT603Vx_CloseConnection();
            APDUDATA = APDUDATA.Remove(APDUDATA.Length - 1, 1);
            APDUSW = APDUSW.Remove(APDUSW.Length - 1, 1);
            RETDATA = RETDATA.Remove(RETDATA.Length - 1, 1);
            BusQueryThatInfo busQueryInfo2 = null;
            while (true)
            {
                param.inapdu = "yes";
                param.step = step;
                param.APDUSUM = APDUSUM.ToString();
                param.APDUDATA = APDUDATA;
                param.APDUSW = APDUSW;
                param.RETDATA = RETDATA;
                //表示最后一条
                if (status.Equals("01"))
                {
                    //结果
                    busQueryInfo2 = access.BusQuery2(param);
                    //处理结果
                    return busQueryInfo2;
                }
                //继续查询
                busQueryInfo2 = access.BusQuery2(param);
                if (busQueryInfo2.cpumsg.OUTPUT != null)
                {
                    isSuccess = true;
                }
                //公交查询未成功
                if (isSuccess == false)
                {
                    throw new Exception("公交查询未成功");
                }
                isSuccess = false;
                APDUDATA = "";
                APDUSW = "";
                RETDATA = "";
                List<BusQueryApdu> apdu2 = busQueryInfo.cpumsg.OUTPUT.OUTAPDU.APDU;

                for (int i = 0; i < apdu.Count; i++)
                {
                    APDUDATA = APDUDATA + apdu2[i].APDUDATA + "|";
                    result = bus.sendApdu(apdu2[i].APDUDATA);
                    last = result.Substring(result.Length - 4);
                    result = result.Remove(result.Length - 4, 4);
                    APDUSW = last + APDUSW + "|";
                    //执行apdu
                    RETDATA = RETDATA + result + "|";

                }
                APDUDATA = APDUDATA.Remove(APDUDATA.Length - 1, 1);
                APDUSW = APDUSW.Remove(APDUSW.Length - 1, 1);
                RETDATA = RETDATA.Remove(RETDATA.Length - 1, 1);
                step = busQueryInfo.cpumsg.OUTPUT.STEP;
                status = busQueryInfo.cpumsg.OUTPUT.OUTAPDU.LASTAPDU;

            }
        }
        public static BusCpuCardInfo ReCharge(BusCpuCardInfo busCpuCardInfo1,BusCpuCardParam param)
        {
            try{
                BusInterface access = new BusInterface();
                Boolean isSuccess = false;
                if (busCpuCardInfo1.cpumsg.OUTPUT.OUTAPDU != null)
                {
                    isSuccess = true;
                }
                if (isSuccess == false)
                {
                    throw new WtException(WtExceptionCode.Bus.BUS_PAY);
                }
                isSuccess = false;
                param.orderno = busCpuCardInfo1.msgrsp.orderno;//订单编号    null 循环最后一步传递
                param.payCode = "Z000000004";//支付渠道编码  null 循环最后一步传递
                param.trandeNo = SysBLL.getSerialNum();//支付渠道交易流水号 null 循环最后一步传递
                param.inapdu = "yes";//指令    不定 第一步不需要传，当服务端有 OUTAPDU 指令后，将 OUTAPDU 指令执行并上传 INAPDU 指令

                string status = busCpuCardInfo1.cpumsg.OUTPUT.OUTAPDU.LASTAPDU;
                param.step = busCpuCardInfo1.cpumsg.OUTPUT.STEP;

                //执行指令
                BusCardBLL bus = new BusCardBLL();
                string port = SysConfigHelper.readerNode("CRT603Port");
                //int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);
                //上电
                int iOutAtrLen = 0;
                byte[] byOutAtrData = new byte[1024];
                int chipRet = CRT603.CRT603Vx_RF_chipPower(ref iOutAtrLen, byOutAtrData);
                string apduData = "";

                string APDUDATA = "";
                string APDUSW = "";
                string RETDATA = "";
                int APDUSUM = 0;
                string result = "";
                string last = "";
                List<BusCpuCardAPDU> apdu = busCpuCardInfo1.cpumsg.OUTPUT.OUTAPDU.APDU;
                for (int i = 0; i < apdu.Count; i++)
                {
                    APDUDATA = APDUDATA + apdu[i].APDUDATA + "|";

                    result = bus.sendApdu(apdu[i].APDUDATA);

                    last = result.Substring(result.Length - 4);
                    APDUSW = APDUSW + last + "|";
                    result = result.Remove(result.Length - 4, 4);
                    if (result.Length == 0)
                    {
                        result = last;
                    }
                    if (apduData != null)
                    {
                        RETDATA = RETDATA + result + "|";
                    }
                    Thread.Sleep(10);
                    APDUSUM++;
                }
                //CRT603.CRT603Vx_CloseConnection();
                APDUDATA = APDUDATA.Remove(APDUDATA.Length - 1, 1);
                APDUSW = APDUSW.Remove(APDUSW.Length - 1, 1);
                RETDATA = RETDATA.Remove(RETDATA.Length - 1, 1);

                BusCpuCardInfo busCpuCardInfo2 = null;
                while (true)
                {
                    param.APDUDATA = APDUDATA;
                    param.APDUSW = APDUSW;
                    param.RETDATA = RETDATA;
                    param.APDUSUM = APDUSUM.ToString();
                    if ("-1".Equals(param.step))
                    {
                        busCpuCardInfo2.msgrsp.Tac = param.RETDATA;
                    }
                    //结果
                    if (status.Equals("01"))
                    {
                        busCpuCardInfo2 = access.BusCpuCard(param);
                        return busCpuCardInfo2;
                    }
                  
                    busCpuCardInfo2 = access.BusCpuCard(param);
                    if (busCpuCardInfo2.cpumsg.OUTPUT.OUTAPDU != null)
                    {
                        isSuccess = true;
                    }
                    if (isSuccess == false)
                    {
                        throw new WtException(WtExceptionCode.Bus.BUS_PAY);
                    }
                    isSuccess = false;
                    APDUDATA = "";
                    APDUSW = "";
                    RETDATA = "";
                    APDUSUM = 0;
                    List<BusCpuCardAPDU> apdu2 = busCpuCardInfo2.cpumsg.OUTPUT.OUTAPDU.APDU;

                    for (int i = 0; i < apdu2.Count; i++)
                    {
                        APDUDATA = APDUDATA + apdu2[i].APDUDATA + "|";
                        result = bus.sendApdu(apdu2[i].APDUDATA);
                        last = result.Substring(result.Length - 4);
                        result = result.Remove(result.Length - 4, 4);
                        APDUSW = last + APDUSW + "|";
                        //执行apdu
                        RETDATA = RETDATA + result + "|";
                        APDUSUM++;

                    }
                    APDUDATA = APDUDATA.Remove(APDUDATA.Length - 1, 1);
                    APDUSW = APDUSW.Remove(APDUSW.Length - 1, 1);
                    RETDATA = RETDATA.Remove(RETDATA.Length - 1, 1);
                    param.step = busCpuCardInfo2.cpumsg.OUTPUT.STEP;
                    status = busCpuCardInfo2.cpumsg.OUTPUT.OUTAPDU.LASTAPDU;
                }
            }
            catch { return null; }
        }
        public static BusPayresInfo BusPayres(BusPayresParam param)
        {
            BusInterface access = new BusInterface();
            param.authcode = SysBLL.Authcode;
            param.servicename = "DD004";
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime(); ;
            param.reqsn = SysBLL.getSerialNum();
            param.loginId = SysBLL.getCpuNo();
            param.payCode = "Z000000004";
            param.trandeNo = SysBLL.getHHMMSSITime10(); ;
            return access.BusPayres(param);
        }


        public static BusCpuCardInfo GetOrder(BusReChangeParam busReChangeParam, BusCpuCardParam param,string paycode)
        {
            Boolean isSuccess = false;
            //签到
            BusRegisterInfo busRegisterInfo = BusAccess.BusRegister();
            if (busRegisterInfo.cpumsg.OUTPUT != null)
            {
                isSuccess = true;
            }
            if (isSuccess == false)
            {
                throw new WtException(WtExceptionCode.Bus.BUS_PAY);
            }
            isSuccess = false;
            //公交卡充值
       

            param.authcode = SysBLL.Authcode; ;// 认证码   not null
            param.servicename = "DD006";//交易类型编号  not null
            param.trandateTime = SysBLL.getYYYYMMDDHHMMSSTime();//交易日期：YYYYMMDDHHMMSS not null
            param.resqn = SysBLL.getSerialNum();//请求流水号  not null
            param.paymentno = busReChangeParam.paymentno;//用户名 用户编号 not null   
            param.paymentAmout = busReChangeParam.paymentAmout + "00";//账单金额   not null
            param.billDate = DateTime.Now.Year.ToString();//账单日期 not null
            param.merchantNo = "S000000200";//缴费单位编号 not null
            param.step = "0";//执行第几步 Not null
            param.money = busReChangeParam.paymentAmout + "00";//充值金额 Not null
            param.serno = SysBLL.getSerialNum().Substring(0, 12);//业务流水号  not null
            param.appsid = busReChangeParam.appsid;//应用序列号
            param.btype = "1931";//业务类型  not null
            param.opno = busRegisterInfo.cpumsg.OUTPUT.OPNO;//操作员卡号 not null
            param.scode = busRegisterInfo.cpumsg.OUTPUT.SCODE;//签到码 not null
            param.terno = SysBLL.getMac();//终端编号 Not null
            param.loginId = SysBLL.getCpuNo();  //设备ID
            param.CMTYPE = "0";
            param.WMONEY = busReChangeParam.wmoney;
            param.inapdu = "no";
            param.tradeno = busReChangeParam.trandeNo;
            //param.payCode = paycode;
            //param.orderno = busReChangeParam.orderno;
            BusInterface access = new BusInterface();
            BusCpuCardInfo busCpuCardInfo1 = access.BusCpuCard(param);
          //  orderno = busCpuCardInfo1.msgrsp.orderno;
            return busCpuCardInfo1;
        }
    }
}