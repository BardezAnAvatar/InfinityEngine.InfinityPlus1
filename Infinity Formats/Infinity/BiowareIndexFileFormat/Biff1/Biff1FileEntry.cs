using System;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Common;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Globals;

namespace Bardez.Projects.InfinityPlus1.Files.Infinity.BiowareIndexFileFormat.Biff1
{
    /// <summary>
    ///     This class is an abstract entity for shared members between the
    ///     tileset resource1 entry and the regular resource1 entry.
    /// </summary>
    public abstract class Biff1FileEntry
    {
        #region Members
        /// <summary>
        ///     The resource1 locator to match to the KEY file entry.
        ///     Only the subtype is matched upon, depending on the resource1 type.
        /// </summary>
        protected ResourceLocator1 resourceLocator;

        /// <summary>Offset within the BIFF (from teh start of the file) to the start of the resource1 data.</summary>
        protected UInt32 offsetResource;

        /// <summary>Short indicating the type of the resource1</summary>
        protected ResourceType typeResource;

        /// <summary>Unknown data field assumed to be binary padding to wrap the structure to 4-byte size</summary>
        protected UInt16 unknownPadding; 
	    #endregion
        
        #region Properties
        /// <summary>
        ///     The resource1 locator to match to the KEY file entry.
        ///     Only the subtype is matched upon, depending on the resource1 type.
        /// </summary>
        public ResourceLocator1 ResourceLocator
        {
            get { return this.resourceLocator; }
            set { this.resourceLocator = value; }
        }

        /// <summary>Offset within the BIFF (from teh start of the file) to the start of the resource1 data.</summary>
        public UInt32 OffsetResource
        {
            get { return this.offsetResource; }
            set { this.offsetResource = value; }
        }

        /// <summary>Int16 indicating the type of the resource1</summary>
        public ResourceType TypeResource
        {
            get { return this.typeResource; }
            set { this.typeResource = value; }
        }

        /// <summary>Unknown data field assumed to be binary padding to wrap the structure to 4-byte size</summary>
        protected UInt16 UnknownPadding
        {
            get { return this.unknownPadding; }
            set { this.unknownPadding = value; }
        }

        public virtual UInt32 SizeResource
        {
            get { return 0U; }
        }
        #endregion
    }
}