using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for ambient sounds</summary>
    [Flags]
    public enum AmbientFlags : uint /* UInt32 */
    {
        Enabled                         = 1U,

        /// <summary>Environmental effects are disabled.</summary>
        [Description("Environmental effects are disabled.")]
        DisabledEnvironmentalEffects    = 2U,

        /// <summary>Global sound.</summary>
        [Description("Global sound.")]
        GlobalSound                     = 4U,

        /// <summary>Random selection.</summary>
        [Description("Random selection.")]
        RandomSelection                 = 8U,

        /// <summary>Low memory.</summary>
        [Description("Low memory.")]
        LowMemory                       = 16U,
    }
}