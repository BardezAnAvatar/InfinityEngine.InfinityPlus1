using System;

namespace Bardez.Projects.Utility
{
    /// <summary>Event Arguments containing a log message to post</summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>Message to be posted to the event</summary>
        public LogItem LogMessage { get; set; }

        /// <summary>Default constructor</summary>
        public LogEventArgs()
        {
            this.LogMessage = null;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="message">message to log</param>
        public LogEventArgs(LogItem message)
        {
            this.LogMessage = message;
        }
    }
}