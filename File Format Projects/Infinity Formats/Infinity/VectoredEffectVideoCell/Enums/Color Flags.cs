using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell.Enums
{
    /// <summary>Various color flags</summary>
    [Flags]
    public enum ColorFlags : ushort /* UInt16 */
    {
        LightSource         = 2,
        InternalBrightness  = 4,
        TimeStopped         = 8,
        InternalGamma       = 32,
        NonReservedPalette  = 64,
        FullPalette         = 128,
        Sepia               = 512,
    }
}