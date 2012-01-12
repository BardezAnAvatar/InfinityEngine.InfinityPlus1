using System;
using System.IO;

using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents an unknown MarkerSegment.</summary>
    public class MarkerSegmentSubStream : MarkerSegment
    {
        #region Fields
        /// <summary>Represents the data of the stream, unprocessed</summary>
        Byte[] Data { get; set; }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public MarkerSegmentSubStream(UInt16 marker, UInt16 length)
        {
            this.Marker = marker;
            this.Length = length;
            this.Data = null;
        }
        #endregion


        #region IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.Data = ReusableIO.BinaryRead(input, this.Length - 2); //two bytes of length already consumed by Length itself.
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public virtual void Write(Stream output)
        {
            output.Write(this.Data, 0, this.Data.Length);
        }
        #endregion
    }
}