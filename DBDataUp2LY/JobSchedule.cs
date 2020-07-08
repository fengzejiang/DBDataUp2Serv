using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace DBDataUp2LY
{
    public class JobSchedule
    {
        private System.Timers.Timer timer;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public string url;
        private DBConfigM configM;
        private string rsql;
        public delegate void UpdateMainLog(string msg);
        public UpdateMainLog updateTabsLogs;
        public JobSchedule() { }
        public bool canRun = true;
        private string cURR_DBID;
        public string cURR_SCM;
        private string cURR_OPR;

        public DBConfigM ConfigM { get => configM; set => configM = value; }

        public JobSchedule(string url,DBConfigM configM) {
            this.url = url;
            this.configM = configM;
            makeSql();
            initTimer();
        }

        public JobSchedule(string url, DBConfigM configM, string cURR_DBID, string cURR_SCM, string cURR_OPR) : this(url, configM)
        {
            this.cURR_DBID = cURR_DBID;
            this.cURR_SCM = cURR_SCM;
            this.cURR_OPR = cURR_OPR;
        }

        private void makeSql()
        {
            string sql = "select ";
            for (int i = 0; i < configM.List.Count; i++) {
                DBConfigItem item = configM.List[i];
                sql += item.Dbfld+" as "+item.ApiKey;
                if (i < configM.List.Count - 1) {
                    sql += ",";
                }
            }
            string from = " from " + configM.TbName+ICL.STR_TB_BAK;
            string where = " where " + configM.Timefld + ">='{0}' and " + configM.Timefld + "<='{1}'";
            rsql = sql + from + where;
        }

        private void initTimer() {
        }

        public void Start() {
            if (timer == null)
            {
                timer = new System.Timers.Timer();
            }
            timer.Interval = 60000 * configM.Inter;//执行间隔时间,单位为毫秒;此时时间间隔为1分钟  
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(UpLoadWeightData);
            timer.Start();

        }

        public void Stop()
        {
            if (timer != null&&timer.Enabled)
            {
                timer.Stop();
                timer.Enabled = false;
                timer.Elapsed -= new System.Timers.ElapsedEventHandler(UpLoadWeightData);
            }
                
        }

        private void UpLoadWeightData(object sender, ElapsedEventArgs e)
        {
            if (canRun) {
                canRun = false;
                try
                {
                    string bgtime = DBTools.GetSearchBgTime(configM.Sid);
                    DateTime d1 = DateTime.Now;
                    int jgmin = -configM.Inter;
                    d1 = d1.AddMinutes(jgmin);
                    if (string.IsNullOrEmpty(bgtime)) {
                        bgtime = configM.Bgtime;
                    }
                    string edtime = d1.ToString(ICL.DATE_FMT_L);
                    //开始同步数据
                    SyncDataToBakTable(edtime);
                    //同步数据结束
                    string s1 = string.Format(rsql, bgtime, edtime);
                    string log = "{0}-->开始执行任务，查询区间{1}===={2}";
                    log = string.Format(log, Tools.Now(), bgtime, edtime);
                    updateTabsLogs(log);
                    logger.Info("任务开始执行：" + s1);//执行sql查询
                    List<JObject> list = null;
                    try
                    {
                        list = DBTools.Query(s1);
                        int size = 0;
                        if (list != null && list.Count > 0)
                        {
                            List<DBParams> listup = new List<DBParams>();
                            foreach (JObject obj in list)
                            {
                                DBParams param = JsonConvert.DeserializeObject<DBParams>(obj.ToString());
                                if (!DBTools.checkRecordUped(param.Bdid))
                                {
                                    size++;
                                    if (string.IsNullOrEmpty(param.Sbid)) {
                                        param.Sbid = cURR_DBID;
                                    }
                                    if (string.IsNullOrEmpty(param.Scm)) {
                                        param.Scm = cURR_SCM;
                                    }
                                    if (string.IsNullOrEmpty(param.Sopr)) {
                                        param.Sopr = cURR_OPR;
                                    }
                                    listup.Add(param);
                                }
                                if (listup.Count >= 10)
                                {
                                    string sup = JsonConvert.SerializeObject(listup);
                                    logger.Info("开始执行上传：" + sup);
                                    sup = Tools.EncodeBase64("UTF-8", sup);
                                    sup = Tools.EscapeExprSpecialWord(sup);
                                    Tools.HttpPostInfo(url + "wmdatas2", "type=210&json=" + sup);
                                    logger.Info("小组执行完成：" + sup);
                                    logger.Info("开始写小组日志：");
                                    DBTools.WriteSysUpLog(listup);
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
                                Tools.HttpPostInfo(url + "wmdatas2", "type=210&json=" + sup);
                                logger.Info("尾数执行完成：" + sup);
                                logger.Info("开始写尾数日志：");
                                DBTools.WriteSysUpLog(listup);
                                listup.Clear();
                            }
                            logger.Info(string.Format("本次执行完成,上传总条数【{0}】",size));
                        }
                        else
                        {
                            logger.Info("没有查询到数据;");
                        }
                        DBTools.insertOrUpDate(configM.Sid, edtime);
                        updateTabsLogs(string.Format(Tools.Now() + "-->任务执行完成【{0}】",size));
                    }
                    catch (Exception ex)
                    {
                        logger.Error("错误SQL：" + s1);
                        logger.Error(ex, "执行查询出错");
                        updateTabsLogs(Tools.Now() + "-->任务执行报错：" + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "上传出错！");
                }
                finally {
                    canRun = true;
                }
            }
             
        }
        /// <summary>
        /// 同步数据到备份表
        /// </summary>
        public void SyncDataToBakTable(string edtime)
        {
            //获取备份表
            string tbName = configM.TbName;
            string bkName = tbName + ICL.STR_TB_BAK;
            string timefld = configM.Timefld;
            string pkfld = configM.getTablePkFld();
            string cont = timefld + "<='" + edtime + "'"; ;
            string bgtime = DBTools.GetMaxBakBgTime(bkName,timefld);
            if (!string.IsNullOrEmpty(bgtime)) {
                cont += " and " + timefld + ">='" +bgtime +"'";
            }
            cont += " and isnull(" + timefld + ",'')<>''";
            string fld = configM.getDBFlds();
            DBTools.WriteRecordToBakTable(bkName, tbName, cont, pkfld,fld);
        }
    }
}
