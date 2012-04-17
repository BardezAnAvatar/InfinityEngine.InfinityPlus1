using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WaveEffects
{
    /// <summary>Various wave effects flags</summary>
    public enum WaveEffectFlag : uint /* UInt32 */
    {
        /// <summary>Play sound as if during a cutscene</summary>
        /// <remarks>Music by 50%, all channels 25%</remarks>
        [Description("Play sound as if during a cutscene")]
        PlaySoundLikeCutscene       = 1,

        /// <summary>Use curve radius</summary>
        [Description("Use curve radius")]
        UseCurveRadius              = 2,

        /// <summary>Use random frequency variation</summary>
        [Description("Use random frequency variation")]
        UseRandomFrequencyVariation = 4,

        /// <summary>Use random volume variation</summary>
        [Description("Use random volume variation")]
        UseRandomVolumeVariation    = 8,

        /// <summary>Disable environmental audio effects (EAX)</summary>
        [Description("Disable environmental audio effects (EAX)")]
        DisableEAX                  = 16,
    }
}