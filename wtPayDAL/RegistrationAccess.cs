using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtPayBLL;
using wtPayModel.RegistrationModel;

namespace wtPayDAL
{
    /// <summary>
    /// 预约挂号类
    /// </summary>
    public class RegistrationAccess
    {
        /// <summary>
        /// 医院列表查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HospitalInfo HospitalQuery(HospitalParam param)
        {
            param.appId = HospitalClass.HospitalAppId;
            param.conName = "医院列表查询";
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("pageNo", param.pageNo);
            parameters.Add("pageSize", param.pageSize);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<HospitalInfo>(jsonText);
        }
        /// <summary>
        /// 科室列表查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DepartmentInfo DepartmentQuery(DepartmentParam param)
        {
            param.appId = DepartmentClass.DepartmentAppId;
            param.conName = "科室列表查询";
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("hospital_code", param.hospital_code);
            parameters.Add("pageNo", param.pageNo);
            parameters.Add("pageSize", param.pageSize);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<DepartmentInfo>(jsonText);
        }
        /// <summary>
        /// 医生列表查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<DoctorInfoDataDataResult_Data> DoctorQuery(DoctorParam param)
        {
            List<DoctorInfoDataDataResult_Data> list = new List<DoctorInfoDataDataResult_Data>();
            param.appId = DoctorClass.DoctorAppId;
            param.conName = "医生列表查询";
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("hospital_code", param.hospital_code);
            parameters.Add("dept_code", param.dept_code);
            parameters.Add("doctor_code", param.doctor_code);
            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);

            JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonText);
            JObject data1 = (JObject)JsonConvert.DeserializeObject(jsonObj["data"].ToString());
            JObject data2 = (JObject)JsonConvert.DeserializeObject(data1["data"][0].ToString());
            JArray data3 = (JArray)JsonConvert.DeserializeObject(data2["Result_Data"].ToString());
            for (int j = 0; j < data3.Count; j++)
            {
                JArray data4 = (JArray)JsonConvert.DeserializeObject(data3[j].ToString());
                for(int i = 0; i < data4.Count; i++)
                {
                    DoctorInfoDataDataResult_Data result=JsonConvert.DeserializeObject<DoctorInfoDataDataResult_Data>(data4[i].ToString());
                    if ((result.REG_COUNT>0)&&(result.IS_STOP_ORDER == 0))
                    {
                        list.Add(result);
                    }
                }
                return list;
            }
            
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return list;
        }
        /// <summary>
        /// 预约挂号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static RegistrationInfo Registration(RegistrationParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName",param.conName);
            parameters.Add("hospital_code", param.hospital_code);
            parameters.Add("tel", param.tel);
            parameters.Add("operate_type", param.operate_type);
            parameters.Add("hb_date", param.hb_date);
            parameters.Add("card_type", param.card_type);
            parameters.Add("sex", param.sex);
            parameters.Add("hid", param.hid);
            parameters.Add("patient_name", param.patient_name);
            parameters.Add("id_no", param.id_no);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);
            //string jsonText = "{\"Result_Code\":\"0000\",\"Error_Msg\":\"成功\",\"Result_Data\":{\"App_Time\":\"2017 - 03 - 30 14:03:18\",\"Appoint_No\":\"10491456\",\"Appoint_Count\":\"4\",\"Security_Code\":[]},\"record\":{\"appiont_id\":\"20170330140148\",\"hospital_code\":\"gsssy\",\"id_no\":\"620421199604264112\",\"patient_name\":\"luzhengning\",\"patient_id\":null,\"doctor_code\":\"40284bd65af5ec97015b10b8f73f2371\",\"order_ip\":\"15002653994\",\"symptom\":\"15002653994\",\"vis_card\":null,\"sex\":\"1\",\"card_type\":\"Idcard\",\"hb_date\":\"2017 - 04 - 04\",\"operate_type\":\"0\",\"flow_no\":\"10491456\",\"app_time\":\"2017 - 03 - 30 14:03:18\",\"appoint_count\":\"4\",\"tel\":\"15002653994\",\"rn\":1}}";
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<RegistrationInfo>(jsonText);
        }
        /// <summary>
        /// 预约挂号撤销
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static UndoRegistration undoRegistration(UndoRegistrationParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("tel", param.tel);
            parameters.Add("operate_type", param.operate_type);
            parameters.Add("flow_no", param.flow_no);
            parameters.Add("appointId", param.appointId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<UndoRegistration>(jsonText);
        }
        /// <summary>
        /// 预约记录查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static RegistrationRecordQueryInfo RegistrationRecordQuery(RegistrationRecordQueryParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("appId", param.appId);
            parameters.Add("conName", param.conName);
            parameters.Add("tel", param.tel);
            parameters.Add("oper_type", "0");
            parameters.Add("flow_no", param.flow_no);
            parameters.Add("appiont_id", param.appointId);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("AppointmentName"), parameters, null);
            log.Write("预约记录查询："+jsonText);
            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            return JsonConvert.DeserializeObject<RegistrationRecordQueryInfo>(jsonText);
        }
        /// <summary>
        /// 查询姓名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PropLoginNameInfo getName(PropLoginNameParam param)
        {
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("pro.mobile", param.mobile);
            parameters.Add("pro.password", param.password);

            string jsonText = HttpHelper.getHttp(SysConfigHelper.readerNode("appointLoginName"), parameters, null);

            //反序列化JSON字符串,将JSON字符串转换成LIST列表  
            PropLoginNameInfo info=JsonConvert.DeserializeObject<PropLoginNameInfo>(jsonText);
            return info;
        }
    }
}
