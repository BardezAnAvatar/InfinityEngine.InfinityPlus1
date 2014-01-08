using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for animations</summary>
    [Flags]
    public enum AnimationFlags : uint /* UInt32 */
    {
        Enabled                         = 1U,

        /// <summary>Black rendered as transparent</summary>
        /// <remarks>if unset, black is rendered as black; BG2 source code calls this flag "glowing"</remarks>
        [Description("Black rendered as transparent")]
        BlackIsTransparent              = 2U,

        /// <summary>Not illuminated</summary>
        /// <remarks>Lightmap does not affect the animation</remarks>
        [Description("Not illuminated")]
        NotIlluminated                  = 4U,

        /// <remarks>IESDP indicates that the animation stops at end frame</remarks>
        Stopped                         = 8U,

        /// <summary>Start At Beginning</summary>
        /// <remarks>Animation starts with first frame (versus random frame)</remarks>
        [Description("Start At Beginning")]
        StartAtBeginning                = 16U,

        /// <summary>Use Start Range</summary>
        /// <remarks>IESDP suggests if not set, used frame 0, otherwise a random one</remarks>
        [Description("Use Start Range")]
        UseStartRange                   = 32U,

        /// <summary>Ignore Clipping</summary>
        /// <remarks>BG2 source code revealed this flag</remarks>
        [Description("Ignore Clipping")]
        IgnoreClipping                  = 64U,

        /// <summary>Disable On Low Performance</summary>
        /// <remarks>BG2 source code: "DISABLEONSLOWMACHINES"</remarks>
        [Description("Disable On Low Performance")]
        DisableOnLowPerformance         = 128U,

        /// <summary>Drawn before actors</summary>
        /// <remarks>BG2 source code calls this "blacklist"</remarks>
        [Description("Drawn before actors")]
        DrawBeforeActors                = 256U,

        /// <summary>Play all frames</summary>
        [Description("Play all frames")]
        PlayAllFrames                   = 512U,

        /// <summary>Uses a BMP palette</summary>
        [Description("Uses a BMP palette")]
        UseBitmapPalette                = 1024U,

        Mirrored                        = 2048U,

        /// <summary>Shown in combat</summary>
        [Description("Shown in combat")]
        ShowInCombat                    = 4096U,
    }
}