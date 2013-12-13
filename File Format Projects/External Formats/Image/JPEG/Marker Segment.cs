using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a base class for Tables, comments, restarts and application data streams</summary>
    /// <remarks>
    ///     See T.81, page 39, section B.2.4
    ///     
    ///     base class for:
    ///     * Quantization Table
    ///     * Huffman table
    ///     * Arethmentic conditioning table
    ///     * Restart interval
    ///     * comment
    ///     * application data stream
    /// </remarks>
    public abstract class MarkerSegment
    {
        #region Fields
        /// <summary>Represents the escape marker for this Marker Segment</summary>
        public UInt16 Marker { get; set; }

        /// <summary>All Marker Segment have an associated 16-bit binary length, including the 16 bytes for this marker</summary>
        public UInt16 Length { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MarkerSegment() { }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        public MarkerSegment(UInt16 marker)
        {
            this.Marker = marker;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Length of this substream</param>
        public MarkerSegment(UInt16 marker, UInt16 length)
        {
            this.Marker = marker;
            this.Length = length;
        }
        #endregion
    }
}