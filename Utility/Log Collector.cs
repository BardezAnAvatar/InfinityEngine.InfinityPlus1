using System;
using System.Collections;
using System.Collections.Generic;

namespace Bardez.Projects.Utility
{
    /// <summary>Event to raise for testing a specific value</summary>
    /// <param name="sender">Object sending/raising the request</param>
    /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
    public delegate void LogMessageHandler(Object sender, LogEventArgs testArgs);

    /// <summary>Collection of LogItem messages</summary>
    public class LogCollector : IEnumerable<LogItem>, IEnumerable
    {
        #region Members
        /// <summary>Internal list of LogItems</summary>
        protected List<LogItem> logList { get; set; }
        #endregion

        #region Events
        /// <summary>Event fired when a message is added to the collection</summary>
        private event LogMessageHandler logMessage;
        
        /// <summary>Event fired when a message is added to the collection</summary>
        public event LogMessageHandler LogMessage
        {
            add { this.logMessage += value; }
            remove { this.logMessage -= value; }
        }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public LogCollector()
        {
            this.logList = new List<LogItem>();
        }
        #endregion

        #region List wrapping exposure
        /// <summary>Adds an object to th end of the collection</summary>
        /// <param name="item">Item to add</param>
        public virtual void Add(LogItem item)
        {
            this.logList.Add(item);
            this.logMessage(this, new LogEventArgs(item));
        }

        /// <summary>Exposes an enumerator for the wrapped list</summary>
        /// <returns>An enumerator for the list</returns>
        public virtual IEnumerator<LogItem> GetEnumerator()
        {
            return this.logList.GetEnumerator();
        }

        /// <summary>Exposes an enumerator for the wrapped list</summary>
        /// <returns>An enumerator for the list</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.logList.GetEnumerator();
        }
        #endregion
    }
}