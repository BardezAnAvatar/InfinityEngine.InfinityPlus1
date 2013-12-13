using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for spawning</summary>
    [Flags]
    public enum SpawnFlags : ushort /* UInt16 */
    {
        /// <summary>Halts spawning.</summary>
        [Description("Halts spawning.")]
        HaltSpawn   = 1,

        /// <summary>One-time spawn point.</summary>
        [Description("One-time spawn point.")]
        OneTime     = 2,

        /// <summary>Spawn has occurred.</summary>
        /// <remarks>
        ///     Set once a spawn ahs occurred.
        ///     Unset in the following conditions:
        ///     1) after the CompressTime() method for the spawn point is called with a time amount of at least 16 hours
        ///     2) if Bit 0 is not set and there are no living spawns in the area
        /// </remarks>
        [Description("Spawn has occurred.")]
        HasSpawned  = 4,
    }
}