using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell.Enums
{
    /// <summary>Various sequencing flags</summary>
    [Flags]
    public enum SequenceFlags : uint /* UInt32 */
    {
        Looping                 = 1,
        SpecialLighting         = 2,
        Height                  = 4,
        DrawAnimation           = 8,
        CustomPalette           = 16,
        Purgeable               = 32,
        NotCoveredByWallgroups  = 64,
        BrightenLevelMid        = 128,
        BrightenLevelHigh       = 256,
    }
}