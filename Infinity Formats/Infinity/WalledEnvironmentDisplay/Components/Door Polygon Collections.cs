using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.Utility;

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


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is DoorPolygonCollections)
                {
                    DoorPolygonCollections compare = obj as DoorPolygonCollections;
                    equal = this.Open.Equals<Polygon>(compare.Open);
                    
                    if (equal)
                        equal = this.Closed.Equals<Polygon>(compare.Closed);
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }
        #endregion
    }
}