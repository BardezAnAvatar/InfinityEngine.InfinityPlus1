using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component
{
    /// <summary>Simple structure that defines a chunk and the offset to that chunk in memory</summary>
    public class RiffSubChunk
    {
        /// <summary>Offset from start of file to position within data containing the sub-chunk</summary>
        protected Int64 offset;

        /// <summary>Riff chunk contained </summary>
        protected RiffChunk chunk;

        /// <summary>Offset from start of file to position within data containing the sub-chunk</summary>
        public Int64 Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }

        /// <summary>Riff chunk contained </summary>
        public RiffChunk Chunk
        {
            get { return this.chunk; }
            set { this.chunk = value; }
        }

        /// <summary>Default constuctor</summary>
        /// <param name="position">Offset within stream to start reading at</param>
        public RiffSubChunk(Int64 position)
        {
            this.offset = position;
        }
    }
}
