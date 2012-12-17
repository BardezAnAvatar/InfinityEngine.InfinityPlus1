using System;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Contains the values for Channel masks</summary>
    [Flags]
    public enum SpeakerPositions : uint /* UInt32 */
    {
        SPEAKER_FRONT_LEFT              = 0x1U,
        SPEAKER_FRONT_RIGHT             = 0x2U,
        SPEAKER_FRONT_CENTER            = 0x4U,
        SPEAKER_LOW_FREQUENCY           = 0x8U,
        SPEAKER_BACK_LEFT               = 0x10U,
        SPEAKER_BACK_RIGHT              = 0x20U,
        SPEAKER_FRONT_LEFT_OF_CENTER    = 0x40U,
        SPEAKER_FRONT_RIGHT_OF_CENTER   = 0x80U,
        SPEAKER_BACK_CENTER             = 0x100U,
        SPEAKER_SIDE_LEFT               = 0x200U,
        SPEAKER_SIDE_RIGHT              = 0x400U,
        SPEAKER_TOP_CENTER              = 0x800U,
        SPEAKER_TOP_FRONT_LEFT          = 0x1000U,
        SPEAKER_TOP_FRONT_CENTER        = 0x2000U,
        SPEAKER_TOP_FRONT_RIGHT         = 0x4000U,
        SPEAKER_TOP_BACK_LEFT           = 0x8000U,
        SPEAKER_TOP_BACK_CENTER         = 0x10000U,
        SPEAKER_TOP_BACK_RIGHT          = 0x20000U,

        /// <summary>Bit mask locations reserved for future use</summary>
        SPEAKER_RESERVED                = 0x7FFC0000U,

        /// <summary>Used to specify that any possible permutation of speaker configurations</summary>
        SPEAKER_ALL                     = 0x80000000U,
    }
}