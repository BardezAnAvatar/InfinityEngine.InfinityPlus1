using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Enum
{
    /// <summary>Enum that describes which overlay layers to render</summary>
    [Flags]
    public enum WedOverlayRender : byte /* Byte */
    {
        /// <summary>Render layer 1</summary>
        Overlay1 = 0x02,

        /// <summary>Render layer 2</summary>
        Overlay2 = 0x04,

        /// <summary>Render layer 3</summary>
        Overlay3 = 0x08,

        /// <summary>Render layer 4</summary>
        Overlay4 = 0x10,

        /// <summary>Render layer 5</summary>
        Overlay5 = 0x20,

        /// <summary>Render layer 6</summary>
        Overlay6 = 0x40,

        /// <summary>Render layer 7</summary>
        Overlay7 = 0x80,
    }
}