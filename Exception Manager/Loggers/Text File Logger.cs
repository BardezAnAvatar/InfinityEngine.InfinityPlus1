using System;
using System.IO;
using System.Text;
using Bardez.Projects.Configuration;
using Bardez.Projects.ExceptionHandler;

namespace Bardez.Projects.ExceptionHandler.Loggers
{
    public class TextFileLogger : IExceptionLogger
    {
        protected String filePath;

        /// <summary>Default constructor</summary>
        public TextFileLogger()
        {
            filePath = ConfigurationHandler.GetSettingValue("ExceptionManager.Logger.TextFile.FilePath");
        }

        public void LogException(Exception Ex)
        {
            using (FileStream ErrorLog = new FileStream(this.filePath, FileMode.Append, FileAccess.Write))
            {
                using (TextWriter TW = new StreamWriter(ErrorLog, UTF8Encoding.UTF8))
                {
                    TW.WriteLine("Exception at " + DateTime.Now.ToUniversalTime());
                    TW.WriteLine(SystemInfo.GetSystemInfo());
                    TW.WriteLine(ExceptionInformation.GetExceptionDetails(Ex));
                }
            }
        }
    }
}