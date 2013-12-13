using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents the restart index between 0 and 7 for MCUs.</summary>
    public class RestartMarker : MarkerSegment
    {
        #region Properties
        /// <summary>Returns the restart interval number of this restart.</summary>
        /// <value>Can be 0 through 7.</value>
        public Byte RestartNumber
        {
            get { return (Byte)(this.Marker & 0x7); }
        }
        #endregion

        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="marker">Marker of this substream</param>
        /// <param name="length">Specifies the length of the parameters</param>
        /// <param name="interval">Specifies the number of MCU in the restart interval.</param>
        public RestartMarker(UInt16 marker)
        {
            this.Marker = marker;
        }
        #endregion
    }
}