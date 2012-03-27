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
    /// <summary>Class that reads the header and indexes data chunks</summary>
    public class MveChunkIndexer : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of data in the MVE chunk descriptor (chunk and length)</summary>
        public const Int32 ChunkDescriptionSize = 4;
        #endregion


        #region Fields
        /// <summary>Represents the MVE header</summary>
        public MveHeader Header { get; set; }

        /// <summary>Index collection of chunks</summary>
        public List<ChunkIndex> ChunkIndexCollection { get; set; }
        #endregion


        #region Properties
        /// <summary>Value for determining whether or not to read opcodes in the chunks</summary>
        protected virtual Boolean ReadOpcodes
        {
            get { return false; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Header = new MveHeader();
            this.ChunkIndexCollection = new List<ChunkIndex>();
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
                this.Header = new MveHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            this.Header.Read(input);
            this.ReadChunkIndex(input);
        }

        /// <summary>This public method reads chunk index data from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        protected virtual void ReadChunkIndex(Stream input)
        {
            while (input.Position < input.Length)
            {
                ChunkIndex chunk = new ChunkIndex(false);
                chunk.Read(input);
                this.ChunkIndexCollection.Add(chunk);
            }
        }

        /// <summary>Reads the opcodes from the input sream, after the chunk indeces have been read (as a second pass).</summary>
        /// <param name="input">Stream to read from</param>
        /// <remarks>Meant to be a threaded background read.</remarks>
        public virtual void ReadChunkOpcodes(Stream input)
        {
            for (Int32 index = 0; index < this.ChunkIndexCollection.Count; ++index)
                this.ChunkIndexCollection[index].ReadOpcodeCollection(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            throw new NotImplementedException("It does not make sense to rewrite the partial data output.");
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "MVE ToC:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Header.ToString());
            builder.Append(StringFormat.ToStringAlignment("Chunk Index"));
            builder.Append(this.GetIndexStringRepresentation());

            return builder.ToString();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetIndexStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Empty);

            for (Int32 i = 0; i < this.ChunkIndexCollection.Count; ++i)
                builder.AppendLine(this.ChunkIndexCollection[i].ToString(i));

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

                    Boolean structureEquality;
                    structureEquality = (this.Header.Equals(compare.Header));
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