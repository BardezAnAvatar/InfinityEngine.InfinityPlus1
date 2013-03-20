using System;

namespace Bardez.Projects.BasicStructures.Win32.Audio
{
    /// <summary>Represents a configuration of speakers</summary>
    public class SpeakerConfiguration
    {
        #region Fields
        /// <summary>Represents the positions of the speakers</summary>
        public SpeakerPositions Positions { get; set; }

        /// <summary>Represents the number of speakers represented</summary>
        public UInt32 Count { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public SpeakerConfiguration() { }

        /// <summary>Definition constructor</summary>
        /// <param name="channelCount">Count of speaker channels</param>
        /// <param name="positions">Positioning of speaker channels</param>
        public SpeakerConfiguration(UInt32 channelCount, SpeakerPositions positions)
        {
            this.Count = channelCount;
            this.Positions = positions;
        }
        #endregion
    }
}