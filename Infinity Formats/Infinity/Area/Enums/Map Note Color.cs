using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Represents the color for a map note</summary>
    public enum MapNoteColor : ushort /* UInt16 */
    {
        Gray        = 0,
        Violet      = 1,
        Green       = 2,
        Orange      = 3,
        Red         = 4,
        Blue        = 5,

        /// <summary>Dark Blue</summary>
        [Description("Dark Blue")]
        DarkBlue    = 6,

        /// <summary>Light Gray</summary>
        [Description("Light Gray")]
        LightGray   = 7,
    }
}