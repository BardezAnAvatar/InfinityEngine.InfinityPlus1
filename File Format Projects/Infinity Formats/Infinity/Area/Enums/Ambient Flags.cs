using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for ambient sounds</summary>
    [Flags]
    public enum AmbientFlags : uint /* UInt32 */
    {
        Enabled                         = 1U,

        /// <summary>Looping</summary>
        /// <remarks>IESDP (still) says this is disable EAX effects</remarks>
        Looping                         = 2U,

        /// <summary>Global sound</summary>
        /// <remarks>BG2 source code calls this "not ranged"</remarks>
        [Description("Global sound")]
        GlobalSound                     = 4U,

        /// <summary>Random selection</summary>
        [Description("Random selection")]
        RandomSelection                 = 8U,

        /// <summary>Disabled if low memory</summary>
        [Description("Disabled if low memory</")]
        DisableIfLowMemory              = 16U,
    }
}