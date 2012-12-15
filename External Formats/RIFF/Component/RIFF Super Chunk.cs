using System;
using System.Collections.Generic;
using System.IO;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Base chunk class that has sub-chunks</summary>
    public class RiffSuperChunk : RiffChunk
    {
        /// <summary>List of sub-chunks</summary>
        public List<RiffSubChunk> SubChunks { get; set; }

        #region Construction
        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        /// <param name="input">Stream to read from.</param>
        public RiffSuperChunk(ChunkType type, Stream input) : base(type, input) { }

        /// <summary>Instantiates reference types</summary>
        protected override void Initialize()
        {
            this.SubChunks = new List<RiffSubChunk>();
        }
        #endregion

        #region Methods
        public RiffSubChunk FindFirstSubChunk(ChunkType fourCC)
        {
            RiffSubChunk chunk = null;

            //search
            foreach (RiffSubChunk subChunk in this.SubChunks)
            {
                if (subChunk.Chunk.ChunkId == fourCC)
                {
                    chunk = subChunk;
                    break;
                }
            }

            return chunk;
        }
        #endregion
    }
}