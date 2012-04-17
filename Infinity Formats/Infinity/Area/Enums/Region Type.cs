using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Enums
{
    /// <summary>Flags for bounding box regions</summary>
    public enum RegionType : ushort /* UInt16 */
    {
        /// <summary>Proximity Trigger</summary>
        [Description("Proximity Trigger")]
        ProximityTrigger    = 0,

        /// <summary>Informational</summary>
        [Description("Informational")]
        InfoPoint           = 1,

        /// <summary>Travel region</summary>
        [Description("Travel region")]
        TravelRegion        = 2,
    }
}