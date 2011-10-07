using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Common.Enums
{
    public enum ItemSpellAbilityEffectTimingMode : byte /* Byte */
    {
        /// <summary>Instant/Limited</summary>
        Duration                        = 0,

        /// <summary>Instant/Permanent</summary>
        Permanent                       = 1,

        /// <summary>Instant/While Equipped</summary>
        [DescriptionAttribute("While equipped")]
        WhileEquipped                   = 2,

        /// <summary>Delay/Limited</summary>
        [DescriptionAttribute("Delayed Duration")]
        DelayedDuration                 = 3,

        /// <summary>Delay/Permanent</summary>
        Delayed                         = 4,

        /// <summary>IESDP declares that this delayed field turns into 8, Permanent (unsaved)</summary>
        [DescriptionAttribute("Delayed (Permanent and unsaved after duration)")]
        DelayedPermanentUnsaved         = 5,

        /// <summary>IESDP suggests a duration, but unknown</summary>
        [DescriptionAttribute("Unknown (suggested duration)")]
        UnknownDuration                 = 6,

        /// <summary>IESDP suggests permanent, but unknown</summary>
        [DescriptionAttribute("Unknown (suggested permanent)")]
        UnknownPermanent                = 7,

        /// <summary>IESDP suggests permanent and unsaved, but unknown</summary>
        [DescriptionAttribute("Unknown (suggested permanent, unsaved)")]
        UnknownPermanentUnsaved         = 8,

        /// <summary>IESDP suggests permanent after death, but probably after other bonuses</summary>
        [DescriptionAttribute("Permanent (applied after bonuses)")]
        UnknownPermanentAfterBonuses    = 9,

        /// <summary>IESDP suggests this is triggered</summary>
        Trigger                         = 10

        //4096=Absolute duration //this is a max 256 field.
    }
}