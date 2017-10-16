using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtPayModel;
using wtPayCommon;
using System.Threading;

using lzocx2Lib;

using System.Configuration;


namespace wtPayBLL
{
    public class XianfengBLL : CardBLL
    {
        private int port = Int32.Parse(SysConfigHelper.readerNode("CJ201"));
        /// <summary>
        /// 读先锋卡
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public Card getCardInfo(string cardNo)
        {
            if (SysBLL.isGasCRT)
            {
                CJ201.Open_Com(2, 9600, 8, 0, 0);
            }
            else
            {
                CJ201.Close_Com(CJ201.handle);
            }

            try
            {
                iccardClass ic = new lzocx2Lib.iccardClass();
                ic.port = (short)port;
                int ret = ic.read_card_lz1();
                if (ret == 0)
                {
                    Thread.Sleep(1000);
                    GasCard card = new GasCard();
                    card.CardNo = ic.userid;
                    card.GasValue = ic.gasnum;

                    return card;
                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new WtException(WtExceptionCode.Card.GAS_READ_CARD, ex.Message);
            }

        }

        /// <summary>
        /// 写先锋卡
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="gasValue"></param>
        /// <returns></returns>
        public bool Recharge(string cardNo, int gasValue)
        {
            try
            {
                //在线缴费
                //写入燃气卡
                iccardClass ic = new lzocx2Lib.iccardClass();
                ic.port = (short)port;

                //气量
                ic.userid = cardNo;
                ic.gasnum = gasValue;
                int ret = ic.write_card_lz1();
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
        }

    }
}