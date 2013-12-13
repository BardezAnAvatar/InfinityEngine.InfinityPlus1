using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.Enums
{
    /// <summary>Represents the collection of orientation values available to characters</summary>
    public enum Direction : ushort /* UInt16 */
    {
        South = 0,

        /// <summary>South by south-west</summary>
        [Description("South by south-west")]
        SouthSouthWest = 1,

        /// <summary>South-west</summary>
        [Description("South-west")]
        SouthWest = 2,

        /// <summary>West by south-west</summary>
        [Description("West by south-west")]
        WestSouthWest = 3,

        West = 4,

        /// <summary>West by north-west</summary>
        [Description("West by north-west")]
        WestNorthWest = 5,

        /// <summary>North-west</summary>
        [Description("North-west")]
        NorthWest = 6,

        /// <summary>North by north-west</summary>
        [Description("North by north-west")]
        NorthNorthWest = 7,

        North = 8,

        /// <summary>North by north-east</summary>
        [Description("North by north-east")]
        NorthNorthEast = 9,

        /// <summary>North-east</summary>
        [Description("North-east")]
        NorthEast = 10,

        /// <summary>East by north-east</summary>
        [Description("East by north-east")]
        EastNorthEast = 11,

        East = 12,

        /// <summary>East by south-east</summary>
        [Description("East by south-east")]
        EastSouthEast = 13,

        /// <summary>South-east</summary>
        [Description("South-east")]
        SouthEast = 14,
        
        /// <summary>South by south-east</summary>
        [Description("South by south-east")]
        SouthSouthEast = 15,
    }
}