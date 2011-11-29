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
        Insignificant,
        Output
    }

    /// <summary>Instance of an item being logged</summary>
    public class LogItem
    {
        /// <summary>Type of itme being logged</summary>
        public LogType Type { get; set; }

        /// <summary>Message to log</summary>
        public String Message { get; set; }

        /// <summary>Message to log</summary>
        public String Description { get; set; }

        /// <summary>Relative text context to the message.</summary>
        public String MessageContext { get; set; }

        /// <summary>Code object context of the message. Can be null.</summary>
        public Object SendingContext { get; set; }

        /// <summary>Definition constructor</summary>
        /// <param name="type">Type/level of item being logged</param>
        /// <param name="message">Message being logged</param>
        /// <param name="description">Short description of the message being logged being logged</param>
        /// <param name="messageContext">String describing the context of the message</param>
        /// <param name="codeContext">Object reference for context of the message</param>
        public LogItem(LogType type, String message, String description, String messageContext, Object codeContext)
        {
            this.Type = type;
            this.Message = message;
            this.Description = description;
            this.MessageContext = messageContext;
            this.SendingContext = codeContext;
        }
    }
}