using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums
{
    /// <summary>Type of projectile</summary>
    public enum ProjectileType : ushort /* UInt16 */
    {
        /// <summary>No BAM animation.</summary>
        [Description("No BAM animation.")]
        NoAnimation         = 1,

        /// <summary>BAM animation with a single target.</summary>
        [Description("BAM animation with a single target.")]
        SingleTarget        = 2,

        /// <summary>Projectile with an area of effect.</summary>
        [Description("Projectile with an area of effect.")]
        AreaOfEffect        = 3,
    }
}