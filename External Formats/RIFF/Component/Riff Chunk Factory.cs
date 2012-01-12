using System;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component
{
    /// <summary>Factory for producing RIFF Chunk objects based off of the input type</summary>
    public static class RiffChunkFactory
    {
        /// <summary>Generates a new RIFF chunk based off of the input chunk tyoe</summary>
        /// <param name="chunkType"></param>
        /// <returns></returns>
        public static RiffChunk CreateChunk(ChunkType chunkType)
        {
            RiffChunk chunk = null;

            switch (chunkType)
            {
                case ChunkType.RIFF:
                    chunk = new RiffThroneChunk(chunkType);
                    break;
                case ChunkType.fmt:
                    chunk = new WaveFormatChunk(chunkType);
                    break;
                case ChunkType.data:
                    chunk = new WaveSampleDataChunk(chunkType);
                    break;
                case ChunkType.JUNK:
                default:
                    chunk = new RiffChunk(chunkType);
                    break;
            }

            return chunk;
        }
    }
}