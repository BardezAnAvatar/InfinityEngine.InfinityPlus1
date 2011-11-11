using System;

using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Output.Audio.Enums;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>Keeps a Voice reference and some state data</summary>
    internal class XAudio2VoiceReference
    {
        /// <summary>Reference to the voice object</summary>
        internal Voice Reference { get; set; }

        /// <summary>Indicates the current state of the Voice</summary>
        internal XAudio2VoiceState State { get; set; }

        /// <summary>Indicates whether ot not an output voice has been set for this voice.</summary>
        internal Boolean OutputVoiceSet { get; set; }

        /// <summary>Defintion constructor</summary>
        /// <param name="voiceInstance">Instance of a voice object to reference</param>
        /// <remarks>Expects the reference to be valid and virgin, unexposed to playback so far</remarks>
        internal XAudio2VoiceReference(Voice voiceInstance)
        {
            this.Reference = voiceInstance;
            this.State = XAudio2VoiceState.NotSubmitted;
            this.OutputVoiceSet = false;
        }
    }
}