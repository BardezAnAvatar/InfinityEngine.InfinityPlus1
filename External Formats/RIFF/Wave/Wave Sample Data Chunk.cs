using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave.Enums;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.Files.External.RIFF.Wave
{
    public class WaveSampleDataChunk : RiffChunk
    {
        /// <summary>Chunk Type constructor</summary>
        /// <param name="type">Chunk id of the chunk</param>
        public WaveSampleDataChunk(ChunkType type) : base(type) { }
    }
}