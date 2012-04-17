using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags matching AREAFLAG.IDS (plus a few more flags), for IWD2</summary>
    [Flags]
    public enum Icewind2AreaFlags : uint /* UInt32 */
    {
        /// <summary>Saves is prohibited in this area.</summary>
        [Description("Saves is prohibited in this area.")]
        SaveDisabled        = 1U,

        /// <summary>Resting is prohibited in this area.</summary>
        [Description("Resting is prohibited in this area.")]
        RestDisabled        = 2U,

        /// <summary>Locks battle music.</summary>
        [Description("Locks battle music.</.")]
        LockBattleMusic     = 8U,
    }
}