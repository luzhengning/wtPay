using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.SocialSecurityModel
{
    /// <summary>
    /// 养老发放信息查询
    /// 个人参保信息查询
    /// 医保账户信息查询
    /// 养老月账户信息查询
    /// 医疗账户消费信息查询
    /// </summary>
    public class SocialSecurityParam
    {
        public string grcard;
        public string idcard;
        public string password;
        public string qstime;
        public string jztime;
        public string Id;
        public string conName;
        /// <summary>
        /// 社保查询类型
        /// </summary>
        public int type;
    }
    /// <summary>
    /// 养老发放信息查询
    /// </summary>
    public class PensionGrantInfo
    {
        public List<PensionGrantInfoData> data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class PensionGrantInfoData
    {
        /// <summary>
        /// 个人编号
        /// </summary>
        public string grcard { get; set; }
        /// <summary>
        /// 实付年月
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public string amountDue { get; set; }
        /// <summary>
        /// 银行发放金额
        /// </summary>
        public string actualAmount { get; set; }
        /// <summary>
        /// 备注1
        /// </summary>
        public string remark1 { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        public string remark2 { get; set; }
        /// <summary>
        /// 备注3
        /// </summary>
        public string remark3 { get; set; }
    }
    /// <summary>
    /// 个人参保信息查询
    /// </summary>
    public class SocialSecurityInfo
    {
        public List<SocialSecurityInfoData> data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class SocialSecurityInfoData
    {
        public string time { get; set; }
        public string grcard { get; set; }
        public string CompanyNo { get; set; }
        public string state { get; set; }
        public string CompanyName { get; set; }
        public string insuredType { get; set; }
    }
    /// <summary>
    /// 医保账户信息查询
    /// </summary>
    public class MedicalAccountInfo
    {
        public MedicalAccountInfoData data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class MedicalAccountInfoData
    {
        public string remark { get; set; }
        public string coordinate { get; set; }
        public string cadres { get; set; }
        public string otherAccounts { get; set; }
        public string balance { get; set; }
        public string civilServants { get; set; }
        public string expense { get; set; }
        public string grcard { get; set; }
        public string income { get; set; }
        public string interest { get; set; }
        public string illness { get; set; }
        public string year { get; set; }
        public string returns { get; set; }

    }

    /// <summary>
    /// 养老月账户信息查询
    /// </summary>
    public class PensionAccountInfo
    {
        public List<PensionAccountInfoData> data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }
    public class PensionAccountInfoData
    {
        public string grcard { get; set; }
        public string time { get; set; }
        public string baseNumber { get; set; }
        public string average { get; set; }
        public string companyTransfers { get; set; }
        public string person { get; set; }
        public string total { get; set; }
        public string months { get; set; }
        public string postingDate { get; set; }

    }

    /// <summary>
    /// 医疗账户消费信息查询
    /// </summary>
    public class MedicalAccountConsumeInfo
    {
        public List<MedicalAccountConsumeInfoData> data { get; set; }
        public string dateTime { get; set; }
        public string success { get; set; }
        public string msg { get; set; }
        public string u { get; set; }
        public string recode { get; set; }
    }

    public class MedicalAccountConsumeInfoData
    {
        public string grcard { get; set; }
        public string name { get; set; }
        public string idcard { get; set; }
        public string hospitalNumber { get; set; }
        public string hospitalName { get; set; }
        public string time { get; set; }
        public string expense { get; set; }
        public string type { get; set; }

    }

}
