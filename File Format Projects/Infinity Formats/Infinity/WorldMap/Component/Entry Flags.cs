using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>The entry point flags for an area link</summary>
    public enum EntryFlags : uint /* UInt32 */
    {
        North   = 1U,
        East    = 2U,
        South   = 4U,
        West    = 8U,
    }
}