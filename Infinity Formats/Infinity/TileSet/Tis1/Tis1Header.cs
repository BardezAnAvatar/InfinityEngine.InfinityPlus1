using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.TileSet.Tis1
{
    /// <summary>This class is a representation of the TIS version 1 file format.</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    ///     
    ///     Offset  Size    (data type) 	Description
    ///     0x0000  4   	(char )		    Signature ('TIS ')
    ///     0x0004  4   	(char) 		    Version ('V1  ')
    ///     0x0008  4   	(dword)		    Count of tiles within this tileset
    ///     0x000c  4   	(dword)		    Length of tiles section
    ///     0x0010  4   	(dword)		    Size of the header (offset to tiles)
    ///     0x0014  4   	(dword)		    Dimension of 1 tile in pixels (64x64)
    ///     
    ///     When cutsom implementing the next version, for the love of God, write the basic dimensions of the tileset in the header
    /// </remarks>
    public class Tis1Header : InfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of this structure on disk</summary>
        public const Int32 StructSize = 24;
        #endregion


        #region Fields
        /// <summary>Number of tiles in this tileset</summary>
        public UInt32 CountTiles { get; set; }

        /// <summary>Length of a single tile's data</summary>
        public UInt32 LengthSingleTileData { get; set; }

        /// <summary>Offset from start of file (sub-stream if BIFF?) to tile data</summary>
        public UInt32 OffsetTileData { get; set; }

        /// <summary>This property exposes the dimension of a single tile</summary>
        /// <value>Typically 0x40 or 64</value>
        /// <remarks>The dimension of a tile is square, so only one dimension is needed.</remarks>
        public UInt32 DimensionTile { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the expected size of all tile data</summary>
        public UInt32 TileDataSize
        {
            get { return this.LengthSingleTileData * this.CountTiles; }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region Public Methods
        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, Tis1Header.StructSize - 8);   //header buffer

            //Number of tiles
            this.CountTiles = ReusableIO.ReadUInt32FromArray(buffer, 0x00);

            //Length of a single tile
            this.LengthSingleTileData = ReusableIO.ReadUInt32FromArray(buffer, 0x04);

            //Offset to tile data
            this.OffsetTileData = ReusableIO.ReadUInt32FromArray(buffer, 0x08);

            //Dimension(s) of a single tile
            this.DimensionTile = ReusableIO.ReadUInt32FromArray(buffer, 0x0C);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.CountTiles, output);            //Number of tiles
            ReusableIO.WriteUInt32ToStream(this.LengthSingleTileData, output);  //Length of a single tile
            ReusableIO.WriteUInt32ToStream(this.OffsetTileData, output);        //Offset to tile data
            ReusableIO.WriteUInt32ToStream(this.DimensionTile, output);         //Dimension(s) of a single tile
        }

        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tis1Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Tile Count"));
            builder.Append(this.CountTiles);
            builder.Append(StringFormat.ToStringAlignment("Single Tile Length"));
            builder.Append(this.LengthSingleTileData);
            builder.Append(StringFormat.ToStringAlignment("Tile Data Offset"));
            builder.Append(this.OffsetTileData);
            builder.Append(StringFormat.ToStringAlignment("Single Tile Dimension"));
            builder.Append(this.DimensionTile);

            return builder.ToString();
        }
        #endregion
    }
}