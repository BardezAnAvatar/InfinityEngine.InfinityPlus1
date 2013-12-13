using System;
using System.IO;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Base class that implements an IRiffChunk that receives a previously read RiffChunk base</summary>
    public abstract class RiffChunkExtensionBase : IRiffChunk
    {
        #region Fields
        /// <summary>Chunk already read that contains basic details for this chunk</summary>
        protected IRiffChunk rootChunk;
        #endregion


        #region IRiffChunk
        /// <summary>Leading four byte character code of the chunk</summary>
        public virtual ChunkType ChunkId
        {
            get { return this.rootChunk.ChunkId; }
        }

        /// <summary>Exposes the name of the chunk type. Preserves spaces in the name.</summary>
        public virtual String ChunkName
        {
            get { return this.rootChunk.ChunkName; }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        public virtual UInt32 Size
        {
            get { return this.rootChunk.Size; }
        }

        /// <summary>Unsigned 32-bit interger indicating the chunk size</summary>
        /// <remarks>
        ///     The spec. defines this as a byte array. However, the problem with this is that, in the example of raw audio-video interleave,
        ///     the data chunk may be so incredibly large as to surpass RAM, in which case reading the data is foolish. Instead, open the file
        ///     and read the stream, seeking as appropriate, storing offsets and reading on-demand.
        /// </remarks>
        public virtual Byte[] Data
        {
            get { return this.rootChunk.Data; }
        }

        /// <summary>Unsigned offset to the data</summary>
        public virtual Int64 DataOffset
        {
            get { return this.ChunkOffset + 8; }
        }

        /// <summary>Position within the stream where this chunk starts</summary>
        public virtual Int64 ChunkOffset
        {
            get { return this.rootChunk.ChunkOffset; }
        }

        /// <summary>Stream object containing the RIFF data to read from.</summary>
        public virtual Stream DataStream
        {
            get { return this.rootChunk.DataStream; }
        }
        #endregion


        #region Construction
        /// <summary>Chunk constructor</summary>
        /// <param name="baseChunk">Chunk already read that contains basic details for this chunk</param>
        public RiffChunkExtensionBase(IRiffChunk baseChunk)
        {
            this.rootChunk = baseChunk;
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing a human-readable representation</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.rootChunk.ToString();
        }

        /// <summary>This method prints a human-readable representation to the given StringBuilder</summary>
        /// <param name="builder">StringBuilder to write to</param>
        public virtual void WriteString(StringBuilder builder)
        {
            this.rootChunk.WriteString(builder);
        }
        #endregion
    }
}