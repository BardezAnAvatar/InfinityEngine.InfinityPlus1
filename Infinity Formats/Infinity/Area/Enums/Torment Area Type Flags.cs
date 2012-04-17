using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags matching AREATYPE.IDS (plus a few more flags), for PS:T</summary>
    [Flags]
    public enum TormentAreaTypeFlags : ushort /* UInt16 */
    {
        Hive                = 1,

        /// <summary>Hive (at night?)</summary>
        [Description("Hive (at night?)")]
        HiveNight           = 2,

        /// <summary>Clerk's Ward</summary>
        [Description("Clerk's Ward")]
        ClerksWard          = 4,

        /// <summary>Lower Ward</summary>
        [Description("Lower Ward")]
        LowerWard           = 8,

        /// <summary>Ravel's Maze</summary>
        [Description("Ravel's Maze")]
        RavelsMaze          = 16,

        Baator              = 32,

        /// <summary>Modron Cube</summary>
        [Description("Modron Cube")]
        ModronCube          = 64,

        /// <summary>Fortress of Regrets</summary>
        [Description("Fortress of Regrets")]
        FortressOfRegrets   = 128,

        Curst               = 256,

        Carceri             = 512,

        Outdoors            = 1024,
    }
}