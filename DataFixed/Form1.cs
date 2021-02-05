using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataFixed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s1 = richTextBox1.Text;
            string uri = tb_uri.Text;
            if (string.IsNullOrEmpty(uri)) {
                MessageBox.Show("请输入地址:");
                return;
            }
            if (!string.IsNullOrEmpty(s1)) {
                s1 = "[" + s1 + "]";
                String sup = Tools.EncodeBase64("UTF-8", s1);
                sup = Tools.EscapeExprSpecialWord(sup);
                try
                {
                    string res = Tools.HttpPostInfo(uri, "type=210&json=" + sup);
                    MessageBox.Show(res);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
