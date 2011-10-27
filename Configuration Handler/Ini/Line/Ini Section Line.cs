using System;

namespace Bardez.Projects.Configuration.Ini.Line
{
    /// <summary>Section of an INI File</summary>
    public class IniSectionLine : IniLineBase
    {
        /// <summary>Exposure of the Section name</summary>
        public String Name
        {
            get
            {
                String name = null;

                if (this.line != null)
                {
                    name = this.line.Trim();

                    Int32 open = name.IndexOf("["), close = name.IndexOf("]");

                    if (open < close && open > -1 && (close-open) > 1)
                        name = name.Substring(open + 1, (close - open) -1);
                }

                return name;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                    this.line = String.Empty;
                else
                    this.line = "[" + value + "]";
            }
        }

        /// <summary>Default constructor</summary>
        public IniSectionLine()
        {
        }

        /// <summary>Default constructor</summary>
        public IniSectionLine(String line)
        {
            this.line = line;
        }

        /// <summary>Default constructor</summary>
        public IniSectionLine(String line, String name)
        {
            this.line = line;
            this.Name = name;
        }
    }
}