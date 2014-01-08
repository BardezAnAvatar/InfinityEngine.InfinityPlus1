using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for containers</summary>
    [Flags]
    public enum ContainerFlags : uint /* UInt32 */
    {
        Locked          = 1U,

        /// <summary>Disable If No Owner</summary>
        /// <remarks>Uncertain</remarks>
        [Description("Disable If No Owner")]
        DisableIfNoOwner    = 2U,

        /// <summary>Magically Locked</summary>
        [Description("Magically Locked")]
        MagicallyLocked     = 4U,

        /// <summary>Trap resets</summary>
        [Description("Trap resets")]
        TrapResets          = 8U,

        /// <summary>Remove Only</summary>
        [Description("Remove Only")]
        RemoveOnly          = 16U,

        Disabled            = 32U,

        /// <summary>Disabled (PS:T)</summary>
        [Description("Disabled (PS:T)")]
        DisabledPst         = 128U,
    }
}