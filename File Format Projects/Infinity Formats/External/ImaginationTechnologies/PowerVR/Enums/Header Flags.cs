using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.ImaginationTechnologies.PowerVR.Enums
{
    /// <summary>Any flags set for a PVR file</summary>
    [Flags]
    public enum HeaderFlags : uint /* UInt32 */
    {
        None = 0U,

        /// <summary>Pre-multiplied alpha values</summary>
        [Description("Pre-multiplied alpha values")]
        PreMultiplied = 2U,
    }
}