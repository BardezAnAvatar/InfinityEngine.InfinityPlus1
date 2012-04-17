using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums
{
    /// <summary>Various are of effect flags</summary>
    [Flags]
    public enum AreaOfEffectFlags : uint /* UInt32 */
    {
        /// <summary>Visible until detonation.</summary>
        [Description("Visible until detonation.")]
        VisibleUntilDetonated       = 1U,

        /// <summary>Affects inanimates.</summary>
        [Description("Affects inanimates.")]
        AffectsInanimates           = 2U,

        /// <summary>Needs trigger.</summary>
        [Description("Needs trigger.")]
        NeedsTrigger                = 4U,

        /// <summary>Synchronize explosions.</summary>
        [Description("Synchronize explosions.")]
        SynchronizeExplosion        = 8U,

        /// <summary>Has secondary projectiles.</summary>
        [Description("Has secondary projectiles.")]
        HasSecondaryProjectiles     = 16U,

        /// <summary>Has fragments.</summary>
        [Description("Has fragments.")]
        HasFragments                = 32U,

        /* target selection */
        /// <summary>Affects enemies.</summary>
        [Description("Affects enemies.")]
        AffectsEnemies              = 64U,

        /// <summary>Affects allies.</summary>
        [Description("Affects allies.")]
        AffectsAllies               = 128U,

        /// <summary>Affects everyone.</summary>
        /// <remarks>Bitfield of both AffectsEnemies and AffectsAllies</remarks>
        [Description("Affects everyone.")]
        AffectsAll                  = 192U,
        
        /// <summary>Mage caster level determines trigger count.</summary>
        [Description("Mage caster level determines trigger count.")]
        TriggerCountLevelMage       = 256U,

        /// <summary>Cleric caster level determines trigger count.</summary>
        [Description("Cleric caster level determines trigger count.")]
        TriggerCountLevelCleric     = 512U,

        /// <summary>Has a VVC animation.</summary>
        [Description("Has a VVC animation.")]
        HasVvcAnimation             = 1024U,

        /// <summary>Has a cone shape.</summary>
        [Description("Has a cone shape.")]
        HasConeShape                = 2048U,

        /// <remarks>Explodes through walls</remarks>
        Intangible                  = 4096U,
        
        /// <summary>Delayed trigger.</summary>
        /// <remarks>Requires VisibleUntilDetonated</remarks>
        [Description("Delayed trigger.")]
        DelayedTrigger              = 8192U,

        /// <summary>Delayed explosion.</summary>
        [Description("Delayed explosion.")]
        DelayedExplosion            = 0x4000U,
        
        /// <summary>Single target.</summary>
        [Description("Single target.")]
        SingleTarget                = 0x8000U,
    }
}