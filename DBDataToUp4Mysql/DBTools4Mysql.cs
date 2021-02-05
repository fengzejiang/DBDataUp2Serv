using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace DBDataToUp4Mysql
{
    public class DBTools4Mysql
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly string LOG_TABLE = "up_sys_logs_wt_mysql";
        public static readonly string CONF_TABLE = "up_sys_config_wt_mysql";
        public static readonly string Create_Log_sql = "CREATE TABLE " + LOG_TABLE + "(id varchar(30) not NULL,remark text(8000) NULL,up_time datetime NULL,primary key (id)) ";
        public static readonly string Create_Config_sql = "CREATE TABLE " + CONF_TABLE + "(id varchar(50) NOT NULL,	bgtime varchar(20) NULL,primary key (id)) ";
        public static string DBLink = "";
        public static List<JObject> Query(string sql)
        {
            MySqlConnection connection = new MySqlConnection(DBLink);
            List<JObject> list = null;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds, "table");
                DataTable datas = ds.Tables["table"];
                //SqlDataReader dr = cmd.ExecuteReader();
                if (datas.Rows.Count > 0)
                {
                    list = new List<JObject>();
                    for (int i = 0; i < datas.Rows.Count; i++)
                    {
                        DataRow row = datas.Rows[i];
                        JObject jObject = new JObject();
                        foreach (DataColumn item in row.Table.Columns)
                        {
                            string fld = item.ColumnName;
                            object objv = row[fld];
                            if (item.DataType.FullName.Equals("System.Boolean"))
                            {
                                if (row.IsNull(fld))
                                {
                                    jObject.Add(item.ColumnName.ToLower(), 0);
                                }
                                else
                                {
                                    jObject.Add(item.ColumnName.ToLower(), bool.Parse(objv.ToString()) ? 1 : 0);
                                }
                            }
                            else
                            {
                                if (row.IsNull(fld))
                                {
                                    jObject.Add(item.ColumnName.ToLower(), "");
                                }
                                else
                                {
                                    jObject.Add(item.ColumnName.ToLower(), objv == null ? "" : objv + "");
                                }

                            }

                        }
                        list.Add(jObject);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("查询出错：", ex);
                return list;
            }
            finally
            {
                try
                {
                    connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }
            }

        }

        /// <summary>
        /// 检测并创建日志表
        /// </summary>
        /// <returns></returns>
        public static bool checkLogTable()
        {
            bool bok = false;
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();

            }
            catch (Exception e)
            {
                Trace.TraceWarning("创建Access表出错", e);
                bok = false;
            }
            string sql = "select * from " + LOG_TABLE;
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            try
            {
                cmd.ExecuteScalar();
                bok = true;
            }
            catch (System.Exception ex)
            {
                Trace.TraceWarning("表不存在：", ex);
                //创建配置表
                logger.Error(ex, "查询日志表失败");
                logger.Info("开始创建日志表：" + Create_Log_sql);
                cmd.CommandText = Create_Log_sql;
                int k = cmd.ExecuteNonQuery();
                logger.Info("完成创建日志表;");
                bok = true;
            }
            finally
            {
                try
                {
                    connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }

            }
            return bok;
        }

        /// <summary>
        /// 检查记录是否上传过
        /// </summary>
        /// <param name="pkid">地磅记录号</param>
        /// <returns>true:上传过;false:没上传</returns>
        public static bool checkRecordUped(string pkid)
        {
            bool buped = false;
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();
                string sql = "select count(*) from " + LOG_TABLE + " where id='" + pkid + "'";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "获取开始查询时间出错！");
            }
            finally
            {
                try
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }

            }

            return buped;
        }

        public static void WriteSysUpLog(List<JObject> list)
        {
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                string sql = "insert into " + LOG_TABLE + "(id,remark,up_time) values('{0}','{1}','{2}');";
                foreach (JObject p in list)
                {
                    string sql0 = string.Format(sql, p.GetValue("bdid").ToString(), JsonConvert.SerializeObject(p), DateTime.Now);
                    cmd.CommandText = sql0;
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "写sys_log错误");
            }
            finally
            {
                try
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }
            }

        }


        /// <summary>
        /// 修改配置表最新开始查询时间
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="bgtime"></param>
        public static void insertOrUpDate(string sid, string bgtime)
        {
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();
                string sql = "select count(*) from " + CONF_TABLE + " where id='" + sid + "'";
                if (connection.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count != 0)
                    {
                        sql = "update " + CONF_TABLE + " set bgtime='" + bgtime + "' where id='" + sid + "'";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        logger.Info("更新取数开始时间：" + sql);
                    }
                    else
                    {
                        sql = "insert into " + CONF_TABLE + "(id,bgtime) values('" + sid + "','" + bgtime + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        logger.Info("更新取数开始时间：" + sql);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "获取开始查询时间出错！");
            }

        }

        public static string GetSearchBgTime(string id)
        {
            string bgtime = "";
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();
                string sql = "select bgtime from " + CONF_TABLE + " where id='" + id + "' limit 1";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd != null)
                {
                    while (rd.Read())
                    {
                        if (!(rd[0] is DBNull))
                        {
                            bgtime = rd.GetString(0);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "获取开始查询时间出错！");
            }
            finally
            {
                try
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }

            }

            return bgtime;
        }

        public static bool checkConfigTable()
        {
            bool bok = false;
            MySqlConnection connection = new MySqlConnection(DBLink);
            try
            {
                connection.Open();
            }
            catch (System.Exception e)
            {
                Trace.TraceWarning("创建Access表出错", e);
                return false;
            }
            string sql = "select * from " + CONF_TABLE;
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            try
            {
                cmd.ExecuteScalar();
                bok = true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning("表不存在：", ex);
                //创建配置表
                logger.Error(ex, "查询日志表失败");
                logger.Info("开始创建日志表：" + Create_Config_sql);
                cmd.CommandText = Create_Config_sql;
                int k = cmd.ExecuteNonQuery();
                logger.Info("完成创建日志表");
                bok = true;
            }
            finally
            {
                try
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "关闭连接出错！");
                }

            }
            return bok;
        }
    }
}
