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
    public class JinCardBLL
    {
        /// <summary>
        /// 读金卡
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public Card getCardInfo(string cardNo)
        {
            try
            {
                CJ201.Close_Com(CJ201.handle);
                StringBuilder vskh = new StringBuilder(260);
                int vlql = 0;
                int vlzyql = 0;
                StringBuilder lpInfo = new StringBuilder(260);
                string port = SysConfigHelper.readerNode("CJ201");
                CJ201.handle = CJ201.Open_Com(Int32.Parse(port), 9600, 8, 0, 0);
                int ret = CJ201.GoldCard_Read_zz(CJ201.handle, vskh, ref vlql, ref vlzyql, lpInfo);
                //关闭端口
                CJ201.Close_Com(CJ201.handle);
                if (vskh.Length > 2)
                {
                    GasCard card = new GasCard();
                    card.CardNo = vskh.ToString();
                    card.GasValue = vlql;
                    card.Total = vlzyql;

                    return card;
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new WtException(WtExceptionCode.Card.GAS_READ_CARD, ex.Message);
            }
            finally
            {
                CJ201.Close_Com(CJ201.handle);
            }
        }

        /// <summary>
        /// 写金卡
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="gasValue"></param>
        /// <returns></returns>
        public bool Recharge(string cardNo, int gasValue)
        {
            CJ201.Close_Com(CJ201.handle);
            try
            {
                //TODO 调用拉卡拉在线缴费
                //TODO 调用拉卡拉在线缴费，如果支付失败，提示支付失败，跳出方法
                //TODO 根据拉卡拉的支付结果，如果支付成功那么执行，写入燃气卡
                StringBuilder vskh = new StringBuilder(1024);
                vskh.Append(cardNo);
                int vlql = gasValue;
                //byte lpInfo = new byte[1024];
                StringBuilder lpInfo = new StringBuilder(1024);
                //lpInfo.Append("0");
                string port = SysConfigHelper.readerNode("CJ201");
                CJ201.handle = CJ201.Open_Com(Int32.Parse(port), 9600, 8, 0, 0);
                //CJ201.lpInfo = new StringBuilder(1024);
                int ret = -1;
                int info = 0;
                try
                {
                    ret = CJ201.GoldCard_Write_zz(CJ201.handle, vskh, vlql, lpInfo);
                }
                catch (Exception ae)
                {
                    log.Write("error:"+ae.Message);

                }
                finally
                {
                    //ret = 0;
                    CJ201.Close_Com(CJ201.handle);

                }

                //int ret = CJ201.GoldCard_Write_zz(CJ201.handle, vskh, vlql, lpInfo);
                //关闭端口
                CJ201.Close_Com(CJ201.handle);
                if (ret == 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                log.Write("error：燃气写卡失败："+ex.Message);
                return false;
            }
            finally
            {
                CJ201.Close_Com(CJ201.handle);
            }
        }

    }
}
