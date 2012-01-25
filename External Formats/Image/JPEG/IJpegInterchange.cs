using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Provides a common base for JPEG Interchanges, providing I ever want to implement EXIF</summary>
    public interface IJpegInterchange
    {
        /// <summary>Merges the MCU data read in into the individual components collection.</summary>
        /// <param name="frame">JPEG frame containing reference data</param>
        /// <param name="scan">JPEG scan to merge the data of</param>
        /// <remarks>
        ///     If components are spread throughout scans, merges them into one collection.
        ///     If progressive, merges multiple scans' component data into one collection.
        /// </remarks>
        void MergeScanData(JpegFrame frame, JpegScan scan);

        /// <summary>Represents the component data of the interchange, compiled from scans</summary>
        Dictionary<Int32, ComponentDataInteger> ComponentData { get; set; }
    }
}