using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.Tools.Controllers.ApproachingInfinity
{
    /// <summary>This enum indicates the types of views available</summary>
    public enum AssetTree
    {
        /// <summary>Displays only the assets as they are overridden; only single instances returned; grouped by their extension</summary>
        OverriddenGrouped,

        /// <summary>Displays all assets found, grouped by their extension </summary>
        AllInstancesGrouped,

        /// <summary>Displays all assets found, in the location found</summary>
        AllInstancesLocation,

        /// <summary>Displays only the overridden assets, grouped by their type in the location that the highest priority was found</summary>
        OverriddenGroupedLocation,

        /// <summary>Displays all assets in their locations grouped by their type</summary>
        AllInstancesGroupedLocation,
    }
}