namespace DBDataUp2LY
{
    partial class DBDataUp2LYForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBDataUp2LYForm));
            this.tab0 = new System.Windows.Forms.TabControl();
            this.tabp0 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_modify_data = new System.Windows.Forms.Button();
            this.btn_mod_conf = new System.Windows.Forms.Button();
            this.btn_test = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.pic_state = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_save_local = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_scm = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_opr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_dbid = new System.Windows.Forms.TextBox();
            this.g1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_conf_id = new System.Windows.Forms.TextBox();
            this.btn_conf = new System.Windows.Forms.Button();
            this.tb_uri = new System.Windows.Forms.TextBox();
            this.tabp1 = new System.Windows.Forms.TabPage();
            this.log_box = new System.Windows.Forms.RichTextBox();
            this.tab0.SuspendLayout();
            this.tabp0.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_state)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.g1.SuspendLayout();
            this.tabp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab0
            // 
            this.tab0.Controls.Add(this.tabp0);
            this.tab0.Controls.Add(this.tabp1);
            this.tab0.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tab0.ItemSize = new System.Drawing.Size(117, 30);
            this.tab0.Location = new System.Drawing.Point(3, 12);
            this.tab0.Margin = new System.Windows.Forms.Padding(5);
            this.tab0.Name = "tab0";
            this.tab0.SelectedIndex = 0;
            this.tab0.Size = new System.Drawing.Size(699, 424);
            this.tab0.TabIndex = 0;
            // 
            // tabp0
            // 
            this.tabp0.BackColor = System.Drawing.Color.Transparent;
            this.tabp0.Controls.Add(this.groupBox2);
            this.tabp0.Controls.Add(this.groupBox1);
            this.tabp0.Controls.Add(this.g1);
            this.tabp0.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabp0.ForeColor = System.Drawing.Color.Red;
            this.tabp0.Location = new System.Drawing.Point(4, 34);
            this.tabp0.Name = "tabp0";
            this.tabp0.Padding = new System.Windows.Forms.Padding(3);
            this.tabp0.Size = new System.Drawing.Size(691, 386);
            this.tabp0.TabIndex = 0;
            this.tabp0.Text = "配置和运行状态";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.btn_modify_data);
            this.groupBox2.Controls.Add(this.btn_mod_conf);
            this.groupBox2.Controls.Add(this.btn_test);
            this.groupBox2.Controls.Add(this.btn_stop);
            this.groupBox2.Controls.Add(this.btn_start);
            this.groupBox2.Controls.Add(this.pic_state);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox2.Location = new System.Drawing.Point(6, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(682, 102);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "运行状态:";
            // 
            // btn_modify_data
            // 
            this.btn_modify_data.ForeColor = System.Drawing.Color.Red;
            this.btn_modify_data.Location = new System.Drawing.Point(566, 39);
            this.btn_modify_data.Name = "btn_modify_data";
            this.btn_modify_data.Size = new System.Drawing.Size(108, 44);
            this.btn_modify_data.TabIndex = 13;
            this.btn_modify_data.Text = "数据更正";
            this.btn_modify_data.UseVisualStyleBackColor = true;
            this.btn_modify_data.Click += new System.EventHandler(this.Modify_data_Click);
            // 
            // btn_mod_conf
            // 
            this.btn_mod_conf.ForeColor = System.Drawing.Color.Red;
            this.btn_mod_conf.Location = new System.Drawing.Point(316, 39);
            this.btn_mod_conf.Name = "btn_mod_conf";
            this.btn_mod_conf.Size = new System.Drawing.Size(108, 44);
            this.btn_mod_conf.TabIndex = 12;
            this.btn_mod_conf.Text = "修改配置";
            this.btn_mod_conf.UseVisualStyleBackColor = true;
            this.btn_mod_conf.Click += new System.EventHandler(this.ModConf);
            // 
            // btn_test
            // 
            this.btn_test.ForeColor = System.Drawing.Color.Red;
            this.btn_test.Location = new System.Drawing.Point(441, 39);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(108, 44);
            this.btn_test.TabIndex = 11;
            this.btn_test.Text = "上传测试";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.ForeColor = System.Drawing.Color.Red;
            this.btn_stop.Location = new System.Drawing.Point(191, 39);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(108, 44);
            this.btn_stop.TabIndex = 10;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_start
            // 
            this.btn_start.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.btn_start.Location = new System.Drawing.Point(66, 39);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(108, 44);
            this.btn_start.TabIndex = 9;
            this.btn_start.Text = "启动";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // pic_state
            // 
            this.pic_state.BackColor = System.Drawing.Color.Transparent;
            this.pic_state.Image = global::DBDataUp2LY.Properties.Resources.stop;
            this.pic_state.Location = new System.Drawing.Point(11, 38);
            this.pic_state.Margin = new System.Windows.Forms.Padding(4);
            this.pic_state.Name = "pic_state";
            this.pic_state.Size = new System.Drawing.Size(48, 45);
            this.pic_state.TabIndex = 8;
            this.pic_state.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.btn_save_local);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_scm);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_opr);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_dbid);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Teal;
            this.groupBox1.Location = new System.Drawing.Point(6, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(682, 118);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "本地参数：";
            // 
            // btn_save_local
            // 
            this.btn_save_local.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save_local.ForeColor = System.Drawing.Color.Red;
            this.btn_save_local.Location = new System.Drawing.Point(368, 68);
            this.btn_save_local.Name = "btn_save_local";
            this.btn_save_local.Size = new System.Drawing.Size(292, 34);
            this.btn_save_local.TabIndex = 10;
            this.btn_save_local.Text = "保存";
            this.btn_save_local.UseVisualStyleBackColor = true;
            this.btn_save_local.Click += new System.EventHandler(this.LocalSave);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(7, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 26);
            this.label5.TabIndex = 9;
            this.label5.Text = "公司编码:";
            // 
            // tb_scm
            // 
            this.tb_scm.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_scm.Location = new System.Drawing.Point(136, 69);
            this.tb_scm.Name = "tb_scm";
            this.tb_scm.Size = new System.Drawing.Size(149, 31);
            this.tb_scm.TabIndex = 8;
            this.tb_scm.Text = "02003";
            this.tb_scm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(364, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 26);
            this.label4.TabIndex = 6;
            this.label4.Text = "操作员：";
            // 
            // tb_opr
            // 
            this.tb_opr.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_opr.Location = new System.Drawing.Point(478, 33);
            this.tb_opr.Name = "tb_opr";
            this.tb_opr.Size = new System.Drawing.Size(182, 31);
            this.tb_opr.TabIndex = 7;
            this.tb_opr.Text = "02003";
            this.tb_opr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(6, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "地磅编号：";
            // 
            // tb_dbid
            // 
            this.tb_dbid.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_dbid.Location = new System.Drawing.Point(136, 30);
            this.tb_dbid.Name = "tb_dbid";
            this.tb_dbid.Size = new System.Drawing.Size(149, 31);
            this.tb_dbid.TabIndex = 5;
            this.tb_dbid.Text = "02003";
            this.tb_dbid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // g1
            // 
            this.g1.BackColor = System.Drawing.Color.White;
            this.g1.Controls.Add(this.button1);
            this.g1.Controls.Add(this.label2);
            this.g1.Controls.Add(this.label1);
            this.g1.Controls.Add(this.tb_conf_id);
            this.g1.Controls.Add(this.btn_conf);
            this.g1.Controls.Add(this.tb_uri);
            this.g1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.g1.ForeColor = System.Drawing.Color.Teal;
            this.g1.Location = new System.Drawing.Point(3, 6);
            this.g1.Name = "g1";
            this.g1.Size = new System.Drawing.Size(685, 124);
            this.g1.TabIndex = 0;
            this.g1.TabStop = false;
            this.g1.Text = "服务器信息:";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(516, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 44);
            this.button1.TabIndex = 5;
            this.button1.Text = "测试数据库";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.TestDBLink);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(7, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "配置Id:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(7, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "服务器Uri:";
            // 
            // tb_conf_id
            // 
            this.tb_conf_id.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_conf_id.Location = new System.Drawing.Point(136, 72);
            this.tb_conf_id.Name = "tb_conf_id";
            this.tb_conf_id.Size = new System.Drawing.Size(149, 31);
            this.tb_conf_id.TabIndex = 2;
            this.tb_conf_id.Text = "TC0001";
            this.tb_conf_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_conf
            // 
            this.btn_conf.ForeColor = System.Drawing.Color.Red;
            this.btn_conf.Location = new System.Drawing.Point(329, 65);
            this.btn_conf.Name = "btn_conf";
            this.btn_conf.Size = new System.Drawing.Size(181, 44);
            this.btn_conf.TabIndex = 1;
            this.btn_conf.Text = "获取服务端配置";
            this.btn_conf.UseVisualStyleBackColor = true;
            this.btn_conf.Click += new System.EventHandler(this.GetServerConfigInfo);
            // 
            // tb_uri
            // 
            this.tb_uri.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_uri.Location = new System.Drawing.Point(136, 23);
            this.tb_uri.Name = "tb_uri";
            this.tb_uri.Size = new System.Drawing.Size(527, 31);
            this.tb_uri.TabIndex = 0;
            this.tb_uri.Text = "http://127.0.0.1:9999/jd/";
            // 
            // tabp1
            // 
            this.tabp1.Controls.Add(this.log_box);
            this.tabp1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabp1.Location = new System.Drawing.Point(4, 34);
            this.tabp1.Name = "tabp1";
            this.tabp1.Padding = new System.Windows.Forms.Padding(3);
            this.tabp1.Size = new System.Drawing.Size(691, 386);
            this.tabp1.TabIndex = 1;
            this.tabp1.Text = "运行日志";
            this.tabp1.UseVisualStyleBackColor = true;
            // 
            // log_box
            // 
            this.log_box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_box.Location = new System.Drawing.Point(3, 3);
            this.log_box.Name = "log_box";
            this.log_box.Size = new System.Drawing.Size(685, 380);
            this.log_box.TabIndex = 0;
            this.log_box.Text = "";
            // 
            // DBDataUp2LYForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 437);
            this.Controls.Add(this.tab0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DBDataUp2LYForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据定时采集工具--联溢";
            this.Load += new System.EventHandler(this.DBDataUpForm_Load);
            this.tab0.ResumeLayout(false);
            this.tabp0.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_state)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.g1.ResumeLayout(false);
            this.g1.PerformLayout();
            this.tabp1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab0;
        private System.Windows.Forms.TabPage tabp0;
        private System.Windows.Forms.TabPage tabp1;
        private System.Windows.Forms.GroupBox g1;
        private System.Windows.Forms.Button btn_conf;
        private System.Windows.Forms.TextBox tb_uri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_conf_id;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_opr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_dbid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pic_state;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_scm;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.Button btn_save_local;
        private System.Windows.Forms.RichTextBox log_box;
        private System.Windows.Forms.Button btn_mod_conf;
        private System.Windows.Forms.Button btn_modify_data;
    }
}

