using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Component.Management
{
    /// <summary>This class holds basic Audio parameters for an MVE file</summary>
    public class MveAudioChannelParams
    {
        #region Fields
        /// <summary>Exposes the number of channels in the audio stream</summary>
        public Int32 Channels { get; set; }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public Int32 SampleSize { get; set; }

        /// <summary>Exposes the size of samples in the audio stream</summary>
        public Boolean Compressed { get; set; }

        /// <summary>Represents  the sample rate of the audio buffer</summary>
        public Int32 SampleRate { get; set; }

        /// <summary>Represents the chunk number in which the opcode to Initialize Audio buffers fisr appears</summary>
        public Int32 StartFrame { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveAudioChannelParams()
        {
            this.StartFrame = -1;
        }
        #endregion
    }
}