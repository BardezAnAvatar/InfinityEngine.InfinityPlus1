using System;

using Bardez.Projects.Configuration.Ini.Component;

namespace Bardez.Projects.Configuration.Ini.Line
{
    /// <summary>Base class for any INI file item</summary>
    public abstract class IniLineBase
    {
        /// <summary>Raw data of the INI file line</summary>
        protected String line;

        /// <summary>Raw data of the INI file line</summary>
        public String Line
        {
            get { return this.line; }
            set { this.line = value; }
        }
    }
}