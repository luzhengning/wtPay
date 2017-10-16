using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WtPayBLL;
using wtPayModel;
using wtPayModel.PayParamModel;

namespace wtPayBLL
{
    public class PayBLL
    {
        /// <summary>
        /// 根据icParams获取payCode和terminalNo和万通拉卡拉交易流水
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ShopNo"></param>
        public static void payCode_terminalNo(ref PayParam p)
        {
            try
            {
                //向后台发送订单记录
                if (p.icParams == null)
                {
                    //万通
                    p.WtLklorderNo = Wanton.GetOrderNo();
                    p.ClientNo = p.MERCHANTNO_shopNo + "|" + SysConfigHelper.readerNode("ClientNo");
                    p.payCode = "Z000000003";
                    p.terminalNo = SysConfigHelper.readerNode("ClientNo");
                }
                else
                {
                    //拉卡拉
                    p.WtLklorderNo = LKLProcedure.GetOrderNo();
                    p.ClientNo = p.MERCHANTNO_shopNo + "|" + p.TERMINALNO_clientNo;
                    //拉卡拉
                    p.payCode = "Z000000005";
                    p.terminalNo = SysConfigHelper.readerNode("LklClientNo");
                }
            }catch(Exception ex)
            {
                log.Write("error：发送订单失败："+ex.Message);
            }
        }
        /// <summary>
        /// 获取支付报文
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string PayMessage(ref PayParam p)
        {
            string sendDataStr = "";
            PasswordBLL pwdBLL = new PasswordBLL();
            if (p.icParams == null)
            {
                log.Write("交易类型：万通交易");
                pwdBLL.SetCryptMode(1);
                log.Write("万通卡号：" + p.WtNo);
                log.Write("交易金额：" + p.rechageAmount);
                log.Write("终端交易流水号" + p.WtLklorderNo);
                log.Write("开始支付");
                Wanton.pay(p,ref sendDataStr);
                p.payType = "1";
                return sendDataStr;
            }
            else
            {
                log.Write("交易类型：拉卡拉交易");
                //银行卡支付
                pwdBLL.SetCryptMode(0);
                
                pwdBLL.SetCryptMode(0);//拉卡拉加密方式

                log.Write("银行卡号：" + p.icParams["cardNo"]);
                log.Write("交易金额：" + p.rechageAmount);
                log.Write("终端交易流水号" + p.WtLklorderNo);
                log.Write("开始支付");
                wtPayBLL.LKLProcedure.PayOrder(p,ref sendDataStr);
                p.payType = "0";
                return sendDataStr;
            }
            p.shopNo = p.WtLklorderNo + "|" + SysConfigHelper.readerNode("ClientNo") + "|" + SysConfigHelper.readerNode("ShopNo");

        }
        
        /// <summary>
        /// 冲正
        /// </summary>
        public static string Correct(ref PayParam p)
        {
            log.Write("支付异常，发起冲正");
            string sendDataStr = "";
            //冲正
            if (p.icParams == null)
            {
                //万通冲正
                Wanton.correct(p,ref sendDataStr);
                return sendDataStr;
            }
            else
            {
                //拉卡拉冲正
                LKLProcedure.correct(p, ref sendDataStr);
                return sendDataStr;
            }

        }

        /// <summary>
        /// 判断支付结果
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static string PayResult(PayParam p, Dictionary<string, string> map)
        {
            if (map == null) throw new Exception();
            PasswordBLL pwdBLL = new PasswordBLL();
            if (!map["recode"].Equals("00"))
            {
                if ("55".Equals(map["recode"]))
                {
                    SysBLL.PasswordErrorInfo = "密码错误，请重新输入！";
                    log.Write("密码错误");
                    log.Write("--------------------交易结束--------------------");
                    if (p.icParams == null)
                    {
                        pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", p.WtNo);
                        return "55";
                    }
                    else
                    {
                        pwdBLL.OpenKeyboard(SysConfigHelper.readerNode("ZT598Port"), "9600", p.icParams["cardNo"]);
                        return "55";
                    }
                }
                else if ("51".Equals(map["recode"]))
                {
                    log.Write("余额不足");
                    return "51";
                }
                else
                {
                    log.Write("支付失败：返回码：" + map["recode"]);
                    return map["recode"];
                }
            }
            return null;
        }












    }
}
