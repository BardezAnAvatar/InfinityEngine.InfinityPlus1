using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1
{
    /// <summary>Represents a small info class for the tileset</summary>
    public class TileSetInfo
    {
        #region Constants
        /// <summary>Represents the dimension of the square tile</summary>
        private const Int32 TileDimension = 64;
        #endregion


        #region Fields
        /// <summary>Count of vertical tile columns</summary>
        public UInt16 Columns { get; set; }

        /// <summary>Count of horizontal tile rows</summary>
        public UInt16 Rows { get; set; }
        #endregion

        /// <summary>Width, in pixels, of the main tileset background</summary>
        public Int32 Width
        {
            get { return this.Columns * TileSetInfo.TileDimension; }
        }

        /// <summary>Height, in pixels, of the main tileset background</summary>
        public Int32 Height
        {
            get { return this.Rows * TileSetInfo.TileDimension; }
        }
    }
}