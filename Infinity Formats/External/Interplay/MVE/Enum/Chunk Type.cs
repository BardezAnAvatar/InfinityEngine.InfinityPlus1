using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum
{
    /// <summary>Reprsens the six chunk types</summary>
    public enum ChunkType : ushort /* UInt16 */
    {
        /// <summary>Initializes audio</summary>
        [Description("Initialize Audio")]
        InitializeAudio = 0,

        /// <summary>Chunk only contains audio data</summary>
        [Description("Audio Only")]
        AudioOnly       = 1,

        /// <summary>Initializes Video</summary>
        [Description("Initialize Video")]
        InitializeVideo = 2,

        /// <summary>Chunk contains both audio and video data</summary>
        [Description("Audio/Video Chunk")]
        AudioVideoChunk = 3,

        /// <summary>Shuts down the decoder?</summary>
        [Description("Shutdown Chunk")]
        ShutdownChunk   = 4,

        /// <summary>Termination chunk?</summary>
        [Description("End Chunk")]
        EndChunk        = 5,
    }
}