using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    /// <summary>Enumeration of types of abilities associated with an item's or spell's ability</summary>
    public enum ItemSpellAbilityType : byte /* Byte */
    {
        None        = 0,

        /// <summary>"Standard" for Spells in IESDP, "Melee" in IESDP</summary>
        Melee       = 1,
        Projectile  = 2,
        Magic       = 3,
        Launcher    = 4
    }
}