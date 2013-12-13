using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Effect.Enums
{
    /// <summary>Indicates the effect's magic type</summary>
    public enum EffectMagicType : uint /* UInt32 */
    {
        None            = 0U,

        [Description("Spell Protections")]
        SPELLPROTECTIONS,

        [Description("Specific Protections")]
        SPECIFICPROTECTIONS,

        [Description("Illusionary Protections")]
        ILLUSIONARYPROTECTIONS,

        [Description("Magic Attack")]
        MAGICATTACK,

        [Description("Divination Attack")]
        DIVINATIONATTACK,

        [Description("Conjuration")]
        CONJURATION,

        [Description("Combat Protections")]
        COMBATPROTECTIONS,

        [Description("Contingency")]
        CONTINGENCY,

        [Description("Battleground")]
        BATTLEGROUND,

        [Description("Offensive Damage")]
        OFFENSIVEDAMAGE,

        [Description("Disabling")]
        DISABLING,

        [Description("Combination")]
        COMBINATION,

        [Description("Non-Combat")]
        NON_COMBAT
    }
}