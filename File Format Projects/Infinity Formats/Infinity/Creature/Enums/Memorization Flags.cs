using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Enums
{
    /// <summary>Flags indicating the various memorization settings</summary>
    [Flags]
    public enum MemorizationFlags : uint /* UInt32 */
    {
        Memorized   = 1U,
        Disabled    = 2U,
    }
}