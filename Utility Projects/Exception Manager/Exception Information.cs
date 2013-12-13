using System;
using System.Collections.Generic;
using System.Text;

namespace Bardez.Projects.ExceptionHandler
{
    /// <summary>Represents exception information</summary>
    public static class ExceptionInformation
    {
        /// <summary>Recursively builds the stack trace of a given exception.</summary>
        /// <param name="Ex">Exception object to analyze. Should be the top(outer)-most exception.</param>
        /// <returns>A String object containing the contents of the (nested) Stack Trace(s).</returns>
        private static String GetStackTrace(Exception Ex)
        {
            StringBuilder st = new StringBuilder();

            if (Ex.InnerException != null)
            {
                st.Append(GetStackTrace(Ex.InnerException));
                st.AppendLine("---Next Stack Trace---");
            }
            else
                st.AppendLine("---First Stack Trace---");

            st.Append(Ex.StackTrace);

            return st.ToString();
        }

        /// <summary>Recursively builds the stack of Exception types of a given exception.</summary>
        /// <param name="Ex">Exception object to analyze. Should be the top(outer)-most exception.</param>
        /// <returns>A String object containing the contents of the (nested) Exception type(s).</returns>
        private static String GetExceptionTypeStack(Exception Ex)
        {
            StringBuilder ets = new StringBuilder();
            
            if (Ex.InnerException != null)
            {
                ets.Append(GetExceptionTypeStack(Ex.InnerException));
                ets.AppendLine("\tinside of");
            }

            ets.Append("\t");
            ets.Append(Ex.GetType().ToString());

            return ets.ToString();
        }

        /// <summary>Recursively builds the stack of Exception types of a given exception.</summary>
        /// <param name="Ex">Exception object to analyze. Should be the top(outer)-most exception.</param>
        /// <returns>A String object containing the contents of the (nested) Exception type(s).</returns>
        private static String GetExceptionMessageStack(Exception Ex)
        {
            StringBuilder message = new StringBuilder();

            if (Ex.InnerException != null)
            {
                message.Append(GetExceptionMessageStack(Ex.InnerException));
                message.AppendLine("---Next Message---");
            }
            else
                message.AppendLine("---First Message---");

            message.Append(Ex.Message);

            return message.ToString();
        }


        public static String GetExceptionDetails(Exception Ex)
        {
            StringBuilder detailsBuilder = new StringBuilder();
            detailsBuilder.AppendLine("Exception Type(s):");
            detailsBuilder.AppendLine(GetExceptionTypeStack(Ex));
            detailsBuilder.AppendLine("Message(s):");
            detailsBuilder.AppendLine(GetExceptionMessageStack(Ex));
            detailsBuilder.AppendLine("Stack Trace(s):");
            detailsBuilder.AppendLine(GetStackTrace(Ex));

            return detailsBuilder.ToString();
        }
    }
}