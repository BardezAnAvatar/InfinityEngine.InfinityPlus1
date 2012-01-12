using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Enums
{
    /// <summary>Effect version 2 mage schol enumerator</summary>
    /// <remarks>Values taken out of DLTCEP</remarks>
    public enum EffectMageSchool : ushort /* UInt16 */
    {
        None            = 0,
        Abjurationist,
        Conjurer,
        Divination,
        Enchanter,
        Illusionist,
        Invoker,
        Necromancer,
        Transmuter,
        Generalist,

        [Description("Wild Magic")]
        WildMagic
    }
}
