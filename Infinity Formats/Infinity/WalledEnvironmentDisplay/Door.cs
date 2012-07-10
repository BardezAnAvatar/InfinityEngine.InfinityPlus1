using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Components;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents a single door structure in a WED file</summary>
    /// <remarks>The tile indeces in this file map back to Overlay #0's tilemap index, not their tileset index.</remarks>
    public class Door : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 26;
        #endregion


        #region Fields
        /// <summary>Represents the door name</summary>
        /// <remarks>Matches the name in the ARE file</remarks>
        public ZString Name { get; set; }

        /// <summary>Represents the status (open/closed) of the door</summary>
        public UInt16 Status { get; set; }

        /// <summary>Represents the first door till cell index</summary>
        public UInt16 TileCellIndex { get; set; }

        /// <summary>Represents the number of dor tile cells for this door</summary>
        public UInt16 TileCount { get; set; }

        /// <summary>Represents the number of polygons for the open state</summary>
        public Int16 PolygonCountOpen { get; set; }

        /// <summary>Represents the number of polygons for the closed state</summary>
        public Int16 PolygonCountClosed { get; set; }

        /// <summary>Offset to the open polygon data</summary>
        public Int32 OffsetPolygonsOpen { get; set; }

        /// <summary>Offset to the closed polygon data</summary>
        /// <remarks>Observed several -65535 values</remarks>
        public Int32 OffsetPolygonsClosed { get; set; }

        /// <summary>Collection of doors' tilemap indecies to toggle when the open state is toggled</summary>
        public List<Int16> TilemapIndeces { get; set; }

        /// <summary>Collection of door polygon collections</summary>
        /// <remarks>Data tha is logicaly pointed to, not stored as part of the binary struct on disk</remarks>
        public DoorPolygonCollections Polygons { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Name = new ZString();
            this.TilemapIndeces = new List<Int16>();
            this.Polygons = new DoorPolygonCollections();
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

            Byte[] data = ReusableIO.BinaryRead(input, Door.StructSize);

            this.Name.Source = ReusableIO.ReadStringFromByteArray(data, 0, CultureConstants.CultureCodeEnglish);
            this.Status = ReusableIO.ReadUInt16FromArray(data, 8);
            this.TileCellIndex = ReusableIO.ReadUInt16FromArray(data, 10);
            this.TileCount = ReusableIO.ReadUInt16FromArray(data, 12);
            this.PolygonCountOpen = ReusableIO.ReadInt16FromArray(data, 14);
            this.PolygonCountClosed = ReusableIO.ReadInt16FromArray(data, 16);
            this.OffsetPolygonsOpen = ReusableIO.ReadInt32FromArray(data, 18);
            this.OffsetPolygonsClosed = ReusableIO.ReadInt32FromArray(data, 22);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.Name.Source, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt16ToStream(this.Status, output);
            ReusableIO.WriteUInt16ToStream(this.TileCellIndex, output);
            ReusableIO.WriteUInt16ToStream(this.TileCount, output);
            ReusableIO.WriteInt16ToStream(this.PolygonCountOpen, output);
            ReusableIO.WriteInt16ToStream(this.PolygonCountClosed, output);
            ReusableIO.WriteInt32ToStream(this.OffsetPolygonsOpen, output);
            ReusableIO.WriteInt32ToStream(this.OffsetPolygonsClosed, output);
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

        /// <summary>Returns the printable read-friendly version of the format</summary>
        protected String GetVersionString()
        {
            return "WED Overlay:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Door # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Name"));
            builder.Append(String.Concat("'", this.Name.Value, "'"));
            builder.Append(StringFormat.ToStringAlignment("Status"));
            builder.Append(this.Status);
            builder.Append(StringFormat.ToStringAlignment("First Tile Cell Index"));
            builder.Append(this.TileCellIndex);
            builder.Append(StringFormat.ToStringAlignment("Tile Cell Count"));
            builder.Append(this.TileCount);
            builder.Append(StringFormat.ToStringAlignment("Open Polygon Count"));
            builder.Append(this.PolygonCountOpen);
            builder.Append(StringFormat.ToStringAlignment("Closed Polygon Count"));
            builder.Append(this.PolygonCountClosed);
            builder.Append(StringFormat.ToStringAlignment("Open Polygon Offset"));
            builder.Append(this.OffsetPolygonsOpen);
            builder.Append(StringFormat.ToStringAlignment("Closed Polygons Offset"));
            builder.Append(this.OffsetPolygonsClosed);

            return builder.ToString();
        }
        #endregion


        #region Equality
        /// <summary>Overridden (value) equality method</summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating equality</returns>
        public override Boolean Equals( Object obj)
        {
            Boolean equal = false;  //assume the worst

            try
            {
                if (obj != null && obj is Door)
                {
                    Door compare = obj as Door;

                    Boolean structureEquality = (this.Name.Value == compare.Name.Value);
                    structureEquality &= (this.Status == compare.Status);
                    structureEquality &= (this.TileCellIndex == compare.TileCellIndex);
                    structureEquality &= (this.TileCount == compare.TileCount);
                    structureEquality &= (this.PolygonCountOpen == compare.PolygonCountOpen);
                    structureEquality &= (this.PolygonCountClosed == compare.PolygonCountClosed);
                    structureEquality &= (this.OffsetPolygonsOpen == compare.OffsetPolygonsOpen);
                    structureEquality &= (this.OffsetPolygonsClosed == compare.OffsetPolygonsClosed);

                    //short-circuit large equality tests
                    if (structureEquality)
                    {
                        structureEquality &= this.TilemapIndeces.Equals<Int16>(compare.TilemapIndeces);
                        if (structureEquality)
                            structureEquality &= this.Polygons.Equals(compare.Polygons);
                    }

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
            hash ^= this.Name.GetHashCode();
            hash ^= this.Status.GetHashCode();
            hash ^= this.TileCellIndex.GetHashCode();
            hash ^= this.TileCount.GetHashCode();
            hash ^= this.PolygonCountOpen.GetHashCode();
            hash ^= this.PolygonCountClosed.GetHashCode();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}