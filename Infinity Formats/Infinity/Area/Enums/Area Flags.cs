using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags matching AREAFLAG.IDS (plus a few more flags), for BG, BG2, IWD</summary>
    [Flags]
    public enum AreaFlags : uint /* UInt32 */
    {
        /// <summary>Saves are allowed in this area.</summary>
        [Description("Saves are allowed in this area.")]
        SaveAllowed     = 1U,

        /// <summary>This area is a tutorial area.</summary>
        [Description("This area is a tutorial area.")]
        TutorialArea    = 2U,

        /// <summary>This area does not allow magic to work.</summary>
        [Description("This area does not allow magic to work.")]
        DeadMagicArea   = 4U,

        /// <summary>This area is used in dream sequences.</summary>
        [Description("This area is used in dream sequences.")]
        DreamArea       = 8U,
    }
}