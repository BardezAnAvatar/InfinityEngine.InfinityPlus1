using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Enums
{
    /// <summary>Audio flags for an audio track</summary>
    public enum AudioFlags : ushort /* UInt16*/
    {
        Stereo  = 0x2000,

        /// <remarks>When not set, this implies FFT</remarks>
        DCT     = 0x1000,
    }
}