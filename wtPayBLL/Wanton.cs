using AnalysisBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using wtPayBLL;
using wtPayModel;
using wtPayModel.WintopModel;
using wtPayCommon;
using Newtonsoft.Json;
using wtPayModel.PayParamModel;

namespace WtPayBLL
{
    public class Wanton
    {
        public static string LakalaIp = SysConfigHelper.readerNode("payIp");//124.74.143.162
        public static int LakalaPort = Convert.ToInt32(SysConfigHelper.readerNode("wtPort"));//6022
        public static string matherKey = "83D7BB2A87F20D7B0F60A4B45E6FD36A";//官方给的key
        public static string sonKey = "38A6FBD59F876A82E598F93BED474B40"; //string.Empty;//自己颁发的key
        public static string mackey = "0CEA58541761E2609539C7469D02A6E5";//string.Empty;
        public static string pinKey = "CFB06A0F384F99DB226E1BAFA752F68A";// string.Empty;

        /// <summary>
        /// 客户端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public static byte[] Client(string ip, int port, byte[] messages)
        {




            string sfd = BCDUtil.byteArrToString(messages);
            byte[] datas;
            try
            {

                //1.发送数据                
                TcpClient client = new TcpClient(ip, port);
                IPEndPoint ipendpoint = client.Client.RemoteEndPoint as IPEndPoint;
                NetworkStream stream = client.GetStream();
                stream.Write(messages, 0, messages.Length);
                BCDUtil.printData("推送数据", messages);

                //2.接收状态,长度<1024字节
                byte[] bytes = new Byte[1024 * 8];
                string data = string.Empty;
                //读取超时时间，10秒
                stream.ReadTimeout = 30000;
                int length = stream.Read(bytes, 0, bytes.Length);
                if (length == 2)
                {
                    string headLenStr1 = Convert.ToString(bytes[0], 10);
                    string headLenStr2 = Convert.ToString(bytes[1], 10);
                    int headLen = Convert.ToInt32(headLenStr1 + headLenStr2);
                    length = stream.Read(bytes, length, (headLen));
                    length += 2;
                }

                datas = new byte[length];
                Array.Copy(bytes, datas, length);
                BCDUtil.printData("接收数据", datas);
                //3.关闭对象
                stream.Close();
                client.Close();
                return datas;
            }
            catch (Exception ex)
            {
                throw new WtException(WtExceptionCode.Sys.NETWORK, ex.Message);
            }

        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ResultData> sign()
        {
            try
            {
                //签到
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(3, "000000");
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
                data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
                data.Add(63, "01");
                byte[] result = Client(LakalaIp, LakalaPort, EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null))); //发送数据
                AnalysisBaseWT lk = new AnalysisBaseWTSign();
                Dictionary<string, ResultData> rd = lk.analysis(result);
                return rd;
            }
            catch (WtException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Sys.WT_SIGN, e.Message);
            }

        }
        public static Dictionary<string, string> pay(WintopPayParam param, TradeRecord tradeRecord, string secondShopNo)
        {
            try
            {
                tradeRecord.shop_type = "1";
                tradeRecord.order_type = "1";
                tradeRecord.order_no = param.orderNo;

                tradeRecord.relation_order = "";
                tradeRecord.amount = param.price;

                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.wintopNo);//万通卡号
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.price));//交易金额
                data.Add(11, param.orderNo); //POS终端交易流水
                map.Add("11", data[11]);


                data.Add(25, "00");
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
                map.Add("41", data[41]);
                tradeRecord.termail_no = data[41];

                data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
                data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);                                                 //data.Add(48, "TC_ONE|Y00000001|"+secondShopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(62, param.ExpressPwd);//明文，卡号
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                //Console.WriteLine();
                //Console.WriteLine(BCDUtil.byteArrToString(sendData));
                AnalysisBaseWT lk = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, sendData);

                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(result);
                log.Write("接收支付的数据：" + receiveStr);

                Dictionary<string, ResultData> ret = lk.analysis(result);
                string recode = Wanton.handleDataMac(ret); //发送数据
                log.Write("支付返回码:" + recode);
                if ("00".Equals(recode))
                {
                    tradeRecord.lkl_wt_state = "1";
                    tradeRecord.batch_no = ret["59"].value + "|" + SysBLL.getMMDDHHMMSSTime() + "|" + param.wintopNo;

                }
                else if ("A0".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {
                    tradeRecord.lkl_wt_state = "2";

                }
                map.Add("recode", recode);
                map.Add("sendStr", sendStr);
                map.Add("receiveStr", receiveStr);
                map.Add("ShopNo", param.orderNo + "|" + data[41] + "|" + data[42]);
                map.Add("money", param.price);

                if (ret.ContainsKey("2"))
                {
                    map.Add("2", ret["2"].value);
                }

                map.Add("4", param.price);

                if (ret.ContainsKey("59"))
                {
                    map.Add("59", ret["59"].value);
                    map.Add("cloudOrderNo", ret["59"].value);
                }

                string serialStr = JsonConvert.SerializeObject(map);
                tradeRecord.data_id = serialStr;

                TradeBLL.SendOrderPayRecord(tradeRecord);



                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.WT_PAY, e.Message);
            }


        }

        public static Dictionary<string, string> pay(WintopPayParam param,  string secondShopNo)
        {
            try
            {
                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.wintopNo);//万通卡号
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.price));//交易金额
                data.Add(11, param.orderNo); //POS终端交易流水
                map.Add("11", data[11]);


                data.Add(25, "00");
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
                map.Add("41", data[41]);

                data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
                data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);                                                 //data.Add(48, "TC_ONE|Y00000001|"+secondShopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(62, param.pwd);//明文，卡号
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送支付的数据：" + sendStr);

                //Console.WriteLine();
                //Console.WriteLine(BCDUtil.byteArrToString(sendData));
                AnalysisBaseWT lk = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, sendData);

                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(result);
                log.Write("接收支付的数据：" + receiveStr);

                Dictionary<string, ResultData> ret = lk.analysis(result);
                string recode = Wanton.handleDataMac(ret); //发送数据
                log.Write("支付返回码:"+recode);
                if ("00".Equals(recode))
                {

                }
                else if("A0".Equals(recode))
                {
                    DeviceState.SendState("333333");
                }
                else
                {

                }
                map.Add("recode", recode);
                map.Add("sendStr", sendStr);
                map.Add("receiveStr", receiveStr);
                map.Add("ShopNo", param.orderNo + "|" + data[41] + "|" + data[42]);
                map.Add("money", param.price);

                if (ret.ContainsKey("2"))
                {
                    map.Add("2", ret["2"].value);
                }
              
                map.Add("4", param.price);
                
                if (ret.ContainsKey("59"))
                {
                    map.Add("59", ret["59"].value);
                    map.Add("cloudOrderNo", ret["59"].value);
                }

                string serialStr = JsonConvert.SerializeObject(map);



                return map;
            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.WT_PAY, e.Message);
            }


        }
        public static void pay(PayParam param,ref string sendDataStr)
        {
            try
            {
                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.WtNo);//万通卡号
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.rechageAmount));//交易金额
                data.Add(11, param.WtLklorderNo); //POS终端交易流水
                map.Add("11", data[11]);


                data.Add(25, "00");
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));//终端代码
                map.Add("41", data[41]);

                data.Add(42, SysConfigHelper.readerNode("ShopNo"));//商户代码
                data.Add(48, "TC_ONE|Y00000001|" + param.MERCHANTNO_shopNo);                                                 //data.Add(48, "TC_ONE|Y00000001|"+secondShopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(62, param.pwd);//明文，卡号z
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                sendDataStr = sendStr;
            }
            catch (WtException e)
            {
                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Bus.WT_PAY, e.Message);
            }


        }

        //public static string getOrder()
        //{
        //    int num = Convert.ToInt32(SysConfigHelper.readerNode("orderNo"));
        //    num++;
        //    SysBLL.SetAppConfig("orderNo", num.ToString());
        //    return num.ToString();

        //}

        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> correct(WintopPayParam param, TradeRecord tradeRecord, string secondShopNo)
        {
            try
            {
                tradeRecord.order_type = "2";
                string termailNo = SysConfigHelper.readerNode("ClientNo");
                tradeRecord.amount = param.price;
                tradeRecord.batch_no = "";
                tradeRecord.order_no = param.orderNo;
                tradeRecord.termail_no = termailNo;

                tradeRecord.lkl_wt_state = "0";

                tradeRecord.shop_type = "1";


                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.wintopNo);
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.price));
                data.Add(11, param.orderNo);
                map.Add("11", data[11]);


                data.Add(25, "00");
                data.Add(39, "17");
                data.Add(41, termailNo);
                map.Add("41", data[41]);

                tradeRecord.termail_no = data[41];

                data.Add(42, SysConfigHelper.readerNode("ShopNo"));

                data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0400", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送冲正的数据：" + sendStr);

                //byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
                Console.WriteLine(BCDUtil.byteArrToString(sendData));
                AnalysisBaseWT lk = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, sendData);
                string recode = Wanton.handleDataMac(lk.analysis(result)); //发送数据
                if ("00".Equals(recode))
                {
                    tradeRecord.lkl_wt_state = "1";
                }
                else
                {
                    tradeRecord.lkl_wt_state = "2";
                }


                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(result);
                log.Write("接收冲正的数据：" + result);
                map.Add("recode", recode);
                map.Add("sendStr", sendStr);
                map.Add("receiveStr", receiveStr);
                map.Add("ShopNo", param.orderNo + "|" + data[41] + "|" + data[42]);
                map.Add("money", param.price);


                string serialStr = JsonConvert.SerializeObject(map);
                tradeRecord.data_id = serialStr;
                TradeBLL.SendOrderPayRecord(tradeRecord);
                return map;

            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }

        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> correct(WintopPayParam param, string secondShopNo)
        {
            try
            {
                string termailNo = SysConfigHelper.readerNode("ClientNo");

                Dictionary<string, string> map = new Dictionary<string, string>();

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.wintopNo);
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.price));
                data.Add(11, param.orderNo);
                map.Add("11", data[11]);


                data.Add(25, "00");
                data.Add(39, "17");
                data.Add(41, termailNo);
                map.Add("41", data[41]);
                
                data.Add(42, SysConfigHelper.readerNode("ShopNo"));

                data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0400", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                string sendStr = BCDUtil.byteArrToString(sendData);
                log.Write("发送冲正的数据：" + sendStr);

                //byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
                Console.WriteLine(BCDUtil.byteArrToString(sendData));
                AnalysisBaseWT lk = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, sendData);
                string recode = Wanton.handleDataMac(lk.analysis(result)); //发送数据

                //接收的数据
                string receiveStr = BCDUtil.byteArrToString(result);
                log.Write("接收冲正的数据：" + result);
                map.Add("recode", recode);
                map.Add("sendStr", sendStr);
                map.Add("receiveStr", receiveStr);
                map.Add("ShopNo", param.orderNo + "|" + data[41] + "|" + data[42]);
                map.Add("money", param.price);
                

                string serialStr = JsonConvert.SerializeObject(map);
                return map;

            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }

        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        public static void correct(PayParam param,ref string sendDataStr)
        {
            try
            {
                string termailNo = SysConfigHelper.readerNode("ClientNo");

                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(2, param.WtNo);
                data.Add(3, "000000");
                data.Add(4, wtPayUtils.ConvertMoney(param.rechageAmount));
                data.Add(11, param.WtLklorderNo);


                data.Add(25, "00");
                data.Add(39, "17");
                data.Add(41, termailNo);
                
                data.Add(42, SysConfigHelper.readerNode("ShopNo"));

                data.Add(48, "TC_ONE|Y00000001|" + param.MERCHANTNO_shopNo);
                //data.Add(48, "Y00000001");
                data.Add(49, "156");
                data.Add(63, "01");

                byte[] countMacData = EncryptWT.countMacData("0400", EncryptWT.encryptLKL(data, null));

                byte[] mac = PasswordBLL.GetMac(2, countMacData);

                byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, mac));

                //发送的数据
                sendDataStr = BCDUtil.byteArrToString(sendData);
                
                

            }
            catch (Exception e)
            {
                
            }

        }
        /// <summary>
        /// 计算mac 公共类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] calculateMac(Dictionary<int, string> data, string msgType)
        {
            //计算MAC用到的数据
            byte[] countMacData = EncryptWT.countMacData(msgType, EncryptWT.encryptLKL(data, null));
            Console.WriteLine(BCDUtil.byteArrToString(countMacData));
            string str = MacUtils.CSMMAC(mackey, BCDUtil.byteArrToString(countMacData));//SysConfigHelper.readerNode("WTMacKey")
            Console.WriteLine("bcd2Str:" + PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8))));
            //MAC转byte[]
            String strMac = PosProtocol.bcd2Str(System.Text.Encoding.Default.GetBytes(str.Substring(0, 8).ToUpper()));
            byte[] macByte = BCDUtil.ToByteArray(strMac);
            return macByte;
        }
        /// <summary>
        /// 获取8583报文格式的时间
        /// </summary>
        /// <returns></returns>
        public static string getMMDDHHMMSSTime()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return getMMDD() + getHHMMSS();
        }
        /// <summary>
        /// 获取当前月日
        /// </summary>
        /// <returns></returns>
        public static string getMMDD()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            return month + day;
        }
        /// <summary>
        /// 获取当前时分秒
        /// </summary>
        /// <returns></returns>
        public static string getHHMMSS()
        {

            string hour = TimeStrLenZero(DateTime.Now.Hour.ToString());
            string minute = TimeStrLenZero(DateTime.Now.Minute.ToString());
            string second = TimeStrLenZero(DateTime.Now.Second.ToString());
            return hour + minute + second;
        }
        /// <summary>
        /// 时间处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TimeStrLenZero(string str)
        {
            int length = str.Length;
            if (length != 2)
            {
                str = "0" + str;
            }
            return str;
        }
        /// <summary>
        /// 签退
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ResultData> signOut()
        {
            try
            {
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(3, "000000");
                data.Add(11, "000006");
                data.Add(12, getHHMMSS());
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));
                data.Add(42, SysConfigHelper.readerNode("ShopNo"));
                data.Add(60, "22000003000");
                data.Add(63, "01");
                byte[] sendData = EncryptWT.EncryptData("0820", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
                Console.WriteLine(BCDUtil.byteArrToString(sendData));
                AnalysisBaseWT lk = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, sendData);

                return lk.analysis(result); //发送数据

            }
            catch (WtException e)
            {

                throw e;
            }

            catch (Exception e)
            {

                throw new WtException(WtExceptionCode.Sys.WT_SIGN_OUT, e.Message);
            }
        }
        /// <summary>
        /// 换算PIN
        /// </summary>
        /// <param name="pwd">用户输入的密码</param>
        /// <param name="cardNo">获取的ic卡卡号</param>
        /// <returns></returns>
        public static string getPin(string pwd, string cardNo)
        {
            string result = string.Empty;
            int len = cardNo.Length - 4;
            string pwdLen = pwd.Length + "";
            if (pwdLen.Length == 1)
            {
                pwd = "0" + pwdLen + pwd;
            }
            string cardNoCover = "";

            if (len < 12)
            {
                cardNo = cardNo.Substring(4, len);
                for (int i = 0; i < 12 - len; i++)
                {
                    cardNoCover += "f";
                }
            }
            else
            {
                cardNo = cardNo.Substring(3, 12);
            }
            cardNo = cardNoCover + cardNo;
            for (int i = 0; i < 32; i++)
            {
                if (cardNo.Length < 32)
                {
                    cardNo = "0" + cardNo;
                }
                if (pwd.Length < 32)
                {
                    pwd = pwd + "f";
                }
            }

            string orx = MacUtils.xOrString(cardNo, pwd);
            //Console.WriteLine("orx="+ orx);
            result = BCDUtil.byteArrToString(CSm4.EncriptB(BCDUtil.ToByteArray(pinKey), BCDUtil.ToByteArray(orx)));//BCDUtil.ToByteArray(SysConfigHelper.readerNode("WTPinKey"))
            // Console.WriteLine("result="+ result);
            Console.WriteLine(result.ToUpper());
            return result.ToUpper();
        }
        /// <summary>
        /// 打印数据到控制的 同时重要功能：如果返回80 更新pinKey 与 macKey
        /// </summary>
        /// <param name="data">传入服务器返回byte数组</param>
        /// <returns>返回信息为系统返回信息</returns>
        public static string handleDataMac(Dictionary<string, ResultData> rd)
        {
            if (rd == null)
            {
                return "系统无返回数据！！";
            }
            //if (rd.ContainsKey("62"))
            //{
            //    ResultData _62 = rd["62"];
            //    //获取44域中的密文密钥
            //    string mackeyExpress = _62.value.Substring(_62.value.Length - 40, 40);
            //    //将密文密钥解析明文密钥
            //    //SysBLL.SetAppConfig("WTMacKey", validateKey(mackeyExpress, sonKey));
            //    mackey = validateKey(mackeyExpress, sonKey);
            //    //Console.WriteLine("mackey=" + mackey);
            //    string pinKeyExpress = _62.value.Substring(0, 40);
            //    //Console.WriteLine("加密key:" + sonKey);
            //    //SysBLL.SetAppConfig("WTPinKey", validateKey(pinKeyExpress, sonKey));
            //    pinKey = validateKey(pinKeyExpress, sonKey);
            //    //Console.WriteLine("加密后pinKey:" + pinKey);
            //}
            //foreach (var item in rd)
            //{
            //    Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            //}
            return rd["39"].value;
        }

        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="msgNo"></param>
        /// <returns></returns>
        public static string handleDataMsg(string msgNo)
        {
            switch (msgNo)
            {
                case "00":
                    return "交易成功";
                case "01":
                    return "查询发卡方";
                case "02":
                    return "CALL BANK 查询";
                case "03":
                    return "无效商户";
                case "05":
                    return "不承兑";
                case "10":
                    return "承兑部分金额";
                case "12":
                    return "无效交易";
                case "13":
                    return "无效金额";
                case "14":
                    return "无此卡号";
                case "19":
                    return "稍候重做交易";
                case "23":
                    return "不能接受的交易费";
                case "24":
                    return "接收者不支持";
                case "25":
                    return "记录不存在";
                case "26":
                    return "重复的文件更新记录";
                case "27":
                    return "文件更新域错";
                case "28":
                    return "文件锁定";
                case "29":
                    return "文件更新不成功";
                case "30":
                    return "格式错误";
                case "31":
                    return "交换站不支持代理方";
                case "33":
                    return "到期卡, 请没收";
                case "34":
                    return "舞弊嫌疑, 请没收";
                case "35":
                    return "与受卡行联系";
                case "36":
                    return "";
                case "40":
                    return "";
                case "41":
                    return "";
                case "43":
                    return "";
                case "51":
                    return "";
                case "53":
                    return "";
                case "54":
                    return "";
                case "55":
                    return "";
                case "56":
                    return "";
                case "57":
                    return "";
                case "58":
                    return "";
                case "59":
                    return "";
                case "61":
                    return "";
                case "65":
                    return "";
                case "68":
                    return "";
                case "75":
                    return "";
                case "76":
                    return "";
                case "78":
                    return "";
                case "79":
                    return "";
                case "80":
                    return "";
                case "81":
                    return "";
                case "82":
                    return "";
                case "83":
                    return "";
                case "84":
                    return "";
                case "85":
                    return "";
                case "86":
                    return "";
                case "88":
                    return "";
                case "89":
                    return "";
                case "90":
                    return "";
                case "91":
                    return "";
                case "92":
                    return "";
                case "93":
                    return "";
                case "95":
                    return "";
                case "96":
                    return "";
                case "C0":
                    return "";
            }
            return null;
        }
        /// <summary>
        /// ZMK密钥下载
        /// </summary>
        /// <returns></returns>
        public static string downKey()
        {
            try
            {
                Dictionary<int, string> data = new Dictionary<int, string>();
                data.Add(3, "060000");
                data.Add(11, "000002");
                data.Add(41, SysConfigHelper.readerNode("ClientNo"));
                data.Add(42, SysConfigHelper.readerNode("ShopNo"));
                data.Add(62, "83D7BB2A87F20D7B0F60A4B45E6FD36A");
                data.Add(63, "01");
                AnalysisBaseWT wt = new AnalysisBaseWT();
                byte[] result = Client(LakalaIp, LakalaPort, EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null))); //发送数据
                Dictionary<string, ResultData> rd = wt.analysis(result);


                if (rd["39"].value.Trim().Equals("00"))
                {
                    return validateKey(rd["62"].value.Trim(), matherKey);
                }

                return null;
            }
            catch (Exception e)
            {
                throw new WtException(WtExceptionCode.Sys.NETWORK, e.Message);
            }

        }
        /// <summary>
        /// 验证数据是否通过验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string validateKey(string data, string key)
        {
            string validata = data.Substring(data.Length - 8, 8).ToUpper();
            byte[] sources = BCDUtil.HexStrToByteArray(data.Substring(0, 32));
            byte[] desKeys = BCDUtil.HexStrToByteArray(key);
            byte[] encSource = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] encKeys = CSm4.DecriptB(desKeys, sources);
            if (PosProtocol.bytesToHexString(CSm4.EncriptB(encKeys, encSource)).Substring(0, 8).Equals(validata))
            {
                Console.WriteLine("密钥验证通过返回灌输密钥！！！！");
                return PosProtocol.bytesToHexString(encKeys);
            }
            return "";
        }

        public static void printInfo(Dictionary<string, ResultData> rd)
        {
            foreach (var item in rd)
            {
                Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            }
        }
        public static Dictionary<string, string> refund(string _2, string _4, string _59, TradeRecord tradeRecord, string secondShopNo)
        {
            string theOrderNo = Wanton.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("ClientNo");
            string shopNo = SysConfigHelper.readerNode("ShopNo");
            tradeRecord.amount = _4;
            tradeRecord.batch_no = "" + "|" + SysBLL.getMMDDHHMMSSTime() + "|" + _2;
            tradeRecord.order_no = theOrderNo;
            tradeRecord.termail_no = termailNo;
            tradeRecord.lkl_wt_shop_no = shopNo;
            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(_4));
            data.Add(11, theOrderNo);
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
            //data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(59, _59);
            data.Add(60, "22000003000");
            data.Add(63, "01");

            byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(2, countMacData);

            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));


            //byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            Console.WriteLine();
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);

            //接收的数据
            string receiveStr1 = BCDUtil.byteArrToString(result);
            log.Write("tuikuan的数据：" + receiveStr1);

            Dictionary<string, ResultData> ret = lk.analysis(result);


            string recode = Wanton.handleDataMac(ret); //发送数据
            if ("00".Equals(recode))
            {
                tradeRecord.lkl_wt_state = "1";
                tradeRecord.order_state = "1";
            }
            else
            {
                tradeRecord.lkl_wt_state = "2";
                tradeRecord.order_state = "0";
            }
            //发送的数据
            string sendStr = BCDUtil.byteArrToString(sendData);
            //接收的数据
            string receiveStr = BCDUtil.byteArrToString(result);
            map.Add("recode", recode);
            map.Add("sendStr", sendStr);
            map.Add("receiveStr", receiveStr);
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42]);

            map.Add("39", ret["39"].value);

            log.Write("万通卡退款交易返回" + ret["39"].value);


            string serialStr = JsonConvert.SerializeObject(map);
            tradeRecord.data_id = serialStr;
            TradeBLL.SendOrderRefundRecord(tradeRecord);

            return map;


            //if (result.Length > 0)
            //{
            //    return lk.analysis(result); //发送数据
            //}
            //else
            //{
            //    return null;
            //}
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> refund(WintopPayParam param, string _59,string secondShopNo)
        {
            string theOrderNo = Wanton.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("ClientNo");
         

            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, param.wintopNo);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(param.price));
            data.Add(11, getHHMMSS());
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(41, SysConfigHelper.readerNode("ClientNo"));
            map.Add("41", data[41]);

            data.Add(42, SysConfigHelper.readerNode("ShopNo"));
            data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
            //data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(59, _59);
            data.Add(60, "22000003000");
            data.Add(63, "01");

            byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(2, countMacData);

            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));

            string sendStr = BCDUtil.byteArrToString(sendData);
            log.Write("发送退款的数据：" + sendStr);

            //byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            Console.WriteLine();
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);

            //result
             string resultstr = BCDUtil.byteArrToString(result);
             log.Write("接收退款的数据：" + resultstr);



            string recode = Wanton.handleDataMac(lk.analysis(result)); //发送数据
           
            log.Write("万通退款结果" + recode);

            string serialStr = JsonConvert.SerializeObject(map);
           

            //接收的数据
            string receiveStr = BCDUtil.byteArrToString(result);
            map.Add("recode", recode);
            map.Add("sendStr", sendStr);
            map.Add("receiveStr", receiveStr);
            map.Add("ShopNo", param.orderNo + "|" + data[41] + "|" + data[42]);
            map.Add("money", param.price);
            return map;


            //if (result.Length > 0)
            //{
            //    return lk.analysis(result); //发送数据
            //}
            //else
            //{
            //    return null;
            //}
        }
        public static Dictionary<string, string> refund(string _2,string _4, string _59,string secondShopNo)
        {
            string theOrderNo = Wanton.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("ClientNo");
            string shopNo = SysConfigHelper.readerNode("ShopNo");
           
            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(_4));
            data.Add(11, theOrderNo);
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
            //data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(59, _59);
            data.Add(60, "22000003000");
            data.Add(63, "01");

            byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(2, countMacData);

            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));


            //byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            Console.WriteLine();
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);

            //接收的数据
            string receiveStr1 = BCDUtil.byteArrToString(result);
            log.Write("tuikuan的数据：" + receiveStr1);

            Dictionary<string, ResultData> ret = lk.analysis(result);


            string recode = Wanton.handleDataMac(ret); //发送数据
            //发送的数据
            string sendStr = BCDUtil.byteArrToString(sendData);
            //接收的数据
            string receiveStr = BCDUtil.byteArrToString(result);
            map.Add("recode", recode);
            map.Add("sendStr", sendStr);
            map.Add("receiveStr", receiveStr);
            map.Add("ShopNo", data[11] + "|" + data[41] + "|" + data[42]);

            map.Add("39", ret["39"].value);

            log.Write("万通卡退款交易返回" + ret["39"].value);


            string serialStr = JsonConvert.SerializeObject(map);

            return map;


            //if (result.Length > 0)
            //{
            //    return lk.analysis(result); //发送数据
            //}
            //else
            //{
            //    return null;
            //}
        }
        public static string wtRefund(Dictionary<string,string> list, string secondShopNo)
        {
            string _2=list["2"],  _4= list["4"],  _59= list["59"];
            string theOrderNo = Wanton.GetOrderNo();
            string termailNo = SysConfigHelper.readerNode("ClientNo");
            string shopNo = SysConfigHelper.readerNode("ShopNo");
            Dictionary<string, string> map = new Dictionary<string, string>();

            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, _2);
            data.Add(3, "200000");
            data.Add(4, wtPayUtils.ConvertMoney(_4));
            data.Add(11, theOrderNo);
            map.Add("11", data[11]);

            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(41, termailNo);
            map.Add("41", data[41]);

            data.Add(42, shopNo);
            data.Add(48, "TC_ONE|Y00000001|" + secondShopNo);
            //data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(59, _59);
            data.Add(60, "22000003000");
            data.Add(63, "01");

            byte[] countMacData = EncryptWT.countMacData("0200", EncryptWT.encryptLKL(data, null));

            byte[] mac = PasswordBLL.GetMac(2, countMacData);

            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, mac));


            //byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            Console.WriteLine();
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            return BCDUtil.byteArrToString(sendData);


        }
        public static void RebuildBatchNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "WantongPay.xml";
            string batchNo = SysConfigHelper.readerNode("WtBatchNo");
            string newBatchNo = (Convert.ToInt32(batchNo) + 1).ToString();
            SysConfigHelper.writerNode("WtBatchNo", newBatchNo);

        }
        /// <summary>
        /// 获取支付批次号
        /// </summary>
        /// <returns></returns>
        public static string GetBatchNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "WantongPay.xml";
            string batchNo = SysConfigHelper.readerNode("WtBatchNo");
            return batchNo;

        }
        /// <summary>
        /// 获取支付流水号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNo()
        {
            //string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "WantongPay.xml";
            string orderNo = SysConfigHelper.readerNode("WtOrderNo");
            string newOrderNo = (Convert.ToInt32(orderNo) + 1).ToString();
            SysConfigHelper.writerNode("WtOrderNo", newOrderNo);
            return orderNo;
        }
    }
}
