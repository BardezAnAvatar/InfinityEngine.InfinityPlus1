using System;
using System.Collections.Generic;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1
{
    /// <summary>Represents a single tile from a tileset</summary>
    public class Tile : IInfinityFormat, IImage
    {
        #region Constants
        /// <summary>Default dimension of the tile</summary>
        public const Int32 DefaultDimension = 64;
        #endregion


        #region Fields
        /// <summary>Represents the quantity of pixels in one dimension for the square tile</summary>
        protected Int32 Dimension { get; set; }

        /// <summary>Single palette entry for the tile data</summary>
        public PaletteEntry Palette { get; set; }

        /// <summary>The collection of palette indexes for this tile</summary>
        public Byte[] TileData { get; set; }

        /// <summary>BGRA byte array of pixel data</summary>
        protected Byte[] PixelByteData { get; set; }
        #endregion


        #region Properties
        /// <summary>BGRA byte array of pixel data</summary>
        public Byte[] ImageData
        {
            get
            {
                if (this.PixelByteData == null)
                    this.PixelByteData = this.GetExpandedData();

                return this.ImageData;
            }
        }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public Tile() : this(Tile.DefaultDimension) { }

        /// <summary>Definition constructor</summary>
        public Tile(Int32 dimension)
        {
            this.Dimension = dimension;
            this.Palette = null;
            this.TileData = null;
            this.PixelByteData = null;
        }

        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Palette = new PaletteEntry();
        }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            this.Palette.Read(input);
            this.TileData = ReusableIO.BinaryRead(input, this.Dimension * this.Dimension);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.Palette.Write(output);
            output.Write(this.TileData, 0, this.TileData.Length);
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public Frame GetFrame()
        {
            Frame frame = new Frame();
            frame.Pixels = this.GetPixelData();

            return frame;
        }
        #endregion


        #region Frame support methods
        /// <summary>Expands the tile data from the palette indexed color to a fully BGRA pixel data array</summary>
        /// <returns>A fully decompressed pixel data Byte array</returns>
        protected Byte[] GetExpandedData()
        {
            Byte[] pixels = new Byte[4 * this.Dimension * this.Dimension];

            for (Int32 dataIndex = 0; dataIndex < this.TileData.Length; ++dataIndex)
            {
                Int32 pixelOffset = dataIndex * 4;
                pixels[pixelOffset] = this.Palette.Colors[this.TileData[dataIndex]].Blue;
                pixels[pixelOffset + 1] = this.Palette.Colors[this.TileData[dataIndex]].Green;
                pixels[pixelOffset + 2] = this.Palette.Colors[this.TileData[dataIndex]].Red;
                pixels[pixelOffset + 3] = (this.TileData[dataIndex] == Byte.MinValue) ? Byte.MinValue: Byte.MaxValue;  //Index 0 is transparent, premultiplied; everything else is opaque
            }

            return pixels;
        }

        /// <summary>Gets the pixel data fro this tile.</summary>
        /// <returns>The pixel data for this tile</returns>
        protected PixelData GetPixelData()
        {
            Byte[] binData = this.GetExpandedData();
            PixelData pd = new PixelData(binData, ScanLineOrder.TopDown, PixelFormat.RGBA_B8G8R8A8, this.Dimension, this.Dimension, 0, 0, 32);

            return pd;
        }
        #endregion
    }
}