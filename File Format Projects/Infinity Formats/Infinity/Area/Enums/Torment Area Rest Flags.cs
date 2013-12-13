using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags matching AREAFLAG.IDS (plus a few more flags), for PS:T</summary>
    /// <remarks>Seem to indicate whether rest is allowed, only</remarks>
    [Flags]
    public enum TormentAreaRestFlags : uint /* UInt32 */
    {
        /// <summary>You are allowed to rest here.</summary>
        [Description("You are allowed to rest here.")]
        RestAllowed                 = 1U,

        /// <summary>You are allowed to rest here. Duplicate flag.</summary>
        [Description("You are allowed to rest here. Duplicate flag.")]
        RestAllowedDupe             = 2U,

        /// <summary>Raises the message "You cannot rest here."</summary>
        [Description("Raises the message \"You cannot rest here.\"")]
        CannotRest                  = 4U,

        /// <summary>Raises the message "You cannot rest here." Duplicate flag.</summary>
        [Description("Raises the message \"You cannot rest here.\" Duplicate flag.")]
        CannotRestDupe              = 8U,

        /// <summary>Raises the message "You cannot rest right now."</summary>
        [Description("Raises the message \"You cannot rest right now.\"")]
        CannotRestRightNow          = 16U,

        /// <summary>Raises the message "You cannot rest right now." Duplicate flag.</summary>
        [Description("Raises the message \"You cannot rest right now.\" Duplicate flag.")]
        CannotRestRightNowDupe      = 32U,

        /// <summary>Raises the message "You must obtain permission to rest here."</summary>
        [Description("Raises the message \"You must obtain permission to rest here.\"")]
        RestNeedPermission          = 64U,

        /// <summary>Raises the message "You must obtain permission to rest here." Duplicate flag.</summary>
        [Description("Raises the message \"You must obtain permission to rest here.\" Duplicate flag.")]
        RestNeedPermissionDupe      = 128U,
    }
}