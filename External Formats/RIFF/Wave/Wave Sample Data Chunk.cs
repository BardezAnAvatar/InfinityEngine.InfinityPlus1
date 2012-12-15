using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave.Enums;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RIFF.Wave
{
    //TODO: Remove this class. It is worthless.
    public class WaveSampleDataChunk : RiffChunk
    {
        public WaveSampleDataChunk() : base(ChunkType.JUNK, null) { }
    }
}