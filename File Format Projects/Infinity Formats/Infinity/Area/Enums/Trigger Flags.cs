using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for a trigger</summary>
    [Flags]
    public enum TriggerFlags : uint /* UInt32 */
    {
        /// <summary>Key Must Be In Possession</summary>
        /// <remarks>IESDP suggested "Invisible"</remarks>
        [Description("Key Must Be In Possession")]
        KeyMustBeInPossession   = 1U,

        /// <summary>Trap resets itself</summary>
        [Description("Trap resets itself")]
        ResetTrap               = 2U,

        /// <summary>Party required for travel</summary>
        [Description("Party required for travel")]
        PartyRequired           = 4U,

        /// <summary>Trap can be detected</summary>
        [Description("Trap can be detected")]
        Detectable              = 8U,

        /// <summary>Trap can be triggered by enemies</summary>
        [Description("Trap can be triggered by enemies")]
        MonsterActiveated       = 16U,

        /// <summary>Active in tutorial area only</summary>
        [Description("Active in tutorial area only")]
        TutorialOnly            = 32U,

        /// <summary>NPCs can trigger</summary>
        [Description("NPCs can trigger")]
        NpcTriggerable          = 64U,

        /// <summary>No string</summary>
        [Description("No string")]
        NoString                = 128U,

        Deactivated             = 256U,

        /// <summary>Activated by party only</summary>
        [Description("Activated by party only")]
        PartyOnly               = 512U,

        /// <summary>Override usage point of Travel regions for PS:T traps</summary>
        [Description("Override usage point of Travel regions for PS:T traps")]
        UsePoint                = 1024U,

        /// <summary>Info trigger, blocked by door</summary>
        [Description("Info trigger, blocked by door")]
        InfoBlockedDoor         = 2048U,
    }
}