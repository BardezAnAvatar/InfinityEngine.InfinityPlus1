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
    public struct ResourceLocator1
    {
        /// <summary>
        ///     This member contains is a bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        private Int32 locator;

        #region Properties
        /// <summary>
        ///     This property represents the bitfield indicating the resource1 location, with three data values:
        ///     the BIF index, the BIF tileset index and the BIF resource1 index.
        /// </summary>
        public Int32 Locator
        {
            get { return this.locator; }
            set { this.locator = value; }
        }

        /// <summary>This property exposes the BIF index of the resource1</summary>
        public Int32 BifIndex
        {
            get { return this.locator >> 20; }
            set
            {
                Int32 temp = (this.locator << 12) >> 12;
                this.locator = value << 20 + temp;
            }
        }

        /// <summary>This property exposes the tileset index within the indexed BIF</summary>
        public Int32 TilesetIndex
        {
            get { return (this.locator << 12) >> 20; }
            set
            {
                Int32 bifIndex, resourceIndex;
                bifIndex = this.BifIndex;
                resourceIndex = this.ResourceIndex;
                this.locator = bifIndex + resourceIndex + (value << 14);
            }
        }

        /// <summary>This property exposes the resource1 index within the indexed BIF</summary>
        public Int32 ResourceIndex
        {
            get { return this.locator << 18 >> 18; }
            set
            {
                Int32 temp = this.locator >> 14 << 14;
                this.locator = temp + value;
            }
        }
        #endregion
    }
}