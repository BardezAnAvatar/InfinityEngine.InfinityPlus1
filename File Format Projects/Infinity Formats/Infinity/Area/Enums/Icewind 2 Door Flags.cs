using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for doors</summary>
    [Flags]
    public enum Icewind2DoorFlags : uint /* UInt32 */
    {
        Open = 1U,

        Locked = 2U,

        /// <summary>Trap resets</summary>
        [Description("Trap resets")]
        TrapResets = 4U,

        /// <summary>Trap is detectable.</summary>
        [Description("Trap is detectable.")]
        TrapDetectable = 8U,

        /// <summary>Door has been bashed.</summary>
        [Description("Door has been bashed.")]
        Bashed = 16U,

        /// <summary>Door cannot be closed.</summary>
        [Description("Door cannot be closed.")]
        CannotClose = 32U,

        /// <remarks>how is this different than discovered?</remarks>
        Detected = 64U,

        Secret = 128U,

        /// <remarks>...if hidden</remarks>
        Found = 512U,

        /// <summary>Has locked information text?</summary>
        [Description("Door has text associated with lock.")]
        LockedInfoText = 1024U,

        /// <remarks>Doesn't block line of sight</remarks>
        Transparent = 2048U,

        /// <summary>Has warning information text?</summary>
        [Description("Door has text associated warning text.")]
        WarningInfoText = 4096U,

        /// <summary>Has warning information text displayed?</summary>
        [Description("Door has text associated warning text displayed.")]
        DisplayWarningInfoText = 8192,

        Hidden = 16384,

        /// <summary>Destroys related key item.</summary>
        [Description("Destroys related key item.")]
        DestroysKey = 32768,
    }
}