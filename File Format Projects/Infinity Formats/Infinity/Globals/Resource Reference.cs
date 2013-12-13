using System;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals
{
    /// <summary>
    ///     This public class represents a resource reference within the Infinity Engine Game files.
    ///     It is typically an 8-byte string in official Infinity Engine games.
    /// </summary>
    public class ResourceReference
    {
        #region Fields
        /// <summary>This member represents the resource reference string</summary>
        public String ResRef { get; set; }

        /// <summary>This member represents the type of resource referenced</summary>
        private ResourceType Type { get; set; }
        #endregion


        #region Properties
        /// <summary>This public property gets or sets the NULL-terminated representation of the resource reference string</summary>
        public String ZResRef
        {
            get { return ZString.GetZString(this.ResRef); }
            set { this.ResRef = ZString.GetZString(value); }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ResourceReference() { }

        /// <summary>Default constructor</summary>
        /// <param name="type">Resource type of this resource</param>
        public ResourceReference(ResourceType type)
        {
            this.Type = type;
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
                if (obj != null && obj is ResourceReference)
                {
                    ResourceReference compare = obj as ResourceReference;

                    Boolean structureEquality = (this.ZResRef == compare.ZResRef);
                    structureEquality &= (this.Type == compare.Type);

                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.ZResRef.GetHashCode();
            hash ^= this.Type.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}