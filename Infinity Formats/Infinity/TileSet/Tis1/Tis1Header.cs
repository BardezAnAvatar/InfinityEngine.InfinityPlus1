using System;
using System.IO;
using System.Text;

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
    /// </remarks>
    public class Tis1Header
    {
        #region Members
        /// <summary>This member contains the signature of the file. In all Infinity Engine cases it should be 'TIS '.</summary>
        protected String signature;

        /// <summary>This member contains the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        protected String version;

        protected UInt32 countTiles;

        protected UInt32 lengthSingleTileData;

        protected UInt32 offsetTileData;

        /// <summary>The dimension of a single tile</summary>
        /// <value>Typically 0x40 or 64</value>
        /// <remarks>
        ///     The dimension of a tile is square, so only one dimension is needed. Note to self: version 2
        ///     could use similar structure, have this broken out into shorts and have rectangular shape.
        /// </remarks>
        protected UInt32 dimensionTile;
        #endregion

        #region Properties
        /// <summary>This property exposes the signature of the file. In all Infinity Engine cases it should be 'TIS '.</summary>
        public String Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        /// <summary>This property exposes the version of the file format. In all Infinity Engine cases it should be 'V1  '.</summary>
        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        public UInt32 CountTiles
        {
            get { return countTiles; }
            set { countTiles = value; }
        }

        public UInt32 LengthSingleTileData
        {
            get { return lengthSingleTileData; }
            set { lengthSingleTileData = value; }
        }

        public UInt32 OffsetTileData
        {
            get { return offsetTileData; }
            set { offsetTileData = value; }
        }


        /// <summary>This property exposes the dimension of a single tile</summary>
        /// <value>Typically 0x40 or 64</value>
        /// <remarks>The dimension of a tile is square, so only one dimension is needed.</remarks>
        public UInt32 DimensionTile
        {
            get { return dimensionTile; }
            set { dimensionTile = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>This public method reads the 20-byte header into the header record</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Read(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, 20);   //header buffer
            Byte[] temp = new Byte[4];
            Encoding encoding = new ASCIIEncoding();

            //signature
            Array.Copy(buffer, temp, 4);
            this.signature = encoding.GetString(temp);

            //version
            Array.Copy(buffer, 4, temp, 0, 4);
            this.version = encoding.GetString(temp);

            //Number of tiles
            this.countTiles = ReusableIO.ReadUInt32FromArray(buffer, 0x08);

            //Length of a single tile
            this.lengthSingleTileData = ReusableIO.ReadUInt32FromArray(buffer, 0x0C);

            //Offset to tile data
            this.offsetTileData = ReusableIO.ReadUInt32FromArray(buffer, 0x10);

            //Dimension(s) of a single tile
            this.dimensionTile = ReusableIO.ReadUInt32FromArray(buffer, 0x14);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public void Write(Stream output)
        {
            Byte[] writeBytes;

            //signature
            writeBytes = ReusableIO.WriteStringToByteArray(this.signature, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //version
            writeBytes = ReusableIO.WriteStringToByteArray(this.version, 4);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Number of tiles
            writeBytes = BitConverter.GetBytes(this.countTiles);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Length of a single tile
            writeBytes = BitConverter.GetBytes(this.lengthSingleTileData);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Offset to tile data
            writeBytes = BitConverter.GetBytes(this.offsetTileData);
            output.Write(writeBytes, 0, writeBytes.Length);

            //Dimension(s) of a single tile
            writeBytes = BitConverter.GetBytes(this.dimensionTile);
            output.Write(writeBytes, 0, writeBytes.Length);
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
            builder.Append(this.countTiles);
            builder.Append(StringFormat.ToStringAlignment("Single Tile Length"));
            builder.Append(this.lengthSingleTileData);
            builder.Append(StringFormat.ToStringAlignment("Tile Data Offset"));
            builder.Append(this.offsetTileData);
            builder.Append(StringFormat.ToStringAlignment("Single Tile Dimension"));
            builder.Append(this.dimensionTile);

            return builder.ToString();
        }
        #endregion
    }
}