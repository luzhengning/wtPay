using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    public class RegistrationRecordClass
    {
        public static string RegistrationRecordAppId = "YYGH009";
    }
    /// <summary>
    /// 预约记录查询参数
    /// </summary>
    public class RegistrationRecordQueryParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string tel { get; set; }
        public string oper_type { get; set; }
        public string flow_no { get; set; }
        public string appointId{ get; set; }

    }

    
    /// <summary>
    /// 预约记录查询列表
    /// </summary>
    public class RegistrationRecordQueryInfo
    {
        public string Result_Code { get; set; }
        public string Error_Msg { get; set; }
        public List<RegistrationRecordQueryInfoRecord> record{ get;set; }
    }
    public class RegistrationRecordQueryInfoRecord
    {
        public string appiont_id { get; set; }
        public string hospital_code { get; set; }
        public string id_no { get; set; }
        public string patient_name { get; set; }
        public string patient_id { get; set; }
        public string doctor_code { get; set; }
        public string order_ip { get; set; }
        public string symptom { get; set; }
        public string vis_card { get; set; }
        public string sex { get; set; }
        public string card_type { get; set; }
        public string hb_date { get; set; }
        public string operate_type { get; set; }
        public string flow_no { get; set; }
        public string app_time { get; set; }
        public string appoint_count { get; set; }
        public string tel { get; set; }
        public string rn { get; set; }
    }
}
