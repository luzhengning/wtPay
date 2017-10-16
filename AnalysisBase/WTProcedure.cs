using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace AnalysisBase
{
  public  class WTProcedure
    {
        public static string LakalaIp = "172.16.27.86";//124.74.143.162
        public static int LakalaPort = 8885;//6022
        public static string matherKey = "83D7BB2A87F20D7B0F60A4B45E6FD36A";//官方给的key
        public static string sonKey = "1928B1490019409EA6583D4668237EEB"; //string.Empty;//自己颁发的key
        public static string mackey = "28712D7644F99C3C19618AF0396FA390";//string.Empty;
        public static string pinKey = "F88E8810DB0F767C6921540FE4D2AB52";// string.Empty;
        /// <summary>
        /// 客户端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        static byte[] Client(string ip, int port, byte[] messages)
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
                Console.WriteLine(string.Format("{0:HH:mm:ss}->{1}", DateTime.Now, ex.Message));
            }
            //Console.ReadKey();
            datas = new byte[0];
            return datas;
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, ResultData> sign()
        {
            //签到


            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "000000");
            data.Add(11, "000008");
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            data.Add(63, "01");
            byte[] result = Client(LakalaIp, LakalaPort, EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null))); //发送数据
            if (result == null || result.Length <= 0)
            {
                return null;
            }

            Console.WriteLine(BCDUtil.byteArrToString(result));
            AnalysisBaseWT lk = new AnalysisBaseWTSign();
            Dictionary<string, ResultData> rd = lk.analysis(result);
            return rd;
        }
        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="mackey"></param>
        /// <returns></returns>
        static Dictionary<string, ResultData> pay()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, "9300000000000600");
            data.Add(3, "000000");
            data.Add(4, "000000000001");
            data.Add(11, "000006");
            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(60, "22000003000");
            data.Add(62, getPin("111111", "9300000000000600"));
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0200", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0200")));
            Console.WriteLine();
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 冲正
        /// </summary>
        /// <returns></returns>
        static Dictionary<string, ResultData> correct()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(2, "9300000000000600");
            data.Add(3, "000000");
            data.Add(4, "000000000001");
            data.Add(11, "000002");
            data.Add(22, "051");
            data.Add(25, "00");
            data.Add(39, "17");
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            data.Add(48, "Y00000001");
            data.Add(49, "156");
            data.Add(60, "22000003000");
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0400", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
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
            string str = MacUtils.CSMMAC(mackey, BCDUtil.byteArrToString(countMacData));
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
        static string getMMDDHHMMSSTime()
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
        static string getMMDD()
        {
            string month = TimeStrLenZero(DateTime.Now.Month.ToString());
            string day = TimeStrLenZero(DateTime.Now.Day.ToString());
            return month + day;
        }
        /// <summary>
        /// 获取当前时分秒
        /// </summary>
        /// <returns></returns>
        static string getHHMMSS()
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
        static string TimeStrLenZero(string str)
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
        static Dictionary<string, ResultData> signOut()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "000000");
            data.Add(11, "000006");
            data.Add(12, getHHMMSS());
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            data.Add(60, "22000003000");
            data.Add(63, "01");
            byte[] sendData = EncryptWT.EncryptData("0820", "6000000000", EncryptWT.encryptLKL(data, calculateMac(data, "0400")));
            Console.WriteLine(BCDUtil.byteArrToString(sendData));
            AnalysisBaseWT lk = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, sendData);
            if (result.Length > 0)
            {
                return lk.analysis(result); //发送数据
            }
            else
            {
                return null;
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
            result = BCDUtil.byteArrToString(CSm4.EncriptB(BCDUtil.ToByteArray(pinKey), BCDUtil.ToByteArray(orx)));
            // Console.WriteLine("result="+ result);
            Console.WriteLine(result.ToUpper());
            return result.ToUpper();
        }
        /// <summary>
        /// 打印数据到控制的 同时重要功能：如果返回80 更新pinKey 与 macKey
        /// </summary>
        /// <param name="data">传入服务器返回byte数组</param>
        /// <returns>返回信息为系统返回信息</returns>
        static string handleDataMac(Dictionary<string, ResultData> rd)
        {
            if (rd == null)
            {
                return "系统无返回数据！！";
            }
            if (rd.ContainsKey("62"))
            {
                ResultData _62 = rd["62"];
                Console.WriteLine("加密key:" + sonKey);
                //获取44域中的密文密钥
                string mackeyExpress = _62.value.Substring(_62.value.Length - 40, 40);
                Console.WriteLine("密文mackey=" + mackey);
                //将密文密钥解析明文密钥
                mackey = validateKey(mackeyExpress, sonKey);
                Console.WriteLine("明文mackey=" + mackey);
                string pinKeyExpress = _62.value.Substring(0, 40);


                Console.WriteLine("密文pinKey:" + pinKeyExpress);
                pinKey = validateKey(pinKeyExpress, sonKey);
                Console.WriteLine("明文pinkey:" + pinKey);
            }
            foreach (var item in rd)
            {
                Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            }
            return rd["39"].value;
        }
        /// <summary>
        /// 信息处理
        /// </summary>
        /// <param name="msgNo"></param>
        /// <returns></returns>
        static string handleDataMsg(string msgNo)
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
        static void downKey()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(3, "060000");
            data.Add(11, "000002");
            data.Add(41, "pos00001");
            data.Add(42, "000000000000257");
            //data.Add(62, mackey);
            data.Add(63, "01");
            AnalysisBaseWT wt = new AnalysisBaseWT();
            byte[] result = Client(LakalaIp, LakalaPort, EncryptWT.EncryptData("0800", "6000000000", EncryptWT.encryptLKL(data, null))); //发送数据
            Dictionary<string, ResultData> rd = wt.analysis(result);
            sonKey = validateKey(rd["62"].value, matherKey);
            printInfo(rd);
        }
        /// <summary>
        /// 验证数据是否通过验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static string validateKey(string data, string key)
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

        static void printInfo(Dictionary<string, ResultData> rd)
        {
            foreach (var item in rd)
            {
                Console.WriteLine(item.Key + "-----" + item.Value.name + ":::::" + item.Value.value);
            }
        }

        /// <summary>
        /// RSA Encrypt
        /// </summary>
        /// <param name="sSource" >Source string</param>
        /// <param name="sPublicKey" >public key</param>
        /// <returns></returns>
        public static string EncryptString(string sSource, string sPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string plaintext = sSource;
            rsa.FromXmlString(sPublicKey);
            byte[] cipherbytes;
            byte[] byteEn = rsa.Encrypt(Encoding.UTF8.GetBytes("a"), false);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), false);



            StringBuilder sbString = new StringBuilder();
            for (int i = 0; i < cipherbytes.Length; i++)
            {
                sbString.Append(cipherbytes[i] + ",");
            }
            return sbString.ToString();
        }
    }
}
