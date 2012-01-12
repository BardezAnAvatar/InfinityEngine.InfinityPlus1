using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Enums
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     These values are in the ROW ORDER of the values foundin BG2's msectype.2da
    ///     These rows have String References to their names in dialog.tlk.
    ///     
    ///     ... I suspect, however, that the leading byte is part this field, and the preceeding
    ///     unused trailing byte of the previous field is actually the leading byte of this field,
    ///     for purposes of storage. I could see bit-shifting as needed, but don't want to jump into
    ///     assembly reading and commenting like Avenger did.
    /// </remarks>
    public enum SpellNature : ushort /* UInt16 */
    {
        None					= 0x0000,
        [Description("Spell protections")]
        SpellProtections		= 0x0100,
        [Description("Specific protections")]
        SpecificProtections		= 0x0200,
        [Description("Illusionary protections")]
        IllusionaryProtections  = 0x0300,
        [Description("Magic attack")]
        MagicAttack				= 0x0400,
        [Description("Divination attack")]
        DivinationAttack		= 0x0500,
        Conjuration				= 0x0600,
        [Description("Combat protections")]
        CombatProtections		= 0x0700,
        Contingency				= 0x0800,
        Battleground			= 0x0900,
        [Description("Offensive damage")]
        OffensiveDamage			= 0x0A00,
        Disabling				= 0x0B00,
        Combination				= 0x0C00,
        [Description("Non-combat")]
        NonCombat				= 0x0D00
    }
}