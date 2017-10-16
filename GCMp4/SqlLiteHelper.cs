using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCMp4
{
    public class SqlLiteHelper
    {
        #region 连接字符串

        /// <summary>

        /// 连接数据库字符串

        /// </summary>

        /// <returns></returns>

        public static String getSQLiteConn()

        {

            return  System.AppDomain.CurrentDomain.BaseDirectory + "\\wtPay.db"; //获取绝对路径，看好了，别搞错了

        }

        #endregion
        /// <summary>
        /// 查询配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<ConfigClass> query(string name)
        {
            try
            {
                List<ConfigClass> list = new List<ConfigClass>();
                SQLiteConnection conn = new SQLiteConnection();
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = SqlLiteHelper.getSQLiteConn();
                connstr.Password = "123";//设置密码，SQLite ADO.NET实现了数据库密码保护  
                conn.ConnectionString = connstr.ToString();
                SQLiteCommand cmd = new SQLiteCommand();//是不是很熟悉呢？

                DateTime StartComputerTime = DateTime.Now;

                cmd.Connection = conn;

                if (name == null)
                {
                    cmd.CommandText = "select * from t_config order by id asc";
                }
                else
                {
                    cmd.CommandText = "select * from t_config where name='" + name + "' order by id asc";
                }
                conn.Close();
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ConfigClass config = new ConfigClass();
                    config.Id = reader["id"].ToString();
                    config.Name = reader["name"].ToString();
                    config.FormalValue = reader["formalValue"].ToString();
                    config.TestValue = reader["testValue"].ToString();
                    config.Remark = reader["remark"].ToString();
                    list.Add(config);
                }
                conn.Close();
                return list;
            }catch(Exception ex) { return new List<ConfigClass>(); }
        }
        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool update(ConfigClass config)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection();
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = SqlLiteHelper.getSQLiteConn();
                connstr.Password = "123";//设置密码，SQLite ADO.NET实现了数据库密码保护  
                conn.ConnectionString = connstr.ToString();
                SQLiteCommand comm = new SQLiteCommand(conn);
                comm.CommandText = "update t_config set testValue='" + config.FormalValue + "',formalValue='" + config.FormalValue + "' where name='" + config.Name + "'";

                conn.Open();
                int result = comm.ExecuteNonQuery();
                if (result > 0) return true;
                conn.Close();
                return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
