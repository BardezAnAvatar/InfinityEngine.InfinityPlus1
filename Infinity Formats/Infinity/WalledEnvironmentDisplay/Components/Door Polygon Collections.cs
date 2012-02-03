using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components
{
    /// <summary>Contains a list of polygon collections for doors, both open and closed</summary>
    public class DoorPolygonCollections
    {
        #region Fields
        /// <summary>Collection of open polygons</summary>
        public List<Polygon> Open { get; set; }

        /// <summary>Collection of losed polygons</summary>
        public List<Polygon> Closed { get; set; }
        #endregion


        #region Construction
        /// <summary>Default contructor</summary>
        public DoorPolygonCollections()
        {
            this.Open = new List<Polygon>();
            this.Closed = new List<Polygon>();
        }
        #endregion
    }
}