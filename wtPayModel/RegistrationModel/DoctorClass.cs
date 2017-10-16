using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtPayModel.RegistrationModel
{
    /// <summary>
    /// 医生
    /// </summary>
    public class DoctorClass
    {
        public static string DoctorAppId = "YYGH003";
    }
    /// <summary>
    /// 医生查询参数
    /// </summary>
    public class DoctorParam
    {
        public string appId { get; set; }
        public string conName { get; set; }
        public string hospital_code { get; set; }
        public string dept_code { get; set; }
        public string doctor_code { get; set; }
    }
    /// <summary>
    /// 医生列表
    /// </summary>
    public class DoctorInfoList
    {
        public List<DoctorInfoDataDataResult_Data> data { get; set; }
    }
    /// <summary>
    /// 医生列表
    /// </summary>
    public class DoctorInfo
    {
        public DoctorInfoData data { get; set; }
    }
    public class DoctorInfoData
    {
        public List<DoctorInfoDataData> data { get; set; }
    }
    public class DoctorInfoDataData
    {
        public string Result_Code { get; set; }
        public string Error_Msg { get; set; }
        public List<DoctorInfoDataDataResult_Data>  Result_Data { get; set; }
    }
    public class DoctorInfoDataDataResult_Data
    {
        public string HID { get; set; }
        public string DEPT_CODE { get; set; }
        public string DEPT_NAME { get; set; }
        public string PICTURE { get; set; }//202.100.92.38/arweb-server/service/pic?key=4028818d3a4d3244013a5964afa30cea,
        public string DOCTOR_CODE { get; set; }
        public string DOCTOR_NAME { get; set; }
        public string MARK_TYPE { get; set; }
        public string HB_DATE { get; set; }
        //public string HB_TIME { get; set; }
        public string AM_PM { get; set; }
        public string REGISTER_WAY { get; set; }
        public string FLAG_USED { get; set; }
        public string FLAG_AVAILABLE { get; set; }
        public string SUM_FEE { get; set; }
        //public string CLINIC_POSITION { get; set; }
        //public string MARK_DESC { get; set; }
        //public string CLINIC_FEE { get; set; }
        public string APPOINT_COUNT { get; set; }
        public int REG_COUNT { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string IS_TEMP_ORDER { get; set; }
        public int IS_STOP_ORDER { get; set; }
        //public string DOCTOR_INTRODUCE { get; set; }
        public string SEQ { get; set; }
    }
}
