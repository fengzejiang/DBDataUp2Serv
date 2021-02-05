namespace DBDataToUp4Access
{
    public class JobHelperData
    {
        /// <summary>
        /// 定义数据上传委托
        /// </summary>
        /// <param name="strMsg">发送给主窗体的消息</param>
        public delegate void ExecUploadEvent(string strMsg);

        /// <summary>
        /// 定义ExecUploadEvent委托事件
        /// </summary>
        public static event ExecUploadEvent OnExecUploadEvent;

        /// <summary>
        /// 定时任务执行方法（上传数据）
        /// </summary>
        /// <param name="note">消息</param>
        /// <returns></returns>
        public void ExecUpload(string note)
        {
            //传递消息给主窗体
            OnExecUploadEvent?.Invoke(note);
        }
    }
}
