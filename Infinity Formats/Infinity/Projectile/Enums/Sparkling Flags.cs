using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums
{
    /// <summary>Various sparkling flags</summary>
    [Flags]
    public enum SparklingFlags : uint /* UInt32 */
    {
        /// <remarks>Show sparkle</remarks>
        Sparkles                    = 1U,

        /// <summary>Use Z co-ordinate.</summary>
        /// <remarks>Gem RB calls this flying</remarks>
        [Description("Use Z co-ordinate.")]
        UseCoordinateZ              = 2U,

        /// <summary>Loop the travelling sound.</summary>
        [Description("Loop the travelling sound.")]
        LoopSoundTravelling         = 4U,

        /// <summary>Loop the destination sound.</summary>
        [Description("Loop the destination sound.")]
        LoopSoundDestination        = 8U,

        /// <summary>Ignore center target.</summary>
        /// <remarks>Does not affect the direct target/center of the area.</remarks>
        [Description("Ignore center target.")]
        IgnoreCenter                = 16U,

        /// <summary>Draw below Animated objects.</summary>
        [Description("Draw below Animated objects.")]
        DrawBelowAnimateObjects     = 32U,
    }
}