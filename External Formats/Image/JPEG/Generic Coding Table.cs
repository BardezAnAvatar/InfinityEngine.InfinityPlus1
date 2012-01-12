using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG
{
    /// <summary>Represents a base class for JPEG coding tables, be they Arithmetic, or Huffman</summary>
    public abstract class GenericCodingTable : GenericTable
    {
        #region Fields
        /// <summary>Table class.</summary>
        /// <value>0 = DC table or lossless table, 1 = AC table.</value>
        public Byte TableClass { get; set; }
        #endregion
    }
}