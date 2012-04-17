using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for a trigger</summary>
    [Flags]
    public enum TriggerFlags : uint /* UInt32 */
    {
        Invisible           = 1U,

        /// <summary>Trap resets itself</summary>
        [Description("Trap resets itself")]
        ResetTrap           = 1U,

        /// <summary>Party required for travel</summary>
        [Description("Party required for travel")]
        PartyRequired       = 4U,

        /// <summary>Trap can be detected</summary>
        [Description("Trap can be detected")]
        Detectable          = 8U,

        /// <summary>Unknown, related to low memory</summary>
        [Description("Unknown, related to low memory")]
        LowMemory           = 32U,

        /// <summary>NPCs can trigger</summary>
        [Description("NPCs can trigger")]
        NpcTriggerable      = 64U,

        Deactivated         = 256U,

        /// <summary>Travel region for PNCs</summary>
        [Description("Travel region for PNCs")]
        TravelNonPC         = 512U,

        /// <summary>Override usage point of Travel regions for PS:T traps</summary>
        [Description("Override usage point of Travel regions for PS:T traps")]
        UsePoint            = 1024U,

        /// <summary>Info trigger, blocked by door</summary>
        [Description("Info trigger, blocked by door")]
        InfoBlockedDoor     = 2048U,
    }
}