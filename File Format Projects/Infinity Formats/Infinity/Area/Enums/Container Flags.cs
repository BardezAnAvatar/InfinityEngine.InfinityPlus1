using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for containers</summary>
    [Flags]
    public enum ContainerFlags : uint /* UInt32 */
    {
        Locked          = 1U,

        /// <summary>Trap resets</summary>
        [Description("Trap resets")]
        TrapResets      = 8U,

        Disabled        = 32U,

        /// <summary>Disabled (PS:T)</summary>
        [Description("Disabled (PS:T)")]
        DisabledPst     = 128U,
    }
}