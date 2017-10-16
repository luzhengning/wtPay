using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    /// <summary>
    /// 医院
    /// </summary>
    public class HospitalClass
    {
        public static string HospitalAppId = "YYGH001";
    }
    /// <summary>
    /// 查询医院列表参数
    /// </summary>
    public class HospitalParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }

    }
    /// <summary>
    /// 医院列表
    /// </summary>
    public class HospitalInfo
    {
        public HospitalInfoData data { get; set; }
        public string msg { get; set; }
        public string success { get; set; }
    }
    public class HospitalInfoData
    {
        public string total { get; set; }
        public string pageNo { get; set; }
        public string pageSize { get; set; }
        public List<HospitalInfoDataRows> rows { get; set; }
    }
    public class HospitalInfoDataRows
    {
        public string hospital_code { get; set; }
        public string hospital_name { get; set; }
        public string hospital_level { get; set; }
        //public string hospital_insts { get; set; }
        //public string hospital_text { get; set; }
        public string rn{ get; set; }
    }
}
