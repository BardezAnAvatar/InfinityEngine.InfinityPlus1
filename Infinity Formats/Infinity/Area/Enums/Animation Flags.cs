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
        /// <remarks>if unset, black is rendered as black</remarks>
        [Description("Black rendered as transparent")]
        BlackIsTransparent              = 2U,

        /// <summary>Not illuminated</summary>
        /// <remarks>Lightmap does not affect the animation</remarks>
        [Description("Not illuminated")]
        NotIlluminated                  = 4U,

        /// <summary>Played only once</summary>
        /// <remarks>Animation stops at end frame</remarks>
        [Description("Played only once")]
        PlayOnce                        = 8U,

        /// <summary>Synchronized draw</summary>
        /// <remarks>Skips frames as needed</remarks>
        [Description("Synchronized draw")]
        SyncDraw                        = 16U,

        /// <summary>Not hidden by walls</summary>
        /// <remarks>Drawn after walls/doors?</remarks>
        [Description("Not hidden by walls")]
        NotHiddenByWalls                = 64U,

        /// <summary>Invisible in fog of war</summary>
        /// <remarks>Not shown when obscured by fog of war</remarks>
        [Description("Invisible in fog of war")]
        InvisibleInFogOfWar             = 128U,

        /// <summary>Drawn before actors</summary>
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