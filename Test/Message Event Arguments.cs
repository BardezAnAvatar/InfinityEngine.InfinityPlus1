using System;

namespace Bardez.Projects.InfinityPlus1.Test
{
    /// <summary>Event Arguments containing a message to post</summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>Message to be posted to the event</summary>
        public String Message { get; set; }

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
    }
}