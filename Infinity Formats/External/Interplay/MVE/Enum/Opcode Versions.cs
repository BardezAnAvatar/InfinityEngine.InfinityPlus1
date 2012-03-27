using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum
{
    /// <summary>Enumerator indicating the opcode</summary>
    /// <remarks>I will clarify the existence of opcodes in IE MVEs with Obsolete tags once I have primitive read/write code in place</remarks>
    public enum OpcodeVersions
    {
        /// <summary>Terminates all playback</summary>
        [Description("End of Stream")]
        EndOfStream,

        /// <summary>Ends current chunk and fetches the next one</summary>
        [Description("End of Chunk")]
        EndOfChunk,

        /// <summary>Creates a framerate timer</summary>
        [Description("Create Timer")]
        CreateTimer,

        /// <summary>Initializes audio buffers</summary>
        [Description("Initialize Audio Buffers")]
        InitializeAudioBuffers0,

        /// <summary>Initializes audio buffers with a larger buffer</summary>
        [Description("Initialize Audio Buffers larger buffer")]
        InitializeAudioBuffers1,

        /// <summary>Starts or stops audio; toggles playback state</summary>
        [Description("Toggle Audio")]
        ToggleAudio,

        /// <summary>Initializes video buffers</summary>
        [Description("Initialize Video Buffers")]
        InitializeVideoBuffers0,

        /// <summary>Initializes video buffers with a size</summary>
        [Description("Initialize Video Buffers with a size")]
        InitializeVideoBuffers1,

        /// <summary>Initializes video buffers with a highcolor flag</summary>
        [Description("Initialize Video Buffers with a highcolor flag")]
        InitializeVideoBuffers2,

        /// <summary>Sends the bufer to the display</summary>
        [Description("Render Video Buffer")]
        RenderVideoBuffer0,

        /// <summary>Sends the bufer to the display</summary>
        [Description("Render Video Buffer with unknown param")]
        RenderVideoBuffer1,

        /// <summary>Contains audio sample data</summary>
        [Description("Audio Samples")]
        AudioSamples,

        /// <summary>Contains audio sample data</summary>
        [Description("Audio Silence")]
        AudioSilence,

        /// <summary>Initializes the video mode</summary>
        /// <remarks>This is remnant DOS VGA code? Apparently Infinity Engine treats this as a no-op</remarks>
        [Description("Initialize Video Stream")]
        InitializeVideoStream,

        /// <summary>Sets or overwrites the existing palette?</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Set Palette")]
        SetPalette,

        /// <summary>Sets the decoding map for video data</summary>
        [Description("Set Decoding Map")]
        SetDecodingMap,

        /// <summary>Contains video data</summary>
        [Description("Video Data")]
        VideoData,

        /// <summary>Invalid op code</summary>
        Invalid,
    }
}