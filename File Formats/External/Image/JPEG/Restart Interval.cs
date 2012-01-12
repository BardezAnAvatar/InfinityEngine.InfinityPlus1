using System;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG
{
    /// <summary>Represents the integer number of MCUs processed as an independent sequence within a scan.</summary>
    public class RestartInterval : MarkerSegment
    {
        #region Properties
        /// <summary>The number of MCU in the restart interval</summary>
        public UInt16 Interval { get; set; }
        #endregion

        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Specifies the length of the parameters</param>
        /// <param name="interval">Specifies the number of MCU in the restart interval.</param>
        public RestartInterval(UInt16 marker, UInt16 length, UInt16 interval)
        {
            this.Marker = marker;
            this.Length = length;
            this.Interval = interval;
        }
        #endregion
    }
}