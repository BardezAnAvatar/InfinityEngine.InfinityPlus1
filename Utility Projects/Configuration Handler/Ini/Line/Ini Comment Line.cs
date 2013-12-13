using System;

namespace Bardez.Projects.Configuration.Ini.Line
{
    /// <summary>A blank line or a comment</summary>
    public class IniCommentLine : IniLineBase
    {
        /// <summary>Exposure of a meaningless *.ini line</summary>
        public String Contents
        {
            get
            {
                String comment = null;

                if (this.line != null)
                {
                    comment = this.line.Trim();

                    if (comment.StartsWith(";"))
                        comment = comment.Substring(1, comment.Length - 2);
                }

                return comment;
            }

            set
            {
                if (value == null)
                    this.line = String.Empty;
                else
                    this.line = ";" + value;
            }
        }

        /// <summary>Default constructor</summary>
        public IniCommentLine()
        {
        }

        /// <summary>Default constructor</summary>
        public IniCommentLine(String line)
        {
            this.line = line;
        }
    }
}