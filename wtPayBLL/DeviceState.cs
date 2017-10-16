using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using wtPayCommon;
using wtPayModel;

namespace wtPayBLL
{
    public class DeviceState
    {
        public static string SendState(string excption, string decription = "")
        {
            log.Write("发送设备状态：" + excption);
            string update_state = "0";
            string t_id = ConfigurationManager.AppSettings["MechineNo"];
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            try
            {
                parameters.Add("excption.update_state", update_state);
                parameters.Add("excption.t_id", t_id);
                parameters.Add("excption.excption", excption);
                if (!"".Equals(decription))
                {
                    parameters.Add("excption.decription", decription);
                }
                string jsonText = null;
                int count = 2;
                for (int i = 1; i <= count; i++)
                {
                    jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("UpdateStateName"), parameters, null);
                    log.Write("设备状态返回："+jsonText);
                    if (jsonText == null) continue;
                    break;
                }
                return jsonText;
            }
            catch { return null; }
        }

        /// <summary>
        /// 电动读卡器异常发送状态
        /// </summary>
        public static string SendCRT310State()
        {
            string port = SysConfigHelper.readerNode("CRT310Port");
            StringBuilder info = new StringBuilder(1024);
            Thread.Sleep(1000 * 1);
            CRT310.TT_CloseDevice(info);
            CRT310.TT_CloseDevice(info);
            Thread.Sleep(1000*1);
            int handle = -1;
            for (int i = 1; i <= 2; i++)
            {
                handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), info);
                if (handle == 0) break;
            }
            int jubing=handle;
            if (handle == 0)
            {
                handle = CRT310.TT_GetDeviceStatus(info);
                CRT310.TT_CloseDevice(info);
                if (handle == 4101) return null;
                if (handle == 4104) return null;
                if (handle != 0)
                {
                   return "01"+handle.ToString();
                } 
            }
            else
            {
                return "01"+handle.ToString();
            }
            return null;
        }

        /// <summary>
        /// 密码键盘异常时发送状态
        /// </summary>
        public static string SendZT598State()
        {
            string port = SysConfigHelper.readerNode("ZT598Port");
            StringBuilder info = new StringBuilder(1024);
            int handle = -1;
            for (int i = 1; i <= 2; i++)
            {
                handle = ZT598.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());
                if (handle == 0) break;
            }
            int jubing = handle;
            if (handle == 0)
            {
                handle = ZT598.TT_GetDeviceStatus(info);
                if (handle != 0)
                {
                    return "02"+handle.ToString();
                }
            }
            else
            {
                return "02"+handle.ToString();
            }
            return null;
        }

        /// <summary>
        /// 打印机异常时发送状态//VT-V1.5.1_CE9
        /// </summary>
        public static string SendPrintState()
        {
            string port = SysConfigHelper.readerNode("PrintPort");
            StringBuilder info = new StringBuilder(1024);
            int handle = -1;
            for (int i = 1; i <= 2; i++)
            {
                handle = Print.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("38400"), new StringBuilder());
                if (handle == 0) break;
            }
            int jubing = handle;
            if (handle == 0)
            {
                handle = Print.TT_GetDeviceStatus(info);
                Print.TT_CloseDevice(info);
                if (handle != 0)
                {
                    return "05"+ handle.ToString();
                }
            }
            else
            {
                return "05"+ handle.ToString();
            }
            return null;
        }

        /// <summary>
        /// 燃气读卡器端口打开失败时发送状态
        /// </summary>
        public static string SendCJ201State()
        {
            string result = GasBLL.GasJudge();
            if ("06".Equals(result))
            {
                return null;
            }
            try
            {
                SerialPort sPort = new SerialPort();
                sPort.PortName = "com" + SysConfigHelper.readerNode("CJ201");//串口的portname 
                sPort.BaudRate = 9600;//串口的波特率 
                sPort.Open();
                if (sPort.IsOpen)
                {
                    sPort.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Write("error:燃气读卡器检测：" + ex.Message);
            }
            return "040001";
        }

        /// <summary>
        /// 公交读卡器打卡端口失败时发送状态 ，
        /// </summary>
        public static string SendCRT603State()
        {
            string port = SysConfigHelper.readerNode("CRT603Port");
            CRT603.CRT603Vx_CloseConnection();
            int handle = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);
            //CRT603.CRT603Vx_CloseConnection();
            if (handle != 0)
            {
                return "030001";
            }
            return null;
        }

        /// <summary>
        /// 终端机吞卡
        /// </summary>
        public static string SendRetainCard()
        {
            if (SysBLL.isSwallowCard != null)
            {
                SysBLL.isSwallowCard = null;
                return "014107";
            }
            else return null;
        }

        public static void SendStatu()
        {
            try
            {
                string str1 = SendRetainCard();
                string str2 = SendCRT310State();
                string str3 = SendZT598State();
                string str4 = SendPrintState();
                string str5 = SendCJ201State();
                string str6 = SendCRT603State();
                string str = "";
                if (str1 != null)
                {
                    str += str1 + "|";
                }
                if (str2 != null)
                {
                    str += str2 + "|";
                }
                if (str3 != null)
                {
                    str += str3 + "|";
                }
                if (str4 != null)
                {
                    str += str4 + "|";
                }
                if (str5 != null)
                {
                    str += str5 + "|";
                }
                if (str6 != null)
                {
                    str += str6;
                }
                if ("".Equals(str))
                {
                    str = "000000";
                }
                SendState(str);
            }catch(Exception ex) { log.Write("error:发送设备状态异常：" + ex.Message); }
        }

        public static void sendRmdStatu()
        {
            new Thread(delegate () {
                Thread.Sleep(5000);
                try
                {
                    if (PayStaticParam.RmbIsOpen)
                    {
                        int code = CashRMB.TT_GetDeviceStatus(new StringBuilder(1024));
                        log.Write("识币器状态："+code);
                        if (code == 0)
                        {
                            PayStaticParam.isHaveRMB = true;
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("pap.t_id", ConfigurationManager.AppSettings["MechineNo"]);
                            parameters.Add("pap.type", code.ToString());
                            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("submitRmbStatus"), parameters, null);
                            return;
                        }
                        if (!((2101 == code) || (2102 == code) || (2103 == code) || (2104 == code) || (2105 == code) || (2106 == code) || (2107 == code) || (2108 == code) || (2201 == code) || (2202 == code)))
                        {
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("pap.t_id", ConfigurationManager.AppSettings["MechineNo"]);
                            parameters.Add("pap.type", code.ToString());
                            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("submitRmbStatus"), parameters, null);
                            //PayStaticParam.isHaveRMB = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Write("error:sendRmdStatu():" + ex.Message);
                }
            }).Start();
        }
    }
}
