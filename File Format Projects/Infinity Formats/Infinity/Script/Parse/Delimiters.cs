using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Script.Parse
{
    /// <summary>Contains the delimiters of the scripts</summary>
    public static class Delimiters
    {
        #region Constants
        /// <summary>Constant indicating a script block delimiter</summary>
        public static readonly String ScriptDelimiter = "SC";

        /// <summary>The linefeed delimiter character for scripts</summary>
        public static readonly Char LineFeedDelimiter = '\n';

        /// <summary>The response block delimiter</summary>
        public static readonly String ResponseDelimiter = "RE";

        /// <summary>The action block delimiter</summary>
        public static readonly String ActionDelimiter = "AC";

        /// <summary>The condition block delimiter</summary>
        public static readonly String ConditionDelimiter = "CO";

        /// <summary>The Condition-response block delimiter</summary>
        public static readonly String ConditionResponseDelimiter = "CR";

        /// <summary>The object block delimiter</summary>
        public static readonly String ObjectDelimiter = "OB";

        /// <summary>The response set block delimiter</summary>
        public static readonly String ResponseSetDelimiter = "RS";

        /// <summary>The trigger delimiter</summary>
        public static readonly String TriggerDelimiter = "TR";
        #endregion
    }
}