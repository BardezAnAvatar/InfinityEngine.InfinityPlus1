using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums
{
    /// <summary>Animation flag</summary>
    [Flags]
    public enum AnimationPuffFlags : uint /* UInt32 */
    {
        /// <summary>Puff At Target</summary>
        [Description("Puff At Target")]
        PuffAtTarget    = 1U,

        /// <summary>Puff At Source</summary>
        [Description("Puff At Source")]
        PuffAtSource    = 2U,
    }
}