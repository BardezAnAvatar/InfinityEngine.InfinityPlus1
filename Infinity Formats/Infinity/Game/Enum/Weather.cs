using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum
{
    /// <summary>Represents the bits for weather effects</summary>
    [Flags]
    public enum Weather : ushort /* UInt16 */
    {
        Rain        = 0x01,
        Snow        = 0x02,
        Fog         = 0x04,

        /// <summary>A Mask for the basic weather effects</summary>
        Mask        = 0x07,
        Lightning   = 0x08,

        /// <summary>Indicates whether weather is active in the area?</summary>
        [Description("Has weather")]
        HasWeather  = 0x40,

        /// <summary>Indicates whether or not weather is currently active?</summary>
        Active       = 0x80,
    }
}