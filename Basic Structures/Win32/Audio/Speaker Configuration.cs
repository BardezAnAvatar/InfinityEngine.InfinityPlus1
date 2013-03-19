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
    }
}