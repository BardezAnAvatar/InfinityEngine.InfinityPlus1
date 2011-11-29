using System;

namespace Bardez.Projects.InfinityPlus1.Test
{
    /// <summary>Event Arguments containing a message to post</summary>
    public class MessageEventArgs : EventArgs
    {
        #region Fields
        /// <summary>Message to be posted to the event</summary>
        public String Message { get; set; }

        /// <summary>Short description of the message</summary>
        public String Description { get; set; }

        /// <summary>Context of the message being passed along (filename, etc.)</summary>
        public String Context { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public MessageEventArgs()
        {
            this.Message = null;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="message">message to send</param>
        public MessageEventArgs(String message)
        {
            this.Message = message;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="message">message to send</param>
        /// <param name="description">Short description of the message to send</param>
        public MessageEventArgs(String message, String description)
        {
            this.Message = message;
            this.Description = description;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="message">message to send</param>
        /// <param name="description">Short description of the message to send</param>
        /// <param name="context">Relatively short description of the context of the message to send</param>
        public MessageEventArgs(String message, String description, String context)
        {
            this.Message = message;
            this.Description = description;
            this.Context = context;
        }
        #endregion
    }
}