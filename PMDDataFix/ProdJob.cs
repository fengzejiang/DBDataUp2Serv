using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PMDDataFix
{
    public class ProdJob
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string UP_KEY = "type=210&json=";
        public string url;
        private DBConfigM configM;
        private string rsql;
        public ProdJob() { }
        public string cURR_DBID;
        public string cURR_SCM;
        public string cURR_OPR;
        public string bgtime;
        public string edtime;
        JobHelperData jd = new JobHelperData();

        public void setConfigM(DBConfigM value)
        {
            configM = value;
            makeSql();
        }

        public ProdJob(string url,DBConfigM configM) {
            this.url = url;
            this.configM = configM;
            makeSql();
        }

        public ProdJob(string url, DBConfigM configM, string cURR_DBID, string cURR_SCM, string cURR_OPR) : this(url, configM)
        {
            this.cURR_DBID = cURR_DBID;
            this.cURR_SCM = cURR_SCM;
            this.cURR_OPR = cURR_OPR;
        }


        private string makeSql()
        {
            string sql = "select ";
            for (int i = 0; i < configM.List.Count; i++)
            {
                DBConfigItem item = configM.List[i];
                sql += item.Dbfld + " as " + item.ApiKey;
                if (i < configM.List.Count - 1)
                {
                    sql += ",";
                }
            }
            string from = " from " + configM.TbName;
            string where = " where " + configM.Timefld + ">='{0}' and " + configM.Timefld + "<'{1}'";
            rsql = sql + from + where;
            return rsql;
        }


        public void Start() {
            Thread th = new Thread(UpLoadProData);
            th.Start();
            //UpLoadProData();
        }

        private void UpLoadProData()
        {
            try
            {
                string s1 = string.Format(rsql, bgtime, edtime);
                string log = "{0}-->开始执行任务，查询区间{1}===={2}";
                log = string.Format(log, Tools.Now(), bgtime, edtime);
                    
                logger.Info("任务开始执行：" + s1);//执行sql查询
                jd.ExecUpload(log);
                List<JObject> list = null;
                try
                {
                    list = DBTools.Query(s1);
                    int size = 0;
                    if (list != null && list.Count > 0)
                    {
                    List<JObject> listup = new List<JObject>();
                    foreach (JObject obj in list)
                    {
                        JToken jto = obj.GetValue("prodno");
                        if (!DBTools.checkRecordUped(jto.ToString()))
                        {
                            size++;
                            obj.Add("scm", cURR_SCM);
                            obj.Add("mconfigid", configM.Sid);
                            listup.Add(obj);
                        }
                        if (listup.Count >= 10)
                        {
                            string sup = JsonConvert.SerializeObject(listup);
                            logger.Info("开始执行上传：" + sup);
                            jd.ExecUpload(Tools.Now()+"小组开始执行上传：" +listup.Count);
                            sup = Tools.EncodeBase64("UTF-8", sup);
                            sup = Tools.EscapeExprSpecialWord(sup);
                            string url1 = @"http://8.129.40.31:8081/bip-erp/";
                            Tools.HttpPostInfo(url1 + ICL.API_KEY, UP_KEY + sup);
                            jd.ExecUpload(Tools.Now()+"==>小组完成上传：" + listup.Count);
                            logger.Info("小组执行完成：" + sup);
                            logger.Info("开始写小组日志：");
                            DBTools.WriteSysUpLog(listup);
                            jd.ExecUpload(Tools.Now()+" 写日志表");
                            listup.Clear();
                        }
                    }
                    if (listup.Count > 0)
                    {
                        string sup = JsonConvert.SerializeObject(listup);
                        logger.Info("开始执行尾数上传：" + sup);
                        sup = Tools.EncodeBase64("UTF-8", sup);
                        sup = Tools.EscapeExprSpecialWord(sup);
                        string url1 = @"http://8.129.40.31:8081/bip-erp/";
                        Tools.HttpPostInfo(url1 + ICL.API_KEY, UP_KEY + sup);
                        logger.Info("尾数执行完成：" + sup);
                        logger.Info("开始写尾数日志：");
                        DBTools.WriteSysUpLog(listup);
                        listup.Clear();
                    }
                    logger.Info(string.Format(Tools.Now()+"本次执行完成,上传总条数【{0}】",size));
                    }
                    else
                    {
                        logger.Info("没有查询到数据;");
                    }
                    jd.ExecUpload(string.Format(Tools.Now() + "-->任务执行完成【{0}】",size));
                }
                catch (Exception ex)
                {
                    logger.Error("错误SQL：" + s1);
                    logger.Error(ex, "执行查询出错");
                    jd.ExecUpload(Tools.Now() + "-->任务执行报错：" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "上传出错！");
                jd.ExecUpload(ex.Message);
            }
             
        }
    }
}
