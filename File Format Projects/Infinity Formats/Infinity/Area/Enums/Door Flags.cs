using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for doors</summary>
    [Flags]
    public enum DoorFlags : uint /* UInt32 */
    {
        Open            = 1U,

        Locked          = 2U,

        /// <summary>Trap resets</summary>
        [Description("Trap resets")]
        TrapResets      = 4U,

        /// <summary>Trap is detectable.</summary>
        [Description("Trap is detectable.")]
        TrapDetectable  = 8U,

        /// <summary>Door has been bashed.</summary>
        [Description("Door has been bashed.")]
        Bashed          = 16U,

        /// <summary>Door cannot be closed.</summary>
        [Description("Door cannot be closed.")]
        CannotClose     = 32U,

        /// <remarks>There is an info trigger linked to this door</remarks>
        Linked          = 64U,

        Hidden          = 128U,

        /// <remarks>...if hidden</remarks>
        Discovered      = 512U,

        /// <remarks>Doesn't block line of sight; BG2 source code calls this "Gate"</remarks>
        Transparent     = 1024U,

        /// <summary>Destroys related key item.</summary>
        [Description("Destroys related key item.")]
        DestroysKey     = 2048U,

        /// <remarks>Impeded blocks are ignored</remarks>
        Slide           = 4096U,
    }
}