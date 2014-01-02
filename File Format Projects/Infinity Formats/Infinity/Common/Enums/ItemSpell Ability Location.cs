using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    /// <summary>UI slot associated with an item's or spell's ability</summary>
    public enum ItemSpellAbilityLocation : ushort /* UInt16 */
    {
        None = 0,
        Weapon = 1,
        Spell = 2,
        Equipment = 3,
        Innate = 4
    }
}