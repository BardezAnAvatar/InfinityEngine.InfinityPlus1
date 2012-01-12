using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;

using Bardez.Projects.Configuration.Ini;
using Bardez.Projects.Configuration.Ini.Component;
using Bardez.Projects.Configuration.Ini.Line;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Initialization
{
    public class InitConfig : IInfinityFormat
    {
        /// <summary>Represents a read *.INI config file</summary>
        protected IniConfiguration config;

        /// <summary>Represents a read *.INI config file</summary>
        public IniConfiguration Config
        {
            get { return this.config; }
            set { this.config = value; }
        }
        
        #region Construction
        /// <summary>Default constructor</summary>
        public InitConfig()
        {
            this.config = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.config = new IniConfiguration();
        }
        #endregion

        #region IO method implemetations
        /// <summary>This public method reads file format data structure from the output stream. Reads the whole data structure.</summary>
        /// <param name="input">Stream to read from.</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            this.config.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public void Write(Stream output)
        {
            //no using since caller will collect and finalize
            StreamWriter author = new StreamWriter(output);

            foreach (IniSection section in this.config.SectionsList)
            {
                if (!String.IsNullOrEmpty(section.Declaration.Line))
                    author.WriteLine(section.Declaration.Line);

                foreach (IniLineBase entry in section.Lines)
                    author.WriteLine(entry.Line);
            }

            author.Flush();
        }
        #endregion

        #region ToString() helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = "INI Configuration:";

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();

            foreach (String key in this.config.Sections.Keys)
            {
                builder.Append(StringFormat.ReturnAndIndent(String.Concat("[", key, "]"), 0));
                Dictionary<String, IniPropertyLine> section = this.config.Sections[key];

                foreach (String property in section.Keys)
                    builder.Append(StringFormat.ReturnAndIndent(property + " = " + section[property].Value, 0));
            }

            return builder.ToString();
        }
        #endregion
    }
}