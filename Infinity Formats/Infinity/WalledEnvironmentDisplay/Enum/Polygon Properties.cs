using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Enum
{
    /// <summary>Enum indicating various polygon flags</summary>
    [Flags]
    public enum PolygonProperties : byte /* Byte */
    {
        Wall            = 0x01,
        Hovering        = 0x02,

        [Description("Semi-transparent")]
        SemiTransparent = 0x04,
        Covering        = 0x08,
        Door            = 0x80
    }
}