using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    /// <summary>Flags indicating overlay exclusion</summary>
    /// <remarks>According to IESDP, Alignment and School exclusion bits cannot be combined.</remarks>
    [Flags]
    public enum SpellProhibitionFlags : uint /* UInt32 */
    {
        [Description("Chaotic...")]
        OrderChaotic        = 0x00000001U,
        [Description("...Evil")]
        NatureEvil          = 0x00000002U,
        [Description("...Good")]
        NatureGood          = 0x00000004U,
        [Description("...Neutral")]
        NatureNeutral       = 0x00000008U,
        [Description("Lawful...")]
        OrderLawful         = 0x00000010U,
        [Description("Neutral...")]
        OrderNeutral        = 0x00000020U,

        [Description("School: Abjurer")]
        SchoolAbjurer       = 0x00000040U,
        [Description("School: Conjurer")]
        SchoolConjurer      = 0x00000080U,
        [Description("School: Diviner")]
        SchoolDiviner       = 0x00000100U,
        [Description("School: Enchanter")]
        SchoolEnchanter     = 0x00000200U,
        [Description("School: Illusionist")]
        SchoolIllusionist   = 0x00000400U,
        [Description("School: Invoker")]
        SchoolInvoker       = 0x00000800U,
        [Description("School: Necromancer")]
        SchoolNecromancer   = 0x00001000U,
        [Description("School: Transmuter")]
        SchoolTransmuter    = 0x00002000U,
        [Description("School: Generalist")]    //See SPWI222.SPL -- Chaos Shield
        SchoolGeneralist    = 0x00004000U,

        //is there a generalist restriction for wild magic spells?
        
        [Description("Divine: Cleric/Paladin")]
        DivineClericPaladin = 0x40000000U,
        [Description("Divine: Druid/Ranger")]
        DivineDruidRanger   = 0x80000000U,
    }
}
