using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags matching AREATYPE.IDS (plus a few more flags), for BG, BG2, IWD, IWD2</summary>
    [Flags]
    public enum AreaTypeFlags : ushort /* UInt16 */
    {
        Outdoor             = 1,

        /// <summary>Has day/night rotation</summary>
        [Description("Has day/night rotation")]
        HasSolarLight       = 2,

        /// <summary>Exposed to weather</summary>
        [Description("Exposed to weather")]
        HasWeather          = 4,

        /// <summary>Is a city area</summary>
        [Description("Is a city area")]
        IsCity              = 8,

        /// <summary>Is a forest area</summary>
        [Description("Is a forest area")]
        IsForest            = 16,

        /// <summary>Is a dungeon area</summary>
        [Description("Is a dungeon area")]
        IsDungeon           = 32,

        /// <summary>Has a long night</summary>
        /// <remarks>This is more like a permanent night flag...</remarks>
        [Description("Has a long night")]
        HasLongNight        = 64,

        /// <summary>Rest is allowed indoors</summary>
        [Description("Rest is allowed indoors")]
        IndoorRestAllowed   = 128,
    }
}