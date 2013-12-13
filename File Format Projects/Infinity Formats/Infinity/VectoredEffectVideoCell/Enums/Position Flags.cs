using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell.Enums
{
    /// <summary>Various position flags</summary>
    [Flags]
    public enum PositionFlags : uint /* UInt32 */
    {
        Orbit               = 1,
        Relative            = 2,
        IgnoreOrientation   = 8,
    }
}