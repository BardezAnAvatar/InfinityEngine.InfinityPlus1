using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common
{
    /// <summary>
    ///     This class is a representation of a resource1 locator, a bitfield used in BIFF version 1
    ///     and KEY version 1 files.
    /// </summary>
    /// <remarks>
    ///     From IESDP:
    ///     
    ///     bits 31-20: source index (the ordinal value giving the index of the corresponding BIF entry)
    ///     bits 19-14: tileset index
    ///     bits 13- 0: non-tileset file index (any 12 bit value, so long as it matches the value used in the BIF file)
    /// </remarks>
    public class ResourceLocator1
    {
        #region Constants
        /// <summary>Bitmask for BIFF index, segregating the 12 most significant bits</summary>
        public const UInt32 MaskBiffIndex = 0xFFF00000;

        /// <summary>Bitmask for setting BIFF index, taking only 12 bits</summary>
        public const UInt32 MaskBiffIndexLength = 0xFFF;
        
        /// <summary>
        ///     Bitmask for tileset index, segregating the 6 most
        ///     significant bits after the 12 most significant bits
        /// </summary>
        public const UInt32 MaskTilesetIndex = 0xFC000;

        /// <summary>Bitmask for setting tileset index, taking only 6 bits</summary>
        public const UInt32 MaskTilesetIndexLength = 0x3F;
        
        /// <summary>Bitmask for tileset index, segregating the 14 least significant bits</summary>
        public const UInt32 MaskResourceIndex = 0x3FFF;
        #endregion


        #region Fields
        /// <summary>
        ///     This property represents the bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        public UInt32 Locator { get; set; }
        #endregion


        #region Properties
        /// <summary>This property exposes the BIF index of the resource1, shifted to least-significant bits</summary>
        public UInt32 BiffIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskBiffIndex) >> 20; }
            set
            {
                //isolate the existing tileset and resource values; store them
                UInt32 locator = (this.Locator & ~ResourceLocator1.MaskBiffIndex);

                //take only the relevant bits, shift them into place and combine with tileset and resource bits
                this.Locator = locator | ((value & ResourceLocator1.MaskBiffIndexLength) << 20);
            }
        }

        /// <summary>This property exposes the tileset index within the indexed BIF</summary>
        public UInt32 TilesetIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskTilesetIndex) >> 14; }
            set
            {
                //isolate the existing biff index and resource values; store them
                UInt32 locator = (this.Locator & ~ResourceLocator1.MaskTilesetIndex);

                //take only the relevant bits, shift them into place and combine with biff index and resource bits
                this.Locator = locator | ((value & ResourceLocator1.MaskTilesetIndexLength) << 14);
            }
        }

        /// <summary>This property exposes the resource1 index within the indexed BIF</summary>
        public UInt32 ResourceIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskResourceIndex); }
            set
            {
                //isolate the existing biff index and tileset values; store them
                UInt32 locator = (this.Locator & ~ResourceLocator1.MaskResourceIndex);

                //take only the relevant bits and combine with biff index and tileset bits
                this.Locator = locator | (value & ResourceLocator1.MaskResourceIndex);
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public ResourceLocator1() { }

        /// <summary>Definition constructor</summary>
        /// <param name="locator">The underlying value of the locator</param>
        public ResourceLocator1(UInt32 locator)
        {
            this.Locator = locator;
        }

        /// <summary>Definition constructor</summary>
        /// <param name="biffIndex">Index of the BIFF this resource is referencing</param>
        /// <param name="tilesetIndex">Index of the tileset being referenced inside the BIFF archive</param>
        /// <param name="resourceIndex">Index of the resource being referenced inside the BIFF archive</param>
        public ResourceLocator1(UInt32 biffIndex, UInt32 tilesetIndex, UInt32 resourceIndex)
        {
            this.BiffIndex = biffIndex;
            this.TilesetIndex = tilesetIndex;
            this.ResourceIndex = resourceIndex;
        }
        #endregion


        #region Equality operations
        /// <summary>Equality testing method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are conceptually equal, flase if not</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;

            if (obj == this)    //reference check, fastest
                equal = true;
            else if (obj != null && obj is ResourceLocator1)
            {
                ResourceLocator1 reference = obj as ResourceLocator1;

                equal = (reference.Locator == this.Locator);
            }

            return equal;
        }

        /// <summary>Generates a hashcode for this object</summary>
        /// <returns>The generated hash code</returns>
        public override Int32 GetHashCode()
        {
            //only one real field
            return this.Locator.GetHashCode();
        }
        #endregion
    }
}