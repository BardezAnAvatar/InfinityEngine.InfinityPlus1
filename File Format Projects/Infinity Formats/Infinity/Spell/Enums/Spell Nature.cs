using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    /// <summary>Nature of this spell</summary>
    /// <remarks>
    ///     These values are in the ROW ORDER of the values foundin BG2's msectype.2da
    ///     These rows have String References to dialog.tlk containing the spell's dispell string
    ///     (34559 - "Contingency Removed"; 27983 - "Magic Attack Cancelled"; 27986 - "Conjuration Dispelled")
    /// </remarks>
    public enum SpellNature : byte /* Byte */
    {
        None					= 0x00,
        [Description("Spell protections")]
        SpellProtections		= 0x01,
        [Description("Specific protections")]
        SpecificProtections		= 0x02,
        [Description("Illusionary protections")]
        IllusionaryProtections  = 0x03,
        [Description("Magic attack")]
        MagicAttack				= 0x04,
        [Description("Divination attack")]
        DivinationAttack		= 0x05,
        Conjuration				= 0x06,
        [Description("Combat protections")]
        CombatProtections		= 0x07,
        Contingency				= 0x08,
        Battleground			= 0x09,
        [Description("Offensive damage")]
        OffensiveDamage			= 0x0A,
        Disabling				= 0x0B,
        Combination				= 0x0C,
        [Description("Non-combat")]
        NonCombat				= 0x0D,
    }
}