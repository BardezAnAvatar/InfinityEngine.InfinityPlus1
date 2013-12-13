using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.VectoredEffectVideoCell.Enums
{
    /// <summary>Various display flags</summary>
    [Flags]
    public enum DisplayFlags : ushort /* UInt16 */
    {
        Transparent                 = 1,
        Translucent                 = 2,

        /// <summary>Translucent shadow</summary>
        [Description("Translucent shadow")]
        TranslucentShadow           = 4,
        
        Blended                     = 8,

        /// <summary>Mirror along X axis</summary>
        [Description("Mirror along X axis")]
        MirrorAxisX                 = 16,

        /// <summary>Mirror along Y axis</summary>
        [Description("Mirror along Y axis")]
        MirrorAxisY                 = 32,

        Clipped                     = 64,

        /// <summary>Copy from back</summary>
        [Description("Copy from back")]
        CopyFromBack                = 128,

        /// <summary>Clear fill</summary>
        [Description("Clear fill")]
        ClearFill                   = 256,

        /// <summary>3D Blending</summary>
        [Description("3D Blending")]
        Blend3D                     = 512,

        /// <summary>Render above walls</summary>
        /// <remarks>Do not dither for walls</remarks>
        [Description("Render above walls")]
        RenderAboveWall             = 1024,

        /// <summary>Ignore Timestop palette</summary>
        [Description("Ignore Timestop palette")]
        IgnoreDreamTimeStopPalette  = 2048,

        /// <summary>Ignore dream palette</summary>
        [Description("Ignore dream palette")]
        IgnoreDreamPalette          = 4096,

        /// <summary>2D Blending</summary>
        [Description("2D Blending")]
        Blend2D                     = 8192,
    }
}