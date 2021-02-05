using DBDataUp2LY.Properties;
using Newtonsoft.Json;
using NLog;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DBDataUp2LY
{
    public partial class DBDataUp2LYForm : Form
    {
        public static readonly string ini = Application.StartupPath + @"\initFile.ini";//ini文件
        static IniFiles iniFile = new IniFiles(ini);//初始化文件监听类
        private string  CURR_DBConf = "";
        private string CURR_URI = "";//服务器地址
        private string CURR_DBID = "";//地磅编号
        private string CURR_SCM = "";//公司编号
        private string CURR_OPR = "";//操作员
        private Logger logger = LogManager.GetCurrentClassLogger();
        private DBConfigM configM;
        private JobSchedule schedule;

        private delegate void updateLogBox(string strshow);

        public DBDataUp2LYForm()
        {
            InitializeComponent();
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
                string res = Tools.HttpPostInfo(url + "wmdatas2", queryparam);
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
                        DBTools.DBLink = CURR_DBConf;
                        bool bok = checkConfDB();
                        if (bok)
                        {
                            DBTools.insertOrUpDate(configM.Sid, configM.Bgtime);
                        }
                        checkBakDB();
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
                bool bok = DBTools.checkLogTable();
            }
            catch (Exception ex) {
                logger.Error(ex,"创建日志表失败");
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBakDB()
        {
            try
            {
                string tbName = configM.TbName;
                string bkName = tbName + ICL.STR_TB_BAK;
                bool bok = DBTools.checkBakTable(tbName,bkName);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "创建日志表失败");
                MessageBox.Show(ex.Message);
            }
        }

        private bool checkConfDB()
        {
            bool bok = false;
            try
            {
                bok = DBTools.checkConfigTable();
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
                if (string.IsNullOrEmpty(DBTools.DBLink))
                    DBTools.DBLink = CURR_DBConf;
                string bgtime = DBTools.GetSearchBgTime(configM.Sid);
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
        }

        public bool CheckServer(bool bshow = false) {
            bool bok = true;
            DBParams param = new DBParams();
            param.Allweight = "90";
            param.Plateno = "冀B6326R";
            param.Clientid = "11111";
            param.Bdid = "bdid11111{}";
            //List<DBParams> l0 = new List<DBParams>();
            //l0.Add(param);
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
                MessageBox.Show("请先输入地磅编号！");
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

        private void ExcuteTask()
        {
            if (!checkConfigFLDInfo())
            {
                return;
            }
            if (!CheckServer())
            {
                return;
            }
            if (schedule == null)
            {
                schedule = new JobSchedule(CURR_URI, configM, CURR_DBID, CURR_SCM, CURR_OPR);
                schedule.updateTabsLogs += WriteBoxInfo;
                //this.log_box.BeginInvoke(updateTabsLogs);

            }
            else {
                schedule.setConfigM(configM);
                schedule.url = CURR_URI;
                schedule.cURR_SCM = CURR_SCM;
            }
            schedule.Start();
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
                    DBTools.DBLink = dblink;
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

        private void WriteBoxInfo(string msg)
        {
            if (log_box.InvokeRequired)
            {
                log_box.BeginInvoke(new updateLogBox(WriteBoxInfo),msg);
            }
            else {
                if (log_box.Lines.Length > 1000) {
                    log_box.Clear();
                }
                log_box.AppendText(msg + "\r\n");
                log_box.ScrollToCaret();
            }
            GC.Collect();
        }

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
            if (schedule != null) {
                schedule.Stop();
            }
            makeUIEnable(false);
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            pic_state.Image = Resources.stop;
        }

        /// <summary>
        /// 点击按钮自动更新数据到备份表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Modify_data_Click(object sender, EventArgs e)
        {
            if (!checkConfigFLDInfo()) {
                //MessageBox.Show("请先获取配置信息！");
                return;
            }
            string tbName = configM.TbName;
            string bkName = tbName + ICL.STR_TB_BAK;
            string timefld = configM.Timefld;
            string pkfld = configM.getTablePkFld();
            string cont = timefld + "<='" + Tools.Now() + "'";
            string bgtime = DBTools.GetMaxBakBgTime(bkName, timefld);
            if (!string.IsNullOrEmpty(bgtime))
            {
                cont += " and " + timefld + ">='" + bgtime + "'";
            }
            string fld = configM.getDBFlds();
            DBTools.WriteRecordToBakTable(bkName, tbName, cont, pkfld,fld);

        }

        private void DBDataUp2LYForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.Hide();
                //this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void DBDataUp2LYForm_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
