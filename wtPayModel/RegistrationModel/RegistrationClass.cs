using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    public class RegistrationClass
    {
        /// <summary>
        /// 医院列表
        /// </summary>
        public static HospitalInfoDataRows hospitalInfo = new HospitalInfoDataRows();
        /// <summary>
        /// 科室列表
        /// </summary>
        public static DepartmentInfoDataRows departmentInfo = new DepartmentInfoDataRows();
        /// <summary>
        /// 医生列表
        /// </summary>
        public static DoctorInfoDataDataResult_Data doctorInfo = new DoctorInfoDataDataResult_Data();
        /// <summary>
        /// 预约挂号参数
        /// </summary>
        public static RegistrationParam registrationParam = null;
        /// <summary>
        /// 预约挂号AppId
        /// </summary>
        public static string appId = "YYGH005";
        /// <summary>
        /// 预约类型
        /// </summary>
        public static int RegistrationType = 0;
        /// <summary>
        /// 预约挂号返回
        /// </summary>
        public static RegistrationInfo registrationInfo = null;
        /// <summary>
        /// 撤销预约挂号参数
        /// </summary>
        public static UndoRegistrationParam undoRegistrationParam = null;
        /// <summary>
        /// 预约挂号AppId
        /// </summary>
        public static string registrationRecordQueryAppId="YYGH009";
        /// <summary>
        /// 预约记录查询参数
        /// </summary>
        public static RegistrationRecordQueryParam registrationRecordQueryParam = null;

        public static RegistrationAddress registrationAddress = null;

        /// <summary>
        /// app二维码
        /// </summary>
        public static string appImg { set; get;}
    }
    public class RegistrationAddress
    {
        public string hospitalName { get; set; }
        public string DepartmentName { get; set; }
        public string doctorName { get; set; }
        public string time { get; set; }
        public string phone { get; set; }
    }
    public class RegistrationParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string hospital_code { get; set; }
        public string tel { get; set; }
        public string operate_type { get; set; }
        public string hb_date { get; set; }
        public string card_type { get; set; }
        public string sex { get; set; }
        public string hid { get; set; }
        public string patient_name { get; set; }
        public string id_no { get; set; }
    }

    /// <summary>
    /// 预约挂号返回 
    /// </summary>
    public class RegistrationInfo
    {
        public string Result_Code { get; set; }
        public string Error_Msg { get; set; }
        public RegistrationInfoResult_Data Result_Data { get; set; }
        public RegistrationInfoRecord record { get; set;}
    }

    public class RegistrationInfoResult_Data
    {
        public string App_Time { get; set; }
        public string Appoint_No { get; set; }
        public string Appoint_Count { get; set; }
        //public string Security_Code { get; set; }
    }

    public class RegistrationInfoRecord
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
    /// <summary>
    /// 撤销预约返回结果
    /// </summary>
    public class UndoRegistration
    {
        public string Result_Code { get; set; }
        public string Error_Msg { get; set; }
    }
    /// <summary>
    /// 撤销预约参数
    /// </summary>
    public class UndoRegistrationParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string tel { get; set; }
        public string operate_type { get; set; }
        public string flow_no { get; set; }
        public string appointId { get; set; }

    }

}
