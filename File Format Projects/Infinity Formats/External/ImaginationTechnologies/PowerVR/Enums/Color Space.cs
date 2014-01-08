using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums
{
    /// <summary>Possible PVR color space values</summary>
    public enum ColorSpace : uint /* UInt32 */
    {
        /// <summary>Linear RGB</summary>
        [Description("Linear RGB")]
        LinearRGB       = 0,

        /// <summary>Standard RGB</summary>
        [Description("Standard RGB")]
        StandardRGB     = 1,
    }
}