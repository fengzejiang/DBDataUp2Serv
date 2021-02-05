
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DBDataUpPDM
{
    public class DBTools
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly string LOG_TABLE = "up_sys_logs_pdm";
        public static readonly string LOG_TABLE_OLD = "up_sys_logs";
        public static readonly string CONF_TABLE = "up_sys_config_pdm";
        public static readonly string CONF_TABLE_OLD = "up_sys_config";
        public static readonly string Create_Log_sql = "CREATE TABLE "+LOG_TABLE+"(id [numeric](20,0) not NULL,remark nvarchar(MAX) NULL,up_time datetime NULL,primary key (id)) ";
        public static readonly string Create_Config_sql = "CREATE TABLE "+CONF_TABLE+"(id varchar(50) NOT NULL,	bgtime datetime NULL,primary key (id)) ";
        //public static readonly string Sync_Bak_sql = "insert into {0} ({4}) select {4} from {1} where {2} and isnull(F_Net,0)<>0 and {3} not in (select {3} from {0} where {2} )";
        //public static readonly string Create_Sync_sql = "select * into {0} from {1} where 1=2 ";
        
        public static string DBLink = "";
        public static List<JObject> Query(string sql) {
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;
            try {
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds, "table");
                DataTable datas = ds.Tables["table"];
                //SqlDataReader dr = cmd.ExecuteReader();
                List<JObject> list = null;
                if (datas.Rows.Count>0)
                {
                    list = new List<JObject>();
                    for (int i = 0; i < datas.Rows.Count; i++) {
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
                                else {
                                    jObject.Add(item.ColumnName.ToLower(), bool.Parse(objv.ToString()) ? 1 : 0);
                                }
                            }
                            else {
                                if (row.IsNull(fld))
                                {
                                    jObject.Add(item.ColumnName.ToLower(), "");
                                }
                                else {
                                    jObject.Add(item.ColumnName.ToLower(), objv == null ? "" : objv + "");
                                }
                                    
                            }
                            
                        }
                        list.Add(jObject);
                    }
                }
                return list;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                connection.Close();
            }

        }

        /// <summary>
        /// 检测并创建日志表
        /// </summary>
        /// <returns></returns>
        public static bool checkLogTable() {
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            bool bok = false;
            if (connection.State == ConnectionState.Open)
            {
                string sql = "select count(*) from "+LOG_TABLE;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count >= 0) {
                        connection.Close();
                        bok = true;
                    }
                }
                catch (Exception ex) {
                    logger.Error(ex,"查询日志表失败");
                    logger.Info("开始创建日志表：" + Create_Log_sql);
                    cmd.CommandText = Create_Log_sql;
                    int k = cmd.ExecuteNonQuery();
                    logger.Info("完成创建日志表;");
                    bok = true;
                }
                finally
                {
                    connection.Close();
                }
            }
            return bok;
        }

        /// <summary>
        /// 检查记录是否上传过
        /// </summary>
        /// <param name="pkid">地磅记录号</param>
        /// <returns>true:上传过;false:没上传</returns>
        public static bool checkRecordUped(string pkid) {
            bool buped = false;
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                string sql = "select count(*) from "+ LOG_TABLE + " where id="+pkid+"";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                try
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        connection.Close();
                        buped = true;
                    }
                    else
                    {
                        buped = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "查询日志表失败");
                    buped = false;
                }
                finally {
                    connection.Close();
                }
            }
             return buped;
        }

        public static void WriteSysUpLog(List<JObject> list)
        {
            Stopwatch sw = new Stopwatch();
            DataTable dt = GetTableSchema();
            SqlConnection conn = new SqlConnection(DBLink);
            try
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = LOG_TABLE;
                bulkCopy.BatchSize = dt.Rows.Count;
                conn.Open();
                sw.Start();
                foreach (JObject p in list)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = p.GetValue("prodno").ToString();
                    dr[1] = JsonConvert.SerializeObject(p);
                    dr[2] = DateTime.Now;
                    dt.Rows.Add(dr);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    bulkCopy.WriteToServer(dt);
                }
                sw.Stop();
                bulkCopy.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "写sys_log错误");
            }
            finally {
                conn.Close();
            }
            
        }


        static DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("id",typeof(string)),
                new DataColumn("remark",typeof(string)),
                new DataColumn("up_time",typeof(DateTime))});
            return dt;
        }

        /// <summary>
        /// 修改配置表最新开始查询时间
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="bgtime"></param>
        public static void insertOrUpDate(string sid, string bgtime)
        {
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            string sql = "select count(*) from "+CONF_TABLE+" where id='" + sid + "'";
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                int count =  Convert.ToInt32(cmd.ExecuteScalar());
                if (count != 0 )
                {
                    sql = "update "+CONF_TABLE+" set bgtime='" + bgtime + "' where id='" + sid + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    logger.Info("更新取数开始时间：" + sql);
                }
                else {
                    sql = "insert into "+CONF_TABLE+"(id,bgtime) values('"+sid+"','" + bgtime + "')";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    logger.Info("更新取数开始时间：" + sql);
                }
                connection.Close();
            }
        }

        public static string GetSearchBgTime(string id) {
            string bgtime = "";
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            string sql = "select top 1 bgtime from "+CONF_TABLE+" where id='" + id + "'";
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                try
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            string s1 = rd.GetDateTime(0).ToString(ICL.DATE_FMT_L);
                            bgtime = s1;
                        }
                        rd.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "查询数据查询开始时间失败！");
                 
                }
                finally {
                    connection.Close();
                }
            }
            return bgtime;
        }

        /// <summary>
        /// 获取备份表最大开始时间
        /// </summary>
        /// <param name="tbName">备份表表名</param>
        /// <param name="fld">时间字段</param>
        /// <returns></returns>
        public static string GetMaxBakBgTime(string tbName,string fld)
        {
            string bgtime = null;
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            string sql1 = "select max({0}) from {1} where isnull({0},'')<>'' ";
            string sql = string.Format(sql1, fld, tbName);
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                try
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd != null)
                    {
                        while (rd.Read())
                        {
                            if (!(rd[0] is DBNull)) {
                                bgtime = rd.GetString(0);
                            }
                        }
                        rd.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "查询数据查询开始时间失败！");

                }
                finally
                {
                    connection.Close();
                }
            }
            return bgtime;
        }



        public static bool checkConfigTable()
        {
            SqlConnection connection = new SqlConnection(DBLink);
            connection.Open();
            bool bok = false;
            if (connection.State == ConnectionState.Open)
            {
                string sql = "select * from "+CONF_TABLE;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = sql;
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr != null)
                    {
                        dr.Close();
                        connection.Close();
                        bok = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "查询日志表失败");
                    logger.Info("开始创建日志表：" + Create_Config_sql);
                    cmd.CommandText = Create_Config_sql;
                    int k = cmd.ExecuteNonQuery();
                    logger.Info("完成创建日志表");
                    bok = true;
                }
            }
            return bok;
        }

    }
}
