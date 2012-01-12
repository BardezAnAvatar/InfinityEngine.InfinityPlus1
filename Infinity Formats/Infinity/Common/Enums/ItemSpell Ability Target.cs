using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    public enum ItemSpellAbilityTarget : byte /* Byte */
    {
        Invalid     = 0,
        Creature    = 1,
        Inventory   = 2,    //Near Infinity
        Portrait    = 3,    //Near Infinity: Dead actor (raise dead?)
        Area        = 4,
        Self        = 5,
        [Description("Self (ignores dialog/engine pause)")]
        SelfNoPause = 7     //Near Infinity: "Caster (keep overlay, no anim) (7)"
    }
}