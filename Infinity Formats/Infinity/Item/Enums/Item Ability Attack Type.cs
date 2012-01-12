using System;
using System.ComponentModel;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums
{
    /// <summary>This enum is a flag bitfield for item flags</summary>
    [Flags]
    public enum ItemAbilityAttackType : ushort /* UInt16 */
    {
        ///// <summary>Normal attacks</summary>
        //Normal      = 0x00,

        /// <summary>Touch attack for D20?; "Ignore shield" in DLTCEP</summary>
        [Description("Bypass armor")]
        BypassArmor = 0x01,

        /// <summary>Doubles the critical hit range (i.e.: 18-20 -> 15-20</summary>
        Keen        = 0x02
    }
}