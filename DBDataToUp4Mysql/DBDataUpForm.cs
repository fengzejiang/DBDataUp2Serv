using DBDataToUp4Mysql.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBDataToUp4Mysql
{
    public partial class DBDataUp4MysqlForm : Form
    {

        private const string JobName = "job1";
        private const string Group = "group1";
        private const string TrigName = "trigger1";
        public static readonly string ini = Application.StartupPath + @"\initFile.ini";//ini文件
        static IniFiles iniFile = new IniFiles(ini);//初始化文件监听类
        private string  CURR_DBConf = "";
        private string CURR_URI = "";//服务器地址
        private string CURR_DBID = "";//地磅编号
        private string CURR_SCM = "";//公司编号
        private string CURR_OPR = "";//操作员
        private Logger logger = LogManager.GetCurrentClassLogger();
        private DBConfigM configM;
        //private JobSchedule schedule;

        ISchedulerFactory schedFact = new StdSchedulerFactory();//Quartz工厂
        IScheduler sched = null;
        /// <summary>
        /// 定义更新消息文本框内容委托
        /// </summary>
        /// <param name="strMsg">定时任务传递过来的消息</param>
        public delegate void UpdateMsgContentEvent(string strMsg);
        /// <summary>
        /// 定义更新消息文本框内容委托事件
        /// </summary>
        UpdateMsgContentEvent onUpdateMsgContentEvent = null;
        public DBDataUp4MysqlForm()
        {
            InitializeComponent();
            JobHelperData.OnExecUploadEvent += OnExecUploadEvent;
            onUpdateMsgContentEvent = new UpdateMsgContentEvent(UpdateTxtMsgContent);

        }

        private void GetServerConfigInfo(object sender, EventArgs e)
        {
            string url = tb_uri.Text;
            if (string.IsNullOrEmpty(url) && string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("请输入【服务器地址】");
                return;
            }
            string configId = tb_conf_id.Text;
            if (string.IsNullOrEmpty(configId) && string.IsNullOrWhiteSpace(configId)) {
                MessageBox.Show("请输入【配置Id】");
                return;
            }
            string queryparam = "type=400&sid=" + configId;
            try
            {
                string res = Tools.HttpPostInfo(url + ICL.API_KEY, queryparam);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    ResultInfo rInfo = JsonConvert.DeserializeObject<ResultInfo>(res);
                    if (rInfo.Data != null)
                    {
                        string conf = rInfo.Data["conf"];
                        configM = JsonConvert.DeserializeObject<DBConfigM>(conf);
                        string dbconf = configM.Dbconf;
                        if (string.IsNullOrWhiteSpace(dbconf))
                        {
                            MessageBox.Show("服务端没有数据库配置信息！");
                            return;
                        }
                        iniFile.IniWriteValue(ICL.DB_LAB, ICL.DB_DBLINK, configM.Dbconf);
                        iniFile.IniWriteValue(ICL.SERV_LAB, ICL.SERV_SCONF, Tools.EncodeBase64("UTF-8", JsonConvert.SerializeObject(configM)));
                        iniFile.IniWriteValue(ICL.SERV_LAB, ICL.SERV_URI, url);
                        iniFile.IniWriteValue(ICL.SERV_LAB, ICL.SERV_SID, configId);
                        CURR_URI = url;
                        CURR_DBConf = configM.Dbconf;
                        DBTools4Mysql.DBLink = CURR_DBConf;
                        bool bok = checkConfDB();
                        if (bok)
                        {
                            DBTools4Mysql.insertOrUpDate(configM.Sid, configM.Bgtime);
                        }
                        checkLogDB();

                        tb_conf_id.Enabled = false;
                        tb_uri.Enabled = false;
                    }
                    else {
                        MessageBox.Show("没有获取到配置信息");
                    }
                   
                }
                else {
                    MessageBox.Show("没有获取到配置信息");
                }
                
            }
            catch (Exception ex)
            {
                logger.Error(ex, "访问服务器失败");
                MessageBox.Show(ex.Message);
            }
        }

        private void checkLogDB() {
            try
            {
                bool bok = DBTools4Mysql.checkLogTable();
            }
            catch (Exception ex) {
                logger.Error(ex,"创建日志表失败");
                MessageBox.Show(ex.Message);
            }
        }

        private bool checkConfDB()
        {
            bool bok = false;
            try
            {
                bok = DBTools4Mysql.checkConfigTable();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "创建配置表失败");
                MessageBox.Show(ex.Message);
            }
            return bok;
        }

        private void TestDBLink(object sender, EventArgs e)
        {
            if (configM != null&&!string.IsNullOrEmpty(CURR_DBConf))
            {
                if (string.IsNullOrEmpty(DBTools4Mysql.DBLink))
                    DBTools4Mysql.DBLink = CURR_DBConf;
                string bgtime = DBTools4Mysql.GetSearchBgTime(configM.Sid);
                MessageBox.Show(string.Format("获取到任务开始查询时间{0}",bgtime));
            }
            else {
                MessageBox.Show("没有数据库配置信息！");
            }
                
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            if (CheckServer(true)) {
                MessageBox.Show("上传成功","上传测试", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            //DBParams param = new DBParams();
            //param.Allweight = "90";
            //param.Plateno = "冀B6326R";
            //param.Clientid = "11111";
            //param.Bdid = "bdid11111{}";
            //List<DBParams> l0 = new List<DBParams>();
            //l0.Add(param);
            //string s1 = JsonConvert.SerializeObject(l0);
            //s1 = Tools.EncodeBase64("UTF-8",s1);
            //Console.WriteLine(s1);
            //string url = tb_uri.Text;
            //try
            //{
            //    Tools.HttpPostInfo(url + "wmdatas2", "type=210&json="+s1);
            //    //Tools.HttpPostJsonInfo(url + "wmdatas2", json.ToString());
            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.Message);
            //}

           
        }

        public bool CheckServer(bool bshow = false) {
            bool bok = true;
            JObject param = new JObject();
            param.Add("allweight", "90");
            param.Add("plateno", "90");
            param.Add("clientid", "1111");
            param.Add("bdid", "Test90");
            string s1 = JsonConvert.SerializeObject(param);
            s1 = Tools.EncodeBase64("UTF-8", s1);
            try
            {
                string res = Tools.HttpPostInfo(CURR_URI + "wmdatas2", "type=200&json=" + s1);
                if(bshow&&res.Length>0)
                    MessageBox.Show(res);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bok = false;
            }
            return bok;
        }


        private void LocalSave(object sender, EventArgs e)
        {
            string dbid = tb_dbid.Text.Trim();
            string scm = tb_scm.Text.Trim();
            string opr = tb_opr.Text.Trim();
            if (string.IsNullOrEmpty(dbid)) {
                MessageBox.Show("地磅编号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(scm))
            {
                MessageBox.Show("公司编码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(opr))
            {
                MessageBox.Show("操作员不能为空");
                return;
            }

            iniFile.IniWriteValue(ICL.LOCL_LAB, ICL.LOCL_DBID, dbid);
            iniFile.IniWriteValue(ICL.LOCL_LAB, ICL.LOCL_SCM, scm);
            iniFile.IniWriteValue(ICL.LOCL_LAB, ICL.LOCL_OPR, opr);
            CURR_DBID = dbid;
            CURR_OPR = opr;
            CURR_SCM = scm;
            logger.Info("本地配置写入完成");
            tb_dbid.Enabled = false;
            tb_opr.Enabled = false;
            tb_scm.Enabled = false;
        }

        public bool checkConfigFLDInfo() {
            bool bok = false;
            if (string.IsNullOrEmpty(CURR_URI)) {
                MessageBox.Show("没有服务地址");
                bok = false;
                return bok;
            }
            if (string.IsNullOrEmpty(CURR_DBConf))
            {
                MessageBox.Show("请先从服务端获取配置信息！");
                bok = false;
                return bok;
            }
            if (string.IsNullOrEmpty(CURR_DBID))
            {
                MessageBox.Show("请先输入设备编号！");
                bok = false;
                return bok;
            }
            if (string.IsNullOrEmpty(CURR_OPR))
            {
                MessageBox.Show("请先输入操作员！");
                bok = false;
                return bok;
            }
            if (string.IsNullOrEmpty(CURR_SCM))
            {
                MessageBox.Show("请先输入公司编码！");
                bok = false;
                return bok;
            }
            bok = true;
            return bok;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            ExcuteTask();

        }

        private async void ExcuteTask()
        {
            if (!checkConfigFLDInfo())
            {
                return;
            }
            if (!CheckServer())
            {
                return;
            }

            sched = await schedFact.GetScheduler();
            await sched.Start();
            JobDataMap map = new JobDataMap();
            map.Add("conf", configM);
            map.Add("url", CURR_URI);
            map.Add("scm", CURR_SCM);
            map.Add("dbid", CURR_DBID);
            map.Add("opr", CURR_OPR);
            IJobDetail job = JobBuilder.Create<WeighDataUpJob>()
                    .WithIdentity(JobName, Group)
                    .SetJobData(map)
                    .Build();
            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(TrigName, Group)
                .WithCronSchedule(configM.CornStr)
                .Build();

            // 5.使用trigger规划执行任务job
            await sched.ScheduleJob(job, trigger);
            pic_state.Image = Resources.work;
            makeUIEnable(false);
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
        }

        private void MakeCircle()
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pic_state.ClientRectangle);
            Region region = new Region(gp);
            pic_state.Region = region;
            gp.Dispose();
            region.Dispose();
        }

        private void DBDataUpForm_Load(object sender, EventArgs e)
        {
            MakeCircle();
            if (iniFile.ExistINIFile()) {
                string uri = iniFile.IniReadValue(ICL.SERV_LAB, ICL.SERV_URI);
                tb_uri.Text = uri+"";
                CURR_URI = uri;
                string sid = iniFile.IniReadValue(ICL.SERV_LAB, ICL.SERV_SID);
                tb_conf_id.Text = sid;
                string conf = iniFile.IniReadValue(ICL.SERV_LAB, ICL.SERV_SCONF);
                if (!string.IsNullOrEmpty(conf)) {
                    try {
                        conf = Tools.DecodeBase64("UTF-8", conf);
                        configM = JsonConvert.DeserializeObject<DBConfigM>(conf);
                    }
                    catch (Exception ex) {
                        logger.Error(ex, "加载ini文件读取conf错误！！");
                        MessageBox.Show("非法串改配置文件数据");
                    }

                }
                
                string dblink = iniFile.IniReadValue(ICL.DB_LAB, ICL.DB_DBLINK);
                if (!string.IsNullOrEmpty(dblink)) {
                    DBTools4Mysql.DBLink = dblink;
                    CURR_DBConf = dblink;
                }
                string opr = iniFile.IniReadValue(ICL.LOCL_LAB, ICL.LOCL_OPR);
                if (!string.IsNullOrEmpty(opr)) {
                    tb_opr.Text = opr;
                    CURR_OPR = opr;
                }
                string scm = iniFile.IniReadValue(ICL.LOCL_LAB, ICL.LOCL_SCM);
                if (!string.IsNullOrEmpty(scm))
                {
                    tb_scm.Text = scm;
                    CURR_SCM = scm;
                }
                string dbid = iniFile.IniReadValue(ICL.LOCL_LAB, ICL.LOCL_DBID);
                if (!string.IsNullOrEmpty(dbid))
                {
                    tb_dbid.Text = dbid;
                    CURR_DBID = dbid;
                }
                this.Hide();
                notifyIcon1.Visible = true;
                ExcuteTask();
            }
        }

        #region 定义改变消息文本框委托及委托事件

        public void OnExecUploadEvent(string strMsg)
        {
            BeginInvoke(onUpdateMsgContentEvent, strMsg);
        }

        /// <summary>
        /// 更新消息文本框的内容
        /// </summary>
        /// <param name="strMsg">定时任务传递过来的消息</param>
        private void UpdateTxtMsgContent(string strMsg)
        {
            log_box.AppendText(strMsg + Environment.NewLine);

            //将滚动条定位到底部
            log_box.ScrollToCaret();

        }

        #endregion

        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModConf(object sender, EventArgs e)
        {
            if (btn_start.Enabled)
            {
                makeUIEnable(true);
            }
            else {
                MessageBox.Show("请先停止程序运行");
            }
        }

        private void makeUIEnable(bool enable) {
            tb_uri.Enabled = enable;
            tb_conf_id.Enabled = enable;
            btn_conf.Enabled = enable;
            tb_scm.Enabled = enable;
            tb_opr.Enabled = enable;
            tb_dbid.Enabled = enable;
            btn_save_local.Enabled = enable;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            StopTask();
        }

        private async void StopTask()
        {
            try
            {
                sched = await schedFact.GetScheduler();
                await sched.PauseAll();
                await sched.Clear();
                await sched.Shutdown();
                makeUIEnable(false);
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                pic_state.Image = Resources.stop;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //激活窗体并给予它焦点
                this.Show();
                WindowState = FormWindowState.Normal;
                this.Activate();
                //任务栏区显示图标
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void DBDataUp4MysqlForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.Hide();
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void DBDataUp4MysqlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出程序？\n确定则退出程序，取消则最小化至托盘", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
