using System;

namespace Bardez.Projects.InfinityPlus1.Output.Audio.Enums
{
    /// <summary>Enumeration of states for a Voice instance</summary>
    internal enum XAudio2VoiceState
    {
        /// <summary>The Voice is initialized, but has not yet been used by any submission.</summary>
        NotSubmitted,

        /// <summary>The Voice has been referenced and can be considered in use, but has been given the indication that it will be disposed after currently ubmitted playbacks have finished.</summary>
        InUseEnding,

        /// <summary>The Voice has been referenced and can be considered in use, and can be expected to receive further data later during execution.</summary>
        InUsePersisting,

        /// <summary>The Source Voice has been referenced and used, and was populated with flags suggesting that the voice should expect no more data and can now be removed.</summary>
        Depleted
    }
}