using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Opcodes
{
    /// <summary>Base class for opcode parameters and data</summary>
    public abstract class OpcodeData : IInfinityFormat
    {
        #region Fields
        /// <summary>Offset representing the data after parameters</summary>
        public Int64 OffsetData { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize() { }
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
        /// <remarks>The reader must read an opcode prior to reading the body, and the opcode is the only common datum to all opcode subchunks</remarks>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();
            this.ReadParameters(input);
            this.ReacordDataOffset(input);
        }

        /// <summary>This public method reads opcode parameter data, but not other binary data.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadParameters(Stream input) { }

        /// <summary>This public method reads the data offset from the input stream's position, or sets to -1 if not applicable.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReacordDataOffset(Stream input)
        {
            this.OffsetData = -1L;
        }

        /// <summary>This public method reads opcode binary data, but not stored data.</summary>
        /// <param name="input">Stream to read from</param>
        public virtual void ReadData(Stream input) { }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public abstract void Write(Stream output);
        #endregion


        #region ToString() methods
        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected virtual String GetStringRepresentation()
        {
            return String.Empty;
        }
        #endregion
    }
}