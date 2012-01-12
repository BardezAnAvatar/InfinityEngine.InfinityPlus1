using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums
{
    public enum EffectTarget : sbyte /* SByte */
    {
        Invalid                 = -1,

        None                    = 0,

        [Description("Self (before any projectile launched)")]
        SelfPreProjectile       = 1,

        [Description("Pre-set Target")]
        PresetTarget            = 2,

        Party                   = 3,

        [Description("Everyone")]
        EveryoneWithParty       = 4,

        [Description("Everyone (Excluding Party)")]
        EveryoneWithoutParty    = 5,

        [Description("Everyone (Matching caster's affiliation)")]
        EveryoneMatchingCaster  = 6,

        [Description("Everyone (Matching target's affiliation)")]
        EveryoneMatchingTarget  = 7,

        [Description("Everyone (Excluding caster)")]
        EveryoneWithoutCaster   = 8,

        [Description("Self (after any projectile launched)")]
        SelfPostProjectile      = 9
    }
}