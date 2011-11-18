using System;
using System.Windows.Forms;

namespace Bardez.Projects.ExceptionHandler.Loggers
{
    public class MessageBoxLogger : IExceptionLogger
    {
        protected String filePath;
        protected readonly Object locker;

        /// <summary>Default constructor</summary>
        public MessageBoxLogger()
        {
            this.locker = new Object();
        }

        public void LogException(Exception Ex)
        {
            lock(this.locker)
            {
                MessageBox.Show(ExceptionInformation.GetExceptionDetails(Ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}