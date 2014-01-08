using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents an overlay entry of the WED file</summary>
    public class Overlay : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 24;
        #endregion


        #region Fields
        /// <summary>Represents the width of the overlay in tiles</summary>
        public UInt16 TileWidth { get; set; }

        /// <summary>Represents the height of the overlay in tiles</summary>
        public UInt16 TileHeight { get; set; }

        /// <summary>Represents the tileset resource reference to this overlay</summary>
        public ResourceReference TileSet { get; set; }

        /// <summary>Count of unique tiles</summary>
        public UInt16 TileCount { get; set; }

        /// <summary>Movement type</summary>
        /// <remarks>The name does not give away its function</remarks>
        public UInt16 MovementType { get; set; }

        /// <summary>Offset to the tile map data</summary>
        public UInt32 OffsetTilemap { get; set; }

        /// <summary>Offset to the tile index lookup data</summary>
        public UInt32 OffsetTileIndeces { get; set; }

        /// <summary>Collection of data that maps the overlay tile indeces to actual tilset tile indeces</summary>
        /// <remarks>Not actually part of the data structure, but instead directly referenced via reference, and written elsewhere.</remarks>
        public TilesetMappingCollection TileSetMapping { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.TileSet = new ResourceReference();
            this.TileSetMapping = new TilesetMappingCollection();
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

            Byte[] data = ReusableIO.BinaryRead(input, Overlay.StructSize);

            this.TileWidth = ReusableIO.ReadUInt16FromArray(data, 0);
            this.TileHeight = ReusableIO.ReadUInt16FromArray(data, 2);
            this.TileSet.ResRef = ReusableIO.ReadStringFromByteArray(data, 4, CultureConstants.CultureCodeEnglish);
            this.TileCount = ReusableIO.ReadUInt16FromArray(data, 12);
            this.MovementType = ReusableIO.ReadUInt16FromArray(data, 14);
            this.OffsetTilemap = ReusableIO.ReadUInt32FromArray(data, 16);
            this.OffsetTileIndeces = ReusableIO.ReadUInt32FromArray(data, 20);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt16ToStream(this.TileWidth, output);
            ReusableIO.WriteUInt16ToStream(this.TileHeight, output);
            ReusableIO.WriteStringToStream(this.TileSet.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.TileCount, output);
            ReusableIO.WriteUInt16ToStream(this.MovementType, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetTilemap, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetTileIndeces, output);
        }
        #endregion


        #region ToString() methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="showType">Boolean indicating whether or not to display the leading description line.</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Boolean showType)
        {
            String header = this.GetVersionString();

            if (showType)
                header += this.GetStringRepresentation();
            else
                header = this.GetStringRepresentation();

            return header;
        }

        /// <summary>This method complements the default ToString() method, printing the member data line by line</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 index)
        {
            String header = this.GetVersionString(index);
            header += this.GetStringRepresentation();

            return header;
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString()
        {
            return "WED Overlay:";
        }

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Overlay # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Tile Width"));
            builder.Append(this.TileWidth);
            builder.Append(StringFormat.ToStringAlignment("Tile Height"));
            builder.Append(this.TileHeight);
            builder.Append(StringFormat.ToStringAlignment("Tileset"));
            builder.Append(String.Concat("'", this.TileSet.ZResRef ,"'"));
            builder.Append(StringFormat.ToStringAlignment("Tile count"));
            builder.Append(this.TileCount);
            builder.Append(StringFormat.ToStringAlignment("Movement Type"));
            builder.Append(this.MovementType);
            builder.Append(StringFormat.ToStringAlignment("Tilemap Offset"));
            builder.Append(this.OffsetTilemap);
            builder.Append(StringFormat.ToStringAlignment("Tile Index Lookup Offset"));
            builder.Append(this.OffsetTileIndeces);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals(Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is Overlay)
                {
                    Overlay compare = obj as Overlay;

                    Boolean structureEquality = (this.TileWidth == compare.TileWidth);
                    structureEquality &= (this.TileHeight == compare.TileHeight);
                    structureEquality &= (this.TileSet.Equals(compare.TileSet));
                    structureEquality &= (this.TileCount == compare.TileCount);
                    structureEquality &= (this.TileCount == compare.TileCount);
                    structureEquality &= (this.MovementType == compare.MovementType);
                    //offsets are unimportant when it comes to data value equivalence/equality

                    //short-circuit large equality tests
                    if (structureEquality)
                        structureEquality &= this.TileSetMapping.Equals(compare.TileSetMapping);

                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }

        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = base.GetHashCode();
            hash ^= this.TileWidth.GetHashCode();
            hash ^= this.TileHeight.GetHashCode();
            hash ^= this.TileSet.GetHashCode();
            hash ^= this.TileCount.GetHashCode();
            hash ^= this.MovementType.GetHashCode();
            hash ^= this.TileSetMapping.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}