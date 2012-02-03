using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents the WED polygon header</summary>
    public class PolygonHeader : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 20;
        #endregion


        #region Fields
        /// <summary>Represents the count of polygons</summary>
        public UInt32 WallPolygonCount { get; set; }

        /// <summary>Represents the offset from the start of the file to the polygon entries</summary>
        public UInt32 OffsetPolygons { get; set; }

        /// <summary>Represents the offset from the start of the file to the vertex entries</summary>
        public UInt32 OffsetVertices { get; set; }

        /// <summary>Represents the offset from the start of the file to the wall entries</summary>
        public UInt32 OffsetWalls { get; set; }

        /// <summary>Represents the offset from the start of the file to the polugon index entries</summary>
        public UInt32 OffsetPolygonIndeces { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize() { }
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

            Byte[] data = ReusableIO.BinaryRead(input, PolygonHeader.StructSize);

            this.WallPolygonCount = ReusableIO.ReadUInt32FromArray(data, 0);
            this.OffsetPolygons = ReusableIO.ReadUInt16FromArray(data, 4);
            this.OffsetVertices = ReusableIO.ReadUInt32FromArray(data, 8);
            this.OffsetWalls = ReusableIO.ReadUInt32FromArray(data, 12);
            this.OffsetPolygonIndeces = ReusableIO.ReadUInt32FromArray(data, 16);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteUInt32ToStream(this.WallPolygonCount, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetPolygons, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetVertices, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetWalls, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetPolygonIndeces, output);
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

        /// <summary>Returns the printable read-friendly version of the store format</summary>
        protected String GetVersionString()
        {
            return "WED Polygon Header:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Polygon Count"));
            builder.Append(this.WallPolygonCount);
            builder.Append(StringFormat.ToStringAlignment("Polygon Offset"));
            builder.Append(this.OffsetPolygons);
            builder.Append(StringFormat.ToStringAlignment("Vertices Offset"));
            builder.Append(this.OffsetVertices);
            builder.Append(StringFormat.ToStringAlignment("Walls Offset"));
            builder.Append(this.OffsetWalls);
            builder.Append(StringFormat.ToStringAlignment("Polygon Indeces Offset"));
            builder.Append(this.OffsetPolygonIndeces);

            return builder.ToString();
        }
        #endregion
    }
}