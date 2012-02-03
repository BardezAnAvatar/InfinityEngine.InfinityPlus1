using System;
using System.Collections.Generic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components
{
    /// <summary>Contains a list of polygons' vertex index collections for doors, both open and closed</summary>
    public class DoorVertexIndexCollections
    {
        #region Fields
        /// <summary>Collection of open indeces</summary>
        public List<Int16> Open { get; set; }

        /// <summary>Collection of losed indeces</summary>
        public List<Int16> Closed { get; set; }
        #endregion


        #region Construction
        /// <summary>Default contructor</summary>
        public DoorVertexIndexCollections()
        {
            this.Open = new List<Int16>();
            this.Closed = new List<Int16>();
        }
        #endregion
    }
}