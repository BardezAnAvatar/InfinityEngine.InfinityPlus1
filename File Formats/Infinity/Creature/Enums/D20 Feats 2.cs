using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Creature.Enums
{
    /// <summary>Second 64 bits of the feats bitfield</summary>
    [Flags]
    public enum D20Feats2 : ulong /* UInt64 */
    {
        [Description("Spell Penetration")]
        SPELL_PENETRATION           = 0x0000000000000001U,

        [Description("Spirit of Flame")]
        SPIRIT_OF_FLAME             = 0x0000000000000002U,

        [Description("Strong Back")]
        STRONG_BACK                 = 0x0000000000000004U,

        [Description("Stunning Fist")]
        STUNNING_FIST               = 0x0000000000000008U,

        [Description("Subvocal Casting")]
        SUBVOCAL_CASTING            = 0x0000000000000010U,

        [Description("Toughness")]
        TOUGHNESS                   = 0x0000000000000020U,

        [Description("Two Weapon Fighting")]
        TWO_WEAPON_FIGHTING         = 0x0000000000000040U,

        [Description("Weapon Finesse")]
        WEAPON_FINESSE              = 0x0000000000000080U,

        [Description("Wildshape: Boring Beetle")]
        WILDSHAPE_BOAR              = 0x0000000000000100U,

        [Description("Wildshape: Panther")]
        WILDSHAPE_PANTHER           = 0x0000000000000200U,

        [Description("Wildshape: Shambling Mound")]
        WILDSHAPE_SHAMBLER          = 0x0000000000000400U
    }
}
