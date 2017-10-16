using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.PublicFundModel
{
    /// <summary>
    /// 个人客户信息查询
    /// 个人公积金账户查询
    /// 个人公积金贷款余额查询
    /// 个人公积金贷款还款明细查询
    /// </summary>
    public class CustomerParam
    {
        public string selcnt;
        public string seltype;
        public string pwd;
        public string Id;
        public string conName;
        public int queryType;

        /// <summary>
        /// 查询编号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 查询的密码
        /// </summary>
        public string password { get; set; }
    }

    /// <summary>
    /// 个人公积金明细查询
    /// </summary>
    public class PublicDetailedParam
    {
        public string percode;
        public string bdate;
        public string edate;
        public string pwd;
        public string Id;
        public string conName;
    }

    /// <summary>
    /// 个人客户信息查询应答
    /// </summary>
    public class CustomerInfo
    {
        public CustomerInfoData data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }

        public string msg { get; set; }
        public string u { get; set; }

        public string recode { get; set; }
    }
    public class CustomerInfoData
    {
        public string cardtype { get; set; }
        public string result { get; set; }
        public string cardcode { get; set; }
        public string birthday { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }
        public string selcnt { get; set; }
        public string resfilemny { get; set; }
        public string pername { get; set; }
        public string resident { get; set; }
        public string resmsg { get; set; }
        public string forgcode { get; set; }
        public string percode { get; set; }
        public string txchannel { get; set; }
        public string rescode { get; set; }
        public string txcode { get; set; }
        //public string msg { get; set; }
        public string reqident { get; set; }
        //public string checker { get; set; }
        //public string operator { get; set; }
        public string txtime { get; set; }
        //public string prepicnt1 { get; set; }
        //public string prepicnt2 { get; set; }
        public string pwd { get; set; }
        public string flag { get; set; }
        //public string enccode { get; set; }
        public string email { get; set; }
        public string reqfilemny { get; set; }
        public string txdate { get; set; }
        public string seltype { get; set; }
        public string torgcode { get; set; }
    }

    /// <summary>
    /// 公积金账户查询
    /// </summary>
    public class PublicAccountInfo
    {
        public List<PublicAccountInfoDatas> data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class PublicAccountInfoDatas
    {
        public string data1 { get; set; }
        public string data2 { get; set; }
        public string data3 { get; set; }
        public string data4 { get; set; }
        public string data5 { get; set; }
        public string data6 { get; set; }
        public string data7 { get; set; }
        public string data8 { get; set; }
        public string data9 { get; set; }
    }

    /// <summary>
    /// 公积金明细查询
    /// </summary>
    public class PublicFundDetailedInfo
    {
        public List<PublicFundDetailedInfoData> data;
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class PublicFundDetailedInfoData
    {
        public string data1 { get; set; }
        public string data2 { get; set; }
        public string data3 { get; set; }
        public string data4 { get; set; }
        public string data5 { get; set; }
        public string data6 { get; set; }
    }
    /// <summary>
    /// 个人公积金贷款余额查询
    /// </summary>
    public class LoanBalanceInfo
    {
        public LoanBalanceInfoData data;
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class LoanBalanceInfoData
    {
        public string pername { get; set; }
        public string percode { get; set; }
        public string cardtype { get; set; }
        public string cardcode { get; set; }
        public string agrcode { get; set; }
        public string payacc { get; set; }
        public string loanmny { get; set; }
        public string loanbal { get; set; }
        public string payedmths { get; set; }
        public string overmths { get; set; }
        public string lastpaydate { get; set; }
        public string totoalmny { get; set; }
        public string corpus { get; set; }
        public string interests { get; set; }
        public string overmny { get; set; }

    }

    /// <summary>
    /// 个人公积金贷款明细
    /// </summary>
    public class LoanDetailedInfo
    {
        public List<LoanDetailedInfoData> data;
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class LoanDetailedInfoData
    {
        public string data1 { get; set; }
        public string data2 { get; set; }
        public string data3 { get; set; }
        public string data4 { get; set; }
        public string data5 { get; set; }
        public string data6 { get; set; }
        public string data7 { get; set; }
        public string data8 { get; set; }
    }
}
