using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE
{
    /// <summary>Class that reads the header and indexes data chunks ans well as opcodes</summary>
    public class MveChunkOpcodeIndexer : MveChunkIndexer
    {
        #region IO method implemetations
        /// <summary>This public method reads chunk index data from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadChunkIndex(Stream input)
        {
            while (input.Position < input.Length)
            {
                ChunkIndex chunk = new ChunkIndex(false);   //do a chunk indexing path
                chunk.Read(input);
                this.ChunkIndexCollection.Add(chunk);
            }
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected override String GetVersionString()
        {
            return "MVE ToC:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected override String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Header.ToString());
            builder.Append(StringFormat.ToStringAlignment("Chunk Index"));
            builder.Append(this.GetIndexStringRepresentation());

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is MveChunkIndexer)
                {
                    MveChunkIndexer compare = obj as MveChunkIndexer;

                    Boolean structureEquality = (this.Header.Equals(compare.Header));
                    structureEquality &= (this.ChunkIndexCollection.Equals<ChunkIndex>(compare.ChunkIndexCollection));

                    //offsets are unimportant when it comes to data value equivalence/equality
                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.Header.GetHashCode();
            hash ^= this.ChunkIndexCollection.GetHashCode<ChunkIndex>();

            return hash;
        }
        #endregion
    }
}