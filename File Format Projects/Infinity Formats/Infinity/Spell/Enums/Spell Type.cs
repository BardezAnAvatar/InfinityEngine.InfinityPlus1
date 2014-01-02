using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    /// <summary>Type of spell</summary>
    public enum SpellType : ushort /* UInt16 */
    {
        Special = 0x0000,
        Wizard,
        Cleric,
        Psionic,
        Innate,

        [Description("Bard Song")]
        BardSong
    }
}
