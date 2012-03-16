using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    public class MveAudioChannelParams
    {
        /// <summary>Exposes the number of channels in the audio stream</summary>
        public Int32 Channels { get; set; }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public Int32 SampleSize { get; set; }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public Boolean Compressed { get; set; }

        /// <summary>Represents  the sample rate of the audio buffer</summary>
        public Int32 SampleRate { get; set; }
    }
}