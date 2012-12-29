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
using Bardez.Projects.Multimedia.MediaBase.Frame.Image;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Mosaic
{
    /// <summary>Represents an uncompressed MOS V1 file</summary>
    public class Mosaic_v1 : IInfinityFormat, IImage
    {
        #region Constants
        /// <summary>Represents the base dimension of a block</summary>
        private const Int32 blockDimension = 64;    //64x64, not 8x8
        #endregion


        #region Fields
        /// <summary>Header of this MOS file</summary>
        public MosHeader Header { get; set; }

        /// <summary>List of Palettes in this MOS file</summary>
        public List<PaletteEntry> Palettes { get; set; }

        /// <summary>List of offsets within the MOS file to target tile data</summary>
        public List<UInt32> TileOffsets { get; set; }

        /// <summary>Two-dimensional array of tile palette index entries</summary>
        public Byte[,][] TileData { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the size of the Palette entries and the data offset entries.</summary>
        public Int32 PaletteSize
        {
            get 
            {
                Int32 paletteSize = (PaletteEntry.PaletteSize * ColorEntry.StructSize);
                Int32 blockPointers = (this.Header.BlockCount * 4);
                Int32 blockData = 0;
                foreach (Int32 blockSize in this.TileOffsets)
                    if (blockSize > blockData)
                        blockData = blockSize;

                return  + paletteSize + blockPointers + blockData + 4096;   //constant is 64*64 samples, lazy coding.
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Header = new MosHeader();
            this.Palettes = new List<PaletteEntry>();
            this.TileOffsets = new List<UInt32>();
            //cannot know the needed size of TileData until the header is read
        }

        /// <summary>Initializes the two-dimensional array of paletted pixel data for the MOS file. Requires the header to have been read.</summary>
        public void InitializeTileData()
        {
            this.TileData = new Byte[this.Header.Columns, this.Header.Rows][];
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
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new MosHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public void ReadBody(Stream input)
        {
            this.Initialize();

            //read the header
            this.Header.Read(input);

            //read palettes
            ReusableIO.SeekIfAble(input, this.Header.PaletteOffset);   //seek to palettes
            for (Int32 paletteIndex = 0; paletteIndex < this.Header.BlockCount; ++paletteIndex)
            {
                PaletteEntry pe = new PaletteEntry();
                pe.Read(input);
                this.Palettes.Add(pe);
            }

            //immediately follows palettes
            Byte[] tileOffsets = ReusableIO.BinaryRead(input, 4 * this.Header.BlockCount); //do a single large read
            for (Int32 tileIndex = 0; tileIndex < this.Header.BlockCount; ++tileIndex)
                this.TileOffsets.Add(ReusableIO.ReadUInt32FromArray(tileOffsets, tileIndex * 4));

            //the tile data pointer (probably) immediately follows the offsets, so keep this position in memory
            Int64 dataPointer = input.Position;

            //read the tile data
            this.InitializeTileData();  //instantiate the tile data
            for (Int32 y = 0; y < this.Header.Rows; ++y)
            {
                Int32 height = this.RowSampleHeight(y);    //usually a block is 64x64 data, but, can be 1x1, depending on width and height.

                for (Int32 x = 0; x < this.Header.Columns; ++x)
                {
                    Int32 width = this.SampleWidth(x);  //usually a block is 8x8 data, but, can be 1x1, depending on width and height.

                    Int32 tileIndex = (y * this.Header.Columns) + x;
                    ReusableIO.SeekIfAble(input, dataPointer + this.TileOffsets[tileIndex]);  //seek to the tile data
                    Byte[] data = ReusableIO.BinaryRead(input, width * height);
                    this.TileData[x, y] = data;
                }
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            this.MaintainMinimalDataIntegrity();    //Ensure that the data does not corrupt itself

            this.Header.Write(output);      //write header
            this.WritePalettes(output);     //write the block palettes
            this.WriteTileOffsets(output);  //write the tile offsets
            this.WriteTileData(output);     //write the indecies to palettes
        }

        /// <summary>This public method writes the block palette to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WritePalettes(Stream output)
        {
            //seek
            ReusableIO.SeekIfAble(output, this.Header.PaletteOffset);

            foreach (PaletteEntry palette in this.Palettes)
                palette.Write(output);
        }

        /// <summary>This public method writes the tile date offsets to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTileOffsets(Stream output)
        {
            foreach (UInt32 offset in this.TileOffsets)
                ReusableIO.WriteUInt32ToStream(offset, output);
        }

        /// <summary>This public method writes the tile binary data palette indecies to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        protected void WriteTileData(Stream output)
        {
            for (Int32 tile = 0; tile < this.TileOffsets.Count; ++tile)
            {
                ReusableIO.SeekIfAble(output, this.TileOffsets[tile]);

                Byte[] data = this.TileData[tile % this.Header.Columns, tile / this.Header.Columns];
                if (data != null)
                    output.Write(data, 0, data.Length);
            }
        }
        #endregion

        
        #region Data Integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            if (this.Overlaps())
            {
                //reassign the offsets
                this.Header.PaletteOffset = MosHeader.StructSize;
                
                UInt32 tileOffset = Convert.ToUInt32(this.Header.PaletteOffset + this.PaletteSize);    //start position
                for (Int32 tile = 0; tile < this.TileOffsets.Count; ++tile)
                {
                    this.TileOffsets[tile] = tileOffset;
                    tileOffset += Convert.ToUInt32(BlockSize(tile));
                }
            }
        }

        /// <summary>Determines if any of the offset data sections would overlap one another.</summary>
        /// <returns>A Boolean indicating whether or not any of them overlap</returns>
        protected Boolean Overlaps()
        {
            Boolean overlaps = true;

            overlaps = IntExtension.Between(this.Header.PaletteOffset, this.PaletteSize, 0, MosHeader.StructSize); //palette and header

            return overlaps;
        }
        #endregion


        #region Helper Methods
        /// <summary>Gets the count of horizontal samples in one row, given a block number</summary>
        /// <param name="block">Block number to identify the width of</param>
        /// <returns>The count of horizontal row samples in the block</returns>
        protected Int32 SampleWidth(Int32 block)
        {
            Int32 x = block % this.Header.Columns;
            Int32 width = Mosaic_v1.blockDimension;

            if ((x == (this.Header.Columns - 1) && this.Header.Width % Mosaic_v1.blockDimension > 0))
                width = this.Header.Width % Mosaic_v1.blockDimension;

            return width;
        }

        /// <summary>Gets the count of vertical samples in one column, given a block number</summary>
        /// <param name="block">Block number to identify the height of</param>
        /// <returns>The count of vertical column samples in the block</returns>
        protected Int32 SampleHeight(Int32 block)
        {
            return this.RowSampleHeight(block / this.Header.Columns);
        }

        /// <summary>Gets the count of vertical samples in one column, given a block number</summary>
        /// <param name="row">Block number to identify the height of</param>
        /// <returns>The count of vertical column samples in the block</returns>
        protected Int32 RowSampleHeight(Int32 row)
        {
            Int32 height = Mosaic_v1.blockDimension;

            if (row == (this.Header.Rows - 1) && this.Header.Height % Mosaic_v1.blockDimension > 0)
                height = this.Header.Height % Mosaic_v1.blockDimension;

            return height;
        }

        /// <summary>Gets the count of pixels in a block</summary>
        /// <param name="block">Block number to identify the width of</param>
        /// <returns>The number of pixels in the block</returns>
        protected Int32 BlockSize(Int32 block)
        {
            return this.SampleHeight(block) * this.SampleWidth(block);
        }
        #endregion


        #region IImage methods
        /// <summary>Gets a frame image from the pixel data already in place</summary>
        /// <returns>A frame containing the pixel data</returns>
        public IMultimediaImageFrame GetFrame()
        {
            IMultimediaImageFrame frame = new BasicImageFrame(this.GetPixelData());
            return frame;
        }

        /// <summary>Builds a pixel data object from the MOS file</summary>
        /// <returns>A new pixeldata object</returns>
        protected PixelData GetPixelData()
        {
            Byte[] data = this.GetPixels();
            PixelData pd = new PixelData(data, ScanLineOrder.TopDown, PixelFormat.RGBA_B8G8R8A8, null, this.Header.Height, this.Header.Width, 0, 0, 32);
            return pd;
        }

        /// <summary>Gets the pixel data array from the tiles' data</summary>
        /// <returns>An BGRA array of pixels</returns>
        protected Byte[] GetPixels()
        {
            //instantiate the return array
            Byte[] pixels = new Byte[4 * this.Header.Width * this.Header.Height];

            Int32 tileNumber = 0, pixelDataIndex = 0;
            for (Int32 y = 0; y < this.Header.Rows; ++y)    //loop through vertical blocks
            {
                Int32 height = this.SampleHeight(tileNumber);
                for (Int32 line = 0; line < height; ++line) //block-level
                {
                    tileNumber = y * this.Header.Columns;   //reset, so as to keep the value current

                    for (Int32 x = 0; x < this.Header.Columns; ++x) //loop through horizontal blocks
                    {
                        Int32 width = this.SampleWidth(tileNumber);
                        Int32 dataBase = line * width;

                        for (Int32 horz = 0; horz < width; ++horz)  //loop through the values in this block-row (0-[1 to 8 max])
                        {
                            ColorEntry color = this.Palettes[tileNumber].Colors[this.TileData[x, y][dataBase + horz]];
                            pixels[pixelDataIndex] = color.Blue;
                            pixels[pixelDataIndex + 1] = color.Green;
                            pixels[pixelDataIndex + 2] = color.Red;
                            pixels[pixelDataIndex + 3] = Byte.MaxValue; //opaque, and pre-multiplied

                            pixelDataIndex += 4;
                        }

                        ++tileNumber;
                    }
                }
            }

            return pixels;
        }
        #endregion
    }
}