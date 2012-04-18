using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.BioWareIndexFileFormat.Components
{
    /// <summary>This class describes a tileset entry within the BIFF archive</summary>
    /// <remarks>
    ///     Taken from the Infinity Engine (Infinity Engine Structures Description Project):
    /// 
    ///     Offset 	Size    (data type)     Description
    ///     0x0000 	4       (dword)         Resource locator
    ///                                         NB: On disk, only bits 14-19 are matched upon. They are matched against the
    ///                                         tileset index in the "resource1 locator" field from the KEY file
    ///                                         resource1 entries which claim to exist in this BIFF.
    ///     0x0004 	4       (dword)         Offset (from start of file) to resource1 data
    ///     0x0008 	4       (dword)         Count of tiles in this resource1
    ///     0x000c 	4       (dword)         Size of each tile in this resource1
    ///     0x0010 	2       (word) 	        Type of this resource1 (always 0x3eb - TIS)
    ///     0x0012 	2       (word) 	        Unknown
    ///     
    /// </remarks>
    public class Biff1TilesetEntry : Biff1FileEntry
    {
        #region Fields
        /// <summary>Defines the count of tiles within the resource data.</summary>
        public UInt32 CountTile { get; set; }

        /// <summary>Exposes the length of each tile in the resource data.</summary>
        protected UInt32 SizeTile { get; set; }
        #endregion


        #region Properties
        /// <summary>This property exposes the length of each tile in the resource1 data.</summary>
        public override UInt32 SizeResource
        {
            get { return this.SizeTile * this.CountTile; }
            set { throw new NotImplementedException("set Accessor for Biff1TilesetEntry.SizeResource is not"); }
        }
        #endregion



        #region IInfinityFormat I/O methods
        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream object from which to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, 20);   //data buffer

            this.ResourceLocator.Locator = ReusableIO.ReadInt32FromArray(buffer, 0x0);
            this.OffsetResource = ReusableIO.ReadInt32FromArray(buffer, 0x4);
            this.CountTile = ReusableIO.ReadUInt32FromArray(buffer, 0x8);
            this.SizeTile = ReusableIO.ReadUInt32FromArray(buffer, 0xC);
            this.TypeResource = (ResourceType)ReusableIO.ReadInt16FromArray(buffer, 0x10);
            this.UnknownPadding = ReusableIO.ReadInt16FromArray(buffer, 0x12);
        }

        /// <summary>This public method writes the resource1 entry to an output stream</summary>
        /// <param name="output">Stream object into which to write to</param>
        public override void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.ResourceLocator.Locator, output);
            ReusableIO.WriteInt32ToStream(this.OffsetResource, output);
            ReusableIO.WriteUInt32ToStream(this.CountTile, output);
            ReusableIO.WriteUInt32ToStream(this.SizeTile, output);
            ReusableIO.WriteInt16ToStream((Int16)this.TypeResource, output);
            ReusableIO.WriteInt16ToStream(this.UnknownPadding, output);
        }
        #endregion


        #region ToString override
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A String containing the member data line by line</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StringFormat.ToStringAlignment("Biff1TilesetEntry", 0));
            builder.Append(StringFormat.ToStringAlignment("Resource Locator"));
            builder.Append(this.ResourceLocator.Locator);
            builder.Append(StringFormat.ToStringAlignment("BIFF Index"));
            builder.Append(this.ResourceLocator.BiffIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Tileset Index"));
            builder.Append(this.ResourceLocator.TilesetIndex);
            builder.Append(StringFormat.ToStringAlignment("BIFF Resource Index"));
            builder.Append(this.ResourceLocator.ResourceIndex);
            builder.Append(StringFormat.ToStringAlignment("Offset"));
            builder.Append(this.OffsetResource);
            builder.Append(StringFormat.ToStringAlignment("Tile Count"));
            builder.Append(this.CountTile);
            builder.Append(StringFormat.ToStringAlignment("Tile Size"));
            builder.Append(this.SizeTile);
            builder.Append(StringFormat.ToStringAlignment("Size"));
            builder.Append(this.SizeResource);
            builder.Append(StringFormat.ToStringAlignment("Type"));
            builder.Append(this.TypeResource.ToString("G"));
            builder.Append(StringFormat.ToStringAlignment("Type (hex)"));
            builder.AppendLine(String.Format("0x{0:X4}",(Int16)this.TypeResource));

            return builder.ToString();
        }
        #endregion
    }
}