using System;
using System.Text;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Simple structure that defines a chunk and the offset to that chunk in memory</summary>
    public class RiffSubChunk
    {
        #region Fields
        /// <summary>Offset from start of file to position within data containing the sub-chunk</summary>
        public Int64 Offset { get; set; }

        /// <summary>Riff chunk contained </summary>
        public IRiffChunk Chunk { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constuctor</summary>
        /// <param name="position">Offset within stream to start reading at</param>
        public RiffSubChunk(Int64 position)
        {
            this.Offset = position;
        }
        #endregion


        #region ToString() method(s)
        /// <summary>This method overrides the default ToString() method, printing a human-readable representation</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            this.WriteString(builder);

            return builder.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public void WriteString(StringBuilder builder)
        {
            StringFormat.ToStringAlignment("Type", builder);
            builder.Append("Sub-chunk");
            StringFormat.ToStringAlignment("Offset", builder);
            builder.Append(this.Offset);
            StringFormat.ToStringAlignment("Chunk", builder);
            StringFormat.IndentAllLines(this.Chunk.ToString(), 1, builder);
        }
        #endregion
    }
}