using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LXDBDataFix
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.xls)|*.xls";
            bool blx = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                tb_file.Text = file;
                try
                {
                    DataTable dataDt = ReadExcelToTable(file);
                    if (blx)
                    {
                        upToLXServer(dataDt);
                    }
                    else {
                        upToServerAccess(dataDt);
                    }
                    MessageBox.Show("OK");
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void upToServerAccess(DataTable dataDt) {
            int rowCount = dataDt.Rows.Count;
            for (int index = 1; index < rowCount; index++)
            {
                DataRow row = dataDt.Rows[index];
                string bdid = row[0].ToString();
                string plateno = row[1].ToString();
                string gdic = row[2].ToString();
                string allweight = row[3].ToString();
                string weightleave = row[4].ToString();
                string weightnet = row[5].ToString();
                string cdic = row[7].ToString();
                string timeweight = row[9].ToString();
                string timeleave = row[10].ToString();
                string scm = row[11].ToString();
                string sbid = row[12].ToString();
                string sopr = row[13].ToString();
                string qtyqr = row[14].ToString();
                string sdic = row[15].ToString();
                string typestr = row[16].ToString();
                string clientid = row[8].ToString();
                int type = 1;
                if (typestr.IndexOf("采购") > -1)
                {
                    type = 0;
                }

                JObject job = new JObject();
                job.Add("bdid", bdid);
                string s1 = "bdid=" + bdid;
                job.Add("plateno", plateno);
                s1 += "&plateno=" + plateno;
                job.Add("clientid", clientid);
                s1 += "&clientid=" + clientid;
                job.Add("gdic", gdic);
                s1 += "&gdic=" + gdic;
                job.Add("allweight", allweight);
                s1 += "&allweight=" + allweight;
                job.Add("weightleave", weightleave);
                s1 += "&weightleave=" + weightleave;
                job.Add("weightnet", weightnet);
                s1 += "&weightnet=" + weightnet;
                job.Add("scm", scm);
                s1 += "&scm=" + scm;
                job.Add("cdic", cdic);
                s1 += "&cdic=" + cdic;
                job.Add("timeleave", timeleave);
                s1 += "&timeleave=" + timeleave;
                job.Add("timeweight", timeweight);
                s1 += "&timeweight=" + timeweight;
                job.Add("qtyqr", qtyqr);
                s1 += "&qtyqr=" + qtyqr;
                job.Add("sdic", sdic);
                s1 += "&sdic=" + sdic;
                job.Add("type", type);
                s1 += "&type=" + type;
                job.Add("sbid", sbid);
                s1 += "&sbid=" + sbid;
                job.Add("sopr", sopr);
                s1 += "&sopr=" + sopr;
                string str = job.ToString();
                logger.Info(str);
                string sup = JsonConvert.SerializeObject(job);
                sup = Tools.EncodeBase64("UTF-8", sup);
                sup = Tools.EscapeExprSpecialWord(sup);
                Console.WriteLine(str);
                string res = Tools.HttpPostInfo(tb_uri.Text, "type=200&json=" + sup);
                Console.WriteLine(res);
                Thread.Sleep(5);
            }
        }

        private void upToLXServer(DataTable dataDt)
        {
            int rowCount = dataDt.Rows.Count;
            for (int index = 1; index < rowCount; index++)
            {
                DataRow row = dataDt.Rows[index];
                string bdid = row[0].ToString();
                string plateno = row[1].ToString();
                string gdic = row[2].ToString();
                string allweight = row[3].ToString();
                string weightleave = row[4].ToString();
                string weightnet = row[5].ToString();
                string cdic = row[7].ToString();
                string timeweight = row[9].ToString();
                string timeleave = row[10].ToString();
                string scm = row[11].ToString();
                string sbid = row[12].ToString();
                string sopr = row[13].ToString();
                string qtyqr = row[14].ToString();
                string sdic = row[15].ToString();
                string typestr = row[16].ToString();
                string clientid = row[8].ToString();
                int type = 1;
                if (typestr.IndexOf("采购") > -1)
                {
                    type = 0;
                }
                JObject job = new JObject();
                job.Add("bdid", bdid);
                string s1 = "bdid=" + bdid;
                job.Add("plateno", plateno);
                s1 += "&plateno=" + plateno;
                job.Add("clientid", clientid);
                s1 += "&clientid=" + clientid;
                job.Add("gdic", gdic);
                s1 += "&gdic=" + gdic;
                job.Add("allweight", allweight);
                s1 += "&allweight=" + allweight;
                job.Add("weightleave", weightleave);
                s1 += "&weightleave=" + weightleave;
                job.Add("weightnet", weightnet);
                s1 += "&weightnet=" + weightnet;
                job.Add("scm", scm);
                s1 += "&scm=" + scm;
                job.Add("cdic", cdic);
                s1 += "&cdic=" + cdic;
                job.Add("timeleave", timeleave);
                s1 += "&timeleave=" + timeleave;
                job.Add("timeweight", timeweight);
                s1 += "&timeweight=" + timeweight;
                job.Add("qtyqr", qtyqr);
                s1 += "&qtyqr=" + qtyqr;
                job.Add("sdic", sdic);
                s1 += "&sdic=" + sdic;
                job.Add("type", type);
                s1 += "&type=" + type;
                job.Add("sbid", sbid);
                s1 += "&sbid=" + sbid;
                job.Add("sopr", sopr);
                s1 += "&sopr=" + sopr;
                string str = job.ToString();
                logger.Info(str);
                //string sup = JsonConvert.SerializeObject(job);
                //sup = Tools.EncodeBase64("UTF-8", sup);
                //sup = Tools.EscapeExprSpecialWord(sup);
                //Console.WriteLine(str);
                string res = Tools.HttpPostInfo(tb_uri.Text, s1);
                //Console.WriteLine(res);
                Thread.Sleep(5);
            }
        }

        ///<summary>
        ///读取xls\xlsx格式的Excel文件的方法
        ///</ummary>
        ///<param name="path">待读取Excel的全路径</param>
        ///<returns></returns>
        private DataTable ReadExcelToTable(string path)
        {
            //连接字符串
            //string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; // Office 07及以上版本 不能出现多余的空格 而且分号注意
            string connstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; //Office 07以下版本 因为本人用Office2010 所以没有用到这个连接字符串 可根据自己的情况选择 或者程序判断要用哪一个连接字符串
            using (OleDbConnection conn = new OleDbConnection(connstring))
            {
                conn.Open();
                DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                string sql = string.Format("SELECT * FROM [{0}]",firstSheetName); //查询字符串
                OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                DataSet set = new DataSet();
                ada.Fill(set);
                return set.Tables[0];

            }
        }
    }
}
