using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum
{
    /// <summary>Enumerator indicating the opcode</summary>
    /// <remarks>I will clarify the existence of opcodes in IE MVEs with Obsolete tags once I have primitive read/write code in place</remarks>
    public enum OpcodeTypes : byte /* Byte */
    {
        /// <summary>Terminates all playback</summary>
        [Description("End of Stream")]
        EndOfStream             = 0,

        /// <summary>Ends current chunk and fetches the next one</summary>
        [Description("End of Chunk")]
        EndOfChunk              = 1,

        /// <summary>Creates a framerate timer</summary>
        [Description("Create Timer")]
        CreateTimer             = 2,

        /// <summary>Initializes audio buffers</summary>
        [Description("Initialize Audio Buffers")]
        InitializeAudioBuffers  = 3,

        /// <summary>Starts or stops audio; toggles playback state</summary>
        [Description("Toggle Audio")]
        ToggleAudio             = 4,

        /// <summary>Initializes video buffers</summary>
        [Description("Initialize Video Buffers")]
        InitializeVideoBuffers  = 5,

        /// <summary>Does some 'clever' rotation of video buffers</summary>
        /// <remarks>Apparently unused in IE movies</remarks>
        [Description("Rotate Video Buffers"), Obsolete("No occurance in MVEs")]
        RotateVideoBuffers      = 6,

        /// <summary>Sends the bufer to the display</summary>
        [Description("Render Video Buffer")]
        RenderVideoBuffer       = 7,

        /// <summary>Contains audio sample data</summary>
        [Description("Audio Samples")]
        AudioSamples            = 8,

        /// <summary>Contains audio sample data</summary>
        [Description("Audio Silence")]
        AudioSilence            = 9,

        /// <summary>Initializes the video mode</summary>
        /// <remarks>This is remnant DOS VGA code? Apparently Infinity Engine treats this as a no-op</remarks>
        [Description("Initialize Video Stream")]
        InitializeVideoStream   = 10,

        /// <summary>Creates a gradient palette</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Create Gradient"), Obsolete("No occurance in MVEs")]
        CreateGradient          = 11,

        /// <summary>Sets or overwrites the existing palette?</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Set Palette")]
        SetPalette              = 12,

        /// <summary>Sets or overwrites the existing palette, with compressed data</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Compressed Set Palette"), Obsolete("No occurance in MVEs")]
        SetPaletteCompressed    = 13,

        /// <summary>Unknown</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Unknown # 14"), Obsolete("No occurance in MVEs")]
        Unknown14               = 14,

        /// <summary>Sets the decoding map for video data</summary>
        [Description("Set Decoding Map")]
        SetDecodingMap          = 15,

        /// <summary>Unobserved video data storage</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Unknown Video Data Storage"), Obsolete("No occurance in MVEs")]
        UnknownVideoDataStorage = 16,

        /// <summary>Contains video data</summary>
        [Description("Video Data")]
        VideoData               = 17,

        /// <summary>Unknown</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Unknown # 18"), Obsolete("No occurance in MVEs")]
        Unknown18               = 18,

        /// <summary>Unknown</summary>
        /// <remarks>Observed, but unused? Appears in most video chunks?</remarks>
        [Description("Unknown # 19"), Obsolete("No occurance in MVEs")]
        Unknown19               = 19,

        /// <summary>Unknown</summary>
        /// <remarks>Not initially observed?</remarks>
        [Description("Unknown # 20"), Obsolete("No occurance in MVEs")]
        Unknown20               = 20,

        /// <summary>Unknown</summary>
        /// <remarks>Observed, but unused? Only appears in the video initialization chunk?</remarks>
        [Description("Unknown # 21"), Obsolete("No occurance in MVEs")]
        Unknown21               = 21,
    }
}