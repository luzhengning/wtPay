using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtPayModel;
using wtPayCommon;
using System.Configuration;

namespace wtPayBLL
{
    public class BusCardBLL
    {
        //public override Card getCardInfo(string cardNo)
        //{
        //    //TODO 查询公交卡信息
        //    return null;
        //}

        //public override bool Recharge(string cardNo, int gasValue)
        //{
        //    throw new NotImplementedException();
        //}

        //public override bool Recharge(string cardNo, double moneyCount)
        //{
        //    //TODO 调用拉卡拉消费
        //    //TODO 调用拉卡拉消费，如果支付失败，提示错误，退出方法
        //    //TODO 调用拉卡拉消费，如果支付成功，调用北科维拓公交缴费接口，完成充值
        //    return false;
        //}

        /// <summary>
        /// 通过读卡器，读取公交卡卡号
        /// </summary>
        /// <returns></returns>
        public string GetCardNo()
        {
            try
            {
                string port = SysConfigHelper.readerNode("CRT603Port");
                //int openRet = CRT603.CRT603Vx_OpenConnection(Int32.Parse(port), 19200);

                //上电
                int iOutAtrLen = 0;
                byte[] byOutAtrData = new byte[1024];
                int chipRet = CRT603.CRT603Vx_RF_chipPower(ref iOutAtrLen, byOutAtrData);

                string firstAction = "00A40000023F00";
                Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(firstAction);

                int[] exdataLen1 = new int[2];
                Byte[] RxData = new Byte[1024];

                int ret = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);
                if (ret == 0)
                {

                    string secondAction = "00B0850000";
                    sendData = System.Text.Encoding.ASCII.GetBytes(secondAction);
                    int ret2 = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);
                    string strRecv2 = "";
                    for (int i = 0; i < exdataLen1[0]; i++)
                    {
                        strRecv2 += string.Format("{0:X2}", RxData[i]);

                    }
                    //CRT603.CRT603Vx_CloseConnection();
                    string no = strRecv2.Substring(16, 16);
                    if (no == null)
                    {
                        throw new Exception("系统异常");
                    }
                    return no;

                }
                throw new Exception();
            }
            catch(WtException e){
                throw e;
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
            finally
            {
                //CRT603.CRT603Vx_CloseConnection();
            }
        }
        public string sendApdu(string Apdu)
        {
            try{
                string firstAction = Apdu;
                Byte[] sendData = System.Text.Encoding.ASCII.GetBytes(firstAction);

                int[] exdataLen1 = new int[2];
                Byte[] RxData = new Byte[1024];

                int ret = CRT603.CRT603Vx_RF_SendApdu(sendData.Length, sendData, exdataLen1, RxData);
                if (ret == 0)
                {
                    string strRecv2 = "";
                    for (int i = 0; i < exdataLen1[0]; i++)
                    {
                        strRecv2 += string.Format("{0:X2}", RxData[i]);

                    }
                    return strRecv2;
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception("系统异常");
            }
        }
    }
}
