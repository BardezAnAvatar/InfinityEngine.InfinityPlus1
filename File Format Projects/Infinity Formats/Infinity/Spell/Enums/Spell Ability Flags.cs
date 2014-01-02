using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Spell.Enums
{
    /// <summary>Flag describing spell hostility and possibly other values as well</summary>
    /// <remarks>A Hostile spell is one that does not have the friendly bit set</remarks>
    [Flags]
    public enum SpellAbilityFlags : byte /* Byte */
    {
        /// <summary>A non-offensive spell</summary>
        Friendly    = 0x04,
    }
}