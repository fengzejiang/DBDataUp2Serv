using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataToUp4Access
{
    public class ToolMoveFiles
    {
        public static string CopyFile(string form_path, string toPath)//, string dosLine
        {

            Process proc = new Process();
            //string cmd = ($"xcopy {form_path} {toPath} /y /e /i /q"); //xcopy \\10.122.55.4\websites\test E:\demo\test\ /D /E /Y /K
            string cmd = ($"xcopy {form_path} {toPath} /s  /e /Y"); //xcopy \\10.122.55.4\websites\test E:\demo\test\ /D /E /Y /K
            try
            {
                return cmd + " == " + Docopy(proc, cmd);
            }
            catch (Exception ex)
            {
                cleanConnect();
                // connect(dosLine);
                Docopy(proc, cmd);

                return ex.Message;
            }
            //   return cmd;
        }
        //cmd命令拷贝 xcopy
        public static string Docopy(Process proc, string cmd)
        {
            proc.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;

            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;//true表示不显示黑框，false表示显示dos界面
            // proc.StartInfo.Arguments = $" {cmd} ";// redirect ? @"/c " + "\"" + url  +"\"" : @"/k " + "\"" + url + "\"";
            proc.Start();
            proc.StandardInput.WriteLine(cmd);// (@"net use \\172.25.138.150User@123 /user:administrator");//xcopy \\eahis\netlogon\bmp c:\bmp /e/y
            proc.StandardInput.WriteLine("exit");
            while (!proc.HasExited)
            {
                proc.WaitForExit(1000);
            }
            string errormsg = proc.StandardError.ReadToEnd();
            proc.StandardError.Close();
            if (string.IsNullOrEmpty(errormsg))
            {
                // Flag = true;
            }
            else
            {
                throw new Exception(errormsg);
            }

            proc.Close();
            return "";
        }
        public static bool cleanConnect()
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                // string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(" net use * /del /y");
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    // throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
    }
}
