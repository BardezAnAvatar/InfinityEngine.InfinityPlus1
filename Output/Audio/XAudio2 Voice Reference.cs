using System;

using Bardez.Projects.DirectX.XAudio2;
using Bardez.Projects.InfinityPlus1.Output.Audio.Enums;

namespace Bardez.Projects.InfinityPlus1.Output.Audio
{
    /// <summary>Keeps a Voice reference and some state data</summary>
    internal class XAudio2VoiceReference<VoiceType> : IDisposable where VoiceType : Voice
    {
        /// <summary>Reference to the voice object</summary>
        internal VoiceType Reference { get; set; }

        /// <summary>Indicates the current state of the Voice</summary>
        internal XAudio2VoiceState State { get; set; }

        /// <summary>Indicates whether ot not an output voice has been set for this voice.</summary>
        internal Boolean OutputVoiceHasBeenSet { get; set; }

        /// <summary>Defintion constructor</summary>
        /// <param name="voiceInstance">Instance of a voice object to reference</param>
        /// <remarks>Expects the reference to be valid and virgin, unexposed to playback so far</remarks>
        internal XAudio2VoiceReference(VoiceType voiceInstance)
        {
            this.Reference = voiceInstance;
            this.State = XAudio2VoiceState.NotSubmitted;
            this.OutputVoiceHasBeenSet = false;
        }

        #region Destruction
        /// <summary>Disposal code</summary>
        public virtual void Dispose()
        {
            this.Disposal();
        }

        /// <summary>Disposal code</summary>
        /// <remarks>Finalize()</remarks>
        ~XAudio2VoiceReference()
        {
            this.Disposal();
        }

        /// <summary>Method to indicate the underlying type of the </summary>
        /// <param name="voiceType">Type to compare the reference instance type with</param>
        /// <returns>Boolean indicating type equality</returns>
        internal Boolean ReferenceIsOfType(Type voiceType)
        {
            Boolean isType = false;

            if (this.Reference != null)
                isType = (this.Reference.GetType() == voiceType);

            return isType;
        }

        /// <summary>Disposal code</summary>
        protected virtual void Disposal()
        {
            this.State = XAudio2VoiceState.Depleted;
            if (this.Reference != null)
            {
                this.Reference.Dispose();
                this.Reference = null;
            }
            this.OutputVoiceHasBeenSet = false;
        }
        #endregion
    }
}