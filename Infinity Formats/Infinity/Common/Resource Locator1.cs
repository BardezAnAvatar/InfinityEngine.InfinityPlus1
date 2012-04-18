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
        public const Int32 MaskBiffIndex = -0x100000;   //FFF00000... C# is dumb with thee values being interpeted as unsigned

        /// <summary>Bitmask for setting BIFF index, taking only 12 bits</summary>
        public const Int32 MaskBiffIndexLength = 0xFFF;
        
        /// <summary>
        ///     Bitmask for tileset index, segregating the 6 most
        ///     significant bits after the 12 most significant bits
        /// </summary>
        public const Int32 MaskTilesetIndex = 0xFC000;

        /// <summary>Bitmask for setting tileset index, taking only 6 bits</summary>
        public const Int32 MaskTilesetIndexLength = 0x3F;
        
        /// <summary>Bitmask for tileset index, segregating the 14 least significant bits</summary>
        public const Int32 MaskResourceIndex = 0x3FFF;
        #endregion


        #region Fields
        /// <summary>
        ///     This property represents the bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        public Int32 Locator { get; set; }
        #endregion


        #region Properties
        /// <summary>This property exposes the BIF index of the resource1, shifted to least-significant bits</summary>
        public Int32 BiffIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskBiffIndex) >> 20; }
            set
            {
                Int32 locator = (this.Locator & ~ResourceLocator1.MaskBiffIndex);
                this.Locator = locator | ((value << 20) & ResourceLocator1.MaskBiffIndexLength);
            }
        }

        /// <summary>This property exposes the tileset index within the indexed BIF</summary>
        public Int32 TilesetIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskTilesetIndex) >> 14; }
            set
            {
                Int32 locator = (this.Locator & ~ResourceLocator1.MaskTilesetIndex);
                this.Locator = locator | ((value << 14) & ResourceLocator1.MaskTilesetIndexLength);
            }
        }

        /// <summary>This property exposes the resource1 index within the indexed BIF</summary>
        public Int32 ResourceIndex
        {
            get { return (this.Locator & ResourceLocator1.MaskResourceIndex); }
            set
            {
                Int32 locator = (this.Locator & ~ResourceLocator1.MaskResourceIndex);
                this.Locator = locator | (value & ResourceLocator1.MaskResourceIndex);
            }
        }
        #endregion
    }
}