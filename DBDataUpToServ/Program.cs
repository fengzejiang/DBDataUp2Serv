using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DBDataUpToServ
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1) {
                MessageBox.Show("数据定时采集工具已经运行！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }
            else
                Application.Run(new DBDataUpForm());
        }
    }
}
