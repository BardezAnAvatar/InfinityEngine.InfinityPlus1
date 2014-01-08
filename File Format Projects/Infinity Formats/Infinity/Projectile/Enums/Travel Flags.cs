using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Projectile.Enums
{
    /// <summary>Various travel flags</summary>
    [Flags]
    public enum TravelFlags : uint /* UInt32 */
    {
        /// <summary>Use the specified palette.</summary>
        /// <remarks>BG2 source code calls this FALSECOLORED</remarks>
        [Description("Use the specified palette.")]
        UseSpecifiedPalette         = 1U,

        Smokes                      = 2U,

        /// <summary>Use the specified palette for smoke</summary>
        /// <remarks>IESDP still says this s unused. I think it means to use the pallette for smoke as well</remarks>
        [Description("Use the specified palette for smoke")]
        SmokeUsesSpecifiedPalette   = 4U,

        /// <summary>Uses tint from area.</summary>
        [Description("Uses tint from area.")]
        UseTint                     = 8U,

        /// <summary>Uses height map.</summary>
        [Description("Uses height map.")]
        UseHeightMap                = 16U,
        
        /// <summary>Has a shadow BAM.</summary>
        [Description("Has a shadow BAM.")]
        HasShadow                   = 32U,

        /// <summary>Has a light shadow.</summary>
        [Description("Has a light shadow.")]
        HasLightShadow              = 64U,

        Blend                       = 128U,
        
        /// <summary>Low-level brightening.</summary>
        [Description("Low-level brightening.")]
        BrightenLevelLow            = 256U,
        
        /// <summary>High-level brightening.</summary>
        [Description("High-level brightening.")]
        BrightenLevelHigh           = 512U,
    }
}