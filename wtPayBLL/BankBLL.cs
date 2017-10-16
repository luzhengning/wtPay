using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TTReaderCard;
using wtPayCommon;

namespace wtPayBLL
{

    public class BankBLL
    {
        /// <summary>
        /// 读取磁条卡
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> ReadCitiao()
        {
            
            StringBuilder info4 = new StringBuilder(1000);
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(260);
                int handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());

                Dictionary<string, string> citiaoParams = new Dictionary<string, string>();

                byte[] data = new byte[1024];
                System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();


                byte[] sb = new byte[1024];
                int TT_GetTrackData = CRT310.TT_GetTrackData(sb, 2, null);
                //byte[] data = Encoding.ASCII.GetBytes(sb.ToString());

                string str=System.Text.Encoding.GetEncoding("gb2312").GetString(sb);
                citiaoParams["cidao2"] = str.Replace("=", "D");

                citiaoParams["cardNo"] = citiaoParams["cidao2"].Substring(0, citiaoParams["cidao2"].IndexOf("D"));

                byte[] data1 = new byte[1024];
                CRT310.TT_GetTrackData(data1, 3, info4);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(data1).TrimEnd('\0');
                citiaoParams["cidao3"] = str.Replace("=", "D");
                CRT310.TT_CloseDevice(info4);
                return citiaoParams;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
            finally
            {

                CRT310.TT_CloseDevice(info4);
            }
        }


        /// <summary>
        /// 读卡IC卡
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> ReadIC()
        {
            StringBuilder info = new StringBuilder(260);
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());

                Dictionary<string, string> icParams = new Dictionary<string, string>();
                StringBuilder info4 = new StringBuilder(1000);
                byte[] sb = new byte[10240];
                int TT_GetTrackData = CRT310.TT_GetTrackData(sb, 2, null);
                //byte[] data = Encoding.ASCII.GetBytes(sb.ToString());

                string str = System.Text.Encoding.GetEncoding("gb2312").GetString(sb);
                icParams["cidao2"] = str.Replace("=", "D");

                icParams["cardNo"] = icParams["cidao2"].Substring(0, icParams["cidao2"].IndexOf("D"));

                byte[] data1 = new byte[1024];
                CRT310.TT_GetTrackData(data1, 3, info4);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(data1).TrimEnd('\0');
                icParams["cidao3"] = str.Replace("=", "D");


                StringBuilder info1 = new StringBuilder(260);

                StringBuilder info2 = new StringBuilder(260);

                byte[] data = null;
                StringBuilder info3 = new StringBuilder(260);
               
                StringBuilder szMsg = new StringBuilder();


                byte[] result = new byte[2048];
                CRT310.TT_CPUPowerOnOrDown(1, info);
                CRT310.TT_CPUGetICCardInfo(result, info);
                CRT310.TT_CPUPowerOnOrDown(0, info);
                System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
                str = ASCII.GetString(result).TrimEnd('\0');
                string[] sArray = str.Split('|');
                //退卡
                icParams["period"] = sArray[0];
                icParams["serialNo"] = sArray[1];


                if (sArray[3].IndexOf("\0") != -1)
                {
                    sArray[3].Replace("\0", "");
                }
                icParams["area55"] = sArray[3];

           
                //status = CRT310.TT_GetTrackData(data, 3, info4);
                //M1essageBox.Show("3磁道数据：" + data.ToString());
                CRT310.CommClose(handle);
                CRT310.TT_CloseDevice(info);
                return icParams;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
            finally
            {

                CRT310.CommClose(handle);
                CRT310.TT_CloseDevice(info);
            }
            
        }




        public static Dictionary<string, string> ReadBankCard1()
        {
            StringBuilder info = new StringBuilder(260);
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());

                Dictionary<string, string> icParams = new Dictionary<string, string>();
                StringBuilder info4 = new StringBuilder(1000);

                //读磁道信息

                //二磁道
                byte[] cidao2 = new byte[1024*4];
                int TT_GetTrackData = CRT310.TT_GetTrackData(cidao2, 2, null);
                string str = System.Text.Encoding.GetEncoding("gb2312").GetString(cidao2).TrimEnd('\0');
                icParams["cidao2"] = str.Replace("=", "D");

                ///三磁道
                byte[] cidao3 = new byte[1024 * 4];
                CRT310.TT_GetTrackData(cidao3, 3, info4);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(cidao3).TrimEnd('\0');
                icParams["cidao3"] = str.Replace("=", "D");

                //读卡号
                icParams["cardNo"] = icParams["cidao2"].Substring(0, icParams["cidao2"].IndexOf("D"));

                //读IC卡信息
                byte[] icInfo = new byte[2048];
                CRT310.TT_CPUPowerOnOrDown(1, info);
                CRT310.TT_CPUGetICCardInfo(icInfo, info);
                CRT310.TT_CPUPowerOnOrDown(0, info);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(icInfo).TrimEnd('\0');

                if (str!=null && !"".Equals(str.Trim())) {
                    string[] sArray = str.Split('|');
                    //退卡
                    if (sArray!=null && sArray.Length>=4)
                    {
                        icParams["period"] = sArray[0];
                        icParams["serialNo"] = sArray[1];

                        if (sArray[3].IndexOf("\0") != -1)
                        {
                            sArray[3].Replace("\0", "");
                        }
                        icParams["area55"] = sArray[3];
                    }
                }
                CRT310.CommClose(handle);
                CRT310.TT_CloseDevice(info);
                return icParams;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
            finally
            {

                CRT310.CommClose(handle);
                CRT310.TT_CloseDevice(info);
            }

        }
        private static Byte[] icinfo = new Byte[2048];
        private static Byte[] trackinfo = new Byte[2048];
        public static Dictionary<string, string> ReadBankCard()
        {
            StringBuilder info = new StringBuilder(260);
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");

                Byte[] buff = new Byte[1024];
                int result = CCardReader.TT_OpenDevice("COM" + port, "", buff);

                //handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());

                Dictionary<string, string> icParams = new Dictionary<string, string>();




                StringBuilder info4 = new StringBuilder(1000);

                //读磁道信息
                Byte[] trackBuf = new Byte[1024];
                //int trackRet = CCardReader.TT_GetTrackData(trackinfo, 4, trackBuf);




                //二磁道
                byte[] cidao2 = new byte[1024 * 4];
                int TT_GetTrackData = CCardReader.TT_GetTrackData(cidao2, 2, trackBuf);


                string str = System.Text.Encoding.GetEncoding("gb2312").GetString(cidao2).TrimEnd('\0');
                icParams["cidao2"] = str.Replace("=", "D");

                ///三磁道
                byte[] cidao3 = new byte[1024 * 4];
                CCardReader.TT_GetTrackData(cidao3, 3, trackBuf);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(cidao3).TrimEnd('\0');
                icParams["cidao3"] = str.Replace("=", "D");

                //读卡号
                icParams["cardNo"] = icParams["cidao2"].Substring(0, icParams["cidao2"].IndexOf("D"));

                //读IC卡信息
                byte[] icInfo = new byte[2048];
                CCardReader.TT_CPUPowerOnOrDown(1, trackinfo);
                CCardReader.TT_CPUGetICCardInfo(ref icInfo, trackinfo);

                //int icRet = CCardReader.TT_CPUGetICCardInfo(ref icinfo, buff);


                CCardReader.TT_CPUPowerOnOrDown(0, trackinfo);
                str = System.Text.Encoding.GetEncoding("gb2312").GetString(icInfo).TrimEnd('\0');

                if (str != null && !"".Equals(str.Trim()))
                {
                    string[] sArray = str.Split('|');
                    //退卡
                    if (sArray != null && sArray.Length >= 4)
                    {
                        icParams["period"] = sArray[0];
                        icParams["serialNo"] = sArray[1];

                        if (sArray[3].IndexOf("\0") != -1)
                        {
                            sArray[3].Replace("\0", "");
                        }
                        icParams["area55"] = sArray[3];//+ "DF3100"
                    }
                }
                //CRT310.CommClose(handle);
                CCardReader.TT_CloseDevice(trackinfo);
                return icParams;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
            finally
            {

                //CRT310.CommClose(handle);
                //CRT310.TT_CloseDevice(info);

                CCardReader.TT_CloseDevice(trackinfo);
            }

        }

    }
}
