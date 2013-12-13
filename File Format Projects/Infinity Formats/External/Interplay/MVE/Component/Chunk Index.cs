using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component
{
    /// <summary>Represents an index to a chunk of data in an MVE object</summary>
    /// <remarks>Should have some code to fetch the code chunks from </remarks>
    public class ChunkIndex : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of data in the MVE chunk descriptor (chunk and length)</summary>
        public const Int32 ChunkDescriptionSize = 4;

        /// <summary>Represents the size of an opcode in an MVE chunk</summary>
        public const Int32 OpcodeSize = 4;
        #endregion


        #region Fields
        /// <summary>The type of chunk being indexed</summary>
        public ChunkType Chunk { get; set; }

        /// <summary>The size of the chunk's data</summary>
        public UInt16 Size { get; set; }

        /// <summary>Position within the file stream of the chunk</summary>
        public Int64 DataOffset { get; set; }

        /// <summary>Flag for determining whether or not to read opcodes in the chunks</summary>
        public Boolean ReadOpcodes { get; set; }

        /// <summary>Collection of opcodes</summary>
        public List<Opcode> Opcodes { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Opcodes = new List<Opcode>();
        }

        /// <summary>Definition constructor</summary>
        /// <param name="readOpCodes">Flag indicating whether to read Op codes</param>
        public ChunkIndex(Boolean readOpCodes)
        {
            this.ReadOpcodes = readOpCodes;
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] data = ReusableIO.BinaryRead(input, ChunkIndex.ChunkDescriptionSize);

            this.Size = ReusableIO.ReadUInt16FromArray(data, 0);
            this.Chunk = (ChunkType)ReusableIO.ReadUInt16FromArray(data, 2);
            this.DataOffset = (this.Size < 1) ? -1 : input.Position;    //-1 if no data, otherwise stream position

            if (!this.ReadOpcodes)
                ReusableIO.SeekIfAble(input, this.Size, SeekOrigin.Current);
            else
                this.ReadOpcodeCollection(input);
        }

        /// <summary>Reads the chunk's opcodes and indexes their data.</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="termPosition">Posiion at which to terminate reading</param>
        protected virtual void ReadChunkOpcodes(Stream input, Int64 termPosition)
        {
            while (input.Position < termPosition)
            {
                Opcode opcode = new Opcode();
                opcode.Read(input);
                this.Opcodes.Add(opcode);

                //skip non-parameter data elements
                ReusableIO.SeekIfAble(input, opcode.OpcodeDataOffset + opcode.Length); //opcode start does not include length
            }
        }

        /// <summary>Reads the chunk's opcodes and their data from the input stream</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadOpcodeCollection(Stream input)
        {
            if (this.DataOffset > -1)
            {
                ReusableIO.SeekIfAble(input, this.DataOffset);

                Int64 endPosition = input.Position + this.Size;
                this.ReadChunkOpcodes(input, endPosition);

                if (input.Position != endPosition)
                    ReusableIO.SeekIfAble(input, endPosition);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream((UInt16)this.Chunk, output);
            ReusableIO.WriteUInt16ToStream(this.Size, output);

            //do not write or seek; when writing this to disk, data is expected to follow
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

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public virtual String ToString(Int32 index)
        {
            String header = this.GetVersionString(index);
            header += this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected virtual String GetVersionString()
        {
            return "MVE Chunk:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Chunk # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Chunk Type"));
            builder.Append(this.Chunk.GetDescription());
            builder.Append(StringFormat.ToStringAlignment("Chunk Value"));
            builder.Append((UInt16)this.Chunk);
            builder.Append(StringFormat.ToStringAlignment("Size"));
            builder.Append(this.Size);
            builder.Append(StringFormat.ToStringAlignment("Offset"));
            builder.Append(this.DataOffset);
            builder.Append(this.GetOpcodeStringRepresentation());

            return builder.ToString();
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetOpcodeStringRepresentation()
        {
            String representation = String.Empty;

            if (this.ReadOpcodes)
            {
                StringBuilder builder = new StringBuilder();

                for (Int32 index = 0; index < this.Opcodes.Count; ++index)
                    builder.Append(StringFormat.ToStringAlignment(this.Opcodes[index].ToString(index)));

                representation = builder.ToString();
            }

            return representation;
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
                if (obj != null && obj is ChunkIndex)
                {
                    ChunkIndex compare = obj as ChunkIndex;

                    Boolean structureEquality;
                    structureEquality = (this.Chunk == compare.Chunk);
                    structureEquality &= (this.Size == compare.Size);
                    structureEquality &= (this.DataOffset == compare.DataOffset);

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
            Int32 hash = this.Chunk.GetHashCode();
            hash ^= this.Size;
            hash ^= this.DataOffset.GetHashCode();

            return hash;
        }
        #endregion
    }
}