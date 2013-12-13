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
        public static IRiffChunk CreateChunk(Stream dataStream)
        {
            IRiffChunk chunk = new RiffChunk(dataStream);

            switch (chunk.ChunkId)
            {
                case ChunkType.RIFF:
                case ChunkType.LIST:
                    chunk = new RiffSuperChunk(chunk);
                    break;
                case ChunkType.fmt:
                    chunk = new WaveFormatChunk(chunk);
                    break;
                case ChunkType.ISFT:
                    chunk = new SoftwareRiffChunk(chunk);
                    break;
                case ChunkType.data:    //originally had this as a Wave-specific type, but not appropriate unless interpresting data.
                case ChunkType.JUNK:
                default:
                    break;
            }

            return chunk;
        }
    }
}