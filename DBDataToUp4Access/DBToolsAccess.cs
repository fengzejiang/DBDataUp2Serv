
using ADOX;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace DBDataToUp4Access
{
    class DBToolsAccess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly string LOG_TABLE = "up_sys_logs_pdm";
        public static readonly string LOG_TABLE_OLD = "up_sys_logs";
        public static readonly string CONF_TABLE = "up_sys_config_pdm";
        public static readonly string CONF_TABLE_OLD = "up_sys_config";

        public static string DBLink = "";
        public static List<JObject> Query(string sql) {
            logger.Info("查询sql"+sql);
            OleDbConnection connection = new OleDbConnection(DBLink);
            List<JObject> list = null;
            try {
                connection.Open();
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                OleDbDataAdapter sda = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds, "table");
                DataTable datas = ds.Tables["table"];
                //SqlDataReader dr = cmd.ExecuteReader();
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
                logger.Error(ex, "查询出错：");
                //Trace.TraceWarning("查询出错：", ex);
                return list;
            }
            finally {
                try
                {
                    connection.Close();
                }
                catch (Exception ex) {
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
            OleDbConnection connection = new OleDbConnection(DBLink);
            try
            {
                connection.Open();

            }
            catch (Exception e)
            {
                logger.Error(e, "创建Access表出错：");
                //Trace.TraceWarning("创建Access表出错", e);
                bok =  false;
            }
            try
            {
                string sql = "select * from " + LOG_TABLE;
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                cmd.ExecuteScalar();
                bok = true;
            }
            catch (System.Exception e)
            {
                //Trace.TraceWarning("表不存在：", e);
                logger.Error(e, "表不存在：");
                //创建配置表
                bok = CreateLogTable();
            }
            finally {
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
        public static bool checkRecordUped(string pkid) {
            bool buped = false;
            OleDbConnection connection = new OleDbConnection(DBLink);
            try
            {
                connection.Open();
                string sql = "select count(*) from " + LOG_TABLE + " where id='" + pkid + "'";
                OleDbCommand cmd = new OleDbCommand(sql,connection);
                OleDbDataReader dr1 = cmd.ExecuteReader();
                int ttCnt = 0;
                while (dr1.Read())
                {
                    ttCnt = (int)dr1["Count"];
                }
                buped = ttCnt > 0;
            }
            catch (Exception ex) {
                logger.Error(ex, "获取开始查询时间出错！");
            }
            finally
            {
                try
                {
                    if(connection.State == ConnectionState.Open)
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
            OleDbConnection connection = new OleDbConnection(DBLink);
            string sql0 = "";
            try
            {
                connection.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                string sql = "insert into " + LOG_TABLE + "(id,remark,up_time) values('{0}','{1}','{2}');";
                foreach (JObject p in list)
                {
                    sql0 = string.Format(sql, p.GetValue("bdid").ToString(), JsonConvert.SerializeObject(p), DateTime.Now);
                    cmd.CommandText = sql0;
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "写日志表"+ LOG_TABLE + "错误"+sql0);
            }
            finally {
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
            OleDbConnection connection = new OleDbConnection(DBLink);
            try {
                connection.Open();
                string sql = "select count(*) from " + CONF_TABLE + " where id='" + sid + "'";
                if (connection.State == ConnectionState.Open)
                {
                    OleDbCommand cmd = new OleDbCommand();
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
            catch (Exception ex) {
                logger.Error(ex, "获取开始查询时间出错！");
            }
            
        }

        public static string GetSearchBgTime(string id) {
            string bgtime = "";
            OleDbConnection connection = new OleDbConnection(DBLink);
            try
            {
                connection.Open();
                string sql = "select top 1 bgtime from " + CONF_TABLE + " where id='" + id + "'";
                logger.Info(sql);
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                OleDbDataReader rd = cmd.ExecuteReader();
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
            finally {
                try
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                catch (Exception ex) {
                    logger.Error(ex, "关闭连接出错！");
                }
                
            }
            return bgtime;
        }

        public static bool checkConfigTable()
        {
            bool bok = false;
            OleDbConnection connection = new OleDbConnection(DBLink);
            try {
                connection.Open();
            }
            catch (System.Exception e) {
                logger.Error("创建Access表出错", e);
                return false;
            }
            try {
                string sql = "select * from " + CONF_TABLE;
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                cmd.ExecuteScalar();
                bok = true;
            }
            catch (System.Exception e) {
                logger.Error("表不存在：", e);
                //创建配置表
                bok = CreateConfTable();
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

        //在指定的Access数据库中创建配置的表格
        public static bool CreateConfTable()
        {
            Catalog catalog = new Catalog();
            ADODB.Connection cn = new ADODB.Connection();
            try {
                cn.Open(DBLink);
                catalog.ActiveConnection = cn;
                Table table = new Table();
                table.ParentCatalog = catalog;
                table.Name = CONF_TABLE;
                //字段
                Column col = new Column();
                col.ParentCatalog = catalog;
                col.Name = "id";
                table.Columns.Append(col, DataTypeEnum.adVarWChar, 50); //默认数据类型和字段大小
                table.Keys.Append("pk_id", KeyTypeEnum.adKeyPrimary, col, null, null);
                Column col2 = new Column();
                col2.ParentCatalog = catalog;
                col2.Name = "bgtime";
                col2.Attributes = ColumnAttributesEnum.adColNullable; //允许空值
                table.Columns.Append(col2, DataTypeEnum.adDBTime); //默认数据类型和字段大小
                catalog.Tables.Append(table);
                cn.Close();
                logger.Info("完成创建" + CONF_TABLE + "表");
            }
            catch (System.Exception ex)
            {
                //Trace.TraceWarning("Access连接打开失败", ex);
                logger.Error(ex, "创建" + CONF_TABLE + "表失败");
                return false;
            }

            return true;
        }

        //在指定的Access数据库中创建日志的表格
        public static bool CreateLogTable()
        {
            Catalog catalog = new Catalog();
            ADODB.Connection cn = new ADODB.Connection();
            try
            {
                cn.Open(DBLink);
                catalog.ActiveConnection = cn;
                Table table = new Table();
                table.ParentCatalog = catalog;
                table.Name = LOG_TABLE;
                //字段 remark nvarchar(MAX) NULL,up_time datetime
                Column col = new Column();
                col.ParentCatalog = catalog;
                col.Name = "id";
                table.Columns.Append(col, DataTypeEnum.adVarWChar, 30); //默认数据类型和字段大小
                table.Keys.Append("log_pk_id", KeyTypeEnum.adKeyPrimary, col, null, null);

                Column col3 = new Column();
                col3.ParentCatalog = catalog;
                col3.Type = DataTypeEnum.adLongVarWChar;//设置Memo备注字段
                col3.Name = "remark";
                col3.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
                //col3.Attributes = ColumnAttributesEnum.adColNullable; //允许空值
                table.Columns.Append(col3, DataTypeEnum.adLongVarWChar, 16); //默认数据类型和字段大小

                Column col2 = new Column();
                col2.ParentCatalog = catalog;
                col2.Name = "up_time";
                col2.Attributes = ColumnAttributesEnum.adColNullable; //允许空值
                table.Columns.Append(col2, DataTypeEnum.adDBTime); //默认数据类型和字段大小
                catalog.Tables.Append(table);
                cn.Close();
                logger.Info("完成创建" + LOG_TABLE + "表");
            }
            catch (System.Exception ex)
            {
                //Trace.TraceWarning("Access连接打开失败", ex);
                logger.Error(ex, "创建" + LOG_TABLE + "表失败");
                return false;
            }

            return true;
        }

    }
}
