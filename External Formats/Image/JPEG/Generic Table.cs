using System;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents a base class for JPEG tables, be they Arithmetic, Huffman or Quantization</summary>
    public abstract class GenericTable
    {
        #region Fields
        /// <summary>Specifies one of four possible destinations at the decoder into which the quantization table shall be installed.</summary>
        public Byte TableDestinationIndex { get; set; }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public abstract void Read(Stream input);

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public abstract void Write(Stream output);
        #endregion
    }
}