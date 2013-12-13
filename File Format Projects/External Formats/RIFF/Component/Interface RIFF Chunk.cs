using System;
using System.IO;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Interface that defines RIFF Chunk behavior and data exposure</summary>
    public interface IRiffChunk
    {
        #region Properties
        /// <summary>Leading four byte character code of the chunk</summary>
        ChunkType ChunkId { get; }

        /// <summary>Exposes the name of the chunk type. Preserves spaces in the name.</summary>
        String ChunkName { get; }
        
        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        UInt32 Size { get; }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        Byte[] Data { get; }

        /// <summary>Unsigned offset to the data</summary>
        Int64 DataOffset { get; }

        /// <summary>Position within the stream where this chunk starts</summary>
        Int64 ChunkOffset { get; }

        /// <summary>Stream object containing the RIFF data to read from.</summary>
        Stream DataStream { get; }
        #endregion


        #region Methods
        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        void WriteString(StringBuilder builder);
        #endregion
    }
}