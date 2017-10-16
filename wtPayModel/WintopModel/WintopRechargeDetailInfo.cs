﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.WintopModel
{
    /// <summary>
    /// 万通卡充值明细结果
    /// </summary>
    public class WintopRechargeDetailInfo
    {
        public WintopRechargeDetailInfoMsg msgrsp { get; set; }
        public Msghead msghead { get; set; }
    }
    public class WintopRechargeDetailInfoMsg
    {
        public string retcode { get; set; }
        public string retshow { get; set; }
        public string totalCount { get; set; }
        public string totalPage { get; set; }
        public List<WintopRechargeDetailInfoMsgwTRechargeInfo> wTRechargeInfo { get; set; }
    }
    public class WintopRechargeDetailInfoMsgwTRechargeInfo
    {
        public string AMOUNT { get; set; }
        public string BUSINESSNO { get; set; }
        public string LASTTIME { get; set; }
        public string MERCHANTNAME { get; set; }
        public string STATE { get; set; }
        public string TERBUSINESSNO { get; set; }
        public string TYPE { get; set; }
        public string USERID { get; set; }
        public string WTCARDID { get; set; }
    }
}
