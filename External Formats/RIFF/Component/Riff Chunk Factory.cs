using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Factory for producing RIFF Chunk objects based off of the input type</summary>
    public static class RiffChunkFactory
    {
        /// <summary>Generates a new RIFF chunk based off of the input chunk tyoe</summary>
        /// <param name="dataStream">Stream to read the chunk type from</param>
        /// <returns>A Constructed RIFF chunk based on the type of chunk detected</returns>
        public static RiffChunk CreateChunk(Stream dataStream)
        {
            ChunkType chunkType = RiffChunk.ReadFourCC(dataStream);
            RiffChunk chunk = null;

            switch (chunkType)
            {
                case ChunkType.RIFF:
                    chunk = new RiffThroneChunk(chunkType, dataStream);
                    break;
                case ChunkType.fmt:
                    chunk = new WaveFormatChunk(chunkType, dataStream);
                    break;
                case ChunkType.data:    //originally had this as a Wave-specific type, but not appropriate unless interpresting data.
                case ChunkType.JUNK:
                default:
                    chunk = new RiffChunk(chunkType, dataStream);
                    break;
            }

            return chunk;
        }
    }
}