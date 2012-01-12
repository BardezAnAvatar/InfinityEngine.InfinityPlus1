using System;
using System.Runtime.Serialization;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.ACM
{
    /// <summary>Strongly typed exception child of the Exception object</summary>
    public class AcmReturnException : Exception
    {
        /// <summary>Message constructor</summary>
        /// <param name="message">Message in the exception</param>
        public AcmReturnException(String message) : base(message) { }

        /// <summary>Exception constructor</summary>
        /// <param name="message">Message in the exception</param>
        /// <param name="innerException">Inner exception that was found or caught within this one</param>
        public AcmReturnException(String message, Exception innerException) : base(message, innerException) { }
    }
}