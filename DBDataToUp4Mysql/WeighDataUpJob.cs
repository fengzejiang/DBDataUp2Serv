using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DBDataToUp4Mysql
{
    /// <summary>
    /// 地磅数据上传任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class WeighDataUpJob : IJob
    {
        private const string UP_KEY = "type=210&json=";
        public DBConfigM Conf { get => conf; set => conf = value; }
        public string Url { get => url; set => url = value; }
        public string Scm { get => scm; set => scm = value; }
        public string Sbid { get => sbid; set => sbid = value; }
        public string Sopr { get => sopr; set => sopr = value; }

        JobHelperData jd = new JobHelperData();
        private DBConfigM conf;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string url;
        private string scm;
        private string sbid;
        private string sopr;
        private string rsql;
        public Task Execute(IJobExecutionContext context)
        {
            makeSql();
            UpLoadWeightData();
            return Task.Delay(2);
        }

        private void makeSql()
        {
            string sql = "select ";
            for (int i = 0; i < conf.List.Count; i++)
            {
                DBConfigItem item = conf.List[i];
                sql += item.Dbfld + " as " + item.ApiKey;
                if (i < conf.List.Count - 1)
                {
                    sql += ",";
                }
            }
            string from = " from " + conf.TbName;
            string where = " where " + conf.Timefld + ">='{0}' and " + conf.Timefld + "<'{1}'";
            rsql = sql + from + where;
        }


        private void UpLoadWeightData()
        {
            try
            {
                string bgtime = DBTools4Mysql.GetSearchBgTime(conf.Sid);
                DateTime d1 = DateTime.Now;
                int jgmin = -conf.Inter;
                d1 = d1.AddMinutes(jgmin);
                if (string.IsNullOrEmpty(bgtime))
                {
                    bgtime = conf.Bgtime;
                }
                string edtime = d1.ToString(ICL.DATE_FMT_L);
                string s1 = string.Format(rsql, bgtime, edtime);
                string log = "{0}-->开始执行任务，查询区间{1}===={2}";
                log = string.Format(log, Tools.Now(), bgtime, edtime);
                jd.ExecUpload(log);
                logger.Info("任务开始执行：" + s1);//执行sql查询
                List<JObject> list = null;
                try
                {
                    list = DBTools4Mysql.Query(s1);
                    int size = 0;
                    if (list != null && list.Count > 0)
                    {
                        List<JObject> listup = new List<JObject>();

                        foreach (JObject obj in list)
                        {
                            JToken jto = obj.GetValue("bdid");
                            if (jto == null)
                            {
                                continue;
                            }
                            if (!DBTools4Mysql.checkRecordUped(jto.ToString()))
                            {
                                size++;
                                obj.Add("scm", scm);
                                obj.Add("sbid", Sbid);
                                obj.Add("sopr", Sopr);
                                listup.Add(obj);
                            }
                            if (listup.Count >= 10)
                            {
                                string sup = JsonConvert.SerializeObject(listup);
                                logger.Info("开始执行上传：" + sup);
                                sup = Tools.EncodeBase64("UTF-8", sup);
                                sup = Tools.EscapeExprSpecialWord(sup);
                                Tools.HttpPostInfo(url + ICL.API_KEY, UP_KEY + sup);
                                logger.Info("小组执行完成：" + sup);
                                logger.Info("开始写小组日志：");
                                DBTools4Mysql.WriteSysUpLog(listup);
                                listup.Clear();
                                Thread.Sleep(5);
                            }
                        }
                        if (listup.Count > 0)
                        {
                            string sup = JsonConvert.SerializeObject(listup);
                            logger.Info("开始执行尾数上传：" + sup);
                            sup = Tools.EncodeBase64("UTF-8", sup);
                            sup = Tools.EscapeExprSpecialWord(sup);
                            Tools.HttpPostInfo(url + ICL.API_KEY, UP_KEY + sup);
                            logger.Info("尾数执行完成：" + sup);
                            logger.Info("开始写尾数日志：");
                            DBTools4Mysql.WriteSysUpLog(listup);
                            listup.Clear();
                        }
                        logger.Info(string.Format("本次执行完成,上传总条数【{0}】", size));
                    }
                    else
                    {
                        logger.Info("没有查询到数据;");
                    }
                    DBTools4Mysql.insertOrUpDate(conf.Sid, edtime);
                    jd.ExecUpload(string.Format(Tools.Now() + "-->任务执行完成【{0}】", size));
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
            }
        }
    }
}
