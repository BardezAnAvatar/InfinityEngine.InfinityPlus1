using System;
using System.Collections.Generic;

using Bardez.Projects.InfinityPlus1.Utility;

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


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is DoorVertexIndexCollections)
                {
                    DoorVertexIndexCollections compare = obj as DoorVertexIndexCollections;
                    equal = this.Open.Equals<Int16>(compare.Open);
                    
                    if (equal)
                        equal = this.Closed.Equals<Int16>(compare.Closed);
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.Open.GetHashCode<Int16>();
            hash ^= this.Closed.GetHashCode<Int16>();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}