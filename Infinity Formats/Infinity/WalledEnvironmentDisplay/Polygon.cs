using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Enum;
using Bardez.Projects.ReusableCode;


namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents a WED polygon</summary>
    /// <remarks>Generally speaking, must be right-most first, then lower, and then in counter-clockwise order</remarks>
    public class Polygon : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 18;
        #endregion


        #region Fields
        /// <summary>Represents the start vertex index of the polygon</summary>
        public Int32 StartIndex { get; set; }

        /// <summary>Represents the count of vertices in the polygon</summary>
        public Int32 VertexCount { get; set; }

        /// <summary>Represents the known flags for the polygon</summary>
        public PolygonProperties Properties { get; set; }

        /// <summary>Represents the unknown flags of the polygon</summary>
        /// <remarks>Always (usually?) 255</remarks>
        public Byte Unknown { get; set; }

        /// <summary>Represents the polygon's leftmost bounding coordinate</summary>
        public Int16 BoundingRegionMinX { get; set; }

        /// <summary>Represents the polygon's rightmost bounding coordinate</summary>
        public Int16 BoundingRegionMaxX { get; set; }

        /// <summary>Represents the polygon's topmost bounding coordinate</summary>
        public Int16 BoundingRegionMinY { get; set; }

        /// <summary>Represents the polygon's lowermost bounding coordinate</summary>
        public Int16 BoundingRegionMaxY { get; set; }
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

            Byte[] data = ReusableIO.BinaryRead(input, Polygon.StructSize);

            this.StartIndex = ReusableIO.ReadInt16FromArray(data, 0);
            this.VertexCount = ReusableIO.ReadInt16FromArray(data, 4);
            this.Properties = (PolygonProperties)data[8];
            this.Unknown = data[9];
            this.BoundingRegionMinX = ReusableIO.ReadInt16FromArray(data, 10);
            this.BoundingRegionMaxX = ReusableIO.ReadInt16FromArray(data, 12);
            this.BoundingRegionMinY = ReusableIO.ReadInt16FromArray(data, 14);
            this.BoundingRegionMaxY = ReusableIO.ReadInt16FromArray(data, 16);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.StartIndex, output);
            ReusableIO.WriteInt32ToStream(this.VertexCount, output);
            output.WriteByte((Byte)this.Properties);
            output.WriteByte(this.Unknown);
            ReusableIO.WriteInt16ToStream(this.BoundingRegionMinX, output);
            ReusableIO.WriteInt16ToStream(this.BoundingRegionMaxX, output);
            ReusableIO.WriteInt16ToStream(this.BoundingRegionMinY, output);
            ReusableIO.WriteInt16ToStream(this.BoundingRegionMaxY, output);
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
        protected String GetVersionString()
        {
            return "Polygon:";
        }

        /// <summary>Returns the printable read-friendly version of the format</summary>
        /// <param name="index">Index of the overlay</param>
        /// <returns>A descriptive lead to the string representation</returns>
        protected String GetVersionString(Int32 index)
        {
            return String.Format("Polygon # {0,5}:", index);
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Vertex Start Index"));
            builder.Append(this.StartIndex);
            builder.Append(StringFormat.ToStringAlignment("Vertex Count"));
            builder.Append(this.VertexCount);
            builder.Append(StringFormat.ToStringAlignment("Properties value"));
            builder.Append((Byte)this.Properties);
            builder.Append(StringFormat.ToStringAlignment("Properties"));
            builder.Append(this.GetPolygonFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Unknown Properties"));
            builder.Append(this.Unknown);
            builder.Append(StringFormat.ToStringAlignment("Bounding Min X"));
            builder.Append(this.BoundingRegionMinX);
            builder.Append(StringFormat.ToStringAlignment("Bounding Max X"));
            builder.Append(this.BoundingRegionMaxX);
            builder.Append(StringFormat.ToStringAlignment("Bounding Min Y"));
            builder.Append(this.BoundingRegionMinY);
            builder.Append(StringFormat.ToStringAlignment("Bounding Max Y"));
            builder.Append(this.BoundingRegionMaxY);

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which polygon flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetPolygonFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.Properties & PolygonProperties.Wall) == PolygonProperties.Wall, PolygonProperties.Wall.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Properties & PolygonProperties.Hovering) == PolygonProperties.Hovering, PolygonProperties.Hovering.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Properties & PolygonProperties.SemiTransparent) == PolygonProperties.SemiTransparent, PolygonProperties.SemiTransparent.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Properties & PolygonProperties.Covering) == PolygonProperties.Covering, PolygonProperties.Covering.GetDescription());
            StringFormat.AppendSubItem(sb, (this.Properties & PolygonProperties.Door) == PolygonProperties.Door, PolygonProperties.Door.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
        }
        #endregion
    }
}