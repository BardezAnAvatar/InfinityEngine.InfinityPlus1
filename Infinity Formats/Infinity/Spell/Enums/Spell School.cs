using System;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Enums
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     These values are in the ROW ORDER of the values foundin BG2's mschool.2da, and supposedly IWD's school.2da,
    ///     but I cannot find it. These rows have String References to their names.
    ///     
    ///     ... I suspect, however, that the leading byte is part  this field, and the unused trailing byte is a
    ///     similar leading byte of the next field.
    /// </remarks>
    public enum SpellSchool : ushort /* UInt16 */
    {
        None            = 0x0000,
        Abjurer         = 0x0100,
        Conjurer        = 0x0200,
        Diviner         = 0x0300,
        Enchanter       = 0x0400,
        Illusionist     = 0x0500,
        Invoker         = 0x0600,
        Necromancer     = 0x0700,
        Transmuter      = 0x0800,
        Generalist      = 0x0900,
        //, WildMage    = 0x0A00
    }
}
