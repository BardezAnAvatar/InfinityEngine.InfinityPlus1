using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    /// <summary>The magic school associated with a spell</summary>
    /// <remarks>
    ///     These values are in the ROW ORDER of the values foundin BG2's mschool.2da, and supposedly IWD's school.2da,
    ///     but I cannot find it. These rows have String References to their names. Amusingly, the default value is a 64-bit integer (2^32), not -1 (2^32 - 1)
    /// </remarks>
    public enum SpellSchool : byte /* Byte */
    {
        None            = 0x00,
        Abjurer         = 0x01,
        Conjurer        = 0x02,
        Diviner         = 0x03,
        Enchanter       = 0x04,
        Illusionist     = 0x05,
        Invoker         = 0x06,
        Necromancer     = 0x07,
        Transmuter      = 0x08,
        Generalist      = 0x09,
        //, WildMage    = 0x0A
    }
}