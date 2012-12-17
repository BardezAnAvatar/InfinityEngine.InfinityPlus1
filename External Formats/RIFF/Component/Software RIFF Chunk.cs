using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Represents the data within an ISFT chunk</summary>
    public class SoftwareRiffChunk : RiffChunkExtensionBase
    {
        #region Properties
        /// <summary>Exposes the name of the chunk type. Preserves spaces in the name.</summary>
        public String GeneratingSoftware
        {
            get 
            { 
                String temp = ReusableIO.ReadStringFromByteArray(this.Data, 0, "en-US", this.Size);
                return ZString.GetZString(temp);
            }
        }
        #endregion


        #region Construction
        /// <summary>Chunk constructor</summary>
        /// <param name="baseChunk">Chunk already read that contains basic details for this chunk</param>
        public SoftwareRiffChunk(IRiffChunk baseChunk) : base(baseChunk) { }
        #endregion


        #region ToString() methods
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
        public override void WriteString(StringBuilder builder)
        {
            base.WriteString(builder);
            StringFormat.ToStringAlignment("Generating Software", builder);
            builder.Append(this.GeneratingSoftware);
        }
        #endregion
    }
}