using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayModel;
using wtPayCommon;
using System.Configuration;

namespace wtPayBLL
{
    /// <summary>
    /// 密码键盘业务处理类
    /// </summary>
    public class PasswordBLL
    {

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool OpenDevice(string port, string bote)
        {
            try
            {
                int status = GetDeviceStatus();
                if (status != 0)
                {
                    StringBuilder info = new StringBuilder(1024);
                    int ret = ZT598.TT_OpenDevice(new StringBuilder(port), new StringBuilder(bote), info);
                    if (ret == 0)
                    {
                        return true;
                    }
                    else
                    {

                        return false;

                    }
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {

                throw new Exception("系统异常");
            }
            

        }
        /// <summary>
        /// 打开键盘
        /// </summary>
        /// <param name="port"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public int OpenKeyboard(string port, string bote, string account)
        {
            try
            {
                ZT598.TT_SetCardNo(new StringBuilder(account.Substring(3, 12)), null);
                StringBuilder info = new StringBuilder(1024);
                int enableKey = ZT598.TT_EnableKeyboard(1, 0, 60, 6, info);
                return enableKey;
            }
            catch (Exception ex)
            {

                throw new Exception("系统异常");
            }
        }
        /// <summary>
        /// 从键盘获取键盘密文
        /// </summary>
        /// <returns></returns>
        public string GetPasswordFromZT598()
        {
            try
            {
                StringBuilder str = new StringBuilder(1024);
                StringBuilder info = new StringBuilder(1024);
                int ret = ZT598.TT_GetPin(str, info);
                if (ret == 0)
                {
                    closeKey();
                    return str.ToString();
                }
                throw new Exception();
                
            }
            catch (Exception ex)
            {

                throw new Exception("系统异常");
            }
        }


        /// <summary>
        /// 设置加密模式
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool SetCryptMode(int mode)
        {
            try
            {
                bool state = OpenDevice(SysConfigHelper.readerNode("ZT598Port"), "9600");
                if (!state) log.Write("严重:密码键盘打开失败！");
                StringBuilder info = new StringBuilder(1024);
                int ret = ZT598.TT_SetCryptMode(mode, info);
                log.Write("密码键盘设置加密模式,返回码："+ret);
                log.Write("密码键盘设置加密模式,返回："+info.ToString());
                if (ret == 0)
                {
                    log.Write("关闭密码键盘");
                    int result=closeKey();
                    log.Write("关闭密码键盘返回："+result);
                    log.Write("密码键盘已关闭");
                    return true;
                }
                return false;
               
            }
            catch (Exception ex)
            {
                log.Write("error：设置加密模式异常"+ex.Message+ex.InnerException);
                return false;
            }
            
        }

        /// <summary>
        /// 取设备状态：0为正常
        /// </summary>
        /// <returns></returns>
        public int GetDeviceStatus()
        {
            StringBuilder statusMsg = new StringBuilder("SSS");
            int status = ZT598.TT_GetDeviceStatus(statusMsg);

            return status;
        }

        /// <summary>
        /// 下载工作秘钥
        /// </summary>
        /// <param name="masterKeyIndex"></param>
        /// <param name="workKeyIndex"></param>
        /// <param name="workKey"></param>
        /// <returns></returns>

        public bool DownloadWorkKey(int mIndex, int wIndex, string keyExpress)
        {
            try
            {
                StringBuilder sb = new StringBuilder(2048);
                int ret = ZT598.TT_LoadWorkKey(new StringBuilder(keyExpress), mIndex, wIndex, sb);
                log.Write("设置工作秘钥返回：DownloadWorkKey："+ret+",返回信息："+sb.ToString());
                if (ret == 0)
                {
                    return true;
                }
                throw new Exception();
            }
            catch (Exception ex)
            {

                throw new Exception("设置工作秘钥异常：DownloadWorkKey："+ex.Message+ex.InnerException);
            }
            
        }

        /// <summary>
        /// 下载工作密钥
        /// </summary>
        /// <param name="mode">原主密钥的长度模式: DES_MODE:   1     //8 字节 TDES_MODE:  2     //16 字节 TDES_MODE2: 3     //24 字节</param>
        /// <param name="keyIndex">主密钥索引值，从0开始：0为拉卡拉，1为万通</param>
        /// <param name="primaryKey">主密钥值（明文，密文待商榷）</param>
        /// <param name="checkValue">如果是校验下载，输入4 字节检 验值 </param>
        /// <returns></returns>
        public bool DownloadMasterKey(int masterKeyIndex, string workKey)
        {
            try
            {
                int ret = ZT598.TT_LoadMasterKey(new StringBuilder(workKey), masterKeyIndex, null);
                if (ret == 0)
                {
                    return true;
                }

                throw new Exception();
            }
            catch (Exception ex)
            {

                throw new Exception("系统异常");
            }
        }

        /// <summary>
        /// 激活工作秘钥：成功返回true，失败返回false
        /// </summary>
        /// <param name="masterKeyIndex">主密钥索引</param>
        /// <param name="workKeyIndex">工作秘钥索引</param>
        /// <returns></returns>
        public bool ActivWorkKey(int masterKeyIndex, int workKeyIndex)
        {
            try{
                int ret = ZT598.TT_ActivateWorkKey(masterKeyIndex, workKeyIndex, null);

                if (ret == 0)
                {
                    return true;
                }
            
                throw new Exception();
            }
            catch (Exception ex)
            {

                throw new Exception("系统异常");
            }
        }

        public static byte[] GetMac(byte type, byte[] countMacData)
        {
            byte[] mac = new byte[8];

            ZT598.TT_MAC(type, countMacData, countMacData.Length, mac, null);

            return mac;


        }
        /// <summary>
        /// p判断是否能继续输入
        /// </summary>
        /// <returns></returns>
        public string isPassword()
        {
            try{
                StringBuilder sb = new StringBuilder(1024);
                StringBuilder info = new StringBuilder(1024);
                int num = ZT598.TT_GetInput(sb, info);
                if (num == 0)
                {
                    byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
                    if (0x0D.Equals(data[0]))
                    {
                        return "0D";
                    }
                    if (0x08.Equals(data[0]))
                    {
                        return "08";
                    }
                    if (0x1B.Equals(data[0]))
                    {
                        return "1B";
                    }
                    if (0x2A.Equals(data[0]))
                    {
                        return "*";
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 关闭密码键盘
        /// </summary>
        /// <returns></returns>
        public int closeKey()
        {
            try
            {
                StringBuilder info = new StringBuilder(1024);
                int state = ZT598.TT_DisableKeyboard(info);
                //return state;
                return 0;
            }
            catch (Exception ex)
            {
                return -1;	
            }
           
        }

    }
}
