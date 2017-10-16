using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class FormRegistrationHospital_1 : UserControl
    {
        //页码
        int pageNo = 1;
        //医院列表
        HospitalInfo hospitalList = null;
        //是否正在查询
        bool isQuery = false;
        private delegate void setShowGridDelegate(FormRegistrationHospital_1 from, string Gridname, bool isShow);
        private delegate void setButtonValueDelegate(FormRegistrationHospital_1 form, string gridName, string TextBlockName, string value);
        private delegate void setTextBlockDelegate(TextBlock tb,string value);

        private Thread queryThread = null;
        public FormRegistrationHospital_1()
        {
            InitializeComponent();
        }
        private void setTextBlock(TextBlock tb,string value)
        {
            tb.Text = value;
        }
        private void setShowGrid(FormRegistrationHospital_1 from, string Gridname, bool isShow)
        {
            if (isShow) ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Visible;
            else ((Grid)(from.FindName(Gridname))).Visibility = Visibility.Hidden;
        }
        private void setButtonValue(FormRegistrationHospital_1 form,string gridName,string TextBlockName, string value)
        {
            ((Grid)(form.FindName(gridName))).Visibility = Visibility.Visible;
            ((TextBlock)(form.FindName(TextBlockName))).Text = value;
        }
        private void 退出_Click(object sender, RoutedEventArgs e)
        {
            Util.JumpUtil.jumpCommonPage("FormRegistration");
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
                //清空面板
                clear();
                pageNo = 1;
                queryThread = new Thread(delegate() { query(pageNo); });
                queryThread.Start();
            }
            catch(Exception ex)
            {
                log.Write("error:FormRegistrationHospital_1:load():" + ex.Message);
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
                hospitalList = null;
                HospitalParam param = new HospitalParam();
                param.pageNo = pageNo.ToString();
                param.pageSize = "12";
                hospitalList = RegistrationAccess.HospitalQuery(param);
                if ("9999".Equals(hospitalList.success))
                {
                    resultInfo(hospitalList.msg);
                    return;
                }
                if (hospitalList.data.rows.Count == 0)
                {
                    if (pageNo == 1)
                    {
                        resultInfo("今日暂无记录，请明日再查");
                    }else
                    {
                        resultInfo("未查询到医院信息");
                    }
                    if (pageNo > 2) pageNo--;
                    return;
                }
                for(int i=0;i< hospitalList.data.rows.Count;i++)
                    setButton(this, ("button"+(i+1)), ("name"+(i+1)), hospitalList.data.rows[i].hospital_name);
                resultInfo("请选择医院");
                return;
            }
            catch (ThreadAbortException ae) { log.Write("error:"+ae.Message); }
            catch(Exception ex)
            {
                resultInfo("暂无预约信息");
                log.Write("error:FormRegistrationHospital_1:query():"+ex.Message);
            }
            finally
            {
                isQuery = false;
            }
        }
        private void resultInfo(String value)
        {
            showInfo.Dispatcher.Invoke(new setTextBlockDelegate(setTextBlock),showInfo,value);
        }
        //清空面板
        private void clear()
        {
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    this.Dispatcher.Invoke(new setShowGridDelegate(setShowGrid), this, ("button"+i), false);
                }
            }catch(Exception ex)
            {
                log.Write("error:FormRegistrationHospital_1:clear():"+ex.Message);
            }
        }
        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="form"></param>
        /// <param name="gridName"></param>
        /// <param name="TextBlockName"></param>
        /// <param name="value"></param>
        private void setButton(FormRegistrationHospital_1 form, string gridName, string TextBlockName, string value)
        {
            this.Dispatcher.Invoke(new setButtonValueDelegate(setButtonValue), form, gridName, TextBlockName, value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isQuery==false)
                {
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                    pageNo += 1;
                    //查询户号线程
                    queryThread = new Thread(delegate () { query(pageNo); });
                    queryThread.Start();
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationHospital_1:Button_Click:" + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isQuery == false)
                {
                    if (pageNo == 1) return;
                    queryThread.Abort();
                    queryThread.DisableComObjectEagerCleanup();
                    queryThread = null;
                    pageNo -= 1;
                    //查询户号线程
                    queryThread = new Thread(delegate () { query(pageNo); });
                    queryThread.Start();
                }
            }
            catch (Exception ex)
            {
                log.Write("error:FormRegistrationHospital_1:Button_Click_1:" + ex.Message);
            }
        }

        private void name1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock grid = (TextBlock)sender;
                for(int i = 0; i <= hospitalList.data.rows.Count; i++)
                {
                    if (grid.Text.Equals(hospitalList.data.rows[i].hospital_name))
                    {
                        RegistrationClass.hospitalInfo = hospitalList.data.rows[i];
                        RegistrationClass.registrationAddress.hospitalName = grid.Text;
                        Util.JumpUtil.jumpCommonPage("FormRegistrationDepartment_2");
                        return;
                    }
                }
            }catch(Exception ex)
            {
                log.Write("error:" + ex.Message);
            }
        }
    }
}
 