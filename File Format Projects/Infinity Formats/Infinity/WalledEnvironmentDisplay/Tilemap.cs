using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay.Enum;
using Bardez.Projects.Utility;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WalledEnvironmentDisplay
{
    /// <summary>Represents a single tilemap structure in a WED file</summary>
    public class Tilemap : IInfinityFormat
    {
        #region Constants
        /// <summary>Size of this file structure on disk</summary>
        public const Int32 StructSize = 10;
        #endregion


        #region Fields
        /// <summary>Primary start tile index for this tile</summary>
        public Int16 PrimaryStartIndex { get; set; }

        /// <summary>Count of primary tiles for the tile</summary>
        public Int16 PrimaryTileCount { get; set; }

        /// <summary>Secondary start index of tiles for the tile</summary>
        public Int16 SecondaryStartIndex { get; set; }

        /// <summary>Layers to render this tile for</summary>
        public WedOverlayRender RenderLayer { get; set; }
        
        /// <summary>Three padding bytes to align the trailing boolean field</summary>
        public Byte[] Padding { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public void Initialize()
        {
            this.Padding = new Byte[3];
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

            Byte[] data = ReusableIO.BinaryRead(input, Tilemap.StructSize);

            this.PrimaryStartIndex = ReusableIO.ReadInt16FromArray(data, 0);
            this.PrimaryTileCount = ReusableIO.ReadInt16FromArray(data, 2);
            this.SecondaryStartIndex = ReusableIO.ReadInt16FromArray(data, 4);
            this.RenderLayer = (WedOverlayRender)data[6];
            Array.Copy(data, 7, this.Padding, 0, 3);
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public void Write(Stream output)
        {
            ReusableIO.WriteInt16ToStream(this.PrimaryStartIndex, output);
            ReusableIO.WriteInt16ToStream(this.PrimaryTileCount, output);
            ReusableIO.WriteInt16ToStream(this.SecondaryStartIndex, output);
            output.WriteByte((Byte)this.RenderLayer);
            output.Write(this.Padding, 0, 3);
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
            return "WED Tilemap:";
        }

        /// <summary>This method performs the bulk of work for a ToString() implementation that would output to console or similar.</summary>
        /// <returns>A string, formatted largely for console, that describes the item's contents.</returns>
        protected String GetStringRepresentation()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Primary Tile Start Index"));
            builder.Append(this.PrimaryStartIndex);
            builder.Append(StringFormat.ToStringAlignment("Primary Tile Count"));
            builder.Append(this.PrimaryTileCount);
            builder.Append(StringFormat.ToStringAlignment("Secondary Tile Start Index"));
            builder.Append(this.SecondaryStartIndex);
            builder.Append(StringFormat.ToStringAlignment("Render Layer(s) value"));
            builder.Append((Byte)this.RenderLayer);
            builder.Append(StringFormat.ToStringAlignment("Render Layer(s)"));
            builder.Append(this.GetOverlayFlagsString());
            builder.Append(StringFormat.ToStringAlignment("Padding"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Padding));

            return builder.ToString();
        }

        /// <summary>Generates a human-readable multi-line string for console output that indicates which D20 arcetype flags are set</summary>
        /// <returns>A multi-line string</returns>
        protected String GetOverlayFlagsString()
        {
            StringBuilder sb = new StringBuilder();

            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay1) == WedOverlayRender.Overlay1, WedOverlayRender.Overlay1.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay2) == WedOverlayRender.Overlay2, WedOverlayRender.Overlay2.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay3) == WedOverlayRender.Overlay3, WedOverlayRender.Overlay3.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay4) == WedOverlayRender.Overlay4, WedOverlayRender.Overlay4.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay5) == WedOverlayRender.Overlay5, WedOverlayRender.Overlay5.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay6) == WedOverlayRender.Overlay6, WedOverlayRender.Overlay6.GetDescription());
            StringFormat.AppendSubItem(sb, (this.RenderLayer & WedOverlayRender.Overlay7) == WedOverlayRender.Overlay7, WedOverlayRender.Overlay7.GetDescription());

            String result = sb.ToString();
            return result == String.Empty ? StringFormat.ReturnAndIndent("None", 2) : result;
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
                if (obj != null && obj is Tilemap)
                {
                    Tilemap compare = obj as Tilemap;

                    Boolean structureEquality = (this.PrimaryStartIndex == compare.PrimaryStartIndex);
                    structureEquality &= (this.PrimaryTileCount == compare.PrimaryTileCount);
                    structureEquality &= (this.SecondaryStartIndex.Equals(compare.SecondaryStartIndex));
                    structureEquality &= (this.RenderLayer == compare.RenderLayer);

                    if (structureEquality)
                        structureEquality &= this.Padding.Equals<Byte>(compare.Padding);

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
            hash ^= this.PrimaryStartIndex.GetHashCode();
            hash ^= this.PrimaryTileCount.GetHashCode();
            hash ^= this.SecondaryStartIndex.GetHashCode();
            hash ^= this.RenderLayer.GetHashCode();
            hash ^= this.Padding.GetHashCode<Byte>();
            //offsets are unimportant when it comes to data value equivalence/equality

            return hash;
        }
        #endregion
    }
}