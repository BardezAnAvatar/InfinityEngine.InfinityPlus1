using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.MultiMedia.MediaBase.Video;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1
{
    /// <summary>Represents the version 1 tileset file</summary>
    public class TileSet1 : IInfinityFormat, IImageSet
    {
        #region Fields
        /// <summary>Tileset header</summary>
        public Tis1Header Header { get; set; }

        /// <summary>List of tile data, expanded from the palette</summary>
        public List<Tile> Tiles { get; set; }
        #endregion


        #region Properties
        /// <summary>Property exposing the count of frames in the Image Set</summary>
        public Int64 FrameCount
        {
            get { return this.Header.CountTiles; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.InitializeHeader();
            this.InitializeBody();
        }

        /// <summary>Instantiates header reference</summary>
        public void InitializeHeader()
        {
            this.Header = new Tis1Header();
        }

        /// <summary>Instantiates tiles collection reference</summary>
        public void InitializeBody()
        {
            this.Tiles = new List<Tile>();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <remarks>
        ///     Be aware that sometimes, a TIS file does not have a header outside of a BIFF archive.
        ///     This is the case when a TIS is exported via Near Infinity, and there have been similar concerns
        ///     when WeiDU is not used properly to add a TIS to a BIFF archive.
        /// </remarks>
        public void Read(Stream input)
        {
            this.ReadHeader(input);
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new Tis1Header();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.InitializeBody();

            //read palettes
            ReusableIO.SeekIfAble(input, this.Header.OffsetTileData);   //seek to tiles
            for (Int32 tileIndex = 0; tileIndex < this.Header.CountTiles; ++tileIndex)
            {
                Tile tile = new Tile();
                tile.Read(input);
                this.Tiles.Add(tile);
            }
        }

        /// <summary>This public method reads file format's header data structure from the input stream.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadHeader(Stream input)
        {
            this.InitializeHeader();
            this.Header.Read(input);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();    //Ensure that the data does not corrupt itself

            //write header
            this.Header.Write(output);

            //write tiles
            ReusableIO.SeekIfAble(output, this.Header.OffsetTileData);
            foreach (Tile tile in this.Tiles)
                tile.Write(output);
        }
        #endregion


        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            if (this.Overlaps())
            {
                //reassign the offsets
                this.Header.OffsetTileData = Tis1Header.StructSize;
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected Boolean Overlaps()
        {
            return IntExtension.Between(this.Header.OffsetTileData, this.Header.TileDataSize, 0, Tis1Header.StructSize); //palette and header
        }
        #endregion


        #region IImageSet methods
        /// <summary>Returns the frame with the associated index in the tileset</summary>
        /// <param name="index">Index of the frame to retrieve</param>
        /// <returns>The frane at the associated index</returns>
        public IMultimediaVideoFrame GetFrame(Int32 index)
        {
            return this.Tiles[index].GetFrame();
        }
        #endregion
    }
}