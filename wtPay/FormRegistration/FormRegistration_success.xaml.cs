using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wtPayBLL;
using wtPayCommon;
using wtPayModel.BroadCas;
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormBroadCasStep06_success.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistration_success : UserControl
    {
        public FormRegistration_success()
        {
            InitializeComponent();
        }
        bool isPrint = true;
        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            if (isPrint) print();// bool isPrint = true;
            isPrint = false;
        }

        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpMainPage();
        }
        //load事件
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                showinfo.Text = SysConfigHelper.readerNode("RegistrationSucces");
                isPrint = true;
                //姓名
                name.Text = RegistrationClass.registrationParam.patient_name;
                //放号编码
                flow_no.Text = RegistrationClass.registrationInfo.record.appiont_id;
                //预约编号
                appointId.Text = RegistrationClass.registrationAddress.phone;

                hospitalName.Text = RegistrationClass.registrationAddress.hospitalName;
                DepartmentName.Text = RegistrationClass.registrationAddress.DepartmentName;
                doctorName.Text = RegistrationClass.registrationAddress.doctorName;
                timeName.Text = RegistrationClass.registrationAddress.time;
            }
            catch(Exception ex)
            {
                log.Write("error:FormRegistration_success:load():" + ex.Message);
            }
        }
        private void print()
        {
            try {
                StringBuilder outMsg = new StringBuilder();
                Print.TT_OpenDevice(new StringBuilder("COM" + SysConfigHelper.readerNode("PrintPort")), new StringBuilder("38400"), outMsg);
                Print.TT_GetDeviceStatus(outMsg);
                // Print.TT_PrintText(new StringBuilder("三维终端 快捷支付 便利生活\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("  兰州三维便民服务终端交易凭条  \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("--------------------------------"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder("姓名:" + RegistrationClass.registrationParam.patient_name + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("放号编码:" + RegistrationClass.registrationInfo.record.flow_no + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("预约编号:" + RegistrationClass.registrationInfo.record.appiont_id + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("预约时间:" + RegistrationClass.registrationInfo.record.hb_date + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("医院名称:" + RegistrationClass.registrationAddress.hospitalName + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("科室名称:" + RegistrationClass.registrationAddress.DepartmentName + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("医生姓名:" + RegistrationClass.registrationAddress.doctorName + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("时间:" + RegistrationClass.registrationAddress.time + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("来源:" +"兰州市三维便民服务终端" + "\n"), outMsg);
                //Print.TT_PrintText(new StringBuilder("时间:" + SysBLL.getTimeFormat() + "\n"), outMsg);
                Print.TT_PrintText(new StringBuilder("备注：\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);

                Print.TT_PrintText(new StringBuilder("-----------持卡人存根--------\n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_PrintText(new StringBuilder(" \n"), outMsg);
                Print.TT_CutPaper(1, outMsg);
                Print.TT_CloseDevice(outMsg);
            }
            catch(Exception ex)
            {
                log.Write("error:FormRegistration_success:print():" + ex.Message);
            }
        }
    }
}
