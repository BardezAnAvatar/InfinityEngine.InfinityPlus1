using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TalkOverride
{
    /// <summary>Represents a TOT file</summary>
    public class TalkOverrideTable : IInfinityFormat
    {
        #region Fields
        /// <summary>Collection to TOT entries</summary>
        public List<TalkOverrideTableEntry> Entries { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Entries = new List<TalkOverrideTableEntry>();
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
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            while (input.Position != input.Length)
            {
                TalkOverrideTableEntry entry = new TalkOverrideTableEntry();
                entry.Read(input);
                this.Entries.Add(entry);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            foreach (TalkOverrideTableEntry entry in this.Entries)
                entry.Write(output);
        }
        #endregion


        #region ToString() Helpers
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Talk Override Table entry:");

            foreach (TalkOverrideTableEntry entry in this.Entries)
                builder.AppendLine(entry.ToString());
            
            return builder.ToString();
        }
        #endregion
    }
}