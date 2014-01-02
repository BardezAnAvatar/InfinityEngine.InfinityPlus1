using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.WorldMap.Component
{
    /// <summary>An area entry for a world map</summary>
    public class MapEntry : IInfinityFormat
    {
        #region Constants
        /// <summary>Binary size of the struct on disk</summary>
        public const Int32 StructSize = 184;
        #endregion


        #region Fields
        /// <summary>The background image (MOS) to render</summary>
        public ResourceReference BackgroundImage { get; set; }

        /// <summary>Width of the map</summary>
        /// <remarks>Will a larger MOS be trimmed? what about a smaller one?</remarks>
        public UInt32 Width { get; set; }

        /// <summary>Height of the map</summary>
        /// <remarks>Will a larger MOS be trimmed? what about a smaller one?</remarks>
        public UInt32 Height { get; set; }

        /// <summary>The map number within the world map</summary>
        public Int32 MapNumber { get; set; }

        /// <summary>StrRef indicating the map's name</summary>
        public StringReference MapName { get; set; }

        /// <summary>X-coordinate to initially center the map on</summary>
        public Int32 MapCenterX { get; set; }

        /// <summary>Y-coordinate to initially center the map on</summary>
        public Int32 MapCenterY { get; set; }

        /// <summary>Count of area entries for this map</summary>
        public UInt32 AreaCount { get; set; }

        /// <summary>Offset at which area entries for this map are located</summary>
        public UInt32 AreaOffset { get; set; }

        /// <summary>Count of Area Links in this map</summary>
        public UInt32 AreaLinkCount { get; set; }

        /// <summary>Offset at which Area Links in this map are located</summary>
        public UInt32 AreaLinkOffset { get; set; }

        /// <summary>The BAM resource containing the map's icons</summary>
        public ResourceReference Icons { get; set; }

        /// <summary>128 bytes of reserved data that are currently unused</summary>
        public Byte[] Reserved { get; set; }

        /// <summary>Collection of areas on the world map</summary>
        public List<AreaEntry> Areas { get; set; }

        /// <summary>Collection of links between areas on the world map</summary>
        public List<AreaLink> Links { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.BackgroundImage = new ResourceReference(ResourceType.Mosaic);
            this.MapName = new StringReference();
            this.Icons = new ResourceReference(ResourceType.Bam);
            this.Areas = new List<AreaEntry>();
            this.Links = new List<AreaLink>();
        }
        #endregion


        #region I/O Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format data structure from the output stream, after the signature has already been read.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void ReadBody(Stream input)
        {
            //wipe and initialize as much as possible
            this.Initialize();

            //read remiander
            Byte[] buffer = ReusableIO.BinaryRead(input, (MapEntry.StructSize - 128));

            this.BackgroundImage.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 0, CultureConstants.CultureCodeEnglish);
            this.Width = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.Height = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.MapNumber = ReusableIO.ReadInt32FromArray(buffer, 16);
            this.MapName.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 20);
            this.MapCenterX = ReusableIO.ReadInt32FromArray(buffer, 24);
            this.MapCenterY = ReusableIO.ReadInt32FromArray(buffer, 28);
            this.AreaCount = ReusableIO.ReadUInt32FromArray(buffer, 32);
            this.AreaOffset = ReusableIO.ReadUInt32FromArray(buffer, 36);
            this.AreaLinkCount = ReusableIO.ReadUInt32FromArray(buffer, 40);
            this.AreaLinkOffset = ReusableIO.ReadUInt32FromArray(buffer, 44);
            this.Icons.ResRef = ReusableIO.ReadStringFromByteArray(buffer, 48, CultureConstants.CultureCodeEnglish);
            this.Reserved = ReusableIO.BinaryRead(input, 128);

            //areas
            ReusableIO.SeekIfAble(input, this.AreaOffset);
            for (Int32 areaIndex = 0; areaIndex < this.AreaCount; ++areaIndex)
            {
                AreaEntry area = new AreaEntry();
                area.Read(input);
                this.Areas.Add(area);
            }

            //area links
            ReusableIO.SeekIfAble(input, this.AreaLinkOffset);
            for (Int32 linkIndex = 0; linkIndex < this.AreaLinkCount; ++linkIndex)
            {
                AreaLink link = new AreaLink();
                link.Read(input);
                this.Links.Add(link);
            }
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            //maintain a minimal level of data integrity
            this.MaintainMinimalDataIntegrity();

            //write fields
            ReusableIO.WriteStringToStream(this.BackgroundImage.ResRef, output, CultureConstants.CultureCodeEnglish);
            ReusableIO.WriteUInt32ToStream(this.Width, output);
            ReusableIO.WriteUInt32ToStream(this.Height, output);
            ReusableIO.WriteInt32ToStream(this.MapNumber, output);
            ReusableIO.WriteInt32ToStream(this.MapName.StringReferenceIndex, output);
            ReusableIO.WriteInt32ToStream(this.MapCenterX, output);
            ReusableIO.WriteInt32ToStream(this.MapCenterY, output);
            ReusableIO.WriteUInt32ToStream(this.AreaCount, output);
            ReusableIO.WriteUInt32ToStream(this.AreaOffset, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkCount, output);
            ReusableIO.WriteUInt32ToStream(this.AreaLinkOffset, output);
            ReusableIO.WriteStringToStream(this.Icons.ResRef, output, CultureConstants.CultureCodeEnglish);
            output.Write(this.Reserved, 0, 128);

            //write map areas
            //areas
            ReusableIO.SeekIfAble(output, this.AreaOffset);
            foreach (AreaEntry area in this.Areas)
                area.Write(output);

            //area links
            ReusableIO.SeekIfAble(output, this.AreaLinkOffset);
            foreach (AreaLink link in this.Links)
                link.Write(output);
        }
        #endregion


        #region Hash code and comparison
        /// <summary>Override of GetHashCode</summary>
        /// <returns>Computed hash</returns>
        public override Int32 GetHashCode()
        {
            Int32 hash = this.BackgroundImage.GetHashCode();
            hash ^= this.Width.GetHashCode();
            hash ^= this.Height.GetHashCode();
            hash ^= this.MapNumber.GetHashCode();
            hash ^= this.MapName.GetHashCode();
            hash ^= this.MapCenterX.GetHashCode();
            hash ^= this.MapCenterY.GetHashCode();
            hash ^= this.AreaCount.GetHashCode();
            hash ^= this.AreaOffset.GetHashCode();
            hash ^= this.AreaLinkCount.GetHashCode();
            hash ^= this.AreaLinkOffset.GetHashCode();
            hash ^= this.Icons.GetHashCode();
            hash ^= this.Reserved.GetHashCode();

            return hash;
        }
        #endregion


        #region ToString override(s)
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <param name="index">Index of this map entry to describe</param>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public String ToString(Int32 index)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("Map Entry # {0}:", index));
            builder.Append(StringFormat.ToStringAlignment("Background Image"));
            builder.Append(String.Format("'{0}'", this.BackgroundImage.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Width"));
            builder.Append(this.Width);
            builder.Append(StringFormat.ToStringAlignment("Height"));
            builder.Append(this.Height);
            builder.Append(StringFormat.ToStringAlignment("Map number"));
            builder.Append(this.MapNumber);
            builder.Append(StringFormat.ToStringAlignment("Map Name StrRef"));
            builder.Append(this.MapName.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("Map center X"));
            builder.Append(this.MapCenterX);
            builder.Append(StringFormat.ToStringAlignment("Map center Y"));
            builder.Append(this.MapCenterY);
            builder.Append(StringFormat.ToStringAlignment("Area count"));
            builder.Append(this.AreaCount);
            builder.Append(StringFormat.ToStringAlignment("Area offset"));
            builder.Append(this.AreaOffset);
            builder.Append(StringFormat.ToStringAlignment("Area link count"));
            builder.Append(this.AreaLinkCount);
            builder.Append(StringFormat.ToStringAlignment("Area link offset"));
            builder.Append(this.AreaLinkOffset);
            builder.Append(StringFormat.ToStringAlignment("Icons"));
            builder.Append(String.Format("'{0}'", this.Icons.ZResRef));
            builder.Append(StringFormat.ToStringAlignment("Reserved"));
            builder.Append(StringFormat.ByteArrayToHexString(this.Reserved));

            //write the area entries
            for (Int32 i = 0; i < this.AreaCount; ++i)
                builder.Append(this.Areas[i].ToString(i + 1));

            //write the area links
            for (Int32 i = 0; i < this.AreaLinkCount; ++i)
                builder.Append(this.Links[i].ToString(i + 1));

            return builder.ToString();
        }
        #endregion


        #region Data integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        protected void MaintainMinimalDataIntegrity()
        {
            if (this.AreaCount != this.Areas.Count)
                this.AreaCount = Convert.ToUInt32(this.Areas.Count);

            if (this.AreaLinkCount != this.Links.Count)
                this.AreaLinkCount = Convert.ToUInt32(this.Links.Count);
        }
        #endregion
    }
}