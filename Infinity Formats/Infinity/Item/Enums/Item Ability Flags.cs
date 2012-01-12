using System;
using System.ComponentModel;

using Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.Item.Enums
{
    /// <summary>This enum is a flag bitfield for item ability flags</summary>
    [Flags]
    public enum ItemAbilityFlags : ushort /* UInt16 */
    {
        [Description("Add strength bonus (to damage?)")]
        AddStrength     = 0x0001,

        Breakable       = 0x0002,

        //Near Infinity says no=1, byte2 = 256
        //No            = 0x0100,

        //IESDP suggests avalue of 4 for hostile, but this is presently unknown.
        Hostile         = 0x0400,

        [Description("Recharges after resting")]
        AfterRest       = 0x0800
    }
}