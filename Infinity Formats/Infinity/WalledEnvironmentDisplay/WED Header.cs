using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents the WED header structure</summary>
    public class WedHeader : InfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 32;
        #endregion


        #region Fields
        /// <summary>Count of overlays in this display, including the base layer</summary>
        public UInt32 CountOverlay { get; set; }

        /// <summary>Count of doors in this display</summary>
        public UInt32 CountDoor { get; set; }

        /// <summary>Offset from the start of file to the overlay structures</summary>
        public UInt32 OffsetOverlays { get; set; }

        /// <summary>Offset from the start of file to the polygon header structure</summary>
        public UInt32 OffsetPolygonHeader { get; set; }

        /// <summary>Offset from the start of file to the door structures</summary>
        public UInt32 OffsetDoors { get; set; }

        /// <summary>Offset from the start of file to the door tilemap indeces</summary>
        public UInt32 OffsetDoorTileMapIndeces { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize() { }
        #endregion


        #region IO method implemetations
        /// <summary>This public method reads file format from the input stream, after the header has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public override void ReadBody(Stream input)
        {
            Byte[] buffer = ReusableIO.BinaryRead(input, WedHeader.StructSize - 8);   //header buffer

            this.CountOverlay = ReusableIO.ReadUInt32FromArray(buffer, 0);
            this.CountDoor = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.OffsetOverlays = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.OffsetPolygonHeader = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.OffsetDoors = ReusableIO.ReadUInt32FromArray(buffer, 16);
            this.OffsetDoorTileMapIndeces = ReusableIO.ReadUInt32FromArray(buffer, 20);
        }

        /// <summary>This public method writes the header to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteStringToStream(this.signature, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteStringToStream(this.version, output, CultureConstants.CultureCodeEnglish, false, 4);
            ReusableIO.WriteUInt32ToStream(this.CountOverlay, output);
            ReusableIO.WriteUInt32ToStream(this.CountDoor, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetOverlays, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetPolygonHeader, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetDoors, output);
            ReusableIO.WriteUInt32ToStream(this.OffsetDoorTileMapIndeces, output);   
        }
        #endregion


        #region Public Methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        /// <remarks>TIS header signatures can be null-terminated rather than have whitespace</remarks>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Tis1Header:");
            builder.Append(StringFormat.ToStringAlignment("Signature"));
            builder.Append(String.Format("'{0}'", this.signature));
            builder.Append(StringFormat.ToStringAlignment("Version"));
            builder.Append(String.Format("'{0}'", this.version));
            builder.Append(StringFormat.ToStringAlignment("Overlay Count"));
            builder.Append(this.CountOverlay);
            builder.Append(StringFormat.ToStringAlignment("Door Count"));
            builder.Append(this.CountDoor);
            builder.Append(StringFormat.ToStringAlignment("Overlays Offset"));
            builder.Append(this.OffsetOverlays);
            builder.Append(StringFormat.ToStringAlignment("Polygon Header Offset"));
            builder.Append(this.OffsetPolygonHeader);
            builder.Append(StringFormat.ToStringAlignment("Doors Offset"));
            builder.Append(this.OffsetDoors);
            builder.Append(StringFormat.ToStringAlignment("Door Tilemap Indeces Offset"));
            builder.Append(this.OffsetDoorTileMapIndeces);

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
                if (obj != null && obj is WedHeader)
                {
                    WedHeader compare = obj as WedHeader;

                    Boolean structureEquality;
                    structureEquality = (this.CountOverlay == compare.CountOverlay);
                    structureEquality &= (this.CountDoor == compare.CountDoor);
                    structureEquality &= (this.OffsetOverlays == compare.OffsetOverlays);
                    structureEquality &= (this.OffsetPolygonHeader == compare.OffsetPolygonHeader);
                    structureEquality &= (this.OffsetDoors == compare.OffsetDoors);
                    structureEquality &= (this.OffsetDoorTileMapIndeces == compare.OffsetDoorTileMapIndeces);

                    equal = structureEquality;
                }
            }
            catch { equal = false; }    //per MSDN, must not throw exceptions

            return equal;
        }
        #endregion
    }
}