using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using wtPayDAL;
using wtPayModel.RegistrationModel;

namespace wtPay.FormRegistration
{
    /// <summary>
    /// FormSocialSecurity.xaml 的交互逻辑
    /// </summary>
    public partial class FormRegistrationDoctor_3 : UserControl
    {
        Dictionary<string, DoctorInfoDataDataResult_Data> buttonList = new Dictionary<string, DoctorInfoDataDataResult_Data>();
        //页码
        int pageNo = 0;
        //医生列表
        List<DoctorInfoDataDataResult_Data> doctorList = null;
        //是否正在查询
        bool isQuery = false;
        private delegate void setShowGridDelegate(FormRegistrationDoctor_3 from, string Gridname, bool isShow);
        private delegate void setButtonValueDelegate(FormRegistrationDoctor_3 form, string gridName, string lablename, string lableTitle, string imgname,string timename, string namevalue, string titlevalue, string imgurl, string timevalue);
        private delegate void setTextBlockDelegate(TextBlock tb, string value);

        private Thread queryThread = null;
        public FormRegistrationDoctor_3()
        {
            InitializeComponent();
        }
        private void setTextBlock(TextBlock tb, string value)
        {
            tb.Text = value;
        }
        private void setShowGrid(FormRegistrationDoctor_3 from, string Gridname, bool isShow)
        {
            if (isShow) ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Visible;
            else ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Hidden;
        }
        private void setButtonValue(FormRegistrationDoctor_3 form, string gridName, string lablename, string lableTitle, string imgname, string timename, string namevalue, string titlevalue, string imgurl,string timevalue)
        {
            ((Grid)(form.FindName(gridName))).Visibility = Visibility.Visible;
            ((Label)(form.FindName(lablename))).Content = namevalue;
            ((Label)(form.FindName(lableTitle))).Content = titlevalue;
            ((Image)(form.FindName(imgname))).Source = new BitmapImage(new Uri(imgurl));
            ((Label)(form.FindName(timename))).Content = timevalue;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistrationDepartment_2");
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
        private void load()
        {
            try
            {
                //清空面板
                clear();
                pageNo = 0;
                queryThread = new Thread(delegate () { query(pageNo); });
                queryThread.Start();
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:load():" + ex.Message);
            }
        }
        //查询
        private void query(int pageNo)
        {
            try
            {
                resultInfo("正在查询，请稍后...");
                isQuery = true;
                //清空面板
                clear();
                doctorList = null;
                DoctorParam param = new DoctorParam();
                param.hospital_code = RegistrationClass.hospitalInfo.hospital_code;
                param.dept_code = RegistrationClass.departmentInfo.dept_code;
                doctorList = RegistrationAccess.DoctorQuery(param);

                ListSort("HB_DATE", "asc");

                if (doctorList.Count == 0)
                {
                    resultInfo("今日暂无预约信息，请明日再试");
                    return;
                }
                showPage(pageNo);
                resultInfo("请选择医生");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:" + ae.Message); }
            catch (Exception ex)
            {
                resultInfo("暂无预约信息");
                log.Write("error:FormRegistrationDepartment_2:query():" + ex.Message);
            }
            finally
            {
                isQuery = false;
            }
        }
        //展示页面
        private void showPage(int page)
        {
            try
            {
                if ((page * 9) >= doctorList.Count) return;
                clear();
                buttonList.Clear();
                int count = 1;
                for (int i = (page * 9); i < doctorList.Count; i++)
                {
                    setButton(this, ("button" + (count)), ("name" + (count)), ("title" + (count)), ("img" + (count)), ("time" + (count)), doctorList[i].DOCTOR_NAME, doctorList[i].MARK_TYPE, doctorList[i].PICTURE,(fillTime(doctorList[i].HB_DATE)+" "+ doctorList[i].START_TIME + "-"+ doctorList[i].END_TIME));
                    buttonList.Add(("button" + (count)), doctorList[i]);
                    if (count >= 9)
                        return;
                    count++;
                }
            }catch(Exception ex)
            {
                log.Write("error:FormRegistrationDoctor_3:showPage():"+ex.Message);
            }
        }
        private void resultInfo(String value)
        {
            showInfo.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock), showInfo, value);
        }
        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="form"></param>
        /// <param name="gridName"></param>
        /// <param name="TextBlockName"></param>
        /// <param name="value"></param>
        private void setButton(FormRegistrationDoctor_3 form, string gridName, string lablename, string lableTitle, string imgname,string timename , string namevalue, string titlevalue,string imgurl, string timevalue)
        {
            this.Dispatcher.Invoke(new setButtonValueDelegate(setButtonValue),form,  gridName,  lablename, lableTitle, imgname, timename,   namevalue,  titlevalue,imgurl,timevalue);
        }
        //清空面板
        private void clear()
        {
            try
            {
                for (int i = 1; i <= 9; i++)
                {
                    this.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), this, ("button" + i), false);
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationDepartment_2:clear():" + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pageNo++;
            showPage(pageNo);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pageNo == 0) return;
            pageNo--;
            showPage(pageNo);
        }

        private string fillTime(string time)
        {
            return time.Substring(5);
        }

        private void button1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DoctorInfoDataDataResult_Data data= buttonList[((Grid)sender).Name];
                RegistrationClass.doctorInfo = data;
                RegistrationClass.registrationAddress.doctorName = data.DOCTOR_NAME;
                RegistrationClass.registrationAddress.time = data.HB_DATE+" "+ am_pm(data.AM_PM)+" "+data.START_TIME+"-"+data.END_TIME;
                Util.JumpUtil.jumpCommonPage("FormRegistrationInput");
            }
            catch(Exception ex) { log.Write("error:FormRegistrationDoctor_3:button1_MouseLeftButtonDown:"+ex.Message); }
        }
        private string am_pm(string value)
        {
            if ("am".Equals(value.ToLower())) return "上午";
            return "下午";
        }
        private void ListSort(string field, string rule)
        {
            if (!string.IsNullOrEmpty(rule) && (!rule.ToLower().Equals("desc") || !rule.ToLower().Equals("asc")))
            {
                try
                {

                    doctorList.Sort(
                        delegate (DoctorInfoDataDataResult_Data info1, DoctorInfoDataDataResult_Data info2)
                        {
                            Type t1 = info1.GetType();
                            Type t2 = info2.GetType();
                            PropertyInfo pro1 = t1.GetProperty(field);
                            PropertyInfo pro2 = t2.GetProperty(field);
                            return rule.ToLower().Equals("asc") ?
                                pro1.GetValue(info1, null).ToString().CompareTo(pro2.GetValue(info2, null).ToString()) :
                                pro2.GetValue(info2, null).ToString().CompareTo(pro1.GetValue(info1, null).ToString());
                        });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("ruls is wrong");
    }
    }
}
