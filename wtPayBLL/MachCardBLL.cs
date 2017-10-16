using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using wtPayCommon;

namespace wtPayBLL
{
    public class MachCardBLL
    {
        /// <summary>
        /// 电动读卡器退卡
        /// </summary>
        public static void backCard()
        {
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(1024);
                handle = CRT310.CommOpen("COM" + port);
                //进卡控制，不进卡
                CRT310.CRT310_CardSetting(handle, 0x1, 0x1);

                //读卡器状态
                Byte atPosition = new byte();
                Byte frontSetting = new byte();
                Byte rearSetting = new byte();

                //读取状态
                int hasCard = CRT310.CRT310_GetStatus(handle, ref atPosition, ref frontSetting, ref rearSetting);
                if ((atPosition == 0x4a) || (atPosition == 0x4b) || (atPosition == 0x4c) || (atPosition == 0x4d) || (atPosition == 0x46) || (atPosition == 0x47) || (atPosition == 0x48))
                {
                    //弹卡
                    CRT310.CRT310_Reset(handle, 1);
                    SysBLL.Player("请保管好您的卡片.wav");
                }
                //进卡控制，不进卡
                CRT310.CRT310_CardSetting(handle, 0x1, 0x1);
                CRT310.TT_EjectCard(0, new StringBuilder());
            }
            catch (ThreadAbortException ex) { log.Write("error:电动读卡器退卡:"+ex.Message); }
            catch (Exception e)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                CRT310.CommClose(handle);

            }
        }
        /// <summary>
        /// 等待插卡
        /// </summary>
        public static void waitCard()
        {
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(260);
                handle = CRT310.CommOpen("COM" + port);
                //进卡控制
                CRT310.CRT310_CardSetting(handle, 0x3, 0x1);
                //停卡位置
                CRT310.CRT310_CardPosition(handle, 0x4);

            }
            catch (ThreadAbortException ae) { log.Write("error:等待插卡方法异常："+ae.Message); }
            catch (Exception e)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                CRT310.CommClose(handle);
            }
        }

        public static void waitBankCard()
        {
            StringBuilder info1 = new StringBuilder(260);
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(260);
                int handle = CRT310.TT_OpenDevice(new StringBuilder("COM" + port), new StringBuilder("9600"), new StringBuilder());

               
                CRT310.TT_SetCardMode(1, info1);

                StringBuilder info2 = new StringBuilder(260);
                CRT310.TT_WaitCard(0, info2).ToString();

                //停卡位置
                CRT310.CRT310_CardPosition(handle, 0x4);
               
            }
            catch (Exception e)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                //关闭端口
                CRT310.TT_CloseDevice(info1);
            }
        }
        /// <summary>
        /// 电动读卡器吞卡
        /// </summary>
        public static void swallowCard()
        {
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(260);
                handle = CRT310.CommOpen("COM" + port);
                //读卡器状态
                Byte atPosition = new byte();
                Byte frontSetting = new byte();
                Byte rearSetting = new byte();

                //读取状态
                int hasCard = CRT310.CRT310_GetStatus(handle, ref atPosition, ref frontSetting, ref rearSetting);
                if ((atPosition == 0x4b) || (atPosition == 0x4a))
                {
                    //进卡控制，不进卡
                    CRT310.CRT310_CardSetting(handle, 0x1, 0x1);
                    //弹卡
                    CRT310.CRT310_Reset(handle, 2);
                    SysBLL.isSwallowCard = "9999";
                }
               
            }
            catch (Exception e)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                CRT310.CommClose(handle);
            }
        }
        ///// <summary>
        ///// 禁止用户插卡
        ///// </summary>
        //public static void CancelWaitCard()
        //{

        //    int handle = 0;
        //    try
        //    {
        //        string port = SysConfigHelper.readerNode("CRT310Port");
        //        StringBuilder info = new StringBuilder(260);
        //        handle = CRT310.CommOpen("COM" + port);
        //        CRT310.TT_CancelWait(info);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new WtException(WtExceptionCode.Sys.UNION_READ, e.Message);
        //    }
        //    finally
        //    {
        //        CRT310.CommClose(handle);
        //    }
        //}

        /// <summary>
        /// 禁止用户插卡
        /// </summary>
        public static void CancelWaitCard()
        {

            StringBuilder outMsg = new StringBuilder(260);
            try
            {
                StringBuilder port = new StringBuilder(SysConfigHelper.readerNode("CRT310Port"));
                StringBuilder bote = new StringBuilder("9600");
                StringBuilder info = new StringBuilder(260);

                int openPort = CRT310.TT_OpenDevice(port, bote, info);
                int ret = CRT310.TT_CancelWait(info);
            }
            catch (Exception e)
            {

            }
            finally
            {
                CRT310.TT_CloseDevice(outMsg);
            }
        }
        /// <summary>
        /// 卡机内是否有卡
        /// </summary>
        /// <returns></returns>
        public static bool CardUsable()
        {
            bool iss = false;
            int handle = 0;
            try
            {
                string port = SysConfigHelper.readerNode("CRT310Port");
                StringBuilder info = new StringBuilder(1024);
                handle = CRT310.CommOpen("COM" + port);
                //读卡器状态
                Byte atPosition = new byte();
                Byte frontSetting = new byte();
                Byte rearSetting = new byte();
                //读取状态
                int hasCard = CRT310.CRT310_GetStatus(handle, ref atPosition, ref frontSetting, ref rearSetting);
                if ((atPosition == 0x4b) || (atPosition == 0x4a))
                {
                    iss=true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                CRT310.CommClose(handle);
            }
            return iss;
        }
    }

}
