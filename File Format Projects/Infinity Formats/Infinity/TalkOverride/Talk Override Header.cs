using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Represents a TOH file</summary>
    public class TalkOverrideHeader : IInfinityFormat
    {
        #region Fields
        /// <summary>Represents the header to the TOH file</summary>
        public TalkOverrideHeaderHeader Header { get; set; }

        /// <summary></summary>
        public List<TalkOverrideHeaderEntry> Entries { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new TalkOverrideHeaderHeader();
            this.Entries = new List<TalkOverrideHeaderEntry>();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new TalkOverrideHeaderHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);

            for (Int32 index = 0; index < this.Header.StrrefCount; ++index)
            {
                TalkOverrideHeaderEntry entry = new TalkOverrideHeaderEntry();
                entry.Read(input);
                this.Entries.Add(entry);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            this.Header.Write(output);

            foreach (TalkOverrideHeaderEntry entry in this.Entries)
                entry.Write(output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Talk Override Header:");
            builder.AppendLine(this.Header.ToString());

            foreach (TalkOverrideHeaderEntry entry in this.Entries)
                builder.AppendLine(entry.ToString());
            
            return builder.ToString();
        }
        #endregion
    }
}