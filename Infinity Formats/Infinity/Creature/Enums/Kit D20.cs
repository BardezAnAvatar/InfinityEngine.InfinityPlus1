using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>Valid KitD20 values</summary>
    [Flags]
    public enum KitD20 : uint /* UInt32 */
    {
        [Description("Paladin of Ilmater")]
        PALADIN_ILMATER     = 0x00000001U,

        [Description("Paladin of Helm")]
        PALADIN_HELM        = 0x00000002U,

        [Description("Paladin of Mystra")]
        PALADIN_MYSTRA      = 0x00000004U,

        [Description("Monk of the Old Order")]
        MONK_OLD_ORDER      = 0x00000008U,

        [Description("Monk of the Broken Ones")]
        MONK_BROKEN_ONES    = 0x00000010U,

        [Description("Monk of the Dark Moon")]
        MONK_DARK_MOON      = 0x00000020U,

        [Description("Mage: Abjurer")]
        MAGE_ABJURER        = 0x00000040U,

        [Description("Mage: Conjurer")]
        MAGE_CONJURER       = 0x00000080U,

        [Description("Mage: Diviner")]
        MAGE_DIVINER        = 0x00000100U,

        [Description("Mage: Enchanter")]
        MAGE_ENCHANTER      = 0x00000200U,

        [Description("Mage: Evoker")]
        MAGE_EVOKER         = 0x00000800U,

        [Description("Mage: Illusionist")]
        MAGE_ILLUSIONIST    = 0x00000400U,

        [Description("Mage: Necromancer")]
        MAGE_NECROMANCER    = 0x00001000U,

        [Description("Mage: Transmuter")]
        MAGE_TRANSMUTER     = 0x00002000U,

        [Description("Mage: Generalist")]
        MAGE_GENERALIST     = 0x00004000U,

        [Description("Cleric of Imater")]
        CLERIC_ILMATER      = 0x00008000U,

        [Description("Cleric of Lathander")]
        CLERIC_LATHANDER    = 0x00010000U,

        [Description("Cleric of Selune")]
        CLERIC_SELUNE       = 0x00020000U,

        [Description("Cleric of Helm")]
        CLERIC_HELM         = 0x00040000U,

        [Description("Cleric of Oghma")]
        CLERIC_OGHMA        = 0x00080000U,

        [Description("Cleric of Tempus")]
        CLERIC_TEMPUS       = 0x00100000U,

        [Description("Cleric of Bane")]
        CLERIC_BANE         = 0x00200000U,

        [Description("Cleric of Mask")]
        CLERIC_MASK         = 0x00400000U,

        [Description("Cleric of Talos")]
        CLERIC_TALOS        = 0x00800000U
    }
}