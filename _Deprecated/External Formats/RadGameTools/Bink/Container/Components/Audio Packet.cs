using System;

using Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Audio;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.RadGameTools.Bink.Container.Components
{
    public class AudioPacket
    {
        #region Fields
        /// <summary>Represents the length of structure after this fied</summary>
        public Int32 RemainingStructSize { get; set; }

        /// <summary>Number of samples in this audio packet</summary>
        public Int32 CountSamples { get; set; }

        /// <summary>Audio frame data</summary>
        public AudioFrame Frame { get; set; }
        #endregion
    }
}