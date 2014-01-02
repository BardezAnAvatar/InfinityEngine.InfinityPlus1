using System;
using System.ComponentModel;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>Flags for the map area</summary>
    [Flags]
    public enum MapAreaFlags : uint /* UInt32 */
    {
        /// <summary>Always visible on map</summary>
        [Description("Always visible on map")]
        Visible = 1U,

        /// <summary>Visible on map from adjacent area</summary>
        [Description("Visible on map from adjacent area")]
        VisibleByAdjacent = 2U,

        Reachable = 4U,

        Visited = 8U,
    }
}