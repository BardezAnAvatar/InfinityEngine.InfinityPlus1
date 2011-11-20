using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Utility
{
    /// <summary>Enumerator describing an item level being logged</summary>
    public enum LogType
    {
        Message,
        Warning,
        Error,
        Informational,
        Event,
        Insignificant
    }

    /// <summary>Instance of an item being logged</summary>
    public class LogItem
    {
        /// <summary>Type of itme being logged</summary>
        public LogType Type { get; set; }

        /// <summary>Message to log</summary>
        public String Message { get; set; }

        /// <summary>Context of the message. Should be null, normally.</summary>
        public Object Context { get; set; }

        /// <summary>efinition constructor</summary>
        /// <param name="type">Type/level of item being logged</param>
        /// <param name="message">Message being logged</param>
        public LogItem(LogType type, String message) : this (type, message, null) { }

        /// <summary>efinition constructor</summary>
        /// <param name="type">Type/level of item being logged</param>
        /// <param name="message">Message being logged</param>
        /// <param name="context">Object reference for context of the message. Usually null</param>
        public LogItem(LogType type, String message, Object context)
        {
            this.Type = type;
            this.Message = message;
            this.Context = context;
        }
    }
}