using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.Files.External.Image.JPEG.Enums
{
    /// <summary>Represents pixel density measurments</summary>
    public enum DensityUnit : byte /* Byte */
    {
        /// <summary>No units, aspect ratio only specified</summary>
        None = 0,

        /// <summary>Pixels per inch</summary>
        [Description("Pixels per inch")]
        PixelsPerInch,

        /// <summary>Pixels per centimeter</summary>
        [Description("Pixels per centimeter")]
        PixelsPerCentimeter,
    }
}