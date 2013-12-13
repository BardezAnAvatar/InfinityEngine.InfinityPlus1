using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for spawning</summary>
    [Flags]
    public enum Icewind2SpawnFlags : ushort /* UInt32 */
    {
        /// <summary>During rest.</summary>
        [Description("During rest.")]
        OnRest   = 1,

        /// <summary>When fog of war revealed.</summary>
        [Description("When fog of war revealed.")]
        OnRevealed     = 2,
    }
}