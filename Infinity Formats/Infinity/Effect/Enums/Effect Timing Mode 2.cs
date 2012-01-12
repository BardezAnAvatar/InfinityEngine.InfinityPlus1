using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums
{
    public enum EffectTimingMode2 : ushort /* UInt16 */
    {
        /// <summary>Instant/Limited</summary>
        Duration                        = 0,

        /// <summary>Instant/Permanent</summary>
        Permanent                       = 1,

        /// <summary>Instant/While Equipped</summary>
        [Description("While equipped")]
        WhileEquipped                   = 2,

        /// <summary>Delay/Limited</summary>
        [Description("Delayed Duration")]
        DelayedDuration                 = 3,

        /// <summary>Delay/Permanent</summary>
        Delayed                         = 4,

        /// <summary>IESDP declares that this delayed field turns into 8, Permanent (unsaved)</summary>
        [Description("Delayed (Permanent and unsaved after duration)")]
        DelayedPermanentUnsaved         = 5,

        /// <summary>IESDP suggests a duration, but unknown</summary>
        [Description("Unknown (suggested duration)")]
        UnknownDuration                 = 6,

        /// <summary>IESDP suggests permanent, but unknown</summary>
        [Description("Unknown (suggested permanent)")]
        UnknownPermanent                = 7,

        /// <summary>IESDP suggests permanent and unsaved, but unknown</summary>
        [Description("Unknown (suggested permanent, unsaved)")]
        UnknownPermanentUnsaved         = 8,

        /// <summary>IESDP suggests permanent after death, but probably after other bonuses</summary>
        [Description("Permanent (applied after bonuses)")]
        UnknownPermanentAfterBonuses    = 9,

        /// <summary>IESDP suggests this is triggered</summary>
        Trigger                         = 10,

        [Description("Absolute Duration")]
        AbsoluteDuration                = 4096
    }
}